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
        /// 最大化按钮区域
        /// </summary>
        protected override Rectangle MaxRect
        {
            get
            {
                int x = 0;
                int width = 28;
                switch (base.SysButton)
                {
                    case TSysButton.Normal:
                        x = this.Width - this.CloseRect.Width - width;
                        break;
                    default:
                        x = 0;
                        break;
                }
                return new Rectangle(x, -1, width, 20);
            }
        }
        /// <summary>
        /// 最小化按钮区域
        /// </summary>
        protected override Rectangle MiniRect
        {
            get
            {
                int x = 0;
                int width = 28;
                switch (base.SysButton)
                {
                    case TSysButton.Normal:
                        x = this.Width - width - this.CloseRect.Width - this.MaxRect.Width;
                        break;
                    case TSysButton.Close_Mini:
                        x = this.Width - width - this.CloseRect.Width;
                        break;
                    default:
                        x = 0;
                        break;
                }
                Rectangle rect = new Rectangle(x, -1, width, 20);
                return rect;
            }
        }
        /// <summary>
        /// 系统控制按钮区域
        /// </summary>
        protected override Rectangle SysBtnRect
        {
            get
            {
                if (base._sysButton == TSysButton.Normal)
                    return new Rectangle(this.Width - 28 * 2 - 39, 0, 39 + 28 + 28, 20);
                else if (base._sysButton == TSysButton.Close_Mini)
                    return new Rectangle(this.Width - 28 - 39, 0, 39 + 28, 20);
                else
                    return this.CloseRect;
            }
        }
        /// <summary>
        /// 关闭按钮区域
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
        private void DrawButton(Graphics g, TMouseState mouseState, Rectangle rect, string str)
        {
            switch (mouseState)
            {
                case TMouseState.Normal:
                    g.DrawImage(AssemblyHelper.GetImage("QQ.SysButton.btn_" + str + "_normal.png"), rect);
                    break;
                case TMouseState.Move:
                case TMouseState.Up:
                    g.DrawImage(AssemblyHelper.GetImage("QQ.SysButton.btn_" + str + "_highlight.png"), rect);
                    break;
                case TMouseState.Down:
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
            base.OnPaint(e);
            Graphics g = e.Graphics;
            //绘画系统控制按钮
            if (ControlBox)
            {
                switch (base.SysButton)
                {
                    case TSysButton.Normal:
                        this.DrawButton(g, base.MinState, this.MiniRect, "mini");
                        this.DrawButton(g, base.CloseState, this.CloseRect, "close");
                        if (this.WindowState == FormWindowState.Maximized)
                            this.DrawButton(g, base.MaxState, this.MaxRect, "restore");
                        else
                            this.DrawButton(g, base.MaxState, this.MaxRect, "max");
                        break;
                    case TSysButton.Close:
                        this.DrawButton(g, base.CloseState, this.CloseRect, "close");
                        break;
                    case TSysButton.Close_Mini:
                        this.DrawButton(g, base.MinState, this.MiniRect, "mini");
                        this.DrawButton(g, base.CloseState, this.CloseRect, "close");
                        break;
                }
            }
        }

        #endregion
    }
}
