using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Win32;

namespace Paway.Forms
{
    /// <summary>
    /// 窗体自定义基类
    /// </summary>
    public class TForm : Form, IControl
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public TForm()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.None;
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.Name = "TForm";
            this.ResumeLayout(false);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            if (_tBrush != null)
            {
                _tBrush.Dispose();
                _tBrush = null;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region 重置默认属性值
        /// <summary>
        /// 获取或设置控件的自动缩放模式。
        /// </summary>
        [Description("获取或设置控件的自动缩放模式")]
        [DefaultValue(AutoScaleMode.None)]
        public new AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
            set { base.AutoScaleMode = value; }
        }
        /// <summary>
        /// 获取或设置运行时窗体的起始位置。
        /// </summary>
        [Description("获取或设置运行时窗体的起始位置")]
        [DefaultValue(FormStartPosition.CenterScreen)]
        public new FormStartPosition StartPosition
        {
            get { return base.StartPosition; }
            set { base.StartPosition = value; }
        }

        #endregion

        #region 属性
        /// <summary>
        /// 是否启用窗口淡入淡出
        /// </summary>
        [Category("TForm"), Description("是否启用窗口淡入淡出")]
        [DefaultValue(false)]
        public bool ISpecial { get; set; }

        private int _trans = 255;
        /// <summary>
        /// 控件透明度
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
        /// 指定线性渐变的方向
        /// </summary>
        private LinearGradientMode _tBrushMode = LinearGradientMode.Vertical;
        /// <summary>
        /// 指定线性渐变的方向
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
        /// <summary>
        /// 线性渐变绘制背景
        /// </summary>
        private TProperties _tBrush;
        /// <summary>
        /// 线性渐变绘制背景
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
        /// 固定窗体背景
        /// </summary>
        [Category("Appearance"), Description("固定窗体背景")]
        [DefaultValue(false)]
        public bool IFixedBackground { get; set; }

        #endregion

        #region 接口
        /// <summary>
        /// 移动窗体
        /// </summary>
        [Browsable(false), Description("移动窗体")]
        [DefaultValue(true)]
        public bool IMouseMove { get; set; } = true;

        /// <summary>
        /// 坐标点是否包含在项中
        /// </summary>
        public virtual bool Contain(Point p) { return false; }

        #endregion

        #region 窗口淡入淡出
        /// <summary>
        /// </summary>
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                if (!IsDisposed && !DesignMode && ISpecial)
                {
                    NativeMethods.AnimateWindow(Handle, 300, 0xa0000);
                    Update();
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
            base.OnFormClosing(e);
        }

        #endregion

        #region 填充与固定窗体背景
        /// <summary>
        /// 填充背景
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            //绘制背景
            if (TBrush.ColorMove != Color.Empty && TBrush.ColorDown != Color.Empty)
            {
                var normal = TranColor(TBrush.ColorMove);
                var space = TranColor(TBrush.ColorDown);
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
                using (var solidBrush = new SolidBrush(TranColor(BackColor)))
                {
                    g.FillRectangle(solidBrush, ClientRectangle);
                }
            }
        }
        /// <summary>
        /// 绘制背景时自动颜色透明度
        /// </summary>
        private Color TranColor(Color color)
        {
            if (color.A > Trans)
            {
                color = Color.FromArgb(Trans, color.R, color.G, color.B);
            }
            return color;
        }
        /// <summary>
        /// 固定窗体背景-处理滚动条事件
        /// </summary>
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
        /// 固定窗体背景-处理鼠标滚轮事件
        /// </summary>
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
        /// 移动窗体
        /// </summary>
        protected void TMouseMove(Control control)
        {
            if (control == null) return;
            if (control is IControl iControl && iControl.IMouseMove && !(control is TForm)) return;
            control.MouseDown += Control_MouseDown;
        }
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (sender is IControl icontrol)
            {
                if (icontrol.Contain(e.Location)) return;
            }
            if (!IsDisposed && this.WindowState != FormWindowState.Maximized)
            {
                if (this is FormBase formBase && formBase.WindowState == FormWindowState.Maximized) return;
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, (int)WindowsMessage.WM_SYSCOMMAND, (int)WindowsMessage.SC_MOVE, 0);
            }
        }

        #endregion
    }
}