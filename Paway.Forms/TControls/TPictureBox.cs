using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Helper;

namespace Paway.Forms.TControls
{
    /// <summary>
    /// 自定PictureBox，添加缩放
    /// </summary>
    public class TPictureBox : PictureBox
    {
        #region 属性
        /// <summary>
        /// 用于展示的图片
        /// </summary>
        private Image screen;
        /// <summary>
        /// 拖动标记
        /// </summary>
        private bool isMove = false;
        /// <summary>
        /// 图片绘制区域
        /// </summary>
        private Rectangle rect = Rectangle.Empty;
        /// <summary>
        /// 图片显示大小
        /// </summary>
        private Size size = Size.Empty;
        /// <summary>
        /// 图片是否走出窗体容纳
        /// </summary>
        private bool isExceed = false;
        /// <summary>
        /// 起点
        /// </summary>
        private Point pStart = Point.Empty;
        /// <summary>
        /// 放大缩小时定点
        /// </summary>
        private Point point = Point.Empty;
        /// <summary>
        /// 图片宽高比
        /// </summary>
        private double ratio = 0.0;
        /// <summary>
        /// 放大缩小时比例
        /// </summary>
        private double dx = 0, dy = 0;
        /// <summary>
        /// 获取或设置由 System.Windows.Forms.PictureBox 显示的图像
        /// </summary>
        public new Image Image
        {
            get { return this.screen; }
            set
            {
                this.screen = value;
                if (screen == null)
                {
                    rect = Rectangle.Empty;
                }
                else if (rect == Rectangle.Empty)
                {
                    this.ratio = this.screen.Width * 1.0 / this.screen.Height;
                    this.size = value.Size;
                    if (size.Width > this.Width)
                    {
                        size.Width = this.Width;
                        size.Height = (size.Width / this.ratio).ToInt();
                    }
                    if (size.Height > this.Height)
                    {
                        size.Height = this.Height;
                        size.Width = (this.ratio * size.Height).ToInt();
                    }
                    this.rect = new Rectangle((this.Width - size.Width) / 2, (this.Height - size.Height) / 2, size.Width, size.Height);
                }
                this.Refresh();
            }
        }

        #endregion

        #region public
        /// <summary>
        /// 获取原图坐标点
        /// </summary>
        public Point GetPoint(Point point)
        {
            Point temp = Point.Empty;
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

        #region override
        /// <summary>
        /// 重绘绘制方法
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            if (screen != null)
            {
                Graphics g = pe.Graphics;
                //g.DrawImage(screen, rect);

                // 设置画布的描绘质量 - 最临近插值法(显示像素点)
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(screen, rect, new Rectangle(Point.Empty, screen.Size), GraphicsUnit.Pixel);
                g.PixelOffsetMode = PixelOffsetMode.Default;
            }
            base.OnPaint(pe);
        }
        /// <summary>
        /// 滚动鼠标缩放
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
        /// 停止移动
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isMove = false;
        }
        /// <summary>
        /// 移动
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
        /// 移动开始点，获取焦点，允许滚动
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
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
        /// 放大或缩小
        /// </summary>
        /// <param name="steps">鼠标滚轮步进数</param>
        private void Reset(int steps)
        {
            if (screen == null) return;
            if (size.Width / screen.Width > 16 && steps > 0) return;
            if (screen.Width / size.Width > 20 && steps < 0) return;
            this.Invalidate(rect);
            double bit = 1;
            for (int i = 0; i < Math.Abs(steps); i++)
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

            rect = new Rectangle((this.Width - size.Width) / 2, (this.Height - size.Height) / 2, size.Width, size.Height);
            isExceed = rect.X < 0 || rect.Y < 0;
            if (isExceed)
            {
                if (rect.Width > this.Width)
                {
                    rect.X = (point.X - rect.Width * dx).ToInt();
                    if (rect.X > 0) rect.X = 0;
                    if (rect.X + rect.Width < this.Width) rect.X = this.Width - rect.Width;
                }
                if (rect.Height > this.Height)
                {
                    rect.Y = (point.Y - rect.Height * dy).ToInt();
                    if (rect.Y > 0) rect.Y = 0;
                    if (rect.Y + rect.Height < this.Height) rect.Y = this.Height - rect.Height;
                }
            }
            this.Invalidate(rect);
        }

        /// <summary>
        /// 移动图片
        /// </summary>
        /// <param name="pMove">鼠标落点</param>
        private void Reset(Point pMove)
        {
            this.Invalidate(rect);
            int x = 0, y = 0;
            if (rect.Width > this.Width)
            {
                x = rect.X + pMove.X - pStart.X;
                if (x > 0) x = 0;
                if (x + rect.Width < this.Width) x = this.Width - rect.Width;
            }
            else
            {
                x = rect.X;
            }
            if (rect.Height > this.Height)
            {
                y = rect.Y + pMove.Y - pStart.Y;
                if (y > 0) y = 0;
                if (y + rect.Height < this.Height) y = this.Height - rect.Height;
            }
            else
            {
                y = rect.Y;
            }
            rect = new Rectangle(x, y, rect.Width, rect.Height);
            this.Invalidate(rect);
        }

        #endregion
    }
}
