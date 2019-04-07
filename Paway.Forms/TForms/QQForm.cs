using System.Drawing;
using System.Windows.Forms;
using Paway.Resource;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// </summary>
    public class QQForm : FormBase
    {
        #region 方法
        /// <summary>
        ///     绘画按钮
        /// </summary>
        /// <param name="g">画板</param>
        /// <param name="mouseState">鼠标状态</param>
        /// <param name="rect">按钮区域</param>
        /// <param name="str">图片字符串</param>
        protected void DrawButton(Graphics g, TMouseState mouseState, Rectangle rect, string str)
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
        /// </summary>
        /// <param name="e"></param>
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
                        DrawButton(g, CloseState, CloseRect, "close");
                        if (WindowState == FormWindowState.Maximized)
                            DrawButton(g, MaxState, MaxRect, "restore");
                        else
                            DrawButton(g, MaxState, MaxRect, "max");
                        break;
                    case TSysButton.Close:
                        DrawButton(g, CloseState, CloseRect, "close");
                        break;
                    case TSysButton.Close_Mini:
                        DrawButton(g, MinState, MiniRect, "mini");
                        DrawButton(g, CloseState, CloseRect, "close");
                        break;
                }
            }
        }

        #endregion

        #region 属性

        /// <summary>
        ///     最大化按钮区域
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
        ///     最小化按钮区域
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
        ///     系统控制按钮区域
        /// </summary>
        protected override Rectangle SysBtnRect
        {
            get
            {
                if (_sysButton == TSysButton.Normal)
                    return new Rectangle(Width - 28 * 2 - 39, 0, 39 + 28 + 28, 20);
                if (_sysButton == TSysButton.Close_Mini)
                    return new Rectangle(Width - 28 - 39, 0, 39 + 28, 20);
                return CloseRect;
            }
        }

        /// <summary>
        ///     关闭按钮区域
        /// </summary>
        protected override Rectangle CloseRect
        {
            get { return new Rectangle(Width - 39, -1, 39, 20); }
        }

        #endregion
    }
}