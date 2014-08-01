using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Paway.Win32;
using Paway.Resource;

namespace Paway.Forms
{
    /// <summary>
    /// 360 窗体
    /// </summary>
    public class _360Form : FormBase
    {
        #region 资源图片
        /// <summary>
        /// 边框图片
        /// </summary>
        private Image _borderImage = AssemblyHelper.GetImage("_360.Form.framemod.png");
        /// <summary>
        /// 关闭按钮图片
        /// </summary>
        private Image _closeImage = AssemblyHelper.GetImage("_360.SysButton.sys_button_close.png");
        /// <summary>
        /// 最小化按钮图片
        /// </summary>
        private Image _minImage = AssemblyHelper.GetImage("_360.SysButton.sys_button_min.png");
        /// <summary>
        /// 最大化按钮图片
        /// </summary>
        private Image _maxImage = AssemblyHelper.GetImage("_360.SysButton.sys_button_max.png");
        /// <summary>
        /// 还原按钮图片
        /// </summary>
        private Image _restoreImage = AssemblyHelper.GetImage("_360.SysButton.sys_button_restore.png");
        /// <summary>
        /// 标题栏菜单按钮图片
        /// </summary>
        private Image _titleBarMenuImage = AssemblyHelper.GetImage("_360.SysButton.title_bar_menu.png");

        #endregion

        #region 构造函数
        /// <summary>
        /// 实例化 Paway.Forms._360form 新的实例。
        /// </summary>
        public _360Form() : base() { }

        #endregion

        #region 属性
        /// <summary>
        /// 系统按钮与窗体右边边缘的间距
        /// </summary>
        private int _sysButtonPos = 4;
        /// <summary>
        /// 系统控制按钮与右边框之间的距离
        /// </summary>
        [Description("系统控制按钮与右边框之间的距离"), DefaultValue(4)]
        public int SysButtonPos
        {
            get { return this._sysButtonPos; }
            set
            {
                if (this._sysButtonPos != value)
                {
                    this._sysButtonPos = value;
                    this.Invalidate(this.SysBtnRect);
                }
            }
        }
        /// <summary>
        /// 关闭按钮的矩形区域
        /// </summary>
        protected override Rectangle CloseRect
        {
            get
            {
                int width = this._closeImage.Width / 4;
                int height = this._closeImage.Height;
                int x = this.Width - width - this._sysButtonPos;
                int y = -1;

                return new Rectangle(x, y, width, height);
            }
        }
        /// <summary>
        /// 最大化按钮的矩形区域
        /// </summary>
        protected override Rectangle MaxRect
        {
            get
            {
                int width = this._maxImage.Width / 4;
                int height = this._maxImage.Height;
                int x = 0;
                int y = this.CloseRect.Y;
                switch (base.SysButton)
                {
                    case TSysButton.Normal:
                        x = this.Width - width - this.CloseRect.Width - this._sysButtonPos;
                        break;
                }
                return new Rectangle(x, y, width, height);
            }
        }
        /// <summary>
        /// 最小化按钮的矩形区域
        /// </summary>
        protected override Rectangle MiniRect
        {
            get
            {
                int width = this._minImage.Width / 4;
                int height = this._minImage.Height;
                int x = 0;
                int y = this.CloseRect.Y;
                switch (base.SysButton)
                {
                    case TSysButton.Normal:
                        x = this.Width - width - this.CloseRect.Width - this.MaxRect.Width - this._sysButtonPos;
                        break;
                    case TSysButton.Close_Mini:
                        x = this.Width - width - this.CloseRect.Width - this._sysButtonPos;
                        break;
                }

                return new Rectangle(x, y, width, height);
            }
        }
        /// <summary>
        /// 系统按钮的矩形区域
        /// </summary>
        protected override Rectangle SysBtnRect
        {
            get
            {
                if (base._sysButton == TSysButton.Normal)
                {
                    int x = this.TitleBarMenuRect.X;
                    int y = this.CloseRect.Y;
                    int width = this.CloseRect.Width + this.MaxRect.Width +
                        this.MiniRect.Width + this.TitleBarMenuRect.Width - this._sysButtonPos;
                    int height = this.CloseRect.Height;
                    return new Rectangle(x, y, width, height);
                }
                else if (base._sysButton == TSysButton.Close_Mini)
                {
                    int x = this.TitleBarMenuRect.X;
                    int y = this.CloseRect.Y;
                    int width = this.CloseRect.Width + this.MiniRect.Width +
                        this.TitleBarMenuRect.Width - this._sysButtonPos;
                    int height = this.CloseRect.Height;
                    return new Rectangle(x, y, width, height);
                }
                else
                {
                    int x = this.TitleBarMenuRect.X;
                    int y = this.CloseRect.Y;
                    int width = this.TitleBarMenuRect.Width + this.CloseRect.Width;
                    int height = this.CloseRect.Height;
                    return new Rectangle(x, y, width, height);
                }
            }
        }
        /// <summary>
        /// 标题栏菜单按钮的矩形区域
        /// </summary>
        protected override Rectangle TitleBarMenuRect
        {
            get
            {
                int width = this._titleBarMenuImage.Width / 4;
                int height = this._titleBarMenuImage.Height;
                int x = 0;
                int y = this.CloseRect.Y;
                switch (base._sysButton)
                {
                    case TSysButton.Normal:
                    case TSysButton.Close_Mini:
                        x = this.MiniRect.X - width;
                        break;
                    case TSysButton.Close:
                        x = this.CloseRect.X - width;
                        break;
                }
                return new Rectangle(x, y, width, height);
            }
        }
        /// <summary>
        /// 标题栏菜单按钮的鼠标状态
        /// </summary>
        private TMouseState _titleBarMenuState = TMouseState.Normal;
        /// <summary>
        /// 标题栏菜单按钮的鼠标的状态
        /// </summary>
        protected override TMouseState TitleBarMenuState
        {
            get { return this._titleBarMenuState; }
            set
            {
                if (this._titleBarMenuState != value)
                {
                    this._titleBarMenuState = value;
                    this.Invalidate(this.TitleBarMenuRect);
                }
            }
        }
        /// <summary>
        /// 用于展示右键菜单
        /// </summary>
        private Label MenuLabel = new Label();
        /// <summary>
        /// 用于展示右键菜单
        /// </summary>
        [Category("行为"), Description("右键菜单承接按钮。"), DefaultValue(typeof(ContextMenuStrip), null)]
        public ContextMenuStrip ContextMenuShow
        {
            get { return MenuLabel.ContextMenuStrip; }
            set { MenuLabel.ContextMenuStrip = value; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 绘制窗体的系统控制按钮
        /// </summary>
        /// <param name="g">画板</param>
        /// <param name="rect">按钮所在的区域</param>
        /// <param name="image">图片</param>
        /// <param name="state">鼠标状态</param>
        private void DrawSysButton(Graphics g, Rectangle rect, Image image, TMouseState state)
        {
            Rectangle imageRect = Rectangle.Empty;
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
        /// 菜单按钮单击事件
        /// </summary>
        public event EventHandler MenuClick;

        #endregion

        #region Override Methods
        /// <summary>
        /// 引发 System.Windows.Forms.Form.Paint 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.PaintEventArgs。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = this.ClientRectangle;
            if (ControlBox)
            {
                switch (base.SysButton)
                {
                    case TSysButton.Normal:
                        this.DrawSysButton(g, this.CloseRect, this._closeImage, base.CloseState);
                        if (base.WindowState != FormWindowState.Maximized)
                            this.DrawSysButton(g, this.MaxRect, this._maxImage, base.MaxState);
                        else
                            this.DrawSysButton(g, this.MaxRect, this._restoreImage, base.MaxState);
                        this.DrawSysButton(g, this.MiniRect, this._minImage, base.MinState);
                        break;
                    case TSysButton.Close:
                        this.DrawSysButton(g, this.CloseRect, this._closeImage, base.CloseState);
                        break;
                    case TSysButton.Close_Mini:
                        this.DrawSysButton(g, this.CloseRect, this._closeImage, base.CloseState);
                        this.DrawSysButton(g, this.MiniRect, this._minImage, base.MinState);
                        break;
                }
                // 绘制标题栏菜单按钮
                this.DrawSysButton(g, this.TitleBarMenuRect, (Bitmap)this._titleBarMenuImage, this._titleBarMenuState);
            }

            base.OnPaint(e);
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseDown。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.TitleBarMenuRect.Contains(e.Location))
            {
                this.TitleBarMenuState = TMouseState.Down;
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseUp。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.TitleBarMenuRect.Contains(e.Location) && this.TitleBarMenuState == TMouseState.Down)
            {
                this.TitleBarMenuState = TMouseState.Up;
                if (MenuClick != null)
                {
                    MenuClick(this, EventArgs.Empty);
                }
                ContextMenuStrip contextMenuStrip = this.MenuLabel.ContextMenuStrip;
                if (contextMenuStrip != null)
                {
                    contextMenuStrip.Closed -= contextMenuStrip_Closed;
                    contextMenuStrip.Closed += contextMenuStrip_Closed;
                    Point point = this.PointToScreen(new Point(this.TitleBarMenuRect.Left + this.TitleBarMenuRect.Width / 2 - 3,
                        this.TitleBarMenuRect.Top + this.TitleBarMenuRect.Height));
                    contextMenuStrip.Show(point);
                }
            }
        }
        void contextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            base.Invalidate(this.TitleBarMenuRect);
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseLeave。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.TitleBarMenuState = TMouseState.Leave;
        }
        #endregion
    }
}
