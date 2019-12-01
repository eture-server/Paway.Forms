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
    /// 窗体的基类，完成一部分共有的功能
    /// </summary>
    public class FormBase : TForm
    {
        #region 全局事件
        /// <summary>
        /// 关于
        /// </summary>
        public static event EventHandler AboutEvent;

        #endregion

        #region 构造
        /// <summary>
        /// 初始化 Paway.Forms.FormBase 类的新实例。
        /// </summary>
        public FormBase()
        {
            FormBorderStyle = FormBorderStyle.None;
            Padding = new Padding(1, 26, 1, 1);
            ForeColor = Color.White;
            toolTop = new ToolTip();
            if (IMouseMove) TMouseMove(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            if (toolTop != null)
            {
                toolTop.Dispose();
            }
            if (_borderImage != null) _borderImage.Dispose();
            if (skin != null)
            {
                skin.Dispose();
                skin = null;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region 重置默认属性值
        /// <summary>
        /// 获取或设置控件的前景色。
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

        /// <summary>
        /// 是否显示图标
        /// </summary>
        private bool _showIcon = true;
        /// <summary>
        /// 是否显示图标
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
        /// 记录上一次窗体状态，用于还原
        /// </summary>
        private FormWindowState lastState;
        /// <summary>
        /// 指定窗体窗口如何显示
        /// </summary>
        private FormWindowState _windowState = FormWindowState.Normal;
        /// <summary>
        /// 指定窗体窗口如何显示
        /// </summary>
        [Description("指定窗体窗口如何显示")]
        [DefaultValue(FormWindowState.Normal)]
        public new virtual FormWindowState WindowState
        {
            get { return _windowState; }
            set
            {
                bool iSame = _windowState == value;
                if (!iSame)
                    lastState = _windowState;
                switch (value)
                {
                    case FormWindowState.Minimized:
                        _restoreSize = Size;
                        _restorePoint = Location;
                        break;
                }
                _windowState = value;
                switch (_windowState)
                {
                    case FormWindowState.Normal:
                    case FormWindowState.Maximized:
                        base.WindowState = FormWindowState.Normal;
                        break;
                    case FormWindowState.Minimized:
                        base.WindowState = FormWindowState.Minimized;
                        break;
                }
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

        #endregion

        #region 变量
        /// <summary>
        /// 悬停窗口
        /// </summary>
        private readonly ToolTip toolTop;
        /// <summary>
        /// 阴影窗体
        /// </summary>
        private SkinForm skin;
        /// <summary>
        /// 最小化时恢复标记
        /// </summary>
        private bool iRestore;

        /// <summary>
        /// 边框图片
        /// </summary>
        private readonly Image _borderImage = Resources.QQ_FormFrame_fringe_bkg;

        /// <summary>
        /// 记录窗体Normal大小
        /// </summary>
        private Size? _normalSize;
        /// <summary>
        /// 记录窗体Restore大小
        /// </summary>
        private Size? _restoreSize;
        /// <summary>
        /// 记录窗体Normal位置
        /// </summary>
        private Point? _normalPoint;
        /// <summary>
        /// 记录窗体Restore位置
        /// </summary>
        private Point? _restorePoint;

        #endregion

        #region 属性
        private Color _tShadowColor = Color.Black;
        /// <summary>
        /// 窗体阴影颜色
        /// </summary>
        [Category("TForm"), Description("窗体阴影颜色")]
        [DefaultValue(typeof(Color), "Black")]
        public Color TShadowColor
        {
            get { return _tShadowColor; }
            set
            {
                if (_tShadowColor != value)
                {
                    _tShadowColor = value;
                    if (skin != null) skin.SetBits();
                }
            }
        }

        private bool _iRound = true;
        /// <summary>
        /// 是否剪成圆角
        /// </summary>
        [Browsable(true), Description("是否剪成圆角")]
        [DefaultValue(true)]
        public bool IRound
        {
            get { return _iRound; }
            set
            {
                _iRound = value;
                TRadius = value ? 5 : 0;
                DrawRound();
            }
        }

        private int _tRadius = 5;
        /// <summary>
        /// 设置或获取窗体的圆角的大小
        /// 窗体阴影宽度=value+1
        /// 最佳值=4
        /// </summary>
        [Browsable(false)]
        [Category("TForm"), Description("设置或获取窗体的圆角的大小")]
        [DefaultValue(5)]
        internal int TRadius
        {
            get
            {
                if (WindowState == FormWindowState.Maximized) return 0;
                if (this is FormBase formBase && formBase.WindowState == FormWindowState.Maximized) return 0;
                return _tRadius;
            }
            set
            {
                if (_tRadius != value)
                {
                    _tRadius = value;
                    if (skin != null) skin.SetBits();
                    OnResize(EventArgs.Empty);
                    //base.Invalidate();
                }
            }
        }

        /// <summary>
        /// 是否允许改变窗口大小
        /// </summary>
        [Description("是否允许改变窗口大小")]
        [DefaultValue(true)]
        public bool IResize { get; set; } = true;

        /// <summary>
        /// 是否启用窗体阴影
        /// </summary>
        [Category("Shadow"), Description("是否启用窗体阴影")]
        [DefaultValue(true)]
        public bool IShadow { get; set; } = true;

        /// <summary>
        /// 是否透明
        /// </summary>
        [Description("指定窗体是否透明")]
        [DefaultValue(false)]
        public bool ITransfer { get; set; }

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
                    Invalidate(TitleBarRect);
                }
            }
        }

        private string _textShow;
        /// <summary>
        /// 窗体显示的标题文字(Null=显示Text,String.Empty=不显示)
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
        /// 系统按钮
        /// </summary>
        private TSysButton _sysButton = TSysButton.Normal;
        /// <summary>
        /// 系统控制按钮
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
        /// 系统控制按钮区域
        /// </summary>
        [Description("系统控制按钮区域")]
        [DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle SysBtnRect
        {
            get { return Rectangle.Empty; }
        }

        /// <summary>
        /// 标题栏区域
        /// </summary>
        protected virtual Rectangle TitleBarRect
        {
            get { return new Rectangle(0, 0, Width, 30); }
        }

        /// <summary>
        /// 关闭按钮区域
        /// </summary>
        [Description("关闭按钮区域")]
        [DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle CloseRect
        {
            get { return Rectangle.Empty; }
        }

        /// <summary>
        /// 最小化按钮区域
        /// </summary>
        [Description("最小化按钮区域")]
        [DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle MiniRect
        {
            get { return Rectangle.Empty; }
        }

        /// <summary>
        /// 最大化按钮区域
        /// </summary>
        [Description("最大化按钮区域")]
        [DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle MaxRect
        {
            get { return Rectangle.Empty; }
        }

        /// <summary>
        /// 标题栏菜单按钮的矩形区域
        /// </summary>
        [Description("标题栏菜单按钮的矩形区域")]
        [DefaultValue(typeof(Rectangle), "Empty")]
        protected virtual Rectangle TitleBarMenuRect
        {
            get { return Rectangle.Empty; }
        }

        /// <summary>
        /// 图标显示区域
        /// </summary>
        [Description("图标显示区域")]
        [DefaultValue(typeof(Rectangle), "4, 4, 16, 16")]
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
                var width = TitleBarRect.Width - IconRect.Width - 15;
                var height = TitleBarRect.Height - 10;
                var textRect = new Rectangle(8, 2, width, height);
                if (ShowIcon)
                    textRect.X = IconRect.Width + 8;
                return textRect;
            }
        }

        /// <summary>
        /// 关闭按钮的鼠标状态
        /// </summary>
        private TMouseState _closeState = TMouseState.Normal;
        /// <summary>
        /// 关闭按钮当前的鼠标状态
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
        /// 最大化按钮的鼠标状态
        /// </summary>
        private TMouseState _maxState = TMouseState.Normal;
        /// <summary>
        /// 最大化按钮当前的鼠标状态
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
        /// 最小化按钮的鼠标状态
        /// </summary>
        private TMouseState _minState = TMouseState.Normal;
        /// <summary>
        /// 最小化按钮当前的鼠标状态
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
        /// 标题栏菜单按钮的鼠标的状态
        /// </summary>
        [Description("标题栏菜单按钮的鼠标的状态")]
        [DefaultValue(TMouseState.Normal)]
        protected TMouseState TitleBarMenuState { get; set; }

        private bool _iBorder;
        /// <summary>
        /// 是否绘制边框
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

        #endregion

        #region 接口
        /// <summary>
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Contain(Point p)
        {
            return SysBtnRect.Contains(p);
        }

        #endregion

        #region 拖动改变窗口大小
        /// <summary>
        /// 拖动窗口大小
        /// </summary>
        internal void WmNcHitTest(ref Message m)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                var wparam = m.LParam.ToInt32();

                var point = new Point(
                    NativeMethods.LowerWord(wparam),
                    NativeMethods.HightWord(wparam));

                point = PointToClient(point);
                if (IResize)
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
                    if (m.Result != Consts.True)
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

        #endregion

        #region 显示阴影
        /// <summary>
        /// 显示阴影
        /// </summary>
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                if (!IsDisposed && !DesignMode && IShadow)
                {
                    if (skin != null)
                    {
                        skin.Close();
                        skin.Dispose();
                    }
                    skin = new SkinForm(this);
                    skin.Show(this);
                }
                base.OnVisibleChanged(e);
            }
        }

        #endregion

        #region 重绘
        /// <summary>
        /// 重绘
        /// </summary>
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
        /// 绘制标题文字
        /// </summary>
        internal void DrawText(Graphics g)
        {
            string text = _textShow;
            if (text == null) text = this.Text;
            if (text != null)
            {
                TextRenderer.DrawText(g, text, Font, TextRect, ForeColor, TextFormatFlags.VerticalCenter);
            }
        }
        /// <summary>
        /// 绘制窗体边框
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

        #endregion

        #region 圆角窗体
        /// <summary>
        /// 引发 System.Windows.Forms.Form.Resize 事件。
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
        private void DrawRound()
        {
            if (!this.Visible) return;
            //调用API，将窗体剪成圆角
            var ellipse = IRound ? TRadius : 0;
            if (ellipse > 0) ellipse += 1;
            var rgn = NativeMethods.CreateRoundRectRgn(0, 0, Width + 1, Height + 1, ellipse, ellipse);
            if (!IsDisposed)
            {
                NativeMethods.SetWindowRgn(Handle, rgn, true);
            }
        }

        #endregion

        #region 系统消息重载处理
        /// <summary>
        /// 处理 Windows 消息。
        /// </summary>
        /// <param name="m">要处理的 WindowsMessage。</param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WindowsMessage.WM_PAINT:
                    //不能阻止从最小化恢复时的界面绘制
                    {//阻止一次重绘，防止从最小化恢复时闪屏
                        //NativeMethods.SendMessage(this.Handle, (int)WindowsMessage.WM_SETREDRAW, 0, 0);
                        //NativeMethods.SendMessage(this.Handle, (int)WindowsMessage.WM_SETREDRAW, 1, 0);
                    }
                    base.WndProc(ref m);
                    if (iRestore)
                    {
                        iRestore = false;
                        Application.DoEvents();
                        this.Opacity = 1;
                    }
                    break;
                case (int)WindowsMessage.WM_NCPAINT:
                case (int)WindowsMessage.WM_NCCALCSIZE:
                    break;
                case (int)WindowsMessage.WM_NCHITTEST:
                    base.WndProc(ref m);
                    WmNcHitTest(ref m);
                    break;
                case (int)WindowsMessage.WM_SYSCOMMAND:
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
                    if (m.WParam == Consts.False)
                    {
                        m.Result = Consts.True;
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
                    try
                    {//从最小化恢复时闪屏，临时去除双缓存
                        //并设置透明度，在重绘后再恢复。去除双缓存效果更新好，就是会普慢
                        iRestore = true;
                        this.Opacity = 0;
                        //SetStyle(ControlStyles.AllPaintingInWmPaint, false);
                        if (WindowState != FormWindowState.Minimized)
                            WindowState = WindowState;
                        else
                            WindowState = lastState;
                    }
                    finally
                    {
                        //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                    }
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
        /// 引发 System.Windows.Forms.Form.MouseDown。
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
                    var result = NativeMethods.TrackPopupMenu(menu, (int)WindowsMessage.WM_KEYDOWN, MousePosition.X, MousePosition.Y, 0, this.Handle, ref rect);
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
            if ((_sysButton == TSysButton.Normal || _sysButton == TSysButton.Close_Max) && e.Clicks == 2)
            {
                WindowMax();
            }
        }
        /// <summary>
        /// 最大化方法
        /// </summary>
        private void WindowMax()
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
        /// 引发 System.Windows.Forms.Form.MouseMove。
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
        /// 引发 System.Windows.Forms.Form.MouseLeave。
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
        /// 引发 System.Windows.Forms.Form.MouseUp。
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
        /// 封装创建控件时所需的信息。
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
                if (_sysButton == TSysButton.Normal || _sysButton == TSysButton.Close_Mini)
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

        #region ToolTip
        /// <summary>
        /// 表示一个长方形的小弹出窗口，该窗口在用户将指针悬停在一个控件上时显示有关该控件用途的简短说明。
        /// </summary>
        private void ShowTooTip(string toolTipText)
        {
            toolTop.Active = true;
            toolTop.SetToolTip(this, toolTipText);
        }
        /// <summary>
        /// 弹出窗口不活动
        /// </summary>
        private void HideToolTip()
        {
            toolTop.Active = false;
        }

        #endregion

        #region 显示系统菜单
        /// <summary>
        /// 引发 System.Windows.Forms.Form.Load 事件。
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
        /// 显示系统菜单
        /// 改变窗口大小与系统菜单冲突
        /// </summary>
        private void ShowSysMenu()
        {
            int windowLong = (NativeMethods.GetWindowLong(this.Handle, -16));
            NativeMethods.SetWindowLong(this.Handle, -16, windowLong | (int)WindowStyle.WS_SYSMENU);
            var menu = NativeMethods.GetSystemMenu(this.Handle, false);

            NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_RESTORE, 0);
            NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_MOVE, 0);
            NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_SIZE, 0);
            NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_MAXIMIZE, 0);
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
                case TSysButton.None:
                case TSysButton.Close:
                    NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_MINIMIZE, 0x0);
                    NativeMethods.InsertMenu(menu, 0, 0, (IntPtr)MenuType.About, "关于");
                    break;
                //关闭按钮，最小化
                case TSysButton.Close_Mini:
                    NativeMethods.InsertMenu(menu, (int)WindowStyle.SC_MINIMIZE, 0, (IntPtr)MenuType.About, "关于");
                    NativeMethods.InsertMenu(menu, (int)WindowStyle.SC_MINIMIZE, 0, (IntPtr)MenuType.None, null);
                    break;
                case TSysButton.Close_Max:
                    NativeMethods.DeleteMenu(menu, (int)WindowStyle.SC_MINIMIZE, 0x0);
                    NativeMethods.InsertMenu(menu, 0, 0, (IntPtr)MenuType.About, "关于");
                    NativeMethods.InsertMenu(menu, 0, 0, (IntPtr)MenuType.None, null);
                    NativeMethods.InsertMenu(menu, 0, (this.WindowState == FormWindowState.Maximized) ? 0 : 2, (IntPtr)MenuType.Restore, "还原");
                    NativeMethods.InsertMenu(menu, 0, (this.WindowState == FormWindowState.Maximized) ? 2 : 0, (IntPtr)MenuType.MaxSize, "最大化");
                    break;
            }
        }
        /// <summary>
        /// 显示系统菜单
        /// </summary>
        private void HideSysMenu()
        {
            int windowLong = (NativeMethods.GetWindowLong(this.Handle, -16));
            if ((windowLong & (int)WindowStyle.WS_SYSMENU) == (int)WindowStyle.WS_SYSMENU)
            {
                windowLong -= (int)WindowStyle.WS_SYSMENU;
                NativeMethods.SetWindowLong(this.Handle, -16, windowLong);
            }
        }

        #endregion
    }
}