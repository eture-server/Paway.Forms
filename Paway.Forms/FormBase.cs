using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Paway.Win32;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using Paway.Helper;
using Paway.Resource;

namespace Paway.Forms
{
    /// <summary>
    /// 窗体的基类，完成一部分共有的功能
    /// </summary>
    public class FormBase : TForm
    {
        #region 变量
        /// <summary>
        /// 边框图片
        /// </summary>
        private Image _borderImage = AssemblyHelper.GetImage("QQ.FormFrame.fringe_bkg.png");
        /// <summary>
        /// 系统按钮
        /// </summary>
        protected TSysButton _sysButton = TSysButton.Normal;
        /// <summary>
        /// 关闭按钮的鼠标状态
        /// </summary>
        protected TMouseState _closeState = TMouseState.Normal;
        /// <summary>
        /// 最大化按钮的鼠标状态
        /// </summary>
        protected TMouseState _maxState = TMouseState.Normal;
        /// <summary>
        /// 最小化按钮的鼠标状态
        /// </summary>
        protected TMouseState _minState = TMouseState.Normal;
        /// <summary>
        /// 记录窗体大小
        /// </summary>
        protected Size _formSize = Size.Empty;
        /// <summary>
        /// 记录窗体位置
        /// </summary>
        protected Point _formPoint = Point.Empty;
        /// <summary>
        /// 是否允许改变窗口大小
        /// </summary>
        protected bool _isResize = true;
        /// <summary>
        /// 是否显示图标
        /// </summary>
        protected bool _showIcon = true;
        /// <summary>
        /// 是否透明化
        /// </summary>
        private bool _isTransfer = false;
        /// <summary>
        /// 窗体标题文字
        /// </summary>
        private string _textShow;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化 Paway.Forms.FormBase 类的新实例。
        /// </summary>
        public FormBase()
        {
            this.SetStyle(
               ControlStyles.ResizeRedraw |
               ControlStyles.Selectable |
               ControlStyles.ContainerControl, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.UpdateStyles();
            this.Initialize();
            base.Padding = new Padding(1, 26, 1, 1);
            if (IMouseMove)
            {
                this.TMouseMove(this);
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 是否允许改变窗口大小
        /// </summary>
        [Description("是否允许改变窗口大小"), DefaultValue(true)]
        public virtual bool IsResize
        {
            get { return this._isResize; }
            set { _isResize = value; }
        }
        /// <summary>
        /// 指定窗体窗口如何显示
        /// </summary>
        [Description("指定窗体窗口如何显示"), DefaultValue(typeof(FormWindowState), "Normal")]
        public override FormWindowState WindowState
        {
            get { return this._windowState; }
            set
            {
                base.WindowState = value;
                switch (this._windowState)
                {
                    case FormWindowState.Maximized:
                        this._formSize = this.Size;
                        this._formPoint = this.Location;
                        this.Location = new Point(0, 0);
                        this.Width = Screen.PrimaryScreen.WorkingArea.Width;
                        this.Height = Screen.PrimaryScreen.WorkingArea.Height;
                        break;
                }
            }
        }
        /// <summary>
        /// 是否透明
        /// </summary>
        [Description("指定窗体是否透明"), DefaultValue(false)]
        public bool IsTransfer
        {
            get { return this._isTransfer; }
            set { this._isTransfer = value; }
        }
        /// <summary>
        /// 窗体大小的最小值
        /// </summary>
        [Description("窗体大小的最小值"), DefaultValue(typeof(Size), "140, 40")]
        public override Size MinimumSize
        {
            get { return new Size(140, 40); }
            set { base.MinimumSize = value; }
        }
        /// <summary>
        /// 封装创建控件时所需的信息。
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                if (this._isTransfer)
                {
                    param.ExStyle = 0x00080000;
                }
                if (this._sysButton != TSysButton.Close)
                {
                    param.Style = param.Style | (int)WindowStyle.WS_MINIMIZEBOX;   // 允许最小化操作
                }
                return param;
            }
        }
        /// <summary>
        /// 是否显示图标
        /// </summary>
        [Description("是否显示图标"), DefaultValue(true)]
        public virtual new bool ShowIcon
        {
            get { return this._showIcon; }
            set
            {
                if (this._showIcon != value)
                {
                    this._showIcon = value;
                    base.Invalidate(this.TitleBarRect);
                }
            }
        }
        /// <summary>
        /// 窗体标题文字
        /// </summary>
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    base.Invalidate(this.TitleBarRect);
                }
            }
        }
        /// <summary>
        /// 窗体显示的标题文字
        /// </summary>
        public string TextShow
        {
            get { return _textShow; }
            set
            {
                if (this._textShow != value)
                {
                    this._textShow = value;
                    base.Invalidate(this.TitleBarRect);
                }
            }
        }

        /// <summary>
        /// 系统控制按钮
        /// </summary>
        [Description("系统控制按钮的显示与隐藏"), DefaultValue(typeof(TSysButton), "Normal")]
        public TSysButton SysButton
        {
            get { return this._sysButton; }
            set
            {
                if (this._sysButton != value)
                {
                    this._sysButton = value;
                    base.Invalidate(this.TitleBarRect);
                }
            }
        }
        /// <summary>
        /// 系统控制按钮区域
        /// </summary>
        [Description("系统控制按钮区域"), DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle SysBtnRect
        {
            get { return Rectangle.Empty; }
        }
        /// <summary>
        /// 标题栏区域
        /// </summary>
        protected virtual Rectangle TitleBarRect
        {
            get { return new Rectangle(0, 0, this.Width, 30); }
        }
        /// <summary>
        /// 关闭按钮区域
        /// </summary>
        [Description("关闭按钮区域"), DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle CloseRect
        {
            get { return Rectangle.Empty; }
        }
        /// <summary>
        /// 最小化按钮区域
        /// </summary>
        [Description("最小化按钮区域"), DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle MiniRect
        {
            get { return Rectangle.Empty; }
        }
        /// <summary>
        /// 最大化按钮区域
        /// </summary>
        [Description("最大化按钮区域"), DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle MaxRect
        {
            get { return Rectangle.Empty; }
        }
        /// <summary>
        /// 图标显示区域
        /// </summary>
        [Description("图标显示区域"), DefaultValue(typeof(Rectangle), "4, 4, 16, 16")]
        protected virtual Rectangle IconRect
        {
            get { return new Rectangle(4, 4, 16, 16); }
        }
        /// <summary>
        /// 标题文本显示区域
        /// </summary>
        protected virtual Rectangle TextRect
        {
            get
            {
                int width = this.TitleBarRect.Width - this.IconRect.Width - 15;
                int height = this.TitleBarRect.Height - 10;
                Rectangle textRect = new Rectangle(8, 2, width, height);
                if (this.ShowIcon)
                    textRect.X = this.IconRect.Width + 8;
                return textRect;
            }
        }
        /// <summary>
        /// 关闭按钮当前的鼠标状态
        /// </summary>
        [Description("关闭按钮当前的鼠标状态"), DefaultValue(typeof(TSysButton), "Normal")]
        protected TMouseState CloseState
        {
            get { return this._closeState; }
            set
            {
                if (this._closeState != value)
                {
                    this._closeState = value;
                    base.Invalidate(this.CloseRect);
                }
            }
        }
        /// <summary>
        /// 最大化按钮当前的鼠标状态
        /// </summary>
        [Description("最大化按钮当前的鼠标状态"), DefaultValue(typeof(TSysButton), "Normal")]
        protected TMouseState MaxState
        {
            get { return this._maxState; }
            set
            {
                if (this._maxState != value)
                {
                    this._maxState = value;
                    base.Invalidate(this.MaxRect);
                }
            }
        }
        /// <summary>
        /// 最小化按钮当前的鼠标状态
        /// </summary>
        [Description("最小化按钮当前的鼠标状态"), DefaultValue(typeof(TSysButton), "Normal")]
        protected TMouseState MinState
        {
            get { return this._minState; }
            set
            {
                if (this._minState != value)
                {
                    this._minState = value;
                    base.Invalidate(this.MiniRect);
                }
            }
        }

        /// <summary>
        /// 是否绘制边框
        /// </summary>
        protected bool _isDrawBorder = false;
        /// <summary>
        /// 是否绘制边框
        /// </summary>
        [Browsable(true), Description("是否绘制边框"), DefaultValue(false)]
        public bool IsDrawBorder
        {
            get { return _isDrawBorder; }
            set
            {
                _isDrawBorder = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否剪成圆角
        /// </summary>
        protected bool _isDrawRound = true;
        /// <summary>
        /// 是否剪成圆角
        /// </summary>
        [Browsable(true), Description("是否剪成圆角"), DefaultValue(true)]
        public bool IsDrawRound
        {
            get { return _isDrawRound; }
            set
            {
                _isDrawRound = value;
                this.OnSizeChanged(EventArgs.Empty);
            }
        }
        #endregion

        #region 方法

        /// <summary>
        /// 初始化窗口
        /// </summary>
        private void Initialize()
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }

        /// <summary>
        /// 拖动窗口大小
        /// </summary>
        /// <param name="m"></param>
        public override void WmNcHitTest(ref Message m)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                int wparam = m.LParam.ToInt32();

                Point point = new Point(
                    NativeMethods.LOWORD(wparam),
                    NativeMethods.HIWORD(wparam));

                point = PointToClient(point);
                if (_isResize)
                {
                    if (point.X <= 3)
                    {
                        if (point.Y <= 3)
                            m.Result = (IntPtr)WM_NCHITTEST.HTTOPLEFT;
                        else if (point.Y > Height - 3)
                            m.Result = (IntPtr)WM_NCHITTEST.HTBOTTOMLEFT;
                        else
                            m.Result = (IntPtr)WM_NCHITTEST.HTLEFT;
                    }
                    else if (point.X >= Width - 3)
                    {
                        if (point.Y <= 3)
                            m.Result = (IntPtr)WM_NCHITTEST.HTTOPRIGHT;
                        else if (point.Y >= Height - 3)
                            m.Result = (IntPtr)WM_NCHITTEST.HTBOTTOMRIGHT;
                        else
                            m.Result = (IntPtr)WM_NCHITTEST.HTRIGHT;
                    }
                    else if (point.Y <= 3)
                    {
                        m.Result = (IntPtr)WM_NCHITTEST.HTTOP;
                    }
                    else if (point.Y >= Height - 3)
                    {
                        m.Result = (IntPtr)WM_NCHITTEST.HTBOTTOM;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
        }

        /// <summary>
        /// 设置图片为窗体，透明区域根据 opacity 的值决定透明度
        /// </summary>
        /// <param name="bitmap">透明位图</param>
        /// <param name="opacity">透明度的值0~255</param>
        public void SetBitmap(Bitmap bitmap, byte opacity)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

            // The ideia of this is very simple,
            // 1. Create a compatible DC with screen;
            // 2. Select the bitmap with 32bpp with alpha-channel in the compatible DC;
            // 3. Call the UpdateLayeredWindow.

            IntPtr screenDc = NativeMethods.GetDC(IntPtr.Zero);
            IntPtr memDc = NativeMethods.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  // grab a GDI handle from this GDI+ bitmap
                oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

                SIZE size = new SIZE(bitmap.Width, bitmap.Height);
                POINT pointSource = new POINT(0, 0);
                POINT topPos = new POINT(Left, Top);
                BLENDFUNCTION blend = new BLENDFUNCTION();
                blend.BlendOp = Consts.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = Consts.AC_SRC_ALPHA;
                if (!this.IsDisposed)
                {
                    NativeMethods.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Consts.ULW_ALPHA);
                }
            }
            finally
            {
                NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    NativeMethods.SelectObject(memDc, oldBitmap);
                    //Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.
                    NativeMethods.DeleteObject(hBitmap);
                }
                NativeMethods.DeleteDC(memDc);
            }
        }

        #endregion

        #region Override Methods
        /// <summary>
        /// 引发 System.Windows.Forms.Form.Paint 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.PaintEventArgs。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            if (this.ShowIcon)//绘画图标
            {
                Bitmap iconImage = this.Icon.ToBitmap();
                g.DrawImage(iconImage, this.IconRect);
            }
            //绘制标题文字
            if (!string.IsNullOrEmpty(this._textShow))
            {
                TextRenderer.DrawText(g, this._textShow, new Font("宋体", 9f, FontStyle.Bold), this.TextRect, this.ForeColor, TextFormatFlags.VerticalCenter);
            }
            DrawFrameBorder(g);
        }
        /// <summary>
        /// 绘制窗体边框
        /// </summary>
        /// <param name="g"></param>
        private void DrawFrameBorder(Graphics g)
        {
            //绘画边框
            if (_isDrawBorder)
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    //左边
                    g.DrawImage(this._borderImage, new Rectangle(0, 0, 1, this.Height), new Rectangle(5, 5 + 3, 1, 1), GraphicsUnit.Pixel);
                    //右边
                    g.DrawImage(this._borderImage, new Rectangle(this.Width - 1, 0, 1, this.Height), new Rectangle(5, 8, 1, 1), GraphicsUnit.Pixel);
                    //上边
                    g.DrawImage(this._borderImage, new Rectangle(0, 0, this.Width, 1), new Rectangle(8, 5, 6, 1), GraphicsUnit.Pixel);
                    //下边
                    g.DrawImage(this._borderImage, new Rectangle(0, this.Height - 1, this.Width, 1), new Rectangle(8, 5, 6, 1), GraphicsUnit.Pixel);
                }
                else
                {
                    //左上
                    g.DrawImage(this._borderImage, new Rectangle(0, 0, 3, 3), new Rectangle(this._borderImage.Width - 3 - 5, this._borderImage.Height - 5 - 3, 3, 3), GraphicsUnit.Pixel);
                    //右下
                    g.DrawImage(this._borderImage, new Rectangle(this.Width - 3, this.Height - 3, 3, 3), new Rectangle(5, 5, 3, 3), GraphicsUnit.Pixel);
                    //左下
                    g.DrawImage(this._borderImage, new Rectangle(0, this.Height - 3, 3, 3), new Rectangle(this._borderImage.Width - 3 - 5, 5, 3, 3), GraphicsUnit.Pixel);
                    //右上
                    g.DrawImage(this._borderImage, new Rectangle(this.Width - 3, 0, 3, 3), new Rectangle(5, this._borderImage.Height - 5 - 3, 3, 3), GraphicsUnit.Pixel);
                    //左边
                    g.DrawImage(this._borderImage, new Rectangle(0, 2, 1, this.Height - 4), new Rectangle(5, 8, 1, 1), GraphicsUnit.Pixel);
                    //右边
                    g.DrawImage(this._borderImage, new Rectangle(this.Width - 1, 2, 1, this.Height - 4), new Rectangle(5, 8, 1, 1), GraphicsUnit.Pixel);
                    //上边
                    g.DrawImage(this._borderImage, new Rectangle(2, 0, this.Width - 4, 1), new Rectangle(8, 5, 6, 1), GraphicsUnit.Pixel);
                    //下边
                    g.DrawImage(this._borderImage, new Rectangle(2, this.Height - 1, this.Width - 4, 1), new Rectangle(8, 5, 6, 1), GraphicsUnit.Pixel);
                }
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.Resize 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //调用API，将窗体剪成圆角
            int ellipse = (_isDrawRound && this.WindowState != FormWindowState.Maximized) ? base.TRadius : 0;
            int rgn = NativeMethods.CreateRoundRectRgn(0, 0, this.Width + 1, this.Height + 1, ellipse, ellipse);
            if (!this.IsDisposed)
            {
                NativeMethods.SetWindowRgn(this.Handle, rgn, true);
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.Load 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.OnSizeChanged(e);
            if (!this.DesignMode)
            {
                switch (this.StartPosition)
                {
                    case FormStartPosition.CenterParent:
                        this.Location = new Point(
                           (this.Parent.Width - this.Width) / 2,
                           (this.Parent.Height - this.Height) / 2);
                        break;
                    case FormStartPosition.CenterScreen:
                        this.Location = new Point(
                            (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                            (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
                        break;
                    case FormStartPosition.Manual:
                    case FormStartPosition.WindowsDefaultBounds:
                    case FormStartPosition.WindowsDefaultLocation:
                        break;
                }
            }
        }
        /// <summary>
        /// 处理 Windows 消息。
        /// </summary>
        /// <param name="m">要处理的 WindowsMessage。</param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WindowsMessage.WM_NCPAINT:
                case (int)WindowsMessage.WM_NCCALCSIZE:
                    break;
                case (int)WindowsMessage.WM_NCHITTEST:
                    base.WndProc(ref m);
                    this.WmNcHitTest(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
            if (this._windowState != FormWindowState.Minimized)
            {
                if (m.Msg == (int)WindowsMessage.WM_NCACTIVATE)
                {
                    if (m.WParam == Win32Helper.False)
                    {
                        m.Result = Win32Helper.True;
                    }
                }
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseDown。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;

            Point point = e.Location;
            if (this.CloseRect.Contains(point))
                this.CloseState = TMouseState.Down;
            else if (this.MiniRect.Contains(point))
                this.MinState = TMouseState.Down;
            else if (this.MaxRect.Contains(point))
                this.MaxState = TMouseState.Down;
            if (this._sysButton == TSysButton.Normal && e.Clicks == 2)
            {
                WindowMax();
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseMove。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
                return;
            bool flag = true;
            string tipText = null;
            Point point = e.Location;
            if (this.CloseRect.Contains(point))
            {
                flag = false;
                if (CloseState != TMouseState.Move)
                {
                    this.CloseState = TMouseState.Move;
                    tipText = "关闭";
                }
            }
            else
                this.CloseState = TMouseState.Normal;
            if (this.MiniRect.Contains(point))
            {
                flag = false;
                if (MinState != TMouseState.Move)
                {
                    this.MinState = TMouseState.Move;
                    tipText = "最小化";
                }
            }
            else
                this.MinState = TMouseState.Normal;
            if (this.MaxRect.Contains(point))
            {
                flag = false;
                if (MaxState != TMouseState.Move)
                {
                    this.MaxState = TMouseState.Move;
                    tipText = (this.WindowState == FormWindowState.Maximized) ? "还原" : "最大化";
                }
            }
            else
            {
                this.MaxState = TMouseState.Normal;
            }
            if (tipText != null)
            {
                this.ShowTooTip(tipText);
            }
            if (flag)
            {
                this.HideToolTip();
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseLeave。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.CloseState = TMouseState.Normal;
            this.MaxState = TMouseState.Normal;
            this.MinState = TMouseState.Normal;
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseUp。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Left)
                return;
            Point point = e.Location;
            if (this.CloseRect.Contains(point))
            {
                this.CloseState = TMouseState.Move;
                this.Close();
            }
            else
            {
                this.CloseState = TMouseState.Normal;
            }
            if (this.MiniRect.Contains(point))
            {
                this.MinState = TMouseState.Move;
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                this.MinState = TMouseState.Normal;
            }
            if (this.MaxRect.Contains(point))
            {
                this.MaxState = TMouseState.Move;
                WindowMax();
            }
            else
            {
                this.MaxState = TMouseState.Normal;
            }
        }
        /// <summary>
        /// 最大化方法
        /// </summary>
        protected void WindowMax()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = this._formSize;
                this.Location = this._formPoint;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        #endregion

        #region 绘制下圆角路径
        /// <summary>
        /// 绘制下圆角路径
        /// </summary>
        /// <param name="control"></param>
        protected void TDrawBelowPath(Control control)
        {
            if (control == null) return;
            control.Paint += control_PaintDP;
        }
        void control_PaintDP(object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            Graphics g = control.CreateGraphics();
            DrawHelper.CreateBelowPath(g, control.ClientRectangle, this.BackColor);
        }

        #endregion

        #region 绘制下圆角边框
        /// <summary>
        /// 绘制下圆角边框
        /// </summary>
        /// <param name="control"></param>
        protected void TDrawBelowBorder(Control control)
        {
            if (!_isDrawBorder || control == null || _borderImage == null) return;
            control.Paint += control_PaintDB;
        }
        void control_PaintDB(object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            Graphics g = control.CreateGraphics();

            if (this.WindowState == FormWindowState.Maximized)
            {
                //左边
                g.DrawImage(this._borderImage, new Rectangle(0, 0, 1, control.Height), new Rectangle(5, 8, 1, 1), GraphicsUnit.Pixel);
                //右边
                g.DrawImage(this._borderImage, new Rectangle(control.Width - 1, 0, 1, control.Height), new Rectangle(5, 8, 1, 1), GraphicsUnit.Pixel);
                //上边
                g.DrawImage(this._borderImage, new Rectangle(0, 0, control.Width, 1), new Rectangle(8, 5, 6, 1), GraphicsUnit.Pixel);
                //下边
                g.DrawImage(this._borderImage, new Rectangle(0, control.Height - 1, control.Width, 1), new Rectangle(8, 5, 6, 1), GraphicsUnit.Pixel);
            }
            else
            {
                //右下
                g.DrawImage(this._borderImage, new Rectangle(control.Width - 3, control.Height - 3, 3, 3), new Rectangle(5, 5, 3, 3), GraphicsUnit.Pixel);
                //左下
                g.DrawImage(this._borderImage, new Rectangle(0, control.Height - 3, 3, 3), new Rectangle(this._borderImage.Width - 3 - 5, 5, 3, 3), GraphicsUnit.Pixel);
                //左边
                g.DrawImage(this._borderImage, new Rectangle(0, 0, 1, control.Height - 2), new Rectangle(5, 8, 1, 1), GraphicsUnit.Pixel);
                //右边
                g.DrawImage(this._borderImage, new Rectangle(control.Width - 1, 0, 1, control.Height - 2), new Rectangle(5, 8, 1, 1), GraphicsUnit.Pixel);
                //下边
                g.DrawImage(this._borderImage, new Rectangle(2, control.Height - 1, control.Width - 4, 1), new Rectangle(8, 5, 6, 1), GraphicsUnit.Pixel);
            }
        }

        #endregion

        #region 将控件剪成圆角
        private int raw;
        /// <summary>
        /// 将控件剪成圆角
        /// </summary>
        protected void TDrawRoundRect(Control control, int raw)
        {
            if (control == null) return;
            this.raw = raw;
            control.SizeChanged += control_SizeChanged;
            control_SizeChanged(control, EventArgs.Empty);
        }

        void control_SizeChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            int rgn = NativeMethods.CreateRoundRectRgn(control.ClientRectangle.X, control.ClientRectangle.Y,
                control.ClientRectangle.Width + 1 - control.ClientRectangle.X,
                control.ClientRectangle.Height + 1 - control.ClientRectangle.Y,
                raw, raw);
            if (!control.IsDisposed)
            {
                NativeMethods.SetWindowRgn(control.Handle, rgn, true);
            }
        }
        /// <summary>
        /// 将控件剪成圆角
        /// </summary>
        /// <param name="control"></param>
        /// <param name="ellipse"></param>
        protected void DrawRoundRect(Control control, int ellipse)
        {
            if (control == null) return;

            int rgn = NativeMethods.CreateRoundRectRgn(control.ClientRectangle.X, control.ClientRectangle.Y,
                control.ClientRectangle.Width + 1 - control.ClientRectangle.X,
                control.ClientRectangle.Height + 1 - control.ClientRectangle.Y,
                ellipse, ellipse);
            if (!control.IsDisposed)
            {
                NativeMethods.SetWindowRgn(control.Handle, rgn, true);
            }

        }
        /// <summary>
        /// 将控件剪成圆角
        /// </summary>
        /// <param name="control"></param>
        /// <param name="rect"></param>
        /// <param name="ellipse"></param>
        protected void DrawRoundRect(Control control, Rectangle rect, int ellipse)
        {
            if (control == null) return;

            int rgn = NativeMethods.CreateRoundRectRgn(rect.X, rect.Y, rect.Width + 1 - rect.X, rect.Height + 1 - rect.Y, ellipse, ellipse);
            if (!control.IsDisposed)
            {
                NativeMethods.SetWindowRgn(control.Handle, rgn, true);
            }
        }
        #endregion

        #region 接口
        /// <summary>
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Contain(Point p)
        {
            return this.SysBtnRect.Contains(p);
        }

        #endregion
    }
}
