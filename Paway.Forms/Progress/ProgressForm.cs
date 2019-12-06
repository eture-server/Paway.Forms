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
using System.Reflection;

namespace Paway.Forms
{
    internal partial class ProgressForm : TForm
    {
        /// <summary>
        /// 显示/隐藏速度
        /// </summary>
        private double _opacitySpeed = 0.2;
        /// <summary>
        /// 显示/隐藏目标透明度
        /// </summary>
        private double _targetOpacity;
        private readonly Timer timer = null;
        private readonly ProgressStates states;
        public IntPtr CurrentHandle { get { return this.states.CurrentHandle; } }
        public string CurrentCaption { get { return this.states.CurrentCaption; } }
        public bool ShowCancel { get { return this.states.ShowCancel; } }
        public bool IDelay { get { return this.states.IDelay; } }
        public ProgressForm(ProgressStates states)
        {
            InitializeComponent();

            this.states = states;
            this.Location = new Point(-1000, -1000);
            this.StartPosition = FormStartPosition.Manual;
            this.Opacity = 0;
            this.TopMost = true;
            this.TMouseMove(this);
            this.TMouseMove(lbCaption);
            this.TMouseMove(progressBar1);
            this.TMouseMove(tControl1);
            this.toolCancel.ItemClick += ToolCancel_ItemClick;

            timer = new Timer()
            {
                Interval = 10
            };
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }
        private void ToolCancel_ItemClick(ToolItem item, EventArgs e)
        {
            this.states.Cancel();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            lock (this.states)
            {
                Timer();
                Application.DoEvents();
            }
        }
        private void Timer()
        {
            IntPtr handle = this.CurrentHandle;
            IntPtr current = NativeMethods.GetForegroundWindow();
            if (handle == Progress.Auto && Control.FromHandle(current) == null)
            {
                this.Fade(0.0, 0.2);
            }
            else if (handle != Progress.Auto && handle != Progress.Manual && handle != current)
            {
                this.Fade(0.0, 0.2);
            }
            else if (IDelay)
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
                        RECT rect = new RECT();
                        bool result = false;
                        if (handle == Progress.Auto && Control.FromHandle(current) != null)
                        {
                            result = NativeMethods.GetWindowRect(current, ref rect);
                        }
                        else if (handle == Progress.Manual)
                        {
                            result = NativeMethods.GetWindowRect(current, ref rect);
                        }
                        else
                        {
                            result = NativeMethods.GetWindowRect(handle, ref rect);
                        }
                        //成功获取父窗体区域
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
                    else
                    {
                        THide();
                    }
                }
                else
                {
                    THide();
                }
            }
            else if (handle == Progress.Auto)
            {
                RECT rect = new RECT();
                if (Control.FromHandle(current) != null && NativeMethods.GetWindowRect(current, ref rect))
                {
                    Win32Helper.ShowWindow(base.Handle);
                }
            }
            else if (handle == Progress.Manual)
            {
                RECT rect = new RECT();
                if (NativeMethods.GetWindowRect(current, ref rect))
                {
                    Win32Helper.ShowWindow(base.Handle);
                }
            }
            lbCaption.Text = this.CurrentCaption;
            toolCancel.Visible = this.ShowCancel;
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
            if (disposing)
            {
            }
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
