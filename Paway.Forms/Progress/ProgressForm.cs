using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Paway.Forms;

namespace Paway.Forms
{
    internal partial class ProgressForm : TForm
    {
        private double _opacitySpeed;
        private double _targetOpacity;
        private Timer timer = null;
        private ProgressStates states;
        public ProgressState State { get { return this.states.CurrentState; } }
        public IntPtr WindowToWatch { get { return this.states.CurrentHandle; } }
        public string Caption { get { return this.states.CurrentCaption; } }
        public ProgressForm(ProgressStates states)
        {
            InitializeComponent();

            this.states = states;
            this.Location = new Point(-1000, -1000);
            this.StartPosition = FormStartPosition.Manual;
            this.TMouseMove(panel1);
            this.TMouseMove(lbCaption);
            this.TMouseMove(progressBar1);

            timer = new Timer();
            timer.Interval = 40;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                lock (this.states)
                {
                    IntPtr handle = this.WindowToWatch;
                    IntPtr current = GetForegroundWindow();
                    if (handle == ProgressStates.False && Control.FromHandle(current) == null)
                    {
                        this.Fade(0.0, 0.2);
                    }
                    else if (handle != ProgressStates.False && (handle == IntPtr.Zero || handle != current))
                    {
                        this.Fade(0.0, 0.2);
                    }
                    else
                    {//显示进度条
                        this.Fade(1.0, 0.2);
                        this.progressBar1.Maximum = this.states.Max;
                        this.progressBar1.Value = this.states.Value;
                    }
                    if (base.Opacity != this._targetOpacity)
                    {
                        double num = base.Opacity + this._opacitySpeed;
                        if ((this._opacitySpeed > 0.0 && num > this._targetOpacity) || (this._opacitySpeed < 0.0 && num < this._targetOpacity))
                        {
                            num = this._targetOpacity;
                        }
                        base.Opacity = num;
                        if (num > 0.0)
                        {
                            if (this._opacitySpeed > 0)
                            {
                                Rect rect = new Rect();
                                int result = 0;
                                if (handle == ProgressStates.False)
                                {
                                    if (Control.FromHandle(current) != null)
                                    {
                                        result = GetWindowRect(current, out rect);
                                    }
                                }
                                else
                                {
                                    result = GetWindowRect(handle, out rect);
                                }
                                //成功获取父窗体区域
                                if (result != 0)
                                {
                                    ShowWindow(base.Handle, 4);
                                    this.Location = new Point(
                                        rect.Left + (rect.Right - rect.Left) / 2 - this.Width / 2,
                                        rect.Top + (rect.Bottom - rect.Top) / 2 - this.Height / 2);
                                }
                            }
                            else if (this._opacitySpeed < 0)
                            {
                                ShowWindow(base.Handle, 4);
                            }
                            else THide();
                        }
                        else THide();
                    }
                    else if (handle == ProgressStates.False)
                    {
                        Rect rect;
                        if (Control.FromHandle(current) != null)
                        {
                            if (GetWindowRect(current, out rect) != 0)
                            {
                                ShowWindow(base.Handle, 4);
                            }
                        }
                    }
                    lbCaption.Text = this.Caption;
                    Application.DoEvents();
                }
            }
            catch
            {
                THide();
            }
        }
        private void THide()
        {
            this.Location = new Point(-1000, -1000);
            this.Hide();
        }
        /// <summary>
        /// 透明度渐变。
        /// </summary>
        /// <param name="opacity"></param>
        /// <param name="speed"></param>
        private void Fade(double opacity, double speed)
        {
            if (opacity < 0.0) opacity = 0.0;
            else if (opacity > 1.0) opacity = 1.0;

            if (speed < 0.0) speed = 0.0;
            else if (speed > 1.0) speed = 1.0;

            if (base.Opacity > opacity) speed = -speed;
            this._targetOpacity = opacity;
            this._opacitySpeed = speed;
        }

        #region override
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.ExStyle = 0x08080008;
                return createParams;
            }
        }
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region user32.dll
        internal struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        internal static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        internal static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);

        #endregion
    }
}
