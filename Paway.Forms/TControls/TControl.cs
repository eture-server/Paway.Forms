using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Helper;
using Paway.Resource;
using Paway.Win32;
using System.Reflection;

namespace Paway.Forms
{
    /// <summary>
    ///     自定义基控件
    /// </summary>
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design")]
    public class TControl : UserControl, IControl
    {
        #region 构造

        /// <summary>
        ///     构造
        /// </summary>
        public TControl()
        {
            Licence.Checking(MethodBase.GetCurrentMethod().DeclaringType);
            IFixedBackground = false;
            IMouseMove = false;
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
            TConfig.Init(this);
            InitShow();

            AutoScaleMode = AutoScaleMode.None;
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion

        #region 重载属性默认值
        /// <summary>
        ///     获取或设置在 System.Windows.Forms.ImageLayout 枚举中定义的背景图像布局。
        /// </summary>
        [Description("获取或设置在 System.Windows.Forms.ImageLayout 枚举中定义的背景图像布局")]
        [DefaultValue(typeof(ImageLayout), "Stretch")]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        /// <summary>
        ///     获取或设置控件的自动缩放模式。
        /// </summary>
        [Description("获取或设置控件的自动缩放模式")]
        [DefaultValue(typeof(AutoScaleMode), "None")]
        public new AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
            set { base.AutoScaleMode = value; }
        }

        #endregion

        #region 事件
        /// <summary>
        ///     移动特效正常完成事件。
        /// </summary>
        public event EventHandler MoveFinished;

        #endregion

        #region 重绘背景
        /// <summary>
        ///     重绘背景
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            //绘制背景
            if (TBrush.ColorSpace != Color.Empty && TBrush.ColorNormal != Color.Empty)
            {
                TranColor = TBrush.ColorNormal;
                var normal = TranColor;
                TranColor = TBrush.ColorSpace;
                var space = TranColor;
                using (var brush = new LinearGradientBrush(ClientRectangle, normal, space, _tBrushMode))
                {
                    var blend = new Blend()
                    {
                        Factors = new[] { 1f, 0.5f, 0f },
                        Positions = new[] { 0f, 0.5f, 1f }
                    };
                    g.FillRectangle(brush, ClientRectangle);
                }
            }
        }

        #endregion

        #region 移动窗体
        /// <summary>
        ///     移动控件父窗体
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (IsDisposed) return;
            if (!IMouseMove) return;
            if (e.Button != MouseButtons.Left) return;
            if (Contain(e.Location)) return;
            if (ParentForm != null && ParentForm.WindowState != FormWindowState.Maximized)
            {
                if (ParentForm is TForm)
                {
                    var form = ParentForm as TForm;
                    if (form.WindowState == FormWindowState.Maximized) return;
                }
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(ParentForm.Handle, (int)WindowsMessage.WM_SYSCOMMAND,
                    (int)WindowsMessage.SC_MOVE, 0);
            }
        }

        #endregion

        #region 变量
        /// <summary>
        ///     加载标记
        /// </summary>
        public bool ILoad;

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

        #region 接口 属性
        private TMDirection _mDirection;
        /// <summary>
        ///     移动特效方向
        /// </summary>
        [Description("移动特效方向"), DefaultValue(typeof(TMDirection), "None")]
        public TMDirection MDirection
        {
            get { return _mDirection; }
            set
            {
                MStop();
                _mDirection = value;
            }
        }

        /// <summary>
        ///     透明过度(旋转前)图片
        /// </summary>
        [Browsable(false)]
        [Description("透明过渡(旋转前)图片"), DefaultValue(typeof(Image), "null")]
        public Image TranImage { get; set; }

        /// <summary>
        ///     旋转后图片
        /// </summary>
        [Browsable(false)]
        [Description("旋转后图片"), DefaultValue(typeof(Image), "null")]
        public Image TranLaterImage { get; set; }

        private int _mInterval = 7;
        /// <summary>
        ///     移动特效间隔
        /// </summary>
        [Description("移动特效间隔"), DefaultValue(7)]
        public int MInterval
        {
            get { return _mInterval; }
            set { _mInterval = value; }
        }

        /// <summary>
        ///     获取或设置控件的背景色
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
                base.BackColor = value;
            }
        }

        /// <summary>
        ///     获取或设置控件的前景色。
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

        private TProperties _tBrush;
        /// <summary>
        ///     线性渐变绘制
        /// </summary>
        [Description("线性渐变绘制，从ColorNormal到ColorSpace")]
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

        private LinearGradientMode _tBrushMode = LinearGradientMode.Vertical;
        /// <summary>
        ///     指定线性渐变的方向
        /// </summary>
        [Description("指定线性渐变的方向")]
        [DefaultValue(typeof(LinearGradientMode), "Vertical")]
        public LinearGradientMode TBrushMode
        {
            get { return _tBrushMode; }
            set
            {
                _tBrushMode = value;
                Invalidate(ClientRectangle);
            }
        }

        private int _trans = 255;
        /// <summary>
        ///     控件透明度
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
                Invalidate(ClientRectangle);
            }
        }

        /// <summary>
        ///     移动控件父窗体
        /// </summary>
        [Description("移动控件父窗体"), DefaultValue(false)]
        public bool IMouseMove { get; set; }

        /// <summary>
        ///     坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Contain(Point p)
        {
            return false;
        }

        #endregion

        #region 固定窗体背景 - 同TForm
        /// <summary>
        ///     固定窗体背景
        /// </summary>
        [Category("Appearance"), Description("固定窗体背景"), DefaultValue(false)]
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
        private volatile bool iStop = true;
        private object mdLock = new object();

        private void InitShow()
        {
            sTimer = new Timer()
            {
                Interval = 45
            };
            sTimer.Tick += STimer_Tick;
        }

        /// <summary>
        ///     初始化控件位置
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            ILoad = true;
            base.OnLoad(e);
            MChild();
        }

        /// <summary>
        ///     为标题栏准备
        /// </summary>
        public void MChild()
        {
            for (var i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is Panel || Controls[i] is TControl)
                {
                    var panel = Controls[i];
                    for (var j = 0; j < panel.Controls.Count; j++)
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
        ///     启动特效
        /// </summary>
        public void MStart(TMDirection dirction, int interval = 0)
        {
            MDirection = dirction;
            MStart(interval);
        }

        /// <summary>
        /// 停止全部物资
        /// </summary>
        public static void MStop(Control.ControlCollection controls)
        {
            for (var i = 0; i < controls.Count; i++)
            {
                var item = controls[i];
                if (item is TControl)
                {
                    (item as TControl).MStop();
                }
                if (item.Controls.Count > 0)
                    MStop(item.Controls);
            }
        }
        /// <summary>
        ///     停止特效，并还原
        /// </summary>
        public void MStop()
        {
            lock (mdLock)
            {
                iStop = true;
                MStop2();
            }
        }
        private void MStop2()
        {
            if (sTimer.Enabled)
            {
                sTimer.Stop();
            }
            else return;
            Size = size;
            Dock = dock;
            switch (MDirection)
            {
                case TMDirection.Left:
                case TMDirection.Right:
                    Left = point.X;
                    break;
                case TMDirection.Up:
                case TMDirection.Down:
                    Top = point.Y;
                    break;
                case TMDirection.Center:
                    Location = point;
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
                    Controls.Remove(alpha);
                    BackgroundImage = image;
                    break;
            }
        }

        /// <summary>
        ///     启动特效
        /// </summary>
        public void MStart(int interval = 0)
        {
            MStop();
            if (MDirection == TMDirection.None) return;
            lock (mdLock)
            {
                iStop = false;
                MStart2(interval);
            }
        }
        private void MStart2(int interval)
        {
            if (interval > 0)
            {
                sTimer.Interval = interval;
            }
            point = Location;
            dock = Dock;
            if (Parent != null && dock == DockStyle.Fill)
            {
                Size = Parent.Size;
            }
            size = Size;
            Dock = DockStyle.None;
            Size = size;
            sTimer.Interval = 10;
            switch (MDirection)
            {
                case TMDirection.Left:
                    Left = -Width;
                    intervel = Width / MInterval;
                    break;
                case TMDirection.Right:
                    Left = Parent != null ? Parent.Width : Right;
                    intervel = Width / MInterval;
                    break;
                case TMDirection.Up:
                    Top = -Height;
                    intervel = Height / MInterval;
                    break;
                case TMDirection.Down:
                    Top = Parent != null ? Parent.Height : Bottom;
                    intervel = Height / MInterval;
                    break;
                case TMDirection.Center:
                    step = new Size(Width / MInterval, Height / MInterval);
                    Size = step;
                    Left = point.X + (size.Width - Size.Width) / 2;
                    Top = point.Y + (size.Height - Size.Height) / 2;
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
                    image = BackgroundImage;
                    if (Width > 0 && Height > 0)
                    {
                        //if (this.TranImage != null)
                        {
                            Parent.BackgroundImage = null;
                            var bitmap = new Bitmap(Width, Height);
                            DrawToBitmap(bitmap, new Rectangle(0, 0, Width, Height));
                            switch (MDirection)
                            {
                                case TMDirection.Transparent:
                                case TMDirection.T3DLeft:
                                case TMDirection.T3DRight:
                                case TMDirection.T3DUp:
                                case TMDirection.T3DDown:
                                    BackgroundImage = bitmap;
                                    break;
                            }
                            if (TranLaterImage == null)
                            {
                                TranLaterImage = bitmap;
                            }
                        }
                        color = 255;
                        i3d = true;
                        intervel = 255 * 2 / (3 * MInterval);

                        if (TranImage == null)
                        {
                            //alpha.BackColor = Color.FromArgb(255, TopColor(this));
                            TranImage = TranLaterImage;
                        }
                        Controls.Add(alpha);
                        Controls.SetChildIndex(alpha, 0);
                        alpha.Dock = DockStyle.Fill;

                        AlphaImage();
                    }
                    break;
            }
            sTimer.Start();
        }
        private Color TopColor(Control c)
        {
            if (c == null) return Color.Transparent;
            if (c.BackColor.A == 0)
            {
                return TopColor(c.Parent);
            }
            return c.BackColor;
        }

        private void AlphaImage()
        {
            if (TranImage == null) return;
            var temp = BitmapHelper.ConvertTo(TranImage, TConvertType.Trans, color);
            if (alpha.Size != temp.Size)
            {
                temp = BitmapHelper.CutBitmap(temp, alpha.Bounds);
            }
            alpha.BackgroundImage = temp;
        }

        private void Alpha3DImage(Image image, T3Direction direction)
        {
            if (image == null) return;
            var temp = image.Clone() as Bitmap;
            if (alpha.Size != temp.Size)
            {
                temp = BitmapHelper.CutBitmap(temp, alpha.Bounds);
            }
            var iCenter = false;
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

        private void STimer_Tick(object sender, EventArgs e)
        {
            lock (mdLock)
            {
                if (iStop) return;
                STimer_Tick();
            }
        }
        private void STimer_Tick()
        {
            if (IsDisposed) return;
            if (point == new Point(-1, -1))
            {
                point = Location;
            }
            //NativeMethods.LockWindowUpdate(this.Handle);
            switch (MDirection)
            {
                case TMDirection.Normal:
                    Reset();
                    break;
                case TMDirection.Left:
                    if (Left + intervel < point.X)
                    {
                        Left += intervel;
                    }
                    else
                    {
                        Left = point.X;
                        Reset();
                    }
                    break;
                case TMDirection.Right:
                    if (Left - intervel > point.X)
                    {
                        Left -= intervel;
                    }
                    else
                    {
                        Left = point.X;
                        Reset();
                    }
                    break;
                case TMDirection.Up:
                    if (Top + intervel < point.Y)
                    {
                        Top += intervel;
                    }
                    else
                    {
                        Top = point.Y;
                        Reset();
                    }
                    break;
                case TMDirection.Down:
                    if (Top - intervel > point.Y)
                    {
                        Top -= intervel;
                    }
                    else
                    {
                        Top = point.Y;
                        Reset();
                    }
                    break;
                case TMDirection.Center:
                    if (Size.Width + step.Width < size.Width)
                    {
                        Size = new Size(Size.Width + step.Width, Size.Height + step.Height);
                        Left = point.X + (size.Width - Size.Width) / 2;
                        Top = point.Y + (size.Height - Size.Height) / 2;
                    }
                    else
                    {
                        Location = point;
                        Reset();
                    }
                    break;
                case TMDirection.Transparent:
                    if (color > intervel)
                    {
                        color -= intervel;
                        if (TranImage == null)
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
                Alpha3DImage(TranImage, d1);
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
                        Alpha3DImage(TranLaterImage, d2);
                    }
                    else
                    {
                        color = 255;
                    }
                }
                else
                {
                    color = 0;
                    Alpha3DImage(TranLaterImage, d2);
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
            Size = size;
            Dock = dock;
            switch (MDirection)
            {
                case TMDirection.Transparent:
                case TMDirection.T3DLeft:
                case TMDirection.T3DLeftToRight:
                case TMDirection.T3DRight:
                case TMDirection.T3DRightToLeft:
                case TMDirection.T3DUp:
                case TMDirection.T3DUpToDown:
                case TMDirection.T3DDown:
                case TMDirection.T3DDownToUp:
                    Controls.Remove(alpha);
                    BackgroundImage = image;
                    break;
            }
            MoveFinished?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Dispose
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                image = null;
                TranImage = null;
                TranLaterImage = null;
                if (_tBrush != null)
                    _tBrush.Dispose();
                if (sTimer != null)
                {
                    sTimer.Stop();
                    sTimer.Dispose();
                }
                if (alpha != null)
                    alpha.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}