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
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
            InitMethod.Init(this);
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
            if (_fixedBackground)
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
            if (_fixedBackground)
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
            if (this._windowState != FormWindowState.Maximized)
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
    }
}
