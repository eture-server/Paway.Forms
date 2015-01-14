﻿using Paway.Helper;
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

        #region 事件
        /// <summary>
        /// 移动特效正常完成事件。
        /// </summary>
        public event EventHandler MoveFinished;

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
            InitShow();

            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion

        #region 接口 属性
        private TMDirection _mDirection = TMDirection.None;
        /// <summary>
        /// 移动特效方向
        /// </summary>
        [Description("移动特效方向"), DefaultValue(typeof(TMDirection), "None")]
        public TMDirection MDirection
        {
            get { return _mDirection; }
            set { _mDirection = value; }
        }
        /// <summary>
        /// 透明过度(旋转前)图片
        /// </summary>
        [Description("透明过度(旋转前)图片"), DefaultValue(typeof(Image), "null")]
        public Image TranImage { get; set; }
        /// <summary>
        /// 旋转后图片
        /// </summary>
        [Description("旋转后图片"), DefaultValue(typeof(Image), "null")]
        public Image TranLaterImage { get; set; }
        private int _mInterval = 12;
        /// <summary>
        /// 移动特效间隔
        /// </summary>
        [Description("移动特效间隔"), DefaultValue(12)]
        public int MInterval
        {
            get { return _mInterval; }
            set { _mInterval = value; }
        }
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

        #region 重载属性默认值
        /// <summary>
        /// 获取或设置在 System.Windows.Forms.ImageLayout 枚举中定义的背景图像布局。
        /// </summary>
        [Description("获取或设置在 System.Windows.Forms.ImageLayout 枚举中定义的背景图像布局")]
        [DefaultValue(typeof(ImageLayout), "Stretch")]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        #endregion

        #region 移动窗体
        /// <summary>
        /// 移动控件父窗体
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.IsDisposed) return;
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

        #region 按指定方向显示移动特效
        private Timer sTimer;
        private int intervel;
        private DockStyle dock;
        private Size size;
        private Point point;
        private Size step;
        private TControl alpha;
        private int color = 255;
        private bool i3d;
        private Image image;
        private void InitShow()
        {
            sTimer = new Timer();
            sTimer.Interval = 10;
            sTimer.Tick += sTimer_Tick;
        }

        /// <summary>
        /// 初始化控件位置
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MChild();
        }
        /// <summary>
        /// 为标题栏准备
        /// </summary>
        public void MChild()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is Panel || this.Controls[i] is TControl)
                {
                    Control panel = this.Controls[i];
                    for (int j = 0; j < panel.Controls.Count; j++)
                    {
                        if (panel.Controls[j] is TControl)
                        {
                            (panel.Controls[j] as TControl).MStart();
                        }
                    }
                }
            }
            MStart();
        }
        /// <summary>
        /// 启动特效
        /// </summary>
        public void MStart(TMDirection dirction)
        {
            this.MDirection = dirction;
            MStart();
        }
        /// <summary>
        /// 随机特效
        /// </summary>
        public void MRandom()
        {
            int random = Enum.GetNames(typeof(TMDirection)).Length;
            random = new Random().Next(0, random);
            MStop();
            this.MDirection = (TMDirection)random;
            MStart();
        }
        /// <summary>
        /// 停止特效，并还原
        /// </summary>
        public void MStop()
        {
            if (sTimer.Enabled)
            {
                sTimer.Stop();
            }
            if (!iReader) return;
            this.Size = this.size;
            this.Dock = dock;
            switch (this.MDirection)
            {
                case TMDirection.Left:
                case TMDirection.Right:
                    this.Left = this.point.X;
                    break;
                case TMDirection.Up:
                case TMDirection.Down:
                    this.Top = this.point.Y;
                    break;
                case TMDirection.Center:
                    this.Location = this.point;
                    break;
                case TMDirection.Transparent:
                case TMDirection.T3DLeft:
                case TMDirection.T3DLeftToRight:
                case TMDirection.T3DRight:
                case TMDirection.T3DRightToLeft:
                case TMDirection.T3DUp:
                case TMDirection.T3DUpToDown:
                case TMDirection.T3DDown:
                case TMDirection.T3DDownToUp:
                    this.Controls.Remove(alpha);
                    this.BackgroundImage = this.image;
                    break;
            }
        }
        /// <summary>
        /// 是否使用过特效
        /// </summary>
        private bool iReader;
        /// <summary>
        /// 启动特效
        /// </summary>
        public void MStart()
        {
            MStop();
            if (MDirection == TMDirection.None) return;
            iReader = true;

            this.point = this.Location;
            this.dock = this.Dock;
            if (this.Parent != null && this.dock == DockStyle.Fill)
            {
                this.Size = this.Parent.Size;
            }
            this.size = this.Size;
            this.Dock = DockStyle.None;
            this.Size = this.size;
            this.sTimer.Interval = 10;
            switch (this.MDirection)
            {
                case TMDirection.Left:
                    this.Left = -this.Width;
                    this.intervel = this.Width / this.MInterval;
                    break;
                case TMDirection.Right:
                    this.Left = this.Parent != null ? this.Parent.Width : this.Right;
                    this.intervel = this.Width / this.MInterval;
                    break;
                case TMDirection.Up:
                    this.Top = -this.Height;
                    this.intervel = this.Height / this.MInterval;
                    break;
                case TMDirection.Down:
                    this.Top = this.Parent != null ? this.Parent.Height : this.Bottom;
                    this.intervel = this.Height / this.MInterval;
                    break;
                case TMDirection.Center:
                    this.step = new Size(this.Width / this.MInterval, this.Height / this.MInterval);
                    this.Size = step;
                    this.Left = point.X + (this.size.Width - this.Size.Width) / 2;
                    this.Top = point.Y + (this.size.Height - this.Size.Height) / 2;
                    break;
                case TMDirection.Transparent:
                case TMDirection.T3DLeft:
                case TMDirection.T3DLeftToRight:
                case TMDirection.T3DRight:
                case TMDirection.T3DRightToLeft:
                case TMDirection.T3DUp:
                case TMDirection.T3DUpToDown:
                case TMDirection.T3DDown:
                case TMDirection.T3DDownToUp:
                    if (alpha == null)
                    {
                        alpha = new TControl();
                    }
                    this.sTimer.Interval = 50;
                    image = this.BackgroundImage;
                    if (this.Width > 0 && this.Height > 0)
                    {
                        if (this.TranImage != null)
                        {
                            this.Parent.BackgroundImage = null;
                            Bitmap bitmap = new Bitmap(this.Width, this.Height);
                            this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
                            switch (MDirection)
                            {
                                case TMDirection.Transparent:
                                case TMDirection.T3DLeft:
                                case TMDirection.T3DRight:
                                case TMDirection.T3DUp:
                                case TMDirection.T3DDown:
                                    this.BackgroundImage = bitmap;
                                    break;
                            }
                            if (this.TranLaterImage == null)
                            {
                                this.TranLaterImage = bitmap;
                            }
                        }
                        color = 255;
                        i3d = true;
                        this.intervel = 255 / this.MInterval;

                        alpha.Dock = DockStyle.Fill;
                        this.Controls.Add(this.alpha);
                        this.Controls.SetChildIndex(this.alpha, 0);
                        AlphaImage();
                    }
                    break;
            }
            sTimer.Start();
        }
        private void AlphaImage()
        {
            if (this.TranImage == null) return;
            Bitmap bitmap = BitmapHelper.ConvertTo(this.TranImage, TConvertType.Trans, color);
            if (alpha.Location != Point.Empty)
            {
                bitmap = BitmapHelper.CutBitmap(bitmap, alpha.Bounds);
            }
            alpha.BackgroundImage = bitmap;
        }
        private void Alpha3DImage(Image image, T3Direction direction)
        {
            if (image == null) return;
            Bitmap temp = image.Clone() as Bitmap;
            if (alpha.Location != Point.Empty)
            {
                temp = BitmapHelper.CutBitmap(temp, alpha.Bounds);
            }
            bool iCenter = false;
            switch (MDirection)
            {
                case TMDirection.T3DLeftToRight:
                case TMDirection.T3DRightToLeft:
                case TMDirection.T3DUpToDown:
                case TMDirection.T3DDownToUp:
                    iCenter = true;
                    break;
            }
            temp = BitmapHelper.TrapezoidTransformation(temp, 0.8 + color * 0.2 / 255, color * 1.0 / 255, direction, iCenter);
            alpha.BackgroundImage = temp;
        }
        void sTimer_Tick(object sender, EventArgs e)
        {
            if (this.IsDisposed) return;
            if (point == new Point(-1, -1))
            {
                point = this.Location;
            }
            //NativeMethods.LockWindowUpdate(this.Handle);
            switch (this.MDirection)
            {
                case TMDirection.Normal:
                    Reset();
                    break;
                case TMDirection.Left:
                    if (this.Left + intervel < this.point.X)
                    {
                        this.Left += intervel;
                    }
                    else
                    {
                        this.Left = this.point.X;
                        Reset();
                    }
                    break;
                case TMDirection.Right:
                    if (this.Left - intervel > this.point.X)
                    {
                        this.Left -= intervel;
                    }
                    else
                    {
                        this.Left = this.point.X;
                        Reset();
                    }
                    break;
                case TMDirection.Up:
                    if (this.Top + intervel < this.point.Y)
                    {
                        this.Top += intervel;
                    }
                    else
                    {
                        this.Top = this.point.Y;
                        Reset();
                    }
                    break;
                case TMDirection.Down:
                    if (this.Top - intervel > this.point.Y)
                    {
                        this.Top -= intervel;
                    }
                    else
                    {
                        this.Top = this.point.Y;
                        Reset();
                    }
                    break;
                case TMDirection.Center:
                    if (this.Size.Width + step.Width < this.size.Width)
                    {
                        this.Size = new Size(this.Size.Width + step.Width, this.Size.Height + step.Height);
                        this.Left = point.X + (this.size.Width - this.Size.Width) / 2;
                        this.Top = point.Y + (this.size.Height - this.Size.Height) / 2;
                    }
                    else
                    {
                        this.Location = this.point;
                        Reset();
                    }
                    break;
                case TMDirection.Transparent:
                    if (color > intervel)
                    {
                        color -= intervel;
                        if (this.TranImage == null)
                        {
                            alpha.BackColor = Color.FromArgb(color, alpha.BackColor);
                        }
                        else
                        {
                            AlphaImage();
                        }
                    }
                    else
                    {
                        this.Controls.Remove(alpha);
                        this.BackgroundImage = this.image;
                        Reset();
                    }
                    break;
                case TMDirection.T3DLeft:
                    T3D(T3Direction.Left, T3Direction.None);
                    break;
                case TMDirection.T3DLeftToRight:
                    T3D(T3Direction.Left, T3Direction.Right);
                    break;
                case TMDirection.T3DRight:
                    T3D(T3Direction.Right, T3Direction.None);
                    break;
                case TMDirection.T3DRightToLeft:
                    T3D(T3Direction.Right, T3Direction.Left);
                    break;
                case TMDirection.T3DUp:
                    T3D(T3Direction.Up, T3Direction.None);
                    break;
                case TMDirection.T3DUpToDown:
                    T3D(T3Direction.Up, T3Direction.Down);
                    break;
                case TMDirection.T3DDown:
                    T3D(T3Direction.Down, T3Direction.None);
                    break;
                case TMDirection.T3DDownToUp:
                    T3D(T3Direction.Down, T3Direction.Up);
                    break;
            }
            //NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }
        private void T3D(T3Direction d1, T3Direction d2)
        {
            if (d1 != T3Direction.None && i3d && color > intervel)
            {
                color -= intervel;
                switch (MDirection)
                {
                    case TMDirection.T3DLeftToRight:
                    case TMDirection.T3DRightToLeft:
                    case TMDirection.T3DUpToDown:
                    case TMDirection.T3DDownToUp:
                        if (color > intervel)
                        {
                            color -= intervel;
                        }
                        break;
                }
                Alpha3DImage(this.TranImage, d1);
            }
            else if (d2 != T3Direction.None)
            {
                if (!i3d)
                {
                    if (color < 255 - intervel)
                    {
                        switch (MDirection)
                        {
                            case TMDirection.T3DLeftToRight:
                            case TMDirection.T3DRightToLeft:
                            case TMDirection.T3DUpToDown:
                            case TMDirection.T3DDownToUp:
                                if (color < 255 - intervel)
                                {
                                    color += intervel;
                                }
                                break;
                        }
                        color += intervel;
                        Alpha3DImage(this.TranLaterImage, d2);
                    }
                    else
                    {
                        color = 255;
                    }
                }
                else
                {
                    color = 0;
                    Alpha3DImage(this.TranLaterImage, d2);
                }
                i3d = false;
                if (color == 255)
                {
                    Reset();
                }
            }
            else
            {
                Reset();
            }
        }
        private void Reset()
        {
            sTimer.Stop();
            this.Size = this.size;
            this.Dock = dock;
            switch (MDirection)
            {
                case TMDirection.T3DLeft:
                case TMDirection.T3DLeftToRight:
                case TMDirection.T3DRight:
                case TMDirection.T3DRightToLeft:
                case TMDirection.T3DUp:
                case TMDirection.T3DUpToDown:
                case TMDirection.T3DDown:
                case TMDirection.T3DDownToUp:
                    this.Controls.Remove(alpha);
                    this.BackgroundImage = this.image;
                    break;
            }
            if (MoveFinished != null)
            {
                MoveFinished(this, EventArgs.Empty);
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
            Image star = BitmapHelper.ConvertTo(this.star, TConvertType.Trans, 30);
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
                star = BitmapHelper.ConvertTo(this.star, TConvertType.Trans, 0);
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
