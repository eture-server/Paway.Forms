using Paway.Helper;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 窗体自定义基类
    /// </summary>
    public partial class TForm : Form, IControl
    {
        #region 变量
        /// <summary>
        /// 悬停窗口
        /// </summary>
        private ToolTip toolTop;

        #endregion
        #region 属性
        /// <summary>
        /// 指定窗体窗口如何显示
        /// </summary>
        protected FormWindowState _windowState = FormWindowState.Normal;
        /// <summary>
        /// 指定窗体窗口如何显示
        /// </summary>
        [Description("指定窗体窗口如何显示"), DefaultValue(typeof(FormWindowState), "Normal")]
        public virtual new FormWindowState WindowState
        {
            get { return this._windowState; }
            set
            {
                this._windowState = value;
                switch (this._windowState)
                {
                    case FormWindowState.Normal:
                        base.WindowState = FormWindowState.Normal;
                        break;
                    case FormWindowState.Minimized:
                        base.WindowState = FormWindowState.Minimized;
                        break;
                }
            }
        }

        private int _tRadius = 3;
        /// <summary>
        /// 设置或获取窗体的圆角的大小
        /// </summary>
        [Category("TForm"), Description("设置或获取窗体的圆角的大小"), DefaultValue(3)]
        public int TRadius
        {
            get { return this._tRadius; }
            set
            {
                if (this._tRadius != value)
                {
                    this._tRadius = (value < 1) ? 1 : value;
                    this.OnResize(EventArgs.Empty);
                    //base.Invalidate();
                }
            }
        }

        private int _tShadowWidth = 4;
        /// <summary>
        /// 窗体阴影宽度
        /// </summary>
        [Category("TForm"), Description("窗体阴影宽度"), DefaultValue(typeof(int), "4")]
        public int TShadowWidth
        {
            get { return this._tShadowWidth; }
            set
            {
                if (this._tShadowWidth != value)
                {
                    this._tShadowWidth = (value < 1) ? 1 : value;
                    if (this.skin != null)
                    {
                        this.skin.SetBits();
                    }
                }
            }
        }

        private Color _tShadowColor = Color.Black;
        /// <summary>
        /// 窗体阴影颜色
        /// </summary>
        [Category("TForm"), DefaultValue(typeof(Color), "Black"), Description("窗体阴影颜色")]
        public Color TShadowColor
        {
            get { return this._tShadowColor; }
            set
            {
                if (this._tShadowColor != value)
                {
                    this._tShadowColor = value;
                    if (this.skin != null)
                    {
                        this.skin.SetBits();
                    }
                }
            }
        }

        private bool _iSpecial = false;
        /// <summary>
        /// 是否启用窗口淡入淡出
        /// </summary>
        [Category("TForm"), Description("是否启用窗口淡入淡出"), DefaultValue(false)]
        public bool ISpecial
        {
            get { return _iSpecial; }
            set { _iSpecial = value; }
        }

        private bool _iShadow = true;
        /// <summary>
        /// 是否启用窗体阴影
        /// </summary>
        [Category("Shadow"), DefaultValue(true), Description("是否启用窗体阴影")]
        public bool IShadow
        {
            get { return this._iShadow; }
            set { _iShadow = value; }
        }

        /// <summary>
        /// 是否允许改变窗口大小
        /// </summary>
        protected bool _isResize = true;
        /// <summary>
        /// 是否允许改变窗口大小
        /// </summary>
        [Description("是否允许改变窗口大小"), DefaultValue(true)]
        public virtual bool IsResize
        {
            get { return this._isResize; }
            set { _isResize = value; }
        }

        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public TForm()
        {
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
            InitMethod.Init(this);
            toolTop = new ToolTip();
        }

        #endregion

        #region 固定窗体背景
        private bool _fixedBackground = false;
        /// <summary>
        /// 固定窗体背景
        /// </summary>
        [Category("Appearance"), Description("固定窗体背景"), DefaultValue(false)]
        public bool TFixedBackground
        {
            get { return _fixedBackground; }
            set { _fixedBackground = value; }
        }
        /// <summary>
        /// 处理滚动条事件
        /// </summary>
        /// <param name="se"></param>
        protected override void OnScroll(ScrollEventArgs se)
        {
            if (!this.IsDisposed && _fixedBackground)
            {
                // 执行固定背景的操作
                if (se.Type == ScrollEventType.ThumbTrack)
                {
                    // 若滚动框正在移动，解除对控件用户界面的锁定
                    NativeMethods.LockWindowUpdate(IntPtr.Zero);
                    // 立即重新绘制控件所有的用户界面
                    this.Refresh();
                    // 锁定控件的用户界面
                    NativeMethods.LockWindowUpdate(this.Handle);
                }
                else
                {
                    // 解除对控件用户界面的锁定
                    NativeMethods.LockWindowUpdate(IntPtr.Zero);
                    // 声明控件的所有的内容无效，但不立即重新绘制
                    this.Invalidate();
                }
            }
            base.OnScroll(se);
        }
        /// <summary>
        /// 处理鼠标滚轮事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!this.IsDisposed && _fixedBackground)
            {
                NativeMethods.LockWindowUpdate(this.Handle);
                base.OnMouseWheel(e);
                NativeMethods.LockWindowUpdate(IntPtr.Zero);
                this.Invalidate();
            }
            else
            {
                base.OnMouseWheel(e);
            }
        }

        #endregion

        #region 移动窗体
        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="control"></param>
        protected void TMouseMove(Control control)
        {
            if (control == null) return;
            if (control is IControl && !(control is TForm) && (control as IControl).IMouseMove) return;
            control.MouseDown += control_MouseDown;
        }
        void control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            IControl icontrol = sender as IControl;
            if (icontrol != null)
            {
                if (icontrol.Contain(e.Location)) return;
            }
            if (!this.IsDisposed && this._windowState != FormWindowState.Maximized)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, 274, 61440 + 9, 0);
            }
        }

        #endregion

        #region 接口
        /// <summary>
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Contain(Point p) { return false; }

        private int _trans = 255;
        /// <summary>
        /// 控件透明度
        /// </summary>
        [Browsable(false), Description("透明度 - 无效的"), DefaultValue(255)]
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
            }
        }

        private bool _iMousemove = true;
        /// <summary>
        /// 移动窗体
        /// </summary>
        [Browsable(false), Description("移动窗体"), DefaultValue(true)]
        public bool IMouseMove
        {
            get { return _iMousemove; }
            set { _iMousemove = value; }
        }

        #endregion

        #region 阴影部分
        private SkinForm skin;
        /// <summary>
        /// 显示阴影
        /// </summary>
        /// <param name="e"></param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (base.Visible)
            {
                if (!this.IsDisposed && !base.DesignMode && _iSpecial)
                {
                    NativeMethods.AnimateWindow(base.Handle, 300, 0xa0000);
                    this.Update();
                }
                if (!this.IsDisposed && !base.DesignMode && _iShadow && this.skin == null)
                {
                    this.skin = new SkinForm(this);
                    this.skin.Show(this);
                }
                base.OnVisibleChanged(e);
            }
            else
            {
                base.OnVisibleChanged(e);
                if (!this.IsDisposed && !base.DesignMode && _iSpecial)
                {
                    NativeMethods.AnimateWindow(base.Handle, 150, 0x90000);
                    base.Update();
                }
            }
        }
        /// <summary>
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (!this.IsDisposed && !base.DesignMode && _iSpecial)
            {
                NativeMethods.AnimateWindow(base.Handle, 150, 0x90000);
                base.Update();
            }
        }
        /// <summary>
        /// 拖动窗口大小
        /// </summary>
        /// <param name="m"></param>
        public virtual void WmNcHitTest(ref Message m) { }

        #endregion

        #region ToolTip
        /// <summary>
        /// 表示一个长方形的小弹出窗口，该窗口在用户将指针悬停在一个控件上时显示有关该控件用途的简短说明。
        /// </summary>
        protected void ShowTooTip(string toolTipText)
        {
            this.toolTop.Active = true;
            this.toolTop.SetToolTip(this, toolTipText);
        }
        /// <summary>
        /// 弹出窗口不活动
        /// </summary>
        protected void HideToolTip()
        {
            this.toolTop.Active = false;
        }

        #endregion
    }
}
