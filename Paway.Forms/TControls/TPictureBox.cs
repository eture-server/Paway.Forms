using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Paway.Helper;

namespace Paway.Forms.TControls
{
    /// <summary>
    ///     自定PictureBox，添加缩放
    /// </summary>
    public class TPictureBox : PictureBox
    {
        #region public

        /// <summary>
        ///     获取原图坐标点
        /// </summary>
        public Point GetPoint(Point point)
        {
            var temp = Point.Empty;
            if (screen != null)
            {
                temp.X = (point.X - rect.X) * screen.Width / size.Width;
                temp.Y = (point.Y - rect.Y) * screen.Width / size.Width;
                if (temp.X > screen.Width - 1) temp.X = screen.Width - 1;
                if (temp.Y > screen.Height - 1) temp.Y = screen.Height - 1;
            }
            return temp;
        }

        #endregion

        #region 属性

        /// <summary>
        ///     用于展示的图片
        /// </summary>
        private Image screen;

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
        ///     图片宽高比
        /// </summary>
        private double ratio;

        /// <summary>
        ///     放大缩小时比例
        /// </summary>
        private double dx, dy;

        /// <summary>
        ///     获取或设置由 System.Windows.Forms.PictureBox 显示的图像
        /// </summary>
        public new Image Image
        {
            get { return screen; }
            set
            {
                screen = value;
                if (screen == null)
                {
                    rect = Rectangle.Empty;
                }
                else if (rect == Rectangle.Empty)
                {
                    ratio = screen.Width * 1.0 / screen.Height;
                    size = value.Size;
                    if (size.Width > Width)
                    {
                        size.Width = Width;
                        size.Height = (size.Width / ratio).ToInt();
                    }
                    if (size.Height > Height)
                    {
                        size.Height = Height;
                        size.Width = (ratio * size.Height).ToInt();
                    }
                    rect = new Rectangle((Width - size.Width) / 2, (Height - size.Height) / 2, size.Width, size.Height);
                }
                Refresh();
            }
        }

        #endregion

        #region override

        /// <summary>
        ///     重绘绘制方法
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            if (screen != null)
            {
                var g = pe.Graphics;
                //g.DrawImage(screen, rect);

                // 设置画布的描绘质量 - 最临近插值法(显示像素点)
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(screen, rect, new Rectangle(Point.Empty, screen.Size), GraphicsUnit.Pixel);
            }
            base.OnPaint(pe);
        }

        /// <summary>
        ///     滚动鼠标缩放
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

        /// <summary>
        ///     停止移动
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isMove = false;
        }

        /// <summary>
        ///     移动
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isMove)
            {
                Reset(e.Location);
                pStart = e.Location;
                return;
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        ///     移动开始点，获取焦点，允许滚动
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            if (e.Button == MouseButtons.Left)
            {
                if (isExceed && rect.Contains(e.Location))
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

        #endregion

        #region 缩放与移动

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
            int x = 0, y = 0;
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

        #region Dispose
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                screen = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}