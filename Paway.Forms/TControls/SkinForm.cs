using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 窗体阴影
    /// </summary>
    public class SkinForm : Form
    {
        private TForm Main;
        private Color[] CornerColors;
        private Color[] ShadowColors;

        /// <summary>
        /// 构造
        /// </summary>
        public SkinForm(TForm main)
        {
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
            this.ShadowColors = new Color[] { Color.FromArgb(60, Color.Black), Color.Transparent };
            this.CornerColors = new Color[] { Color.FromArgb(180, Color.Black), Color.Transparent };
            this.Main = main;
            this.InitializeComponent();
            this.Init();
        }

        #region 新加 - 移动窗体
        /// <summary>
        /// 移动窗体
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.IsDisposed) return;
            if (this.Main.WindowState != FormWindowState.Maximized)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(this.Main.Handle, (int)WindowsMessage.WM_SYSCOMMAND, (int)WindowsMessage.SC_MOVE, 0);
            }
        }

        #endregion

        private void CanPenetrate()
        {
            //NativeMethods.GetWindowLong(base.Handle, -20);
            //NativeMethods.SetWindowLong(base.Handle, -20, 0x80020);
        }

        private void DrawCorners(Graphics g, System.Drawing.Size corSize)
        {
            Action<int> action = delegate(int n)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    System.Drawing.Point point;
                    float num2;
                    PointF tf;
                    System.Drawing.Point point2;
                    System.Drawing.Point point3;
                    System.Drawing.Size size = new System.Drawing.Size(corSize.Width * 2, corSize.Height * 2);
                    System.Drawing.Size size2 = new System.Drawing.Size(Main.TRadius * 2, Main.TRadius * 2);
                    switch (n)
                    {
                        case 1:
                            point = new System.Drawing.Point(0, 0);
                            num2 = 180f;
                            tf = new PointF(size.Width - (size2.Width * 0.5f), size.Height - (size2.Height * 0.5f));
                            point2 = new System.Drawing.Point(corSize.Width, Main.TRadius + 1);
                            point3 = new System.Drawing.Point(Main.TRadius + 1, corSize.Height);
                            break;

                        case 3:
                            point = new System.Drawing.Point(this.Width - size.Width, 0);
                            num2 = 270f;
                            tf = new PointF(point.X + (size2.Width * 0.5f), size.Height - (size2.Height * 0.5f));
                            point2 = new System.Drawing.Point(this.Width - (Main.TRadius + 1), corSize.Height);
                            point3 = new System.Drawing.Point(this.Width - corSize.Width, Main.TRadius + 1);
                            break;

                        case 7:
                            point = new System.Drawing.Point(0, this.Height - size.Height);
                            num2 = 90f;
                            tf = new PointF(size.Width - (size2.Width * 0.5f), point.Y + (size2.Height * 0.5f));
                            point2 = new System.Drawing.Point(Main.TRadius + 1, this.Height - corSize.Height);
                            point3 = new System.Drawing.Point(corSize.Width, this.Height - (Main.TRadius + 1));
                            break;

                        default:
                            point = new System.Drawing.Point(this.Width - size.Width, this.Height - size.Height);
                            num2 = 0f;
                            tf = new PointF(point.X + (size2.Width * 0.5f), point.Y + (size2.Height * 0.5f));
                            point2 = new System.Drawing.Point(this.Width - corSize.Width, this.Height - (Main.TRadius + 1));
                            point3 = new System.Drawing.Point(this.Width - (Main.TRadius + 1), this.Height - corSize.Height);
                            break;
                    }
                    Rectangle rect = new Rectangle(point, size);
                    System.Drawing.Point location = new System.Drawing.Point(point.X + ((size.Width - size2.Width) / 2), point.Y + ((size.Height - size2.Height) / 2));
                    Rectangle rectangle2 = new Rectangle(location, size2);
                    path.AddArc(rect, num2, 91f);
                    if (Main.TRadius > 3)
                    {
                        path.AddArc(rectangle2, num2 + 90f, -91f);
                    }
                    else
                    {
                        path.AddLine(point2, point3);
                    }
                    using (PathGradientBrush brush = new PathGradientBrush(path))
                    {
                        Color[] colorArray = new Color[2];
                        float[] numArray = new float[2];
                        ColorBlend blend = new ColorBlend();
                        colorArray[0] = this.CornerColors[1];
                        colorArray[1] = this.CornerColors[0];
                        numArray[0] = 0f;
                        numArray[1] = 1f;
                        blend.Colors = colorArray;
                        blend.Positions = numArray;
                        brush.InterpolationColors = blend;
                        brush.CenterPoint = tf;
                        g.FillPath(brush, path);
                    }
                }
            };
            action(1);
            action(3);
            action(7);
            action(9);
        }

        private void DrawLines(Graphics g, System.Drawing.Size corSize, System.Drawing.Size gradientSize_LR, System.Drawing.Size gradientSize_TB)
        {
            Rectangle rect = new Rectangle(new System.Drawing.Point(corSize.Width, 0), gradientSize_TB);
            Rectangle rectangle2 = new Rectangle(new System.Drawing.Point(0, corSize.Width), gradientSize_LR);
            Rectangle rectangle3 = new Rectangle(new System.Drawing.Point(base.Size.Width - (Main.TRadius + 1), corSize.Width), gradientSize_LR);
            Rectangle rectangle4 = new Rectangle(new System.Drawing.Point(corSize.Width, base.Size.Height - (Main.TRadius + 1)), gradientSize_TB);
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, this.ShadowColors[1], this.ShadowColors[0], LinearGradientMode.Vertical))
            {
                using (LinearGradientBrush brush2 = new LinearGradientBrush(rectangle2, this.ShadowColors[1], this.ShadowColors[0], LinearGradientMode.Horizontal))
                {
                    using (LinearGradientBrush brush3 = new LinearGradientBrush(rectangle3, this.ShadowColors[0], this.ShadowColors[1], LinearGradientMode.Horizontal))
                    {
                        using (LinearGradientBrush brush4 = new LinearGradientBrush(rectangle4, this.ShadowColors[0], this.ShadowColors[1], LinearGradientMode.Vertical))
                        {
                            g.FillRectangle(brush, rect);
                            g.FillRectangle(brush2, rectangle2);
                            g.FillRectangle(brush3, rectangle3);
                            g.FillRectangle(brush4, rectangle4);
                        }
                    }
                }
            }
        }

        private void DrawShadow(Graphics g)
        {
            this.ShadowColors[0] = Color.FromArgb(60, Main.TShadowColor);
            this.CornerColors[0] = Color.FromArgb(180, Main.TShadowColor);
            System.Drawing.Size corSize = new System.Drawing.Size((Main.TRadius + 1) + Main.TRadius, (Main.TRadius + 1) + Main.TRadius);
            System.Drawing.Size size2 = new System.Drawing.Size(Main.TRadius + 1, base.Size.Height - (corSize.Height * 2));
            System.Drawing.Size size4 = new System.Drawing.Size(base.Size.Width - (corSize.Width * 2), Main.TRadius + 1);
            this.DrawLines(g, corSize, size2, size4);
            this.DrawCorners(g, corSize);
        }

        private void Init()
        {
            base.TopMost = this.Main.TopMost;
            this.Main.BringToFront();
            base.ShowInTaskbar = false;
            base.FormBorderStyle = FormBorderStyle.None;
            base.Location = new System.Drawing.Point(this.Main.Location.X - (Main.TRadius + 1), this.Main.Location.Y - (Main.TRadius + 1));
            base.Icon = this.Main.Icon;
            base.ShowIcon = this.Main.ShowIcon;
            base.Width = this.Main.Width + ((Main.TRadius + 1) * 2);
            base.Height = this.Main.Height + ((Main.TRadius + 1) * 2);
            this.Text = this.Main.Text;
            this.Main.LocationChanged += new EventHandler(this.Main_LocationChanged);
            this.Main.SizeChanged += new EventHandler(this.Main_SizeChanged);
            this.Main.VisibleChanged += new EventHandler(this.Main_VisibleChanged);
            this.Main.FormClosing += Main_FormClosing;
            this.SetBits();
            this.CanPenetrate();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SkinForm
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(259, 271);
            this.Name = "SkinForm";
            this.Text = "SkinForm";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        private void Main_LocationChanged(object sender, EventArgs e)
        {
            base.Location = new System.Drawing.Point(this.Main.Left - (Main.TRadius + 1), this.Main.Top - (Main.TRadius + 1));
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            base.Width = this.Main.Width + ((Main.TRadius + 1) * 2);
            base.Height = this.Main.Height + ((Main.TRadius + 1) * 2);
            this.SetBits();
        }

        private void Main_VisibleChanged(object sender, EventArgs e)
        {
            base.Visible = this.Main.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetBits()
        {
            Bitmap image = new Bitmap(this.Main.Width + ((Main.TRadius + 1) * 2), this.Main.Height + ((Main.TRadius + 1) * 2));
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            this.DrawShadow(g);
            if (Image.IsCanonicalPixelFormat(image.PixelFormat) && Image.IsAlphaPixelFormat(image.PixelFormat))
            {
                IntPtr screenDc = NativeMethods.GetDC(IntPtr.Zero);
                IntPtr memDc = NativeMethods.CreateCompatibleDC(screenDc);
                IntPtr hBitmap = IntPtr.Zero;
                IntPtr oldBitmap = IntPtr.Zero;
                try
                {
                    hBitmap = image.GetHbitmap(Color.FromArgb(0));
                    oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

                    SIZE psize = new SIZE(base.Width, base.Height);
                    POINT pointSource = new POINT(0, 0);
                    POINT topPos = new POINT(base.Left, base.Top);
                    BLENDFUNCTION blend = new BLENDFUNCTION();
                    blend.BlendOp = Consts.AC_SRC_OVER;
                    blend.BlendFlags = 0;
                    blend.SourceConstantAlpha = byte.Parse("255");
                    blend.AlphaFormat = Consts.AC_SRC_ALPHA;

                    if (!this.IsDisposed)
                    {
                        NativeMethods.UpdateLayeredWindow(base.Handle, screenDc, ref topPos, ref psize, memDc, ref pointSource, 0, ref blend, 2);
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
            throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
        }

        /// <summary>
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= (int)WindowStyle.WS_SYSMENU;
                return createParams;
            }
        }
        void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 处理 Windows 消息。
        /// 邮阴影部分手动主窗体
        /// </summary>
        /// <param name="m">要处理的 WindowsMessage。</param>
        protected override void WndProc(ref Message m)
        {
            if (!this.Main.IResize)
            {
                base.WndProc(ref m);
                return;
            }
            if (!this.IsDisposed)
            {
                m.HWnd = this.Main.Handle;
            }
            switch (m.Msg)
            {
                case (int)WindowsMessage.WM_NCHITTEST:
                    base.WndProc(ref m);
                    this.Main.WmNcHitTest(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}
