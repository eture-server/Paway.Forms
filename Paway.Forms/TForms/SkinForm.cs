using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Win32;

namespace Paway.Forms
{
    /// <summary>
    /// 窗体阴影
    /// </summary>
    public class SkinForm : Form
    {
        private readonly Color[] CornerColors;
        private readonly TForm Main;
        private readonly Color[] ShadowColors;
        /// <summary>
        /// 背景阴影透明度
        /// </summary>
        private const int alpha = 24;

        /// <summary>
        /// 构造
        /// </summary>
        public SkinForm(TForm main)
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
            ShadowColors = new[] { Color.FromArgb(alpha, Color.Black), Color.Transparent };
            CornerColors = new[] { Color.FromArgb(alpha * 3, Color.Black), Color.Transparent };
            Main = main;
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                createParams.ExStyle |= (int)WindowStyle.WS_SYSMENU;
                return createParams;
            }
        }

        #region 新加 - 移动窗体

        /// <summary>
        /// 移动窗体
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (IsDisposed) return;
            if (Main.WindowState != FormWindowState.Maximized)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Main.Handle, (int)WindowsMessage.WM_SYSCOMMAND, (int)WindowsMessage.SC_MOVE,
                    0);
            }
        }

        #endregion

        private void CanPenetrate()
        {
            //NativeMethods.GetWindowLong(base.Handle, -20);
            //NativeMethods.SetWindowLong(base.Handle, -20, 0x80020);
        }

        private void DrawCorners(Graphics g, Size corSize)
        {
            void action(int n)
            {
                using (var path = new GraphicsPath())
                {
                    Point point;
                    float num2;
                    PointF tf;
                    Point point2;
                    Point point3;
                    var size = new Size(corSize.Width * 2, corSize.Height * 2);
                    var size2 = new Size(Main.TRadius * 2, Main.TRadius * 2);
                    switch (n)
                    {
                        case 1:
                            point = new Point(0, 0);
                            num2 = 180f;
                            tf = new PointF(size.Width - size2.Width * 0.5f, size.Height - size2.Height * 0.5f);
                            point2 = new Point(corSize.Width, Main.TRadius + 1);
                            point3 = new Point(Main.TRadius + 1, corSize.Height);
                            break;

                        case 3:
                            point = new Point(Width - size.Width, 0);
                            num2 = 270f;
                            tf = new PointF(point.X + size2.Width * 0.5f, size.Height - size2.Height * 0.5f);
                            point2 = new Point(Width - (Main.TRadius + 1), corSize.Height);
                            point3 = new Point(Width - corSize.Width, Main.TRadius + 1);
                            break;

                        case 7:
                            point = new Point(0, Height - size.Height);
                            num2 = 90f;
                            tf = new PointF(size.Width - size2.Width * 0.5f, point.Y + size2.Height * 0.5f);
                            point2 = new Point(Main.TRadius + 1, Height - corSize.Height);
                            point3 = new Point(corSize.Width, Height - (Main.TRadius + 1));
                            break;

                        default:
                            point = new Point(Width - size.Width, Height - size.Height);
                            num2 = 0f;
                            tf = new PointF(point.X + size2.Width * 0.5f, point.Y + size2.Height * 0.5f);
                            point2 = new Point(Width - corSize.Width, Height - (Main.TRadius + 1));
                            point3 = new Point(Width - (Main.TRadius + 1), Height - corSize.Height);
                            break;
                    }
                    var rect = new Rectangle(point, size);
                    var location = new Point(point.X + (size.Width - size2.Width) / 2,
                        point.Y + (size.Height - size2.Height) / 2);
                    var rectangle2 = new Rectangle(location, size2);
                    path.AddArc(rect, num2, 91f);
                    if (Main.TRadius > 3)
                    {
                        path.AddArc(rectangle2, num2 + 90f, -91f);
                    }
                    else
                    {
                        path.AddLine(point2, point3);
                    }
                    using (var brush = new PathGradientBrush(path))
                    {
                        var colorArray = new Color[2];
                        var numArray = new float[2];
                        var blend = new ColorBlend();
                        colorArray[0] = CornerColors[1];
                        colorArray[1] = CornerColors[0];
                        numArray[0] = 0f;
                        numArray[1] = 1f;
                        blend.Colors = colorArray;
                        blend.Positions = numArray;
                        brush.InterpolationColors = blend;
                        brush.CenterPoint = tf;
                        g.FillPath(brush, path);
                    }
                }
            }
            action(1);
            action(3);
            action(7);
            action(9);
        }

        private void DrawLines(Graphics g, Size corSize, Size gradientSize_LR, Size gradientSize_TB)
        {
            if (gradientSize_LR.Width == 0 || gradientSize_LR.Height == 0) return;
            if (gradientSize_TB.Width == 0 || gradientSize_TB.Height == 0) return;
            var rect = new Rectangle(new Point(corSize.Width, 0), gradientSize_TB);
            var rectangle2 = new Rectangle(new Point(0, corSize.Width), gradientSize_LR);
            var rectangle3 = new Rectangle(new Point(Size.Width - (Main.TRadius + 1), corSize.Width), gradientSize_LR);
            var rectangle4 = new Rectangle(new Point(corSize.Width, Size.Height - (Main.TRadius + 1)), gradientSize_TB);
            using (var brush = new LinearGradientBrush(rect, ShadowColors[1], ShadowColors[0], LinearGradientMode.Vertical))
            using (var brush2 = new LinearGradientBrush(rectangle2, ShadowColors[1], ShadowColors[0], LinearGradientMode.Horizontal))
            using (var brush3 = new LinearGradientBrush(rectangle3, ShadowColors[0], ShadowColors[1], LinearGradientMode.Horizontal))
            using (var brush4 = new LinearGradientBrush(rectangle4, ShadowColors[0], ShadowColors[1], LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, rect);
                g.FillRectangle(brush2, rectangle2);
                g.FillRectangle(brush3, rectangle3);
                g.FillRectangle(brush4, rectangle4);
            }
        }

        private void DrawShadow(Graphics g)
        {
            ShadowColors[0] = Color.FromArgb(alpha, Main.TShadowColor);
            CornerColors[0] = Color.FromArgb(alpha * 3, Main.TShadowColor);
            var corSize = new Size(Main.TRadius + 1 + Main.TRadius, Main.TRadius + 1 + Main.TRadius);
            var size2 = new Size(Main.TRadius + 1, Size.Height - corSize.Height * 2);
            var size4 = new Size(Size.Width - corSize.Width * 2, Main.TRadius + 1);
            DrawLines(g, corSize, size2, size4);
            DrawCorners(g, corSize);
        }

        private void Init()
        {
            TopMost = Main.TopMost;
            Main.BringToFront();
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(Main.Location.X - (Main.TRadius + 1), Main.Location.Y - (Main.TRadius + 1));
            Icon = Main.Icon;
            ShowIcon = Main.ShowIcon;
            Width = Main.Width + (Main.TRadius + 1) * 2;
            Height = Main.Height + (Main.TRadius + 1) * 2;
            Text = Main.Text;
            Main.LocationChanged += Main_LocationChanged;
            Main.SizeChanged += Main_SizeChanged;
            Main.VisibleChanged += Main_VisibleChanged;
            Main.FormClosed += Main_FormClosed;
            SetBits();
            CanPenetrate();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // SkinForm
            // 
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(259, 271);
            Name = "SkinForm";
            Text = "SkinForm";
            TopMost = true;
            ResumeLayout(false);
        }

        private void Main_LocationChanged(object sender, EventArgs e)
        {
            Location = new Point(Main.Left - (Main.TRadius + 1), Main.Top - (Main.TRadius + 1));
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            Width = Main.Width + (Main.TRadius + 1) * 2;
            Height = Main.Height + (Main.TRadius + 1) * 2;
            SetBits();
        }

        private void Main_VisibleChanged(object sender, EventArgs e)
        {
            if (!IsDisposed) Visible = Main.Visible;
        }

        /// <summary>
        /// </summary>
        public void SetBits()
        {
            using (var image = new Bitmap(Main.Width + (Main.TRadius + 1) * 2, Main.Height + (Main.TRadius + 1) * 2))
            {
                var g = Graphics.FromImage(image);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                DrawShadow(g);
                if (Image.IsCanonicalPixelFormat(image.PixelFormat) && Image.IsAlphaPixelFormat(image.PixelFormat))
                {
                    var screenDc = NativeMethods.GetDC(IntPtr.Zero);
                    var memDc = NativeMethods.CreateCompatibleDC(screenDc);
                    var hBitmap = IntPtr.Zero;
                    var oldBitmap = IntPtr.Zero;
                    try
                    {
                        hBitmap = image.GetHbitmap(Color.FromArgb(0));
                        oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

                        var psize = new SIZE(Width, Height);
                        var pointSource = new POINT(0, 0);
                        var topPos = new POINT(Left, Top);
                        var blend = new BLENDFUNCTION()
                        {
                            BlendOp = Consts.AC_SRC_OVER,
                            BlendFlags = 0,
                            SourceConstantAlpha = byte.Parse("255"),
                            AlphaFormat = Consts.AC_SRC_ALPHA
                        };
                        if (!IsDisposed)
                        {
                            NativeMethods.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref psize, memDc,
                                ref pointSource, 0, ref blend, 2);
                        }
                        return;
                    }
                    finally
                    {
                        NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
                        if (hBitmap != IntPtr.Zero)
                        {
                            NativeMethods.SelectObject(memDc, oldBitmap);
                            NativeMethods.DeleteObject(hBitmap);
                        }
                        NativeMethods.DeleteDC(memDc);
                    }
                }
            }
            throw new ArgumentException("图片必须是32位带Alhpa通道的图片。");
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 处理 Windows 消息。
        /// 邮阴影部分手动主窗体
        /// </summary>
        /// <param name="m">要处理的 WindowsMessage。</param>
        protected override void WndProc(ref Message m)
        {
            if (!Main.IResize)
            {
                base.WndProc(ref m);
                return;
            }
            switch (m.Msg)
            {
                case (int)WindowsMessage.WM_NCHITTEST:
                    base.WndProc(ref m);
                    Main.WmNcHitTest(ref m);
                    break;
                case (int)WindowsMessage.WM_SHOWWINDOW:
                    base.WndProc(ref m);
                    break;
                default:
                    if (!IsDisposed)
                    {
                        m.HWnd = Main.Handle;
                    }
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}