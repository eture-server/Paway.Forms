using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using Paway.Resource;
using Paway.Win32;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class QQForm : FormBase
    {

        #region 构造函数
        /// <summary>
        /// </summary>
        public QQForm() : base() { }

        #endregion

        #region 属性
        /// <summary>
        /// 
        /// </summary>
        protected override Rectangle MaxRect
        {
            get
            {
                int x = 0;
                int width = 28;
                switch (base.SysButton)
                {
                    case ESysButton.Normal:
                        x = this.Width - this.CloseRect.Width - width;
                        break;
                }
                return new Rectangle(x, -1, width, 20);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override Rectangle MiniRect
        {
            get
            {
                int x = 0;
                int width = 28;
                switch (base.SysButton)
                {
                    case ESysButton.Normal:
                        x = this.Width - width - this.CloseRect.Width - this.MaxRect.Width;
                        break;
                    case ESysButton.Close_Mini:
                        x = this.Width - width - this.CloseRect.Width;
                        break;
                }
                Rectangle rect = new Rectangle(x, -1, width, 20);
                return rect;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override Rectangle SysBtnRect
        {
            get
            {
                if (base._sysButton == ESysButton.Normal)
                    return new Rectangle(this.Width - 28 * 2 - 39, 0, 39 + 28 + 28, 20);
                else if (base._sysButton == ESysButton.Close_Mini)
                    return new Rectangle(this.Width - 28 - 39, 0, 39 + 28, 20);
                else
                    return this.CloseRect;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override Rectangle CloseRect
        {
            get { return new Rectangle(this.Width - 39, -1, 39, 20); }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 绘画按钮
        /// </summary>
        /// <param name="g">画板</param>
        /// <param name="mouseState">鼠标状态</param>
        /// <param name="rect">按钮区域</param>
        /// <param name="str">图片字符串</param>
        private void DrawButton(Graphics g, EMouseState mouseState, Rectangle rect, string str)
        {
            switch (mouseState)
            {
                case EMouseState.Normal:
                    g.DrawImage(AssemblyHelper.GetImage("QQ.SysButton.btn_" + str + "_normal.png"), rect);
                    break;
                case EMouseState.Move:
                case EMouseState.Up:
                    g.DrawImage(AssemblyHelper.GetImage("QQ.SysButton.btn_" + str + "_highlight.png"), rect);
                    break;
                case EMouseState.Down:
                    g.DrawImage(AssemblyHelper.GetImage("QQ.SysButton.btn_" + str + "_down.png"), rect);
                    break;
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //绘画系统控制按钮
            switch (base.SysButton)
            {
                case ESysButton.Normal:
                    this.DrawButton(g, base.MinState, this.MiniRect, "mini");
                    this.DrawButton(g, base.CloseState, this.CloseRect, "close");
                    if (this.WindowState == FormWindowState.Maximized)
                        this.DrawButton(g, base.MaxState, this.MaxRect, "restore");
                    else
                        this.DrawButton(g, base.MaxState, this.MaxRect, "max");
                    break;
                case ESysButton.Close:
                    this.DrawButton(g, base.CloseState, this.CloseRect, "close");
                    break;
                case ESysButton.Close_Mini:
                    this.DrawButton(g, base.MinState, this.MiniRect, "mini");
                    this.DrawButton(g, base.CloseState, this.CloseRect, "close");
                    break;
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseDown。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //调用API，将窗体剪成圆角
            int ellipse = _isDrawRound ? 4 : 0;
            int rgn = NativeMethods.CreateRoundRectRgn(0, 0, this.Width + 1, this.Height + 1, ellipse, ellipse);
            NativeMethods.SetWindowRgn(this.Handle, rgn, true);
        }

        #endregion
    }
}
