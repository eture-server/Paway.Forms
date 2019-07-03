using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Win32;
using System.Reflection;
using Paway.Forms.Properties;

namespace Paway.Forms
{
    /// <summary>
    ///     窗体的基类，完成一部分共有的功能
    /// </summary>
    public class FormBase : TForm
    {
        #region 事件
        /// <summary>
        /// 关于
        /// </summary>
        public static event EventHandler AboutEvent;

        #endregion
        #region 构造函数

        /// <summary>
        ///     初始化 Paway.Forms.FormBase 类的新实例。
        /// </summary>
        public FormBase()
        {
            Initialize();
            Padding = new Padding(1, 26, 1, 1);
            if (IMouseMove) TMouseMove(this);
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.None;
            ForeColor = Color.White;
        }

        #endregion

        #region 接口

        /// <summary>
        ///     坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Contain(Point p)
        {
            return SysBtnRect.Contains(p);
        }

        #endregion

        #region 变量

        /// <summary>
        ///     边框图片
        /// </summary>
        private readonly Image _borderImage = Resources.QQ_FormFrame_fringe_bkg;

        /// <summary>
        ///     系统按钮
        /// </summary>
        protected TSysButton _sysButton = TSysButton.Normal;

        /// <summary>
        ///     关闭按钮的鼠标状态
        /// </summary>
        protected TMouseState _closeState = TMouseState.Normal;

        /// <summary>
        ///     最大化按钮的鼠标状态
        /// </summary>
        protected TMouseState _maxState = TMouseState.Normal;

        /// <summary>
        ///     最小化按钮的鼠标状态
        /// </summary>
        protected TMouseState _minState = TMouseState.Normal;

        /// <summary>
        ///     记录窗体Normal大小
        /// </summary>
        protected Size? _normalSize;
        /// <summary>
        ///     记录窗体Restore大小
        /// </summary>
        protected Size? _restoreSize;

        /// <summary>
        ///     记录窗体Normal位置
        /// </summary>
        protected Point? _normalPoint;
        /// <summary>
        ///     记录窗体Restore位置
        /// </summary>
        protected Point? _restorePoint;

        /// <summary>
        ///     是否显示图标
        /// </summary>
        protected bool _showIcon = true;

        private Color _tranColor;

        /// <summary>
        ///     绘制背景时自动颜色透明度
        /// </summary>
        protected Color TranColor
        {
            get
            {
                if (_tranColor.A > Trans)
                {
                    _tranColor = Color.FromArgb(Trans, _tranColor.R, _tranColor.G, _tranColor.B);
                }
                return _tranColor;
            }
            set { _tranColor = value; }
        }

        #endregion

        #region 属性
        /// <summary>
        /// 记录上一次窗体状态，用于还原
        /// </summary>
        private FormWindowState lastState;

        /// <summary>
        ///     指定窗体窗口如何显示
        /// </summary>
        [Description("指定窗体窗口如何显示")]
        [DefaultValue(FormWindowState.Normal)]
        public override FormWindowState WindowState
        {
            get { return _windowState; }
            set
            {
                bool iSame = base.WindowState == value;
                if (!iSame)
                    lastState = base.WindowState;
                switch (value)
                {
                    case FormWindowState.Minimized:
                        _restoreSize = Size;
                        _restorePoint = Location;
                        break;
                }
                base.WindowState = value;
                if (iSame) return;
                ShowSysMenu();
                switch (_windowState)
                {
                    case FormWindowState.Normal:
                        Size = _normalSize ?? _restoreSize ?? Size;
                        Location = _normalPoint ?? _restorePoint ?? Location;
                        break;
                }
                if (lastState == FormWindowState.Minimized) return;
                switch (_windowState)
                {
                    case FormWindowState.Maximized:
                        _normalSize = Size;
                        _normalPoint = Location;
                        Size = Screen.PrimaryScreen.WorkingArea.Size;
                        Location = Point.Empty;
                        break;
                }
            }
        }

        /// <summary>
        ///     是否透明
        /// </summary>
        [Description("指定窗体是否透明")]
        [DefaultValue(false)]
        public bool ITransfer { get; set; }

        /// <summary>
        ///     窗体大小的最小值
        /// </summary>
        [Description("窗体大小的最小值")]
        [DefaultValue(typeof(Size), "140, 40")]
        public override Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        /// <summary>
        ///     是否显示图标
        /// </summary>
        [Description("是否显示图标")]
        [DefaultValue(true)]
        public new virtual bool ShowIcon
        {
            get { return _showIcon; }
            set
            {
                if (_showIcon != value)
                {
                    _showIcon = value;
                    Invalidate(TitleBarRect);
                }
            }
        }

        /// <summary>
        ///     窗体标题文字
        /// </summary>
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    Invalidate(TitleBarRect);
                }
            }
        }

        private string _textShow;
        /// <summary>
        ///     窗体显示的标题文字
        /// </summary>
        [Browsable(false), Description("窗体显示的标题文字")]
        [DefaultValue(null)]
        public string TextShow
        {
            get { return _textShow; }
            set
            {
                if (_textShow != value)
                {
                    _textShow = value;
                    Invalidate(TitleBarRect);
                }
            }
        }

        /// <summary>
        ///     系统控制按钮
        /// </summary>
        [Description("系统控制按钮的显示与隐藏")]
        [DefaultValue(TSysButton.Normal)]
        public TSysButton SysButton
        {
            get { return _sysButton; }
            set
            {
                if (_sysButton != value)
                {
                    _sysButton = value;
                    Invalidate(TitleBarRect);
                }
            }
        }

        /// <summary>
        ///     系统控制按钮区域
        /// </summary>
        [Description("系统控制按钮区域")]
        [DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle SysBtnRect
        {
            get { return Rectangle.Empty; }
        }

        /// <summary>
        ///     标题栏区域
        /// </summary>
        protected virtual Rectangle TitleBarRect
        {
            get { return new Rectangle(0, 0, Width, 30); }
        }

        /// <summary>
        ///     关闭按钮区域
        /// </summary>
        [Description("关闭按钮区域")]
        [DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle CloseRect
        {
            get { return Rectangle.Empty; }
        }

        /// <summary>
        ///     最小化按钮区域
        /// </summary>
        [Description("最小化按钮区域")]
        [DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle MiniRect
        {
            get { return Rectangle.Empty; }
        }

        /// <summary>
        ///     最大化按钮区域
        /// </summary>
        [Description("最大化按钮区域")]
        [DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle MaxRect
        {
            get { return Rectangle.Empty; }
        }

        /// <summary>
        ///     标题栏菜单按钮的矩形区域
        /// </summary>
        [Description("标题栏菜单按钮的矩形区域")]
        [DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle TitleBarMenuRect
        {
            get { return Rectangle.Empty; }
        }

        /// <summary>
        ///     图标显示区域
        /// </summary>
        [Description("图标显示区域")]
        [DefaultValue(typeof(Rectangle), "4, 4, 16, 16")]
        protected virtual Rectangle IconRect
        {
            get { return new Rectangle(4, 4, 16, 16); }
        }

        /// <summary>
        ///     标题文本显示区域
        /// </summary>
        protected virtual Rectangle TextRect
        {
            get
            {
                var width = TitleBarRect.Width - IconRect.Width - 15;
                var height = TitleBarRect.Height - 10;
                var textRect = new Rectangle(8, 2, width, height);
                if (ShowIcon)
                    textRect.X = IconRect.Width + 8;
                return textRect;
            }
        }

        /// <summary>
        ///     关闭按钮当前的鼠标状态
        /// </summary>
        [Description("关闭按钮当前的鼠标状态")]
        [DefaultValue(TSysButton.Normal)]
        protected TMouseState CloseState
        {
            get { return _closeState; }
            set
            {
                if (_closeState != value)
                {
                    _closeState = value;
                    Invalidate(CloseRect);
                }
            }
        }

        /// <summary>
        ///     最大化按钮当前的鼠标状态
        /// </summary>
        [Description("最大化按钮当前的鼠标状态")]
        [DefaultValue(TSysButton.Normal)]
        protected TMouseState MaxState
        {
            get { return _maxState; }
            set
            {
                if (_maxState != value)
                {
                    _maxState = value;
                    Invalidate(MaxRect);
                }
            }
        }

        /// <summary>
        ///     最小化按钮当前的鼠标状态
        /// </summary>
        [Description("最小化按钮当前的鼠标状态")]
        [DefaultValue(TSysButton.Normal)]
        protected TMouseState MinState
        {
            get { return _minState; }
            set
            {
                if (_minState != value)
                {
                    _minState = value;
                    Invalidate(MiniRect);
                }
            }
        }

        /// <summary>
        ///     标题栏菜单按钮的鼠标的状态
        /// </summary>
        [Description("标题栏菜单按钮的鼠标的状态")]
        [DefaultValue(TMouseState.Normal)]
        protected virtual TMouseState TitleBarMenuState { get; set; }

        private bool _iBorder;
        /// <summary>
        ///     是否绘制边框
        /// </summary>
        [Browsable(true), Description("是否绘制边框")]
        [DefaultValue(false)]
        public bool IBorder
        {
            get { return _iBorder; }
            set
            {
                _iBorder = value;
                Invalidate();
            }
        }

        private bool _iRound = true;
        /// <summary>
        ///     是否剪成圆角
        /// </summary>
        [Browsable(true), Description("是否剪成圆角")]
        [DefaultValue(true)]
        public bool IRound
        {
            get { return _iRound; }
            set
            {
                _iRound = value;
                TRadius = value ? 4 : 0;
                DrawRound();
            }
        }

        /// <summary>
        ///     线性渐变绘制
        /// </summary>
        private TProperties _tBrush;

        /// <summary>
        ///     线性渐变绘制
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TProperties TBrush
        {
            get
            {
                if (_tBrush == null)
                {
                    _tBrush = new TProperties(MethodBase.GetCurrentMethod());
                    _tBrush.ValueChange += delegate { Invalidate(ClientRectangle); };
                }
                return _tBrush;
            }
        }

        /// <summary>
        ///     指定线性渐变的方向
        /// </summary>
        private LinearGradientMode _tBrushMode = LinearGradientMode.Vertical;

        /// <summary>
        ///     指定线性渐变的方向
        /// </summary>
        [DefaultValue(LinearGradientMode.Vertical)]
        public LinearGradientMode TBrushMode
        {
            get { return _tBrushMode; }
            set
            {
                _tBrushMode = value;
                Invalidate(ClientRectangle);
            }
        }

        #endregion

        #region 重置默认属性值

        /// <summary>
        ///     获取或设置运行时窗体的起始位置。
        /// </summary>
        [Description("获取或设置运行时窗体的起始位置")]
        [DefaultValue(FormStartPosition.CenterScreen)]
        public new FormStartPosition StartPosition
        {
            get { return base.StartPosition; }
            set { base.StartPosition = value; }
        }

        /// <summary>
        ///     获取或设置控件的自动缩放模式。
        /// </summary>
        [Description("获取或设置控件的自动缩放模式")]
        [DefaultValue(AutoScaleMode.None)]
        public new AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
            set { base.AutoScaleMode = value; }
        }

        /// <summary>
        ///     获取或设置控件的前景色。
        /// </summary>
        [Description("获取或设置控件的前景色")]
        [DefaultValue(typeof(Color), "White")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                if (value == Color.Empty)
                {
                    value = Color.White;
                }
                base.ForeColor = value;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     初始化窗口
        /// </summary>
        private void Initialize()
        {
            FormBorderStyle = FormBorderStyle.None;
        }

        /// <summary>
        ///     拖动窗口大小
        /// </summary>
        /// <param name="m"></param>
        public override void WmNcHitTest(ref Message m)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                var wparam = m.LParam.ToInt32();

                var point = new Point(
                    NativeMethods.LOWORD(wparam),
                    NativeMethods.HIWORD(wparam));

                point = PointToClient(point);
                if (_iResize)
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
                    if (m.Result != Consts.TRUE)
                    {
                        HideSysMenu();
                    }
                    else
                    {
                        ShowSysMenu();
                    }
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
        }

        /// <summary>
        ///     设置图片为窗体，透明区域根据 opacity 的值决定透明度
        /// </summary>
        /// <param name="bitmap">透明位图</param>
        /// <param name="opacity">透明度的值0~255</param>
        protected void SetBitmap(Bitmap bitmap, byte opacity)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ArgumentException("The bitmap must be 32ppp with alpha-channel.");

            // The ideia of this is very simple,
            // 1. Create a compatible DC with screen;
            // 2. Select the bitmap with 32bpp with alpha-channel in the compatible DC;
            // 3. Call the UpdateLayeredWindow.

            var screenDc = NativeMethods.GetDC(IntPtr.Zero);
            var memDc = NativeMethods.CreateCompatibleDC(screenDc);
            var hBitmap = IntPtr.Zero;
            var oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0)); // grab a GDI handle from this GDI+ bitmap
                oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

                var size = new SIZE(bitmap.Width, bitmap.Height);
                var pointSource = new POINT(0, 0);
                var topPos = new POINT(Left, Top);
                var blend = new BLENDFUNCTION()
                {
                    BlendOp = Consts.AC_SRC_OVER,
                    BlendFlags = 0,
                    SourceConstantAlpha = opacity,
                    AlphaFormat = Consts.AC_SRC_ALPHA
                };
                if (!IsDisposed)
                {
                    NativeMethods.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0,
                        ref blend, Consts.ULW_ALPHA);
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

        /// <summary>
        /// 获取下一个控件
        /// </summary>
        protected Control NextControl(Point current, Control parent)
        {
            Control result = null;
            Point next = new Point(this.Width, this.Height);
            foreach (Control item in parent.Controls)
            {
                if (item.Location.Y < next.Y || (item.Location.Y == next.Y && item.Location.X < next.X))
                    if (item.Location.Y > current.Y || (item.Location.Y == current.Y && item.Location.X > current.X))
                    {
                        if (item.Visible && item.Enabled &&
                            ((item is QQTextBox && (item as QQTextBox).Edit.Enabled && !(item as QQTextBox).Edit.ReadOnly) ||
                            item is DateTimePicker ||
                            (item is NumericUpDown && !(item as NumericUpDown).ReadOnly)))
                        {
                            next = item.Location;
                            result = item;
                        }
                    }
            }
            return result;
        }
        /// <summary>
        /// 当前焦点控件
        /// </summary>
        protected Control CurrentPoint(Control parent)
        {
            foreach (Control item in parent.Controls)
            {
                if (item.ContainsFocus)
                {
                    if (item.GetType() == typeof(TControl) || item.GetType() == typeof(TPanel) || item.GetType() == typeof(Panel))
                    {
                        return CurrentPoint(item);
                    }
                    return item;
                }
            }
            return this;
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

            //绘画图标
            if (ShowIcon)
                g.DrawIcon(Icon, IconRect);
            DrawText(g);
            DrawFrameBorder(g);
        }

        /// <summary>
        ///     填充背景
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            //绘制背景
            if (TBrush.ColorMove != Color.Empty && TBrush.ColorDown != Color.Empty)
            {
                TranColor = TBrush.ColorMove;
                var normal = TranColor;
                TranColor = TBrush.ColorDown;
                var space = TranColor;
                using (var brush = new LinearGradientBrush(ClientRectangle, normal, space, _tBrushMode))
                {
                    var blend = new Blend()
                    {
                        Factors = new[] { 1f, 0.5f, 0f },
                        Positions = new[] { 0f, 0.5f, 1f }
                    };
                    var temp = ClientRectangle;
                    //修理毛边
                    temp = new Rectangle(temp.X - 1, temp.Y - 1, temp.Width + 2, temp.Height + 2);
                    g.FillRectangle(brush, temp);
                }
            }
            else if (BackgroundImage == null)
            {
                TranColor = BackColor;
                using (var solidBrush = new SolidBrush(TranColor))
                {
                    g.FillRectangle(solidBrush, ClientRectangle);
                }
            }
        }

        /// <summary>
        ///     绘制标题文字
        /// </summary>
        protected void DrawText(Graphics g)
        {
            string text = _textShow;
            if (text == null) text = this.Text;
            if (text != null)
            {
                TextRenderer.DrawText(g, text, Font, TextRect, ForeColor, TextFormatFlags.VerticalCenter);
            }
        }

        /// <summary>
        ///     绘制窗体边框
        /// </summary>
        private void DrawFrameBorder(Graphics g)
        {
            //绘画边框
            if (_iBorder)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    //左边
                    g.DrawImage(_borderImage, new Rectangle(0, 0, 1, Height), new Rectangle(5, 5 + 3, 1, 1),
                        GraphicsUnit.Pixel);
                    //右边
                    g.DrawImage(_borderImage, new Rectangle(Width - 1, 0, 1, Height), new Rectangle(5, 8, 1, 1),
                        GraphicsUnit.Pixel);
                    //上边
                    g.DrawImage(_borderImage, new Rectangle(0, 0, Width, 1), new Rectangle(8, 5, 6, 1),
                        GraphicsUnit.Pixel);
                    //下边
                    g.DrawImage(_borderImage, new Rectangle(0, Height - 1, Width, 1), new Rectangle(8, 5, 6, 1),
                        GraphicsUnit.Pixel);
                }
                else
                {
                    //左上
                    g.DrawImage(_borderImage, new Rectangle(0, 0, 3, 3),
                        new Rectangle(_borderImage.Width - 3 - 5, _borderImage.Height - 5 - 3, 3, 3), GraphicsUnit.Pixel);
                    //右下
                    g.DrawImage(_borderImage, new Rectangle(Width - 3, Height - 3, 3, 3), new Rectangle(5, 5, 3, 3),
                        GraphicsUnit.Pixel);
                    //左下
                    g.DrawImage(_borderImage, new Rectangle(0, Height - 3, 3, 3),
                        new Rectangle(_borderImage.Width - 3 - 5, 5, 3, 3), GraphicsUnit.Pixel);
                    //右上
                    g.DrawImage(_borderImage, new Rectangle(Width - 3, 0, 3, 3),
                        new Rectangle(5, _borderImage.Height - 5 - 3, 3, 3), GraphicsUnit.Pixel);
                    //左边
                    g.DrawImage(_borderImage, new Rectangle(0, 2, 1, Height - 4), new Rectangle(5, 8, 1, 1),
                        GraphicsUnit.Pixel);
                    //右边
                    g.DrawImage(_borderImage, new Rectangle(Width - 1, 2, 1, Height - 4), new Rectangle(5, 8, 1, 1),
                        GraphicsUnit.Pixel);
                    //上边
                    g.DrawImage(_borderImage, new Rectangle(2, 0, Width - 4, 1), new Rectangle(8, 5, 6, 1),
                        GraphicsUnit.Pixel);
                    //下边
                    g.DrawImage(_borderImage, new Rectangle(2, Height - 1, Width - 4, 1), new Rectangle(8, 5, 6, 1),
                        GraphicsUnit.Pixel);
                }
            }
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.Resize 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnResize(EventArgs e)
        {
            TControl.MStop(this.Controls);
            base.OnResize(e);
            DrawRound();
        }
        /// <summary>
        /// 将窗体剪成圆角
        /// </summary>
        protected void DrawRound()
        {
            if (!this.Visible) return;
            //调用API，将窗体剪成圆角
            var ellipse = _iRound ? TRadius : 0;
            var rgn = NativeMethods.CreateRoundRectRgn(0, 0, Width + 1, Height + 1, ellipse, ellipse);
            if (!IsDisposed)
            {
                NativeMethods.SetWindowRgn(Handle, rgn, true);
            }
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.Load 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.OnResize(e);
            ShowSysMenu();
            if (!DesignMode)
            {
                switch (StartPosition)
                {
                    case FormStartPosition.CenterParent:
                        Location = new Point(
                            (Parent.Width - Width) / 2,
                            (Parent.Height - Height) / 2);
                        break;
                    case FormStartPosition.CenterScreen:
                        Location = new Point(
                            (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2,
                            (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2);
                        break;
                    case FormStartPosition.Manual:
                    case FormStartPosition.WindowsDefaultBounds:
                    case FormStartPosition.WindowsDefaultLocation:
                        break;
                }
            }
        }

        /// <summary>
        ///     处理 Windows 消息。
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
                    WmNcHitTest(ref m);
                    break;
                case 0x112:
                    //过滤窗体重复消息
                    if (!MenuClick((MenuType)m.WParam.ToInt32()))
                        base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
            if (_windowState != FormWindowState.Minimized)
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
        private bool MenuClick(MenuType type)
        {
            switch (type)
            {
                case MenuType.About:
                    if (AboutEvent == null)
                    {
                        if (this.GetType() != typeof(AboutForm))
                        {
                            using (var about = new AboutForm())
                            {
                                about.ShowDialog(this);
                            }
                        }
                    }
                    else
                    {
                        AboutEvent(this, EventArgs.Empty);
                    }
                    break;
                case (MenuType)(int)WindowStyle.SC_MAXIMIZE:
                    WindowState = FormWindowState.Maximized;
                    break;
                case (MenuType)(int)WindowStyle.SC_MINIMIZE:
                    WindowState = FormWindowState.Minimized;
                    break;
                case (MenuType)(int)WindowStyle.SC_RESTORE:
                    if (WindowState != FormWindowState.Minimized)
                        WindowState = WindowState;
                    else
                        WindowState = lastState;
                    break;
                case MenuType.Restore:
                    WindowState = FormWindowState.Normal;
                    break;
                case MenuType.MaxSize:
                    WindowState = FormWindowState.Maximized;
                    break;
                case (MenuType)(int)WindowStyle.SC_CLOSE:
                    this.Close();
                    break;
                default: return false;
            }
            return true;
        }

        private bool CloseContains(Point point)
        {
            return CloseRect.Contains(point) ||
                   (CloseRect.Contains(new Point(CloseRect.X, point.Y)) &&
                    point.Y < CloseRect.Height && point.X > CloseRect.X);
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseDown。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            var point = e.Location;
            if (e.Button == MouseButtons.Right)
            {
                if (this.TitleBarRect.Contains(point) && !SysBtnRect.Contains(point))
                {
                    var menu = NativeMethods.GetSystemMenu(this.Handle, false);
                    RECT rect = new RECT();
                    var result = NativeMethods.TrackPopupMenu(menu, 0x0100, MousePosition.X, MousePosition.Y, 0, this.Handle, ref rect);
                    MenuClick((MenuType)result);
                }
                return;
            }

            if (CloseContains(point))
                CloseState = TMouseState.Down;
            else if (MiniRect.Contains(point))
                MinState = TMouseState.Down;
            else if (MaxRect.Contains(point))
                MaxState = TMouseState.Down;
            if (_sysButton == TSysButton.Normal && e.Clicks == 2)
            {
                WindowMax();
            }
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseMove。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
                return;
            var flag = true;
            string tipText = null;
            var point = e.Location;
            if (CloseContains(point))
            {
                flag = false;
                if (CloseState != TMouseState.Move)
                {
                    CloseState = TMouseState.Move;
                    tipText = "关闭";
                }
            }
            else
                CloseState = TMouseState.Normal;
            if (MiniRect.Contains(point))
            {
                flag = false;
                if (MinState != TMouseState.Move)
                {
                    MinState = TMouseState.Move;
                    tipText = "最小化";
                }
            }
            else
                MinState = TMouseState.Normal;
            if (MaxRect.Contains(point))
            {
                flag = false;
                if (MaxState != TMouseState.Move)
                {
                    MaxState = TMouseState.Move;
                    tipText = WindowState == FormWindowState.Maximized ? "还原" : "最大化";
                }
            }
            else
            {
                MaxState = TMouseState.Normal;
            }
            if (TitleBarMenuRect.Contains(e.Location))
            {
                flag = false;
                if (TitleBarMenuState != TMouseState.Move)
                {
                    TitleBarMenuState = TMouseState.Move;
                    tipText = "菜单栏";
                }
            }
            else
            {
                TitleBarMenuState = TMouseState.Normal;
            }
            if (tipText != null)
            {
                ShowTooTip(tipText);
            }
            if (flag)
            {
                HideToolTip();
            }
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseLeave。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            CloseState = TMouseState.Normal;
            MaxState = TMouseState.Normal;
            MinState = TMouseState.Normal;
        }

        /// <summary>
        ///     引发 System.Windows.Forms.Form.MouseUp。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Left)
                return;
            var point = e.Location;
            if (CloseContains(point) && CloseState == TMouseState.Down)
            {
                CloseState = TMouseState.Move;
                Close();
            }
            else
            {
                CloseState = TMouseState.Normal;
            }
            if (MiniRect.Contains(point) && MinState == TMouseState.Down)
            {
                MinState = TMouseState.Move;
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                MinState = TMouseState.Normal;
            }
            if (MaxRect.Contains(point) && MaxState == TMouseState.Down)
            {
                MaxState = TMouseState.Move;
                WindowMax();
            }
            else
            {
                MaxState = TMouseState.Normal;
            }
        }

        /// <summary>
        ///     最大化方法
        /// </summary>
        protected void WindowMax()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        /// <summary>
        ///     封装创建控件时所需的信息。
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var param = base.CreateParams;
                if (DesignMode) return param;

                if (ITransfer)
                {
                    param.ExStyle = (int)WindowStyle.WS_SYSMENU;
                }
                if (_sysButton != TSysButton.Close)
                {
                    param.Style |= (int)WindowStyle.WS_MINIMIZEBOX; // 允许最小化操作
                }
                return param;
            }
        }

        /// <summary>
        /// 允许弹出右键系统菜单
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.Alt) == Keys.Alt)
            {
                ShowSysMenu();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region 绘制下圆角路径

        /// <summary>
        ///     绘制下圆角路径
        /// </summary>
        /// <param name="control"></param>
        protected void TDrawBelowPath(Control control)
        {
            if (control == null) return;
            control.Paint += Control_PaintDP;
        }

        private void Control_PaintDP(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            DrawHelper.CreateBelowPath(e.Graphics, control.ClientRectangle, BackColor);
        }

        #endregion

        #region 绘制下圆角边框

        /// <summary>
        ///     绘制下圆角边框
        /// </summary>
        /// <param name="control"></param>
        protected void TDrawBelowBorder(Control control)
        {
            if (!_iBorder || control == null || _borderImage == null) return;
            control.Paint += Control_PaintDB;
        }

        private void Control_PaintDB(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            var g = e.Graphics;

            if (WindowState == FormWindowState.Maximized)
            {
                //左边
                g.DrawImage(_borderImage, new Rectangle(0, 0, 1, control.Height), new Rectangle(5, 8, 1, 1),
                    GraphicsUnit.Pixel);
                //右边
                g.DrawImage(_borderImage, new Rectangle(control.Width - 1, 0, 1, control.Height),
                    new Rectangle(5, 8, 1, 1), GraphicsUnit.Pixel);
                //上边
                g.DrawImage(_borderImage, new Rectangle(0, 0, control.Width, 1), new Rectangle(8, 5, 6, 1),
                    GraphicsUnit.Pixel);
                //下边
                g.DrawImage(_borderImage, new Rectangle(0, control.Height - 1, control.Width, 1),
                    new Rectangle(8, 5, 6, 1), GraphicsUnit.Pixel);
            }
            else
            {
                //右下
                g.DrawImage(_borderImage, new Rectangle(control.Width - 3, control.Height - 3, 3, 3),
                    new Rectangle(5, 5, 3, 3), GraphicsUnit.Pixel);
                //左下
                g.DrawImage(_borderImage, new Rectangle(0, control.Height - 3, 3, 3),
                    new Rectangle(_borderImage.Width - 3 - 5, 5, 3, 3), GraphicsUnit.Pixel);
                //左边
                g.DrawImage(_borderImage, new Rectangle(0, 0, 1, control.Height - 2), new Rectangle(5, 8, 1, 1),
                    GraphicsUnit.Pixel);
                //右边
                g.DrawImage(_borderImage, new Rectangle(control.Width - 1, 0, 1, control.Height - 2),
                    new Rectangle(5, 8, 1, 1), GraphicsUnit.Pixel);
                //下边
                g.DrawImage(_borderImage, new Rectangle(2, control.Height - 1, control.Width - 4, 1),
                    new Rectangle(8, 5, 6, 1), GraphicsUnit.Pixel);
            }
            g.Dispose();
        }

        #endregion

        #region 将控件剪成圆角
        private int ellipse;
        /// <summary>
        ///     将控件剪成圆角
        /// </summary>
        protected void TDrawRoundRect(Control control, int ellipse)
        {
            if (control == null) return;
            this.ellipse = ellipse;
            control.SizeChanged += Control_SizeChanged;
            Control_SizeChanged(control, EventArgs.Empty);
        }
        private void Control_SizeChanged(object sender, EventArgs e)
        {
            DrawRoundRect(sender as Control, ellipse);
        }
        /// <summary>
        ///     将控件剪成圆角
        /// </summary>
        /// <param name="control"></param>
        /// <param name="ellipse"></param>
        protected void DrawRoundRect(Control control, int ellipse)
        {
            if (control == null) return;
            DrawRoundRect(control, control.ClientRectangle, ellipse);
        }
        /// <summary>
        ///     将控件剪成圆角
        /// </summary>
        /// <param name="control"></param>
        /// <param name="rect"></param>
        /// <param name="ellipse"></param>
        protected void DrawRoundRect(Control control, Rectangle rect, int ellipse)
        {
            if (control == null) return;

            var rgn = NativeMethods.CreateRoundRectRgn(rect.X, rect.Y, rect.Width + 1 - rect.X, rect.Height + 1 - rect.Y,
                ellipse, ellipse);
            if (!control.IsDisposed)
            {
                NativeMethods.SetWindowRgn(control.Handle, rgn, true);
            }
        }

        #endregion

        #region 显示系统菜单
        /// <summary>
        ///     显示系统菜单
        ///     改变窗口大小与系统菜单冲突
        /// </summary>
        public void ShowSysMenu()
        {
            int windowLong = (NativeMethods.GetWindowLong(this.Handle, -16));
            NativeMethods.SetWindowLong(this.Handle, -16, windowLong | (int)WindowStyle.WS_SYSMENU);
            var menu = NativeMethods.GetSystemMenu(this.Handle, false);

            NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_RESTORE, 0x0);
            NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_MOVE, 0x0);
            NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_SIZE, 0x0);
            NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_MAXIMIZE, 0x0);
            NativeMethods.DeleteMenu(menu, (int)MenuType.About, 0);
            NativeMethods.DeleteMenu(menu, (int)MenuType.None, 0);
            NativeMethods.DeleteMenu(menu, (int)MenuType.Restore, 0);
            NativeMethods.DeleteMenu(menu, (int)MenuType.MaxSize, 0);
            switch (_sysButton)
            {
                //正常
                case TSysButton.Normal:
                    NativeMethods.InsertMenu(menu, (int)WindowStyle.SC_MINIMIZE, 0, (IntPtr)MenuType.About, "关于");
                    NativeMethods.InsertMenu(menu, (int)WindowStyle.SC_MINIMIZE, 0, (IntPtr)MenuType.None, null);
                    NativeMethods.InsertMenu(menu, (int)WindowStyle.SC_MINIMIZE, (this.WindowState == FormWindowState.Maximized) ? 0 : 2, (IntPtr)MenuType.Restore, "还原");
                    NativeMethods.InsertMenu(menu, 0, (this.WindowState == FormWindowState.Maximized) ? 2 : 0, (IntPtr)MenuType.MaxSize, "最大化");
                    break;
                //关闭按钮
                case TSysButton.Close:
                    NativeMethods.InsertMenu(menu, 0, 0, (IntPtr)MenuType.About, "关于");
                    NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_MINIMIZE, 0x0);
                    break;
                //关闭按钮，最小化
                case TSysButton.Close_Mini:
                    NativeMethods.InsertMenu(menu, (int)WindowStyle.SC_MINIMIZE, 0, (IntPtr)MenuType.About, "关于");
                    break;
            }
        }
        /// <summary>
        ///     显示系统菜单
        /// </summary>
        public void HideSysMenu()
        {
            int windowLong = (NativeMethods.GetWindowLong(this.Handle, -16));
            if ((windowLong & (int)WindowStyle.WS_SYSMENU) == (int)WindowStyle.WS_SYSMENU)
            {
                windowLong -= (int)WindowStyle.WS_SYSMENU;
                NativeMethods.SetWindowLong(this.Handle, -16, windowLong);
            }
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_borderImage != null) _borderImage.Dispose();
                if (_tBrush != null) _tBrush.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}