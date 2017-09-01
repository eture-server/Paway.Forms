using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Resource;

namespace Paway.Forms
{
    /// <summary>
    ///     360 窗体
    /// </summary>
    public class _360Form : FormBase
    {
        #region 构造函数

        #endregion

        #region 方法

        /// <summary>
        ///     绘制窗体的系统控制按钮
        /// </summary>
        /// <param name="g">画板</param>
        /// <param name="rect">按钮所在的区域</param>
        /// <param name="image">图片</param>
        /// <param name="state">鼠标状态</param>
        private void DrawSysButton(Graphics g, Rectangle rect, Image image, TMouseState state)
        {
            var imageRect = Rectangle.Empty;
            switch (state)
            {
                case TMouseState.Normal:
                case TMouseState.Leave:
                    imageRect = new Rectangle(0, 0, rect.Width, rect.Height);
                    break;
                case TMouseState.Move:
                case TMouseState.Up:
                    imageRect = new Rectangle(rect.Width, 0, rect.Width, rect.Height);
                    break;
                case TMouseState.Down:
                    imageRect = new Rectangle(rect.Width * 2, 0, rect.Width, rect.Height);
                    break;
            }
            g.DrawImage(image, rect, imageRect, GraphicsUnit.Pixel);
        }

        #endregion

        #region 事件

        /// <summary>
        ///     菜单按钮单击事件
        /// </summary>
        public event EventHandler MenuClick;

        #endregion

        #region 资源图片

        /// <summary>
        ///     边框图片
        /// </summary>
        private readonly Image _borderImage = AssemblyHelper.GetImage("_360.Form.framemod.png");

        /// <summary>
        ///     关闭按钮图片
        /// </summary>
        private readonly Image _closeImage = AssemblyHelper.GetImage("_360.SysButton.sys_button_close.png");

        /// <summary>
        ///     最小化按钮图片
        /// </summary>
        private readonly Image _minImage = AssemblyHelper.GetImage("_360.SysButton.sys_button_min.png");

        /// <summary>
        ///     最大化按钮图片
        /// </summary>
        private readonly Image _maxImage = AssemblyHelper.GetImage("_360.SysButton.sys_button_max.png");

        /// <summary>
        ///     还原按钮图片
        /// </summary>
        private readonly Image _restoreImage = AssemblyHelper.GetImage("_360.SysButton.sys_button_restore.png");

        /// <summary>
        ///     标题栏菜单按钮图片
        /// </summary>
        private readonly Image _titleBarMenuImage = AssemblyHelper.GetImage("_360.SysButton.title_bar_menu.png");

        #endregion

        #region 属性

        /// <summary>
        ///     系统按钮与窗体右边边缘的间距
        /// </summary>
        private int _sysButtonPos = 4;

        /// <summary>
        ///     系统控制按钮与右边框之间的距离
        /// </summary>
        [Description("系统控制按钮与右边框之间的距离"), DefaultValue(4)]
        public int SysButtonPos
        {
            get { return _sysButtonPos; }
            set
            {
                if (_sysButtonPos != value)
                {
                    _sysButtonPos = value;
                    Invalidate(SysBtnRect);
                }
            }
        }

        /// <summary>
        ///     关闭按钮的矩形区域
        /// </summary>
        protected override Rectangle CloseRect
        {
            get
            {
                var width = _closeImage.Width / 4;
                var height = _closeImage.Height;
                var x = Width - width - _sysButtonPos;
                var y = -1;

                return new Rectangle(x, y, width, height);
            }
        }

        /// <summary>
        ///     最大化按钮的矩形区域
        /// </summary>
        protected override Rectangle MaxRect
        {
            get
            {
                var width = _maxImage.Width / 4;
                var height = _maxImage.Height;
                var x = 0;
                var y = CloseRect.Y;
                switch (SysButton)
                {
                    case TSysButton.Normal:
                        x = Width - width - CloseRect.Width - _sysButtonPos;
                        break;
                    default:
                        x = -1 * Width;
                        break;
                }
                return new Rectangle(x, y, width, height);
            }
        }

        /// <summary>
        ///     最小化按钮的矩形区域
        /// </summary>
        protected override Rectangle MiniRect
        {
            get
            {
                var width = _minImage.Width / 4;
                var height = _minImage.Height;
                var x = 0;
                var y = CloseRect.Y;
                switch (SysButton)
                {
                    case TSysButton.Normal:
                        x = Width - width - CloseRect.Width - MaxRect.Width - _sysButtonPos;
                        break;
                    case TSysButton.Close_Mini:
                        x = Width - width - CloseRect.Width - _sysButtonPos;
                        break;
                    default:
                        x = -1 * Width;
                        break;
                }

                return new Rectangle(x, y, width, height);
            }
        }

        /// <summary>
        ///     系统按钮的矩形区域
        /// </summary>
        protected override Rectangle SysBtnRect
        {
            get
            {
                if (_sysButton == TSysButton.Normal)
                {
                    var x = TitleBarMenuRect.X;
                    var y = CloseRect.Y;
                    var width = CloseRect.Width + MaxRect.Width +
                                MiniRect.Width + TitleBarMenuRect.Width - _sysButtonPos;
                    var height = CloseRect.Height;
                    return new Rectangle(x, y, width, height);
                }
                if (_sysButton == TSysButton.Close_Mini)
                {
                    var x = TitleBarMenuRect.X;
                    var y = CloseRect.Y;
                    var width = CloseRect.Width + MiniRect.Width +
                                TitleBarMenuRect.Width - _sysButtonPos;
                    var height = CloseRect.Height;
                    return new Rectangle(x, y, width, height);
                }
                else
                {
                    var x = TitleBarMenuRect.X;
                    var y = CloseRect.Y;
                    var width = TitleBarMenuRect.Width + CloseRect.Width;
                    var height = CloseRect.Height;
                    return new Rectangle(x, y, width, height);
                }
            }
        }

        /// <summary>
        ///     标题栏菜单按钮的矩形区域
        /// </summary>
        protected override Rectangle TitleBarMenuRect
        {
            get
            {
                var width = _titleBarMenuImage.Width / 4;
                var height = _titleBarMenuImage.Height;
                var x = 0;
                var y = CloseRect.Y;
                switch (_sysButton)
                {
                    case TSysButton.Normal:
                    case TSysButton.Close_Mini:
                        x = MiniRect.X - width;
                        break;
                    case TSysButton.Close:
                        x = CloseRect.X - width;
                        break;
                }
                return new Rectangle(x, y, width, height);
            }
        }

        /// <summary>
        ///     标题栏菜单按钮的鼠标状态
        /// </summary>
        private TMouseState _titleBarMenuState = TMouseState.Normal;

        /// <summary>
        ///     标题栏菜单按钮的鼠标的状态
        /// </summary>
        protected override TMouseState TitleBarMenuState
        {
            get { return _titleBarMenuState; }
            set
            {
                if (_titleBarMenuState != value)
                {
                    _titleBarMenuState = value;
                    Invalidate(TitleBarMenuRect);
                }
            }
        }

        /// <summary>
        ///     用于展示右键菜单
        /// </summary>
        private readonly Label MenuLabel = new Label();

        /// <summary>
        ///     用于展示右键菜单
        /// </summary>
        [Category("行为"), Description("右键菜单承接按钮。"), DefaultValue(typeof(ContextMenuStrip), null)]
        public ContextMenuStrip ContextMenuShow
        {
            get { return MenuLabel.ContextMenuStrip; }
            set { MenuLabel.ContextMenuStrip = value; }
        }

        #endregion

        #region Override Methods

        /// <summary>
        ///     引发 System.Windows.Forms.Form.Paint 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.PaintEventArgs。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            var rect = ClientRectangle;
            if (ControlBox)
            {
                switch (SysButton)
                {
                    case TSysButton.Normal:
                        DrawSysButton(g, CloseRect, _closeImage, CloseState);
                        if (WindowState != FormWindowState.Maximized)
                            DrawSysButton(g, MaxRect, _maxImage, MaxState);
                        else
                            DrawSysButton(g, MaxRect, _restoreImage, MaxState);
                        DrawSysButton(g, MiniRect, _minImage, MinState);
                        break;
                    case TSysButton.Close:
                        DrawSysButton(g, CloseRect, _closeImage, CloseState);
                        break;
                    case TSysButton.Close_Mini:
                        DrawSysButton(g, CloseRect, _closeImage, CloseState);
                        DrawSysButton(g, MiniRect, _minImage, MinState);
                        break;
                }
                // 绘制标题栏菜单按钮
                DrawSysButton(g, TitleBarMenuRect, (Bitmap)_titleBarMenuImage, _titleBarMenuState);
            }
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseDown。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (TitleBarMenuRect.Contains(e.Location))
            {
                TitleBarMenuState = TMouseState.Down;
            }
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseUp。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (TitleBarMenuRect.Contains(e.Location) && TitleBarMenuState == TMouseState.Down)
            {
                TitleBarMenuState = TMouseState.Up;
                if (MenuClick != null)
                {
                    MenuClick(this, EventArgs.Empty);
                }
                var contextMenuStrip = MenuLabel.ContextMenuStrip;
                if (contextMenuStrip != null)
                {
                    contextMenuStrip.Closed -= contextMenuStrip_Closed;
                    contextMenuStrip.Closed += contextMenuStrip_Closed;
                    var point = PointToScreen(new Point(TitleBarMenuRect.Left + TitleBarMenuRect.Width / 2 - 3,
                        TitleBarMenuRect.Top + TitleBarMenuRect.Height));
                    contextMenuStrip.Show(point);
                }
            }
        }

        private void contextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            Invalidate(TitleBarMenuRect);
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseLeave。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            TitleBarMenuState = TMouseState.Leave;
        }

        #endregion
    }
}