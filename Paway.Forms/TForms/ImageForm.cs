using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Forms.Properties;
using Paway.Helper;
using Paway.Win32;

namespace Paway.Forms
{
    /// <summary>
    ///     图片展示
    /// </summary>
    public partial class ImageForm : QQForm
    {
        #region 构造
        /// <summary>
        ///     构造
        ///     设置窗体Size
        /// </summary>
        /// <param name="screen"></param>
        public ImageForm(Image screen)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            UpdateStyles();
            InitializeComponent();
            this.screen = screen;
            if (screen == null) this.screen = Resources.Controls_blank;
            else this.screen = screen;
            Cursor = Cursors.Hand;
            ratio = this.screen.Width * 1.0 / this.screen.Height;
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
            TextShow = string.Format("{0:F2}", size.Width * 1.0 / screen.Width);
            rect = new Rectangle((Width - size.Width) / 2, (Height - size.Height) / 2, size.Width, size.Height);
            StartPosition = FormStartPosition.CenterScreen;
            CenterToScreen();
            SetBitmap();
        }

        #endregion

        #region 变量

        /// <summary>
        ///     用于展示的图片
        /// </summary>
        private readonly Image screen;

        /// <summary>
        ///     拖动标记
        /// </summary>
        private bool isMove;

        /// <summary>
        ///     图片绘制区域
        /// </summary>
        private Rectangle rect = Rectangle.Empty;

        /// <summary>
        ///     图片显示大小
        /// </summary>
        private Size size = Size.Empty;

        /// <summary>
        ///     图片宽高比
        /// </summary>
        private readonly double ratio;

        /// <summary>
        ///     图片是否走出窗体容纳
        /// </summary>
        private bool isExceed;

        /// <summary>
        ///     起点
        /// </summary>
        private Point pStart = Point.Empty;

        /// <summary>
        ///     放大缩小时定点
        /// </summary>
        private Point point = Point.Empty;

        /// <summary>
        ///     放大缩小时比例
        /// </summary>
        private double dx, dy;

        #endregion

        #region 方法

        /// <summary>
        ///     放大或缩小
        /// </summary>
        /// <param name="steps">鼠标滚轮步进数</param>
        private void Reset(int steps)
        {
            if (screen == null) return;
            if (size.Width / screen.Width > 16 && steps > 0) return;
            if (screen.Width / size.Width > 20 && steps < 0) return;
            Invalidate(rect);
            double bit = 1;
            for (var i = 0; i < Math.Abs(steps); i++)
            {
                bit *= 1.2;
            }
            if (steps > 0)
            {
                size.Width = (size.Width * bit).ToInt();
                size.Height = (size.Height * bit).ToInt();
            }
            else
            {
                size.Width = (size.Width / bit).ToInt();
                size.Height = (size.Height / bit).ToInt();
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
            TextShow = string.Format("{0:F2}", size.Width * 1.0 / screen.Width);

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
        ///     移动图片
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

        #region Override Methods

        /// <summary>
        ///     重绘窗体
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
                g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), new Rectangle(Point.Empty, bitmap.Size));
                g.DrawImage(screen, rect, new Rectangle(Point.Empty, screen.Size), GraphicsUnit.Pixel);
                DrawText(g);
                DrawButton(g, CloseState, CloseRect, "close");
                DrawButton(g, MaxState, MaxRect, "max");
            }
            SetBitmap(bitmap, 255);
        }

        /// <summary>
        ///     按下
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
        ///     拖动
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
        ///     松开
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isMove = false;
        }

        /// <summary>
        ///     放大缩小
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
        ///     按键关闭窗体
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
        ///     Size改变时重置控件内空白区域
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Padding = new Padding(0);
            this.Reset(0);
        }

        /// <summary>
        ///     关闭时激发父窗体
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