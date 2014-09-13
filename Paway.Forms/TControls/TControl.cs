using Paway.Helper;
using Paway.Resource;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 自定义基控件
    /// </summary>
    [Designer("System.Windows.Forms.Design.ParentControlDesigner,   System.Design ")]
    public class TControl : UserControl, IControl
    {
        #region 变量
        /// <summary>
        /// 指定窗体窗口如何显示
        /// </summary>
        protected FormWindowState _windowState = FormWindowState.Normal;
        /// <summary>
        /// 星星图
        /// </summary>
        private Image star = AssemblyHelper.GetImage("Controls.t.png");

        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public TControl()
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

        #region 接口 属性
        /// <summary>
        /// 获取或设置控件的背景色
        /// </summary>
        [Description("获取或设置控件的背景色"), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (value == Color.Empty || value == SystemColors.Control)
                {
                    value = Color.Transparent;
                }
                if (value.A > _trans)
                {
                    value = Color.FromArgb(_trans, value.R, value.G, value.B);
                }
                base.BackColor = value;
            }
        }

        /// <summary>
        /// 获取或设置控件的前景色。
        /// </summary>
        [Description("获取或设置控件的前景色"), DefaultValue(typeof(Color), "Black")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                if (value == Color.Empty)
                {
                    value = Color.Black;
                }
                base.ForeColor = value;
            }
        }

        private int _trans = 255;
        /// <summary>
        /// 控件透明度
        /// </summary>
        [Description("透明度"), DefaultValue(255)]
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

        private bool _iMousemove = false;
        /// <summary>
        /// 移动控件父窗体
        /// </summary>
        [Description("移动控件父窗体"), DefaultValue(false)]
        public bool IMouseMove
        {
            get { return _iMousemove; }
            set { _iMousemove = value; }
        }

        /// <summary>
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Contain(Point p) { return false; }

        #endregion

        #region 移动窗体
        /// <summary>
        /// 移动控件父窗体
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!_iMousemove) return;
            if (e.Button != MouseButtons.Left) return;
            if (this.Contain(e.Location)) return;
            if (this.ParentForm != null && this.ParentForm.WindowState != FormWindowState.Maximized)
            {
                if (this.ParentForm is TForm)
                {
                    TForm form = this.ParentForm as TForm;
                    if (form.WindowState == FormWindowState.Maximized) return;
                }
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(this.ParentForm.Handle, 274, 61440 + 9, 0);
            }
        }

        #endregion

        #region 固定窗体背景 - 同TForm
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

        #region 动态星星
        private BackgroundWorker timer;
        /// <summary>
        /// 消灭星星
        /// </summary>
        public void Star()
        {
            if (timer == null)
            {
                timer = new BackgroundWorker();
                timer.WorkerSupportsCancellation = true;
                timer.DoWork += timer_DoWork;
            }
            if (!timer.IsBusy)
            {
                timer.RunWorkerAsync();
            }
        }

        void timer_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Random ran = new Random();
            Graphics g = this.CreateGraphics();
            Image star = BitmapHelper.ConvertTo(this.star, BConvertType.Trans, 30);
            while (true)
            {
                if (worker.CancellationPending) break;

                int width = ran.Next(20, 20);
                Rectangle rect = new Rectangle(ran.Next(this.Width - width), ran.Next(this.Height - width), width, width);
                rect = new Rectangle(100, 60, 20, 20);
                Point point = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                for (int i = 1; i <= width / 2; i++)
                {
                    rect = new Rectangle(point.X - i, point.Y - i, i * 2, i * 2);
                    g.DrawImage(star, rect);
                    System.Threading.Thread.Sleep(100 + i * 10);
                }
                System.Threading.Thread.Sleep(ran.Next(300));
                g.FillRectangle(new SolidBrush(Color.Transparent), rect);
                star = BitmapHelper.ConvertTo(this.star, BConvertType.Trans, 0);
                g.DrawImage(star, rect);
                //System.Threading.Thread.Sleep(100);
                //g.DrawImage(BitmapHelper.ConvertTo(this.star, BConvertType.Trans, 50), rect);
                //System.Threading.Thread.Sleep(100);
                //g.DrawImage(BitmapHelper.ConvertTo(this.star, BConvertType.Trans, 0), rect);
                //System.Threading.Thread.Sleep(100);

                System.Threading.Thread.Sleep(ran.Next(3000));
            }
            g.Dispose();
        }
        /// <summary>
        /// 消灭星星
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (timer != null && timer.IsBusy)
            {
                timer.CancelAsync();
            }
        }

        #endregion
    }
}
