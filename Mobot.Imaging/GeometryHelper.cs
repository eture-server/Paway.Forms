using System;
using System.Drawing;

namespace Mobot.Imaging
{
    /// <summary>
    /// 几何功能。
    /// </summary>
    public static class GeometryHelper
    {
        /// <summary>
        /// 取中心点。
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Point Center(Rectangle rect)
        {
            return new Point(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
        }

        public static Point ConvertLongToPoint(long point)
        {
            return ConvertLongToRectangle(point).Location;
        }

        public static Rectangle ConvertLongToRectangle(long rec)
        {
            return new Rectangle((short) ((ushort) ((rec >> 0x30) & 0xffffL)), (short) ((ushort) ((rec >> 0x20) & 0xffffL)), (short) ((ushort) ((rec >> 0x10) & 0xffffL)), (short) ((ushort) (rec & 0xffffL)));
        }

        public static Size ConvertLongToSize(long size)
        {
            return ConvertLongToRectangle(size).Size;
        }

        public static long ConvertPointToLong(Point p)
        {
            return ConvertRectangleToLong(new Rectangle(p.X, p.Y, 0, 0));
        }

        public static long ConvertRectangleToLong(Rectangle rec)
        {
            if (rec.X > 0x7fff)
            {
                rec.X = 0x7fff;
            }
            if (rec.Y > 0x7fff)
            {
                rec.Y = 0x7fff;
            }
            if (rec.Width > 0x7fff)
            {
                rec.Width = 0x7fff;
            }
            if (rec.Height > 0x7fff)
            {
                rec.Height = 0x7fff;
            }
            if (rec.X < -32766)
            {
                rec.X = -32766;
            }
            if (rec.Y < -32766)
            {
                rec.Y = -32766;
            }
            if (rec.Width < -32766)
            {
                rec.Width = -32766;
            }
            if (rec.Height < -32766)
            {
                rec.Height = -32766;
            }
            ushort x = (ushort) rec.X;
            ushort y = (ushort) rec.Y;
            ushort width = (ushort) rec.Width;
            ushort height = (ushort) rec.Height;
            return (long) ((((x << 0x30) + (y << 0x20)) + (width << 0x10)) + height);
        }

        public static long ConvertSizeToLong(Size size)
        {
            return ConvertRectangleToLong(new Rectangle(0, 0, size.Width, size.Height));
        }

        public static float Distance(Point point1, Point point2)
        {
            return (float) Math.Sqrt((double) (((point1.X - point2.X) * (point1.X - point2.X)) + ((point1.Y - point2.Y) * (point1.Y - point2.Y))));
        }

        public static float Distance(Rectangle rect, Point point)
        {
            Point point2 = new Point(rect.Left, rect.Top);
            Point point3 = new Point(rect.Right, rect.Top);
            Point point4 = new Point(rect.Left, rect.Bottom);
            Point point5 = new Point(rect.Right, rect.Bottom);
            float num = Math.Min(Math.Min(DistanceToSegment(point, point2, point3), DistanceToSegment(point, point3, point5)), Math.Min(DistanceToSegment(point, point5, point4), DistanceToSegment(point, point4, point2)));
            if (rect.Contains(point))
            {
                num = -num;
            }
            return num;
        }

        public static float Distance(Rectangle rect1, Rectangle rect2)
        {
            Point point = new Point(rect2.Left, rect2.Top);
            Point point2 = new Point(rect2.Right, rect2.Top);
            Point point3 = new Point(rect2.Left, rect2.Bottom);
            Point point4 = new Point(rect2.Right, rect2.Bottom);
            return Math.Min(Math.Min(Distance(rect1, point), Distance(rect1, point2)), Math.Min(Distance(rect1, point4), Distance(rect1, point3)));
        }

        public static float DistanceToSegment(Point p, Point line0, Point line1)
        {
            PointF tf;
            double num = ((p.X - line0.X) * (line1.X - line0.X)) + ((p.Y - line0.Y) * (line1.Y - line0.Y));
            if (num <= 0.0)
            {
                tf = new PointF((float) line0.X, (float) line0.Y);
            }
            else
            {
                double num2 = ((line1.X - line0.X) * (line1.X - line0.X)) + ((line1.Y - line0.Y) * (line1.Y - line0.Y));
                if (num2 <= num)
                {
                    tf = new PointF((float) line1.X, (float) line1.Y);
                }
                else
                {
                    double num3 = num / num2;
                    tf = new PointF((float) (line0.X + (num3 * (line1.X - line0.X))), (float) (line0.Y + (num3 * (line1.Y - line0.Y))));
                }
            }
            return (float) Math.Sqrt((double) (Sqr(tf.X - p.X) + Sqr(tf.Y - p.Y)));
        }

        /// <summary>
        /// 获取距离最近的区域。
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rects"></param>
        /// <returns></returns>
        public static Rectangle GetNearestRectangle(Point p, Rectangle[] rects)
        {
            double positiveInfinity = double.PositiveInfinity;
            Rectangle empty = Rectangle.Empty;
            if (rects != null)
            {
                foreach (Rectangle rectangle2 in rects)
                {
                    double num2 = Distance(rectangle2, p);
                    if (num2 < positiveInfinity)
                    {
                        positiveInfinity = num2;
                        empty = rectangle2;
                    }
                }
            }
            return empty;
        }

        /// <summary>
        /// 获取距离最近的区域。
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="rects"></param>
        /// <returns></returns>
        public static Rectangle GetNearestRectangle(Rectangle rect, Rectangle[] rects)
        {
            double positiveInfinity = double.PositiveInfinity;
            Rectangle empty = Rectangle.Empty;
            foreach (Rectangle rectangle2 in rects)
            {
                float num2 = Distance(rectangle2, rect);
                if (num2 < positiveInfinity)
                {
                    positiveInfinity = num2;
                    empty = rectangle2;
                }
            }
            return empty;
        }

        /// <summary>
        /// 获取距离最近的区域。
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="rects"></param>
        /// <returns></returns>
        public static int GetNearestRectangleBySpiral(Rectangle rect, Rectangle[] rects)
        {
            double positiveInfinity = double.PositiveInfinity;
            double num2 = double.PositiveInfinity;
            int num3 = -1;
            for (int i = 0; i < rects.Length; i++)
            {
                double num5 = Distance(rects[i], Center(rect));
                double num6 = (Math.Atan2((double) (Center(rect).Y - Center(rects[i]).Y), (double) (Center(rect).X - Center(rects[i]).X)) + 4.71238898038469) % 6.2831853071795862;
                if ((num5 < positiveInfinity) || ((num5 == positiveInfinity) && (num6 < num2)))
                {
                    positiveInfinity = num5;
                    num2 = num6;
                    num3 = i;
                }
            }
            return num3;
        }

        public static Rectangle GetRunTimeAreaByAnchor(Rectangle runTimeAnchor, Rectangle designTimeAnchor, Rectangle designTimeTarget)
        {
            Rectangle rectangle = runTimeAnchor;
            if (designTimeTarget != Rectangle.Empty)
            {
                Point location = new Point((runTimeAnchor.X - designTimeAnchor.X) + designTimeTarget.X, (runTimeAnchor.Y - designTimeAnchor.Y) + designTimeTarget.Y);
                Size size = new Size((runTimeAnchor.Width - (designTimeTarget.X - designTimeAnchor.X)) - ((designTimeAnchor.Width - (designTimeTarget.X - designTimeAnchor.X)) - designTimeTarget.Width), (runTimeAnchor.Height - (designTimeTarget.Y - designTimeAnchor.Y)) - ((designTimeAnchor.Height - (designTimeTarget.Y - designTimeAnchor.Y)) - designTimeTarget.Height));
                rectangle = new Rectangle(location, size);
            }
            return rectangle;
        }

        /// <summary>
        /// 判断区域是否相交。
        /// </summary>
        /// <param name="rects"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public static bool IsIntersect(Rectangle[] rects, Rectangle area)
        {
            foreach (Rectangle rectangle in rects)
            {
                if (rectangle.IntersectsWith(area))
                {
                    return true;
                }
            }
            return false;
        }

        public static Rectangle RoundRectangle(RectangleF r)
        {
            return RoundRectangle(r.Left, r.Top, r.Right, r.Bottom);
        }

        /// <summary>
        /// 四舍五入取舍
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public static Rectangle RoundRectangle(float left, float top, float right, float bottom)
        {
            return Rectangle.FromLTRB((int) Math.Round((double) left), (int) Math.Round((double) top), (int) Math.Round((double) right), (int) Math.Round((double) bottom));
        }

        public static float Sqr(float x)
        {
            return (x * x);
        }
    }
}

