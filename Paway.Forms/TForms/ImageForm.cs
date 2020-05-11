using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Paway.Forms.Properties;
using Paway.Helper;

namespace Paway.Forms
{
    /// <summary>
    /// 图片展示
    /// </summary>
    public partial class ImageForm : QQForm
    {
        #region 构造
        /// <summary>
        /// 构造
        /// 设置窗体Size
        /// </summary>
        /// <param name="screen"></param>
        public ImageForm(Image screen)
        {
            //不去除双缓冲则无法正常缩放
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            UpdateStyles();
            InitializeComponent();
            this.screen = screen;
            if (screen == null) this.screen = Resources.Controls_blank;
            else this.screen = screen;
            Cursor = Cursors.Hand;
            ratio = this.screen.Width * 1.0 / this.screen.Height;
            Reset();
            StartPosition = FormStartPosition.CenterScreen;
            CenterToScreen();
            SetBitmap();
            this.toolReset.Click += ToolReset_Click;
            this.toolAuto.Click += ToolAuto_Click;
            this.toolSave.Click += ToolSave_Click;
        }

        #endregion

        #region 变量

        /// <summary>
        /// 用于展示的图片
        /// </summary>
        private readonly Image screen;

        /// <summary>
        /// 拖动标记
        /// </summary>
        private bool isMove;

        /// <summary>
        /// 图片绘制区域
        /// </summary>
        private Rectangle rect = Rectangle.Empty;

        /// <summary>
        /// 图片显示大小
        /// </summary>
        private Size size = Size.Empty;

        /// <summary>
        /// 图片宽高比
        /// </summary>
        private readonly double ratio;

        /// <summary>
        /// 图片是否走出窗体容纳
        /// </summary>
        private bool isExceed;

        /// <summary>
        /// 起点
        /// </summary>
        private Point pStart = Point.Empty;

        /// <summary>
        /// 放大缩小时定点
        /// </summary>
        private Point point = Point.Empty;

        /// <summary>
        /// 放大缩小时比例
        /// </summary>
        private double dx, dy;

        #endregion

        #region 方法

        /// <summary>
        /// 放大或缩小
        /// </summary>
        /// <param name="steps">鼠标滚轮步进数</param>
        private void Reset(int steps)
        {
            if (screen == null) return;
            Invalidate(rect);
            double bit = 1;
            for (var i = 0; i < Math.Abs(steps); i++)
            {
                bit *= 1.2;
            }
            if (steps > 0)
            {
                int width = (size.Width * bit).ToInt();
                int height = (size.Height * bit).ToInt();
                if (width / screen.Width > 16.0)
                {
                    width = screen.Width * 16;
                    height = screen.Height * 16;
                }
                if (size.Width == width) return;
                size.Width = width;
                size.Height = height;
            }
            else
            {
                int width = (size.Width / bit).ToInt();
                int height = (size.Height / bit).ToInt();
                if (screen.Width / width > 20.0)
                {
                    width = screen.Width / 20;
                    height = screen.Height / 20;
                }
                if (size.Width == width) return;
                size.Width = width;
                size.Height = height;
            }
            if (screen.Width * 1.0 / size.Width > 0.9 && screen.Width * 1.0 / size.Width < 1.1)
            {
                size.Width = screen.Width;
                size.Height = screen.Height;
            }
            if (size.Width < 3)
            {
                size.Width = 3;
                size.Height = (size.Width / ratio).ToInt();
            }
            if (size.Height < 3)
            {
                size.Height = 3;
                size.Width = (size.Height * ratio).ToInt();
            }
            TextShow = string.Format("{0:F0}%", size.Width * 100.0 / screen.Width);

            rect = new Rectangle((Width - size.Width) / 2, (Height - size.Height) / 2, size.Width, size.Height);
            isExceed = rect.X < 0 || rect.Y < 0;
            if (isExceed)
            {
                if (rect.Width > Width)
                {
                    rect.X = (point.X - rect.Width * dx).ToInt();
                    if (rect.X > 0) rect.X = 0;
                    if (rect.X + rect.Width < Width) rect.X = Width - rect.Width;
                }
                if (rect.Height > Height)
                {
                    rect.Y = (point.Y - rect.Height * dy).ToInt();
                    if (rect.Y > 0) rect.Y = 0;
                    if (rect.Y + rect.Height < Height) rect.Y = Height - rect.Height;
                }
            }
            Invalidate(rect);
        }

        /// <summary>
        /// 移动图片
        /// </summary>
        /// <param name="pMove">鼠标落点</param>
        private void Reset(Point pMove)
        {
            Invalidate(rect);
            int x, y;
            if (rect.Width > Width)
            {
                x = rect.X + pMove.X - pStart.X;
                if (x > 0) x = 0;
                if (x + rect.Width < Width) x = Width - rect.Width;
            }
            else
            {
                x = rect.X;
            }
            if (rect.Height > Height)
            {
                y = rect.Y + pMove.Y - pStart.Y;
                if (y > 0) y = 0;
                if (y + rect.Height < Height) y = Height - rect.Height;
            }
            else
            {
                y = rect.Y;
            }
            rect = new Rectangle(x, y, rect.Width, rect.Height);
            Invalidate(rect);
        }

        #endregion

        #region private Method
        private void ToolReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void Reset()
        {
            size = this.screen.Size;
            var task = Win32Helper.TaskRect().Rect.Height;
            if (size.Height > SystemInformation.VirtualScreen.Height)
            {
                size.Height = SystemInformation.VirtualScreen.Height - task;
                size.Width = (ratio * size.Height).ToInt();
            }
            if (size.Width > SystemInformation.VirtualScreen.Width)
            {
                size.Width = SystemInformation.VirtualScreen.Width - task;
                size.Height = (size.Width / ratio).ToInt();
            }
            if (size.Width > Width && size.Height > Height)
            {
                Width = size.Width;
                Height = size.Height;
                Width = (size.Width * 1.1).ToInt();
                Height = (size.Height * 1.1).ToInt();
            }
            else if (size.Height > Height * 1.1)
            {
                Height = (size.Height * 1.1).ToInt();
            }
            else if (size.Width > Width * 1.1)
            {
                Width = (size.Width * 1.1).ToInt();
            }
            if (Height > SystemInformation.VirtualScreen.Height)
            {
                Height = SystemInformation.VirtualScreen.Height - task;
            }
            if (Width > SystemInformation.VirtualScreen.Width)
            {
                Width = SystemInformation.VirtualScreen.Width - task;
            }
            TextShow = string.Format("{0:F0}%", size.Width * 100.0 / screen.Width);
            rect = new Rectangle((Width - size.Width) / 2, (Height - size.Height) / 2, size.Width, size.Height);
        }
        private void ToolAuto_Click(object sender, EventArgs e)
        {
            size = screen.Size;
            isExceed = size.Width > Width || size.Height > Height;
            rect = new Rectangle((Width - size.Width) / 2, (Height - size.Height) / 2, size.Width, size.Height);
            TextShow = string.Format("{0:F0}%", size.Width * 100.0 / screen.Width);
        }
        private void ToolSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Jpeg|*.jpg;*.jpeg|Png|*.png|Bmp|*.bmp",
                Title = "Image Save"
            };
            if (DialogResult.OK == sfd.ShowDialog())
            {
                switch (sfd.FilterIndex)
                {
                    case 1:
                        this.screen.Save(sfd.FileName, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.screen.Save(sfd.FileName, ImageFormat.Png);
                        break;
                    case 3:
                        this.screen.Save(sfd.FileName, ImageFormat.Bmp);
                        break;
                }
            }
            sfd.Dispose();
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// 重绘窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //Graphics g = e.Graphics;

            ////普通绘制
            //g.DrawImage(screen, rect);

            //// 设置画布的描绘质量 - 最临近插值法(显示像素点)
            //g.InterpolationMode = InterpolationMode.NearestNeighbor;
            //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //g.DrawImage(screen, rect, new Rectangle(Point.Empty, screen.Size), GraphicsUnit.Pixel);
            //g.PixelOffsetMode = PixelOffsetMode.Default;

            //其它部分透明
            SetBitmap();
        }

        private void SetBitmap()
        {
            var bitmap = new Bitmap(Width, Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                // 设置画布的描绘质量 - 最临近插值法(显示像素点)
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var solidBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                {
                    g.FillRectangle(solidBrush, new Rectangle(Point.Empty, bitmap.Size));
                }
                g.DrawImage(screen, rect, new Rectangle(Point.Empty, screen.Size), GraphicsUnit.Pixel);
                DrawText(g);
                DrawButton(g, CloseState, CloseRect, "close");
                DrawButton(g, MaxState, MaxRect, "max");
            }
            SetBitmap(bitmap, 255);
        }
        /// <summary>
        /// 设置图片为窗体，透明区域根据 opacity 的值决定透明度
        /// </summary>
        /// <param name="bitmap">透明位图</param>
        /// <param name="opacity">透明度的值0~255</param>
        private void SetBitmap(Bitmap bitmap, byte opacity)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ArgumentException("The bitmap must be 32ppp with alpha-channel.");

            // The ideia of this is very simple,
            // 1. Create a compatible DC with screen;
            // 2. Select the bitmap with 32bpp with alpha-channel in the compatible DC;
            // 3. Call the UpdateLayeredWindow.

            var screenDc = NativeMethods.GetDC(IntPtr.Zero);
            var memDc = NativeMethods.CreateCompatibleDC(screenDc);
            var hBitmap = IntPtr.Zero;
            var oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0)); // grab a GDI handle from this GDI+ bitmap
                oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

                var size = new SIZE(bitmap.Width, bitmap.Height);
                var pointSource = new POINT(0, 0);
                var topPos = new POINT(Left, Top);
                var blend = new BLENDFUNCTION()
                {
                    BlendOp = Consts.AC_SRC_OVER,
                    BlendFlags = 0,
                    SourceConstantAlpha = opacity,
                    AlphaFormat = Consts.AC_SRC_ALPHA
                };
                if (!IsDisposed)
                {
                    NativeMethods.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0,
                        ref blend, Consts.ULW_ALPHA);
                }
            }
            finally
            {
                NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    NativeMethods.SelectObject(memDc, oldBitmap);
                    //Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.
                    NativeMethods.DeleteObject(hBitmap);
                }
                NativeMethods.DeleteDC(memDc);
            }
        }

        /// <summary>
        /// 按下
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 2)
                {
                    Close();
                }
                else if (isExceed && rect.Contains(e.Location) && !SysBtnRect.Contains(e.Location))
                {
                    isMove = true;
                    pStart = e.Location;
                }
            }
            if (!isMove)
            {
                base.OnMouseDown(e);
            }
        }

        /// <summary>
        /// 拖动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isMove)
            {
                Reset(e.Location);
                pStart = e.Location;
            }
            else
            {
                base.OnMouseMove(e);
            }
        }

        /// <summary>
        /// 松开
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isMove = false;
        }

        /// <summary>
        /// 放大缩小
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            point = e.Location;
            dx = (e.X - rect.X) * 1.0 / rect.Width;
            dy = (e.Y - rect.Y) * 1.0 / rect.Height;
            Reset(e.Delta / 120);
        }

        #endregion

        #region 关于窗体

        /// <summary>
        /// 按键关闭窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        /// <summary>
        /// Size改变时重置控件内空白区域
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Padding = new Padding(0);
            this.Reset(0);
        }

        /// <summary>
        /// 关闭时激发父窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Activate();
            }
            base.OnFormClosing(e);
        }

        #endregion
    }
}