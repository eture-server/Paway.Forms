using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Paway.Forms;
using Paway.Win32;

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

            timer = new Timer()
            {
                Interval = 40
            };
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                lock (this.states)
                {
                    IntPtr handle = this.WindowToWatch;
                    IntPtr current = NativeMethods.GetForegroundWindow();
                    if (handle == ProgressStates.False && Control.FromHandle(current) == null)
                    {
                        this.Fade(0.0, 0.2);
                    }
                    else if (handle != ProgressStates.False && (handle == IntPtr.Zero || handle != current))
                    {
                        this.Fade(0.0, 0.2);
                    }
                    else
                    {//��ʾ������
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
                                RECT rect = new RECT();
                                bool result = false;
                                if (handle == ProgressStates.False)
                                {
                                    if (Control.FromHandle(current) != null)
                                    {
                                        result = NativeMethods.GetWindowRect(current, ref rect);
                                    }
                                }
                                else
                                {
                                    result = NativeMethods.GetWindowRect(handle, ref rect);
                                }
                                //�ɹ���ȡ����������
                                if (result)
                                {
                                    Win32Helper.ShowWindow(base.Handle);
                                    this.Location = new Point(
                                        rect.Left + (rect.Right - rect.Left) / 2 - this.Width / 2,
                                        rect.Top + (rect.Bottom - rect.Top) / 2 - this.Height / 2);
                                }
                            }
                            else if (this._opacitySpeed < 0)
                            {
                                Win32Helper.ShowWindow(base.Handle);
                            }
                            else THide();
                        }
                        else THide();
                    }
                    else if (handle == ProgressStates.False)
                    {
                        RECT rect = new RECT();
                        if (Control.FromHandle(current) != null)
                        {
                            if (NativeMethods.GetWindowRect(current, ref rect))
                            {
                                Win32Helper.ShowWindow(base.Handle);
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
        /// ͸���Ƚ��䡣
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
        /// ������������ʹ�õ���Դ��
        /// </summary>
        /// <param name="disposing">���Ӧ�ͷ��й���Դ��Ϊ true������Ϊ false��</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}