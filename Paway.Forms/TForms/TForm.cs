using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Win32;

namespace Paway.Forms
{
    /// <summary>
    ///     窗体自定义基类
    /// </summary>
    public class TForm : Form, IControl
    {
        #region 变量

        /// <summary>
        ///     悬停窗口
        /// </summary>
        private readonly ToolTip toolTop;

        #endregion

        #region 构造

        /// <summary>
        ///     构造
        /// </summary>
        public TForm()
        {
            IFixedBackground = false;
            ISpecial = false;
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
            toolTop = new ToolTip();
        }
        /// <summary>
        /// 应用配置
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TConfig.Init(this);
        }

        #endregion

        #region 属性

        /// <summary>
        ///     指定窗体窗口如何显示
        /// </summary>
        protected FormWindowState _windowState = FormWindowState.Normal;

        /// <summary>
        ///     指定窗体窗口如何显示
        /// </summary>
        [Description("指定窗体窗口如何显示")]
        [DefaultValue(FormWindowState.Normal)]
        public new virtual FormWindowState WindowState
        {
            get { return _windowState; }
            set
            {
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
            }
        }

        private int _tRadius = 4;
        /// <summary>
        ///     设置或获取窗体的圆角的大小
        ///     窗体阴影宽度=value+1
        ///     最佳值=4
        /// </summary>
        [Browsable(false)]
        [Category("TForm"), Description("设置或获取窗体的圆角的大小")]
        [DefaultValue(4)]
        internal int TRadius
        {
            get
            {
                if (WindowState == FormWindowState.Maximized) return 0;
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

        private Color _tShadowColor = Color.Black;

        /// <summary>
        ///     窗体阴影颜色
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

        /// <summary>
        ///     是否启用窗口淡入淡出
        /// </summary>
        [Category("TForm"), Description("是否启用窗口淡入淡出")]
        [DefaultValue(false)]
        public bool ISpecial { get; set; }

        private bool _iShadow = true;
        /// <summary>
        ///     是否启用窗体阴影
        /// </summary>
        [Category("Shadow"), Description("是否启用窗体阴影")]
        [DefaultValue(true)]
        public bool IShadow
        {
            get { return _iShadow; }
            set { _iShadow = value; }
        }

        /// <summary>
        ///     是否允许改变窗口大小
        /// </summary>
        protected bool _iResize = true;

        /// <summary>
        ///     是否允许改变窗口大小
        /// </summary>
        [Description("是否允许改变窗口大小")]
        [DefaultValue(true)]
        public virtual bool IResize
        {
            get { return _iResize; }
            set { _iResize = value; }
        }

        #endregion

        #region 固定窗体背景

        /// <summary>
        ///     固定窗体背景
        /// </summary>
        [Category("Appearance"), Description("固定窗体背景")]
        [DefaultValue(false)]
        public bool IFixedBackground { get; set; }

        /// <summary>
        ///     处理滚动条事件
        /// </summary>
        /// <param name="se"></param>
        protected override void OnScroll(ScrollEventArgs se)
        {
            if (!IsDisposed && IFixedBackground)
            {
                // 执行固定背景的操作
                if (se.Type == ScrollEventType.ThumbTrack)
                {
                    // 若滚动框正在移动，解除对控件用户界面的锁定
                    NativeMethods.LockWindowUpdate(IntPtr.Zero);
                    // 立即重新绘制控件所有的用户界面
                    Refresh();
                    // 锁定控件的用户界面
                    NativeMethods.LockWindowUpdate(Handle);
                }
                else
                {
                    // 解除对控件用户界面的锁定
                    NativeMethods.LockWindowUpdate(IntPtr.Zero);
                    // 声明控件的所有的内容无效，但不立即重新绘制
                    Invalidate();
                }
            }
            base.OnScroll(se);
        }

        /// <summary>
        ///     处理鼠标滚轮事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!IsDisposed && IFixedBackground)
            {
                NativeMethods.LockWindowUpdate(Handle);
                base.OnMouseWheel(e);
                NativeMethods.LockWindowUpdate(IntPtr.Zero);
                Invalidate();
            }
            else
            {
                base.OnMouseWheel(e);
            }
        }

        #endregion

        #region 移动窗体

        /// <summary>
        ///     移动窗体
        /// </summary>
        /// <param name="control"></param>
        protected void TMouseMove(Control control)
        {
            if (control == null) return;
            if (control is IControl iControl && !(control is TForm) && iControl.IMouseMove) return;
            control.MouseDown += Control_MouseDown;
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (sender is IControl icontrol)
            {
                if (icontrol.Contain(e.Location)) return;
            }
            if (!IsDisposed && _windowState != FormWindowState.Maximized)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, (int)WindowsMessage.WM_SYSCOMMAND, (int)WindowsMessage.SC_MOVE, 0);
            }
        }

        #endregion

        #region 接口

        /// <summary>
        ///     坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Contain(Point p)
        {
            return false;
        }

        private int _trans = 255;

        /// <summary>
        ///     控件透明度
        /// </summary>
        [Description("透明度")]
        [DefaultValue(255)]
        public int Trans
        {
            get { return _trans; }
            set
            {
                if (value < 0 || value > 255)
                {
                    value = 255;
                }
                _trans = value;
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     移动窗体
        /// </summary>
        [Browsable(false), Description("移动窗体")]
        [DefaultValue(true)]
        public bool IMouseMove { get; set; } = true;

        #endregion

        #region 阴影部分
        /// <summary>
        /// 阴影窗体
        /// </summary>
        protected SkinForm skin;

        /// <summary>
        ///     显示阴影
        /// </summary>
        /// <param name="e"></param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                if (!IsDisposed && !DesignMode && ISpecial)
                {
                    NativeMethods.AnimateWindow(Handle, 300, 0xa0000);
                    Update();
                }
                if (!IsDisposed && !DesignMode && _iShadow)
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
            else
            {
                base.OnVisibleChanged(e);
                if (!IsDisposed && !DesignMode && ISpecial)
                {
                    NativeMethods.AnimateWindow(Handle, 150, 0x90000);
                    Update();
                }
            }
        }

        /// <summary>
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!IsDisposed && !DesignMode && ISpecial)
            {
                NativeMethods.AnimateWindow(Handle, 150, 0x90000);
                Update();
            }
            this.Dispose();
            base.OnFormClosing(e);
        }

        /// <summary>
        ///     拖动窗口大小
        /// </summary>
        /// <param name="m"></param>
        public virtual void WmNcHitTest(ref Message m)
        {
        }

        #endregion

        #region ToolTip

        /// <summary>
        ///     表示一个长方形的小弹出窗口，该窗口在用户将指针悬停在一个控件上时显示有关该控件用途的简短说明。
        /// </summary>
        protected void ShowTooTip(string toolTipText)
        {
            toolTop.Active = true;
            toolTop.SetToolTip(this, toolTipText);
        }

        /// <summary>
        ///     弹出窗口不活动
        /// </summary>
        protected void HideToolTip()
        {
            toolTop.Active = false;
        }

        #endregion
    }
}