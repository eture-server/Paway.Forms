﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 关于 GDI+ 绘图的辅助类
    /// </summary>
    public abstract class DrawHelper
    {
        /// <summary>
        /// 绘制边框
        /// </summary>
        public static void DrawImage(Graphics g, Image image, int x1, int y1, int width1, int height1, int x2, int y2,
            int width2, int height2)
        {
            g.DrawImage(image, new Rectangle(x1, y1, width1, height1), x2, y2, width2, height2, GraphicsUnit.Pixel);
        }

        #region 一些默认参数
        /// <summary>
        /// 字符串垂直居中对齐
        /// </summary>
        public static StringFormat Center
        {
            get
            {
                return StringFormat();
            }
        }
        /// <summary>
        /// 字符串垂直居中对齐
        /// </summary>
        public static StringFormat VerticalCenter
        {
            get
            {
                return StringFormat(StringAlignment.Near);
            }
        }
        /// <summary>
        /// 文本对齐格式
        /// </summary>
        public static StringFormat StringFormat(StringAlignment alignment = StringAlignment.Center, StringAlignment lineAlignment = StringAlignment.Center)
        {
            var format = new StringFormat
            {
                Alignment = alignment,
                LineAlignment = lineAlignment
            };
            return format;
        }

        /// <summary>
        /// 文本垂直居中对齐(结尾省略)
        /// </summary>
        public static TextFormatFlags TextVerticalCenter
        {
            get
            {
                return TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
            }
        }
        /// <summary>
        /// 文本水平垂直居中对齐(结尾省略)
        /// </summary>
        public static TextFormatFlags TextCenter
        {
            get
            {
                return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis;
            }
        }

        #endregion

        #region RendererBackground 渲染背景图片，使背景图片不失真
        /// <summary>
        /// 渲染背景图片,使背景图片不失真
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="backgroundImage"></param>
        /// <param name="method"></param>
        public static void RendererBackground(Graphics g, Rectangle rect, Image backgroundImage, bool method)
        {
            if (!method)
            {
                g.DrawImage(backgroundImage, new Rectangle(rect.X + 0, rect.Y, 5, rect.Height), 0, 0, 5,
                    backgroundImage.Height, GraphicsUnit.Pixel);
                g.DrawImage(backgroundImage, new Rectangle(rect.X + 5, rect.Y, rect.Width - 10, rect.Height), 5, 0,
                    backgroundImage.Width - 10, backgroundImage.Height, GraphicsUnit.Pixel);
                g.DrawImage(backgroundImage, new Rectangle(rect.Right - 5, rect.Y, 5, rect.Height),
                    backgroundImage.Width - 5, 0, 5, backgroundImage.Height, GraphicsUnit.Pixel);
            }
            else
            {
                RendererBackground(g, rect, 5, backgroundImage);
            }
        }

        /// <summary>
        /// 渲染背景图片,使背景图片不失真
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="cut"></param>
        /// <param name="backgroundImage"></param>
        public static void RendererBackground(Graphics g, Rectangle rect, int cut, Image backgroundImage)
        {
            //左上角
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y, cut, cut), 0, 0, cut, cut, GraphicsUnit.Pixel);
            //上边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Y, rect.Width - cut * 2, cut), cut, 0,
                backgroundImage.Width - cut * 2, cut, GraphicsUnit.Pixel);
            //右上角
            g.DrawImage(backgroundImage, new Rectangle(rect.Right - cut, rect.Y, cut, cut), backgroundImage.Width - cut,
                0, cut, cut, GraphicsUnit.Pixel);
            //左边
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y + cut, cut, rect.Height - cut * 2), 0, cut, cut,
                backgroundImage.Height - cut * 2, GraphicsUnit.Pixel);
            //左下角
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Bottom - cut, cut, cut), 0,
                backgroundImage.Height - cut, cut, cut, GraphicsUnit.Pixel);
            //右边
            g.DrawImage(backgroundImage, new Rectangle(rect.Right - cut, rect.Y + cut, cut, rect.Height - cut * 2),
                backgroundImage.Width - cut, cut, cut, backgroundImage.Height - cut * 2, GraphicsUnit.Pixel);
            //右下角
            g.DrawImage(backgroundImage, new Rectangle(rect.Right - cut, rect.Bottom - cut, cut, cut),
                backgroundImage.Width - cut, backgroundImage.Height - cut, cut, cut, GraphicsUnit.Pixel);
            //下边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Bottom - cut, rect.Width - cut * 2, cut), cut,
                backgroundImage.Height - cut, backgroundImage.Width - cut * 2, cut, GraphicsUnit.Pixel);
            //平铺中间
            g.DrawImage(backgroundImage,
                new Rectangle(rect.X + cut, rect.Y + cut, rect.Width - cut * 2, rect.Height - cut * 2), cut, cut,
                backgroundImage.Width - cut * 2, backgroundImage.Height - cut * 2, GraphicsUnit.Pixel);
        }

        #endregion

        #region CreateRoundPath 构建圆角路径
        /// <summary>
        /// 构建圆角路径
        /// </summary>
        public static GraphicsPath CreateRoundPath(Rectangle rect, Padding radiu, int line = 0)
        {
            var line2 = (line) / 2;
            if (line > 0) line -= 1;

            var radiuRect = new GraphicsPath();
            if (radiu.Top > 0) radiuRect.AddArc(rect.X + line2, rect.Y + line2, radiu.Top * 2, radiu.Top * 2, 180, 90);
            else radiuRect.AddLine(rect.X + radiu.Top + line2, rect.Y + line2, rect.Right - radiu.Top - radiu.Right - line2 + line, rect.Y + line2);

            if (radiu.Right > 0) radiuRect.AddArc(rect.Right - radiu.Right * 2 + line2, rect.Y + line2, radiu.Right * 2, radiu.Right * 2, 270, 90);
            else radiuRect.AddLine(rect.Right - line2 + line, rect.Y + radiu.Right * 2 + line2, rect.Right - line2 + line, rect.Bottom - radiu.Bottom * 2 - line2 + line);

            if (radiu.Bottom > 0) radiuRect.AddArc(rect.Right - radiu.Bottom * 2 + line2, rect.Bottom - radiu.Bottom * 2 + line2, radiu.Bottom * 2, radiu.Bottom * 2, 0, 90);
            else radiuRect.AddLine(rect.Right - radiu.Bottom * 2 - line2 + line, rect.Bottom - line2 + line, rect.X + radiu.Left * 2 + line2, rect.Bottom - line2 + line);

            if (radiu.Left > 0) radiuRect.AddArc(rect.X + line2, rect.Bottom - radiu.Left * 2 + line2, radiu.Left * 2, radiu.Left * 2, 90, 90);
            else radiuRect.AddLine(rect.X + line2, rect.Bottom - radiu.Left * 2 - line2 + line - 1, rect.X + line2, rect.Y + radiu.Top * 2 + line2);

            radiuRect.CloseFigure();
            return radiuRect;
        }

        /// <summary>
        /// 构建下圆角路径
        /// </summary>
        public static void CreateBelowPath(Graphics g, Rectangle rect, Color color)
        {
            using (var pen = new Pen(new SolidBrush(color)))
            using (var path = CreateBelowPath(new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1), 3))
            {
                g.DrawPath(pen, path);
            }
        }

        /// <summary>
        /// 构建下圆角路径
        /// </summary>
        public static GraphicsPath CreateBelowPath(Rectangle rect, int cornerRadius)
        {
            var roundedRect = new GraphicsPath();
            roundedRect.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - cornerRadius);
            roundedRect.AddArc(rect.Right - cornerRadius - 1, rect.Bottom - cornerRadius - 1, cornerRadius, cornerRadius,
                0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius, rect.Bottom, rect.X + cornerRadius, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius - 1, cornerRadius, cornerRadius, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius, rect.X, rect.Y);
            roundedRect.CloseFigure();
            return roundedRect;
        }

        /// <summary>
        /// 构建圆角路径
        /// </summary>
        /// <param name="r"></param>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="r3"></param>
        /// <param name="r4"></param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundRect(RectangleF r, float r1, float r2, float r3, float r4)
        {
            var x = r.X;
            var y = r.Y;
            var width = r.Width;
            var height = r.Height;
            var path = new GraphicsPath();
            path.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y);
            path.AddLine(x + r1, y, x + width - r2, y);
            path.AddBezier(x + width - r2, y, x + width, y, x + width, y + r2, x + width, y + r2);
            path.AddLine(x + width, y + r2, x + width, y + height - r3);
            path.AddBezier(x + width, y + height - r3, x + width, y + height, x + width - r3, y + height, x + width - r3,
                y + height);
            path.AddLine(x + width - r3, y + height, x + r4, y + height);
            path.AddBezier(x + r4, y + height, x, y + height, x, y + height - r4, x, y + height - r4);
            path.AddLine(x, y + height - r4, x, y + r1);
            return path;
        }

        #endregion
    }
}