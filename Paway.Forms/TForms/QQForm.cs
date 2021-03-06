﻿using System.Drawing;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Forms.Properties;

namespace Paway.Forms
{
    /// <summary>
    /// </summary>
    public class QQForm : FormBase
    {
        #region 属性
        /// <summary>
        /// 最大化按钮区域
        /// </summary>
        protected override Rectangle MaxRect
        {
            get
            {
                int x;
                var width = 28;
                switch (SysButton)
                {
                    case TSysButton.Normal:
                    case TSysButton.Close_Max:
                        x = Width - CloseRect.Width - width;
                        break;
                    default:
                        x = -1 * width;
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
                int x;
                var width = 28;
                switch (SysButton)
                {
                    case TSysButton.Normal:
                        x = Width - width - CloseRect.Width - MaxRect.Width;
                        break;
                    case TSysButton.Close_Mini:
                        x = Width - width - CloseRect.Width;
                        break;
                    default:
                        x = -1 * width;
                        break;
                }
                var rect = new Rectangle(x, -1, width, 20);
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
                switch (SysButton)
                {
                    case TSysButton.Normal:
                        return new Rectangle(Width - 28 * 2 - 39, 0, 39 + 28 + 28, 20);
                    case TSysButton.Close:
                        return CloseRect;
                    case TSysButton.Close_Mini:
                    case TSysButton.Close_Max:
                        return new Rectangle(Width - 28 - 39, 0, 39 + 28, 20);
                    default:
                        return Rectangle.Empty;
                }
            }
        }

        /// <summary>
        /// 关闭按钮区域
        /// </summary>
        protected override Rectangle CloseRect
        {
            get
            {
                switch (SysButton)
                {
                    case TSysButton.None:
                        return Rectangle.Empty;
                    default:
                        return new Rectangle(Width - 39, -1, 39, 20);
                }
            }
        }

        #endregion

        #region 重绘按钮
        /// <summary>
        /// 重绘按钮
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            //绘画系统控制按钮
            if (ControlBox)
            {
                switch (SysButton)
                {
                    case TSysButton.Normal:
                        DrawButton(g, MinState, MiniRect, "mini");
                        if (WindowState == FormWindowState.Maximized)
                            DrawButton(g, MaxState, MaxRect, "restore");
                        else
                            DrawButton(g, MaxState, MaxRect, "max");
                        DrawButton(g, CloseState, CloseRect, "close");
                        break;
                    case TSysButton.Close:
                        DrawButton(g, CloseState, CloseRect, "close");
                        break;
                    case TSysButton.Close_Mini:
                        DrawButton(g, MinState, MiniRect, "mini");
                        DrawButton(g, CloseState, CloseRect, "close");
                        break;
                    case TSysButton.Close_Max:
                        if (WindowState == FormWindowState.Maximized)
                            DrawButton(g, MaxState, MaxRect, "restore");
                        else
                            DrawButton(g, MaxState, MaxRect, "max");
                        DrawButton(g, CloseState, CloseRect, "close");
                        break;
                }
            }
        }
        /// <summary>
        /// 绘画按钮
        /// </summary>
        /// <param name="g">画板</param>
        /// <param name="mouseState">鼠标状态</param>
        /// <param name="rect">按钮区域</param>
        /// <param name="str">图片字符串</param>
        internal void DrawButton(Graphics g, TMouseState mouseState, Rectangle rect, string str)
        {
            Bitmap bitmap = null;
            switch (mouseState)
            {
                case TMouseState.Normal:
                    switch (str)
                    {
                        case "mini":
                            bitmap = Resources.QQ_SysButton_btn_mini_normal;
                            break;
                        case "restore":
                            bitmap = Resources.QQ_SysButton_btn_restore_normal;
                            break;
                        case "max":
                            bitmap = Resources.QQ_SysButton_btn_max_normal;
                            break;
                        case "close":
                            bitmap = Resources.QQ_SysButton_btn_close_normal;
                            break;
                    }
                    break;
                case TMouseState.Move:
                case TMouseState.Up:
                    switch (str)
                    {
                        case "mini":
                            bitmap = Resources.QQ_SysButton_btn_mini_highlight;
                            break;
                        case "restore":
                            bitmap = Resources.QQ_SysButton_btn_restore_highlight;
                            break;
                        case "max":
                            bitmap = Resources.QQ_SysButton_btn_max_highlight;
                            break;
                        case "close":
                            bitmap = Resources.QQ_SysButton_btn_close_highlight;
                            break;
                    }
                    break;
                case TMouseState.Down:
                    switch (str)
                    {
                        case "mini":
                            bitmap = Resources.QQ_SysButton_btn_mini_down;
                            break;
                        case "restore":
                            bitmap = Resources.QQ_SysButton_btn_restore_down;
                            break;
                        case "max":
                            bitmap = Resources.QQ_SysButton_btn_max_down;
                            break;
                        case "close":
                            bitmap = Resources.QQ_SysButton_btn_close_down;
                            break;
                    }
                    break;
            }
            if (bitmap != null) g.DrawImage(bitmap, rect);
        }

        #endregion
    }
}