using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using log4net;
using Mobot.Imaging;
using System.Diagnostics;

namespace Mobot.Imaging
{
    /// <summary>
    /// 提供图像对比等功能。
    /// </summary>
    public partial class BitmapHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BitmapHelper));

        /// <summary>
        /// 检查位图对象格式是否正确。
        /// </summary>
        /// <param name="bitmap">要检查的位图对象</param>
        public static void CheckBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new NullReferenceException("Bitmap cannot be null");
            }
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("Bitmap.PixelFormat is invalid");
            }
            if (bitmap.Width <= 0)
            {
                throw new ArgumentException("Bitmap has invalid width");
            }
            if (bitmap.Height <= 0)
            {
                throw new ArgumentException("Bitmap has invalid height");
            }
        }

        /// <summary>
        /// 创建位图对象的一份副本。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static unsafe Bitmap CloneBitmap(Bitmap source)
        {
            Bitmap bitmap2;
            lock (source)
            {
                CheckBitmap(source);
                Bitmap bmp = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
                BitmapData bd = null;
                BitmapData data2 = null;
                try
                {
                    bd = LockBits(source, ImageLockMode.ReadOnly);
                    data2 = LockBits(bmp, ImageLockMode.WriteOnly);
                    uint* numPtr = (uint*)bd.Scan0;
                    uint* numPtr2 = (uint*)data2.Scan0;
                    int num = source.Width * source.Height;
                    for (int i = 0; i < num; i++)
                    {
                        (numPtr2++)[0] = (numPtr++)[0];
                    }
                    bitmap2 = bmp;
                }
                finally
                {
                    if (bd != null)
                    {
                        UnlockBits(source, bd);
                    }
                    if (data2 != null)
                    {
                        UnlockBits(bmp, data2);
                    }
                }
            }
            return bitmap2;
        }

        public static bool CompareBitmap(Bitmap b1, Bitmap b2)
        {
            return CompareBitmap(b1, b2, 0);
        }

        public static bool CompareBitmap(Bitmap b1, Bitmap b2, Rectangle area)
        {
            return CompareBitmap(b1, b2, area.X, area.Y, area.X, area.Y, area.Width, area.Height, 0);
        }

        public static bool CompareBitmap(Bitmap b1, Bitmap b2, int tolerance)
        {
            if ((b1 == null) || (b2 == null))
            {
                return false;
            }
            return (((b1.Width == b2.Width) && (b1.Height == b2.Height)) && CompareBitmap(b1, b2, 0, 0, 0, 0, b1.Width, b1.Height, tolerance));
        }

        public static bool CompareBitmap(Bitmap b1, Bitmap b2, Rectangle area, int tolerance)
        {
            return CompareBitmap(b1, b2, area.X, area.Y, area.X, area.Y, area.Width, area.Height, tolerance);
        }

        public static bool CompareBitmap(Bitmap b1, Bitmap b2, int x1, int y1, int x2, int y2, int width, int height, int tolerance)
        {
            bool flag = false;
            CheckBitmap(b1);
            CheckBitmap(b2);
            if ((((x1 >= 0) && (y1 >= 0)) && (((x1 + width) <= b1.Width) && ((y1 + height) <= b1.Height))) && (((x2 >= 0) && (y2 >= 0)) && (((x2 + width) <= b2.Width) && ((y2 + height) <= b2.Height))))
            {
                BitmapData data = null;
                BitmapData data2 = null;
                try
                {
                    data = LockBits(b1, ImageLockMode.ReadOnly);
                    data2 = LockBits(b2, ImageLockMode.ReadOnly);
                    flag = CompareBitmapLocked(data, data2, x1, y1, x2, y2, width, height, (tolerance * 0xfe01) / 100);
                }
                finally
                {
                    if (data != null)
                    {
                        UnlockBits(b1, data);
                    }
                    if (data2 != null)
                    {
                        UnlockBits(b2, data2);
                    }
                }
            }
            return flag;
        }

        public static bool CompareBitmapLocked(BitmapData searchOnData, BitmapData searchForData, int x, int y)
        {
            return CompareBitmapLocked(searchOnData, searchForData, x, y, 0, 0, searchForData.Width, searchForData.Height, 0);
        }

        public static bool CompareBitmapLocked(BitmapData searchOnData, BitmapData searchForData, int x, int y, int tolerance)
        {
            return CompareBitmapLocked(searchOnData, searchForData, x, y, 0, 0, searchForData.Width, searchForData.Height, tolerance);
        }

        public static unsafe bool CompareBitmapLocked(BitmapData bd1, BitmapData bd2, int x1, int y1, int x2, int y2, int width, int height, int tolerance)
        {
            int num = bd1.Width;
            uint* numPtr = (uint*)((((byte*)bd1.Scan0) + ((num * y1) * 4)) + (x1 * 4));
            int num2 = num - width;
            num = bd2.Width;
            uint* numPtr2 = (uint*)((((byte*)bd2.Scan0) + ((num * y2) * 4)) + (x2 * 4));
            int num3 = num - width;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    uint num6 = (numPtr++)[0];
                    uint num7 = (numPtr2++)[0];
                    if (tolerance == 0)
                    {
                        if (((num6 & 0xff000000) == 0) || (((num7 & 0xff000000) == 0xff000000) && (num6 != num7)))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        int num8 = (int)((num6 & 0xff) - (num7 & 0xff));
                        int num9 = (int)(((num6 >> 8) & 0xff) - ((num7 >> 8) & 0xff));
                        int num10 = (int)(((num6 >> 0x10) & 0xff) - ((num7 >> 0x10) & 0xff));
                        if (((num6 & 0xff000000) == 0) || (((num7 & 0xff000000) == 0xff000000) && ((((num8 * num8) + (num9 * num9)) + (num10 * num10)) > tolerance)))
                        {
                            return false;
                        }
                    }
                }
                numPtr += num2;
                numPtr2 += num3;
            }
            return true;
        }

        public static Bitmap ConvertToArgb(Bitmap bitmap)
        {
            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                return bitmap;
            }
            Bitmap image = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.DrawImageUnscaledAndClipped(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            }
            return image;
        }

        public static unsafe void CopyMask(Bitmap source, Bitmap dest)
        {
            if (source != dest)
            {
                int width = Math.Min(source.Width, dest.Width);
                int height = Math.Min(source.Height, dest.Height);
                Rectangle area = new Rectangle(0, 0, width, height);
                BitmapData bd = null;
                BitmapData data2 = null;
                try
                {
                    bd = LockBits(source, area, ImageLockMode.ReadOnly);
                    data2 = LockBits(dest, area, ImageLockMode.ReadWrite);
                    for (int i = 0; i < height; i++)
                    {
                        uint* numPtr = (uint*)(((byte*)bd.Scan0) + ((i * source.Width) * 4));
                        uint* numPtr2 = (uint*)(((byte*)data2.Scan0) + ((i * dest.Width) * 4));
                        for (int j = 0; j < width; j++)
                        {
                            numPtr2[0] = (numPtr2[0] & 0xffffff) | (numPtr[0] & 0xff000000);
                            numPtr2++;
                            numPtr++;
                        }
                    }
                }
                finally
                {
                    if (bd != null)
                    {
                        UnlockBits(source, bd);
                    }
                    if (data2 != null)
                    {
                        UnlockBits(dest, data2);
                    }
                }
            }
        }

        public static unsafe void CopyMask(Bitmap src, Rectangle srcRect, Bitmap dst, Point dstLocation)
        {
            srcRect.Intersect(new Rectangle(Point.Empty, src.Size));
            Rectangle area = new Rectangle(dstLocation, srcRect.Size);
            area.Intersect(new Rectangle(Point.Empty, dst.Size));
            srcRect.Size = area.Size;
            int width = area.Width;
            int height = area.Height;
            if ((width > 0) && (height > 0))
            {
                BitmapData bd = null;
                BitmapData data2 = null;
                try
                {
                    bd = LockBits(src, srcRect, ImageLockMode.ReadOnly);
                    data2 = LockBits(dst, area, ImageLockMode.WriteOnly);
                    for (int i = 0; i < height; i++)
                    {
                        uint* numPtr = (uint*)(((byte*)bd.Scan0) + ((i * src.Width) * 4));
                        uint* numPtr2 = (uint*)(((byte*)data2.Scan0) + ((i * dst.Width) * 4));
                        for (int j = 0; j < width; j++)
                        {
                            numPtr2[0] = (numPtr2[0] & 0xffffff) | (numPtr[0] & 0xff000000);
                            numPtr2++;
                            numPtr++;
                        }
                    }
                }
                finally
                {
                    if (bd != null)
                    {
                        UnlockBits(src, bd);
                    }
                    if (data2 != null)
                    {
                        UnlockBits(dst, data2);
                    }
                }
            }
        }

        public static unsafe Bitmap CropBitmap(Bitmap image, Rectangle area)
        {
            lock (image)
            {
                CheckBitmap(image);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                if (area.IntersectsWith(rect))
                {
                    area.Intersect(rect);
                }
                else
                {
                    return null;
                }
                int width = area.Width;
                int height = area.Height;
                if ((width > 0) && (height > 0))
                {
                    Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                    BitmapData bd = null;
                    BitmapData data2 = null;
                    try
                    {
                        bd = LockBits(image, area, ImageLockMode.ReadOnly);
                        data2 = LockBits(bmp, ImageLockMode.WriteOnly);
                        for (int i = 0; i < height; i++)
                        {
                            uint* numPtr = (uint*)(((byte*)bd.Scan0) + ((i * image.Width) * 4));
                            uint* numPtr2 = (uint*)(((byte*)data2.Scan0) + ((i * width) * 4));
                            for (int j = 0; j < width; j++)
                            {
                                (numPtr2++)[0] = (numPtr++)[0];
                            }
                        }
                    }
                    finally
                    {
                        if (bd != null)
                        {
                            UnlockBits(image, bd);
                        }
                        if (data2 != null)
                        {
                            UnlockBits(bmp, data2);
                        }
                    }
                    return bmp;
                }
                return null;
            }
        }

        public static unsafe void DrawGrid(Bitmap bitmap, float zoom, PaintEventArgs e, Rectangle picBounds)
        {
            if (zoom > 4f)
            {
                BitmapData bd = LockBits(bitmap, ImageLockMode.WriteOnly);
                float num = Math.Max(e.ClipRectangle.X + ((zoom - (((float)(e.ClipRectangle.X - picBounds.X)) % zoom)) % zoom), (float)picBounds.X);
                float num2 = Math.Max(e.ClipRectangle.Y + ((zoom - (((float)(e.ClipRectangle.Y - picBounds.Y)) % zoom)) % zoom), (float)picBounds.Y);
                float num3 = Math.Min(e.ClipRectangle.Right, picBounds.Right);
                float num4 = Math.Min(e.ClipRectangle.Bottom, picBounds.Bottom);
                try
                {
                    if (zoom > 8f)
                    {
                        for (float i = num2; i < num4; i += zoom)
                        {
                            uint* numPtr = (uint*)(((byte*)bd.Scan0) + ((((int)i) * bd.Width) * 4));
                            for (int k = Math.Max(e.ClipRectangle.X, picBounds.X); k < num3; k++)
                            {
                                uint* numPtr1 = numPtr + k;
                                numPtr1[0] ^= 0x3f3f3f;
                            }
                        }
                        if (e.ClipRectangle.Y > 0)
                        {
                            Thread.Sleep(0);
                        }
                        for (float j = num; j < num3; j += zoom)
                        {
                            int num8 = Math.Max(e.ClipRectangle.Y, picBounds.Y);
                            uint* numPtr2 = (uint*)((((byte*)bd.Scan0) + ((num8 * bd.Width) * 4)) + (((int)j) * 4));
                            for (int m = num8; m < num4; m++)
                            {
                                numPtr2[0] ^= 0x3f3f3f;
                                numPtr2 += bd.Width;
                            }
                        }
                    }
                    else
                    {
                        for (float n = num2; n < num4; n += zoom)
                        {
                            uint* numPtr3 = (uint*)(((byte*)bd.Scan0) + ((((int)n) * bd.Width) * 4));
                            for (float num11 = num; num11 < num3; num11 += zoom)
                            {
                                uint* numPtr4 = numPtr3 + ((int)num11);
                                numPtr4[0] ^= 0x838383;
                            }
                        }
                    }
                }
                finally
                {
                    UnlockBits(bitmap, bd);
                }
            }
        }

        public static List<Rectangle> FindDifferencies(Bitmap b1, Bitmap b2, bool throwExIfNotEqual)
        {
            if (b1 == b2)
            {
                return new List<Rectangle>();
            }
            return FindDifferencies(b1, b2, 10, throwExIfNotEqual);
        }

        private static List<Rectangle> FindDifferencies(Bitmap bmp1, Bitmap bmp2, int distance, bool throwExIfNotEqual)
        {
            List<Rectangle> list;
            lock (bmp1)
            {
                lock (bmp2)
                {
                    int height = bmp1.Height;
                    int width = bmp1.Width;
                    if ((width == 0) || (height == 0))
                    {
                        throw new ArgumentException("Bitmap width and height must be greater than zero");
                    }
                    if ((bmp2.Width != width) || (bmp2.Height != height))
                    {
                        if (throwExIfNotEqual)
                        {
                            throw new ArgumentException("Bitmaps must have same size");
                        }
                        return null;
                    }
                    if (bmp1.PixelFormat != PixelFormat.Format32bppArgb || bmp2.PixelFormat != PixelFormat.Format32bppArgb)
                    {
                        if (throwExIfNotEqual)
                        {
                            throw new ArgumentException("图片的格式必须是 Format32bppArgb 。");
                        }
                        return null;
                    }
                    list = FindDifferencies(bmp1, bmp2, Point.Empty, Point.Empty, new Size(width, height), distance);
                }
            }
            return list;
        }

        private static unsafe List<Rectangle> FindDifferencies(Bitmap bmp1, Bitmap bmp2, Point p1, Point p2, Size size, int distance)
        {
            int width = size.Width;
            int height = size.Height;
            int num3 = bmp1.Width;
            int num4 = bmp1.Height;
            int num5 = bmp2.Width;
            int num6 = bmp2.Height;
            if ((((p1.X + width) > num3) || ((p1.Y + height) > num4)) || (((p1.X + width) > num5) || ((p1.Y + height) > num6)))
            {
                throw new ArgumentException("p1, p2 and size must point the area inside both bitmaps");
            }
            BitmapData bd = null;
            BitmapData data2 = null;
            List<Rectangle> list = null;
            try
            {
                bd = LockBits(bmp1, new Rectangle(p1, size), ImageLockMode.ReadOnly);
                data2 = LockBits(bmp2, new Rectangle(p2, size), ImageLockMode.ReadOnly);
                list = FindDifferenciesInternal((uint*)bd.Scan0, (uint*)data2.Scan0, num3 - width, num5 - width, width, height, distance);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (bd != null)
                {
                    UnlockBits(bmp1, bd);
                }
                if (data2 != null)
                {
                    UnlockBits(bmp2, data2);
                }
            }
            return list;
        }

        private static unsafe List<Rectangle> FindDifferenciesInternal(uint* ptr1, uint* ptr2, int stride1, int stride2, int w, int h, int distance)
        {
            List<Rectangle> res = new List<Rectangle>();
            if (distance < 1)
            {
                distance = 1;
            }
            int width = (distance + distance) + 1;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if ((ptr1++)[0] != (ptr2++)[0])
                    {
                        Rectangle b = new Rectangle(j, i, 1, 1);
                        Rectangle rect = new Rectangle(j - distance, i - distance, width, width);
                        bool flag = false;
                        int count = res.Count;
                        for (int k = 0; k < count; k++)
                        {
                            Rectangle a = res[k];
                            if (a.IntersectsWith(rect))
                            {
                                res[k] = Rectangle.Union(a, b);
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            res.Add(b);
                        }
                    }
                }
                ptr1 += stride1;
                ptr2 += stride2;
            }
            return UnionRectangles(res);
        }

        public static Rectangle FindNearestDifference(Bitmap b1, Bitmap b2, Point origin)
        {
            List<Rectangle> list = FindDifferencies(b1, b2, true);
            return GeometryHelper.GetNearestRectangle(origin, list.ToArray());
        }

        public static Rectangle FindNearestDifference(Bitmap b1, Bitmap b2, Rectangle originRec)
        {
            List<Rectangle> list = FindDifferencies(b1, b2, true);
            return GeometryHelper.GetNearestRectangle(originRec, list.ToArray());
        }
        /*
        public static Image GetImageByLevel(Mobot.Core.Common.Logging.LogLevel level, bool getGif)
        {
            switch (level)
            {
                case Mobot.Core.Common.Logging.LogLevel.Debug:
                    if (!getGif)
                    {
                        return ResourceHolder.ok_16x16;
                    }
                    return ResourceHolder.debug_16x16_GIF;

                case Mobot.Core.Common.Logging.LogLevel.Info:
                    if (!getGif)
                    {
                        return ResourceHolder.info_16x16;
                    }
                    return ResourceHolder.info_16x16_GIF;

                case Mobot.Core.Common.Logging.LogLevel.Warning:
                    if (!getGif)
                    {
                        return ResourceHolder.warning_16x16;
                    }
                    return ResourceHolder.warning_16x16_GIF;

                case Mobot.Core.Common.Logging.LogLevel.Error:
                    if (!getGif)
                    {
                        return ResourceHolder.stop_16x16;
                    }
                    return ResourceHolder.error_16x16_GIF;
            }
            return null;
        }*/

        public static Size GetImageSize(Bitmap image)
        {
            try
            {
                log.Debug("当前线程获取图像大小");
                try
                {
                    return image.Size;
                }
                catch (InvalidOperationException exception)
                {
                    log.Warn("不能获取图像大小，将重试", exception);
                    Thread.Sleep(1);
                    return image.Size;
                }
            }
            catch (Exception exception2)
            {
                log.Error("获取图像大小时发生异常，返回空大小: ", exception2);
                return Size.Empty;
            }
        }

        public static BitmapData LockBits(Bitmap bmp)
        {
            return LockBits(bmp, new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadWrite);
        }

        public static BitmapData LockBits(Bitmap bmp, ImageLockMode mode)
        {
            return LockBits(bmp, new Rectangle(Point.Empty, bmp.Size), mode);
        }

        public static BitmapData LockBits(Bitmap bmp, Rectangle area, ImageLockMode mode)
        {
            return bmp.LockBits(area, mode, bmp.PixelFormat);
        }

        public static void MaskRectangle(Bitmap image, Rectangle rect, bool setMask)
        {
            CheckBitmap(image);
            BitmapData data = LockBits(image, ImageLockMode.ReadWrite);
            try
            {
                MaskRectangle(data, rect, setMask);
            }
            finally
            {
                UnlockBits(image, data);
            }
        }

        /// <summary>
        /// 屏蔽区域，将指定区域的像素值设为纯色。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rect"></param>
        /// <param name="setMask"></param>
        public static unsafe void MaskRectangle(BitmapData data, Rectangle rect, bool setMask)
        {
            rect.Intersect(new Rectangle(0, 0, data.Width, data.Height));
            if (!rect.IsEmpty)
            {
                uint* numPtr = (uint*)((((byte*)data.Scan0) + (data.Stride * rect.Top)) + (rect.Left * 4));
                int num = data.Width - rect.Width;
                for (int i = 0; i < rect.Height; i++)
                {
                    for (int j = 0; j < rect.Width; j++)
                    {
                        if (setMask)
                        {
                            (numPtr++)[0] &= 0x64ffffff;
                        }
                        else
                        {
                            (numPtr++)[0] |= 0xff000000;
                        }
                    }
                    numPtr += num;
                }
            }
        }

        /// <summary>
        /// 判断图像指定区域内是否设置了掩码。
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool IsMasked(Bitmap image, Rectangle rect)
        {
            bool isMasked = false;
            BitmapData data = null;
            try
            {
                data = LockBits(image, ImageLockMode.ReadOnly);
                isMasked = IsMasked(data, rect);
            }
            catch (Exception ex)
            {
                log.Warn("判断掩码失败。", ex);
            }
            finally
            {
                if (data != null)
                {
                    BitmapHelper.UnlockBits(image, data);
                }
            }
            return isMasked;
        }

        /// <summary>
        /// 判断图像指定区域内是否设置了掩码。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rect"></param>
        /// <returns>如果区域内任一像素设置了掩码，则返回true。</returns>
        public static unsafe bool IsMasked(BitmapData data, Rectangle rect)
        {
            if (!rect.IsEmpty)
            {
                uint* numPtr = (uint*)((((byte*)data.Scan0) + (data.Stride * rect.Top)) + (rect.Left * 4));
                for (int i = 0; i < rect.Height; i++)
                {
                    for (int j = 0; j < rect.Width; j++)
                    {
                        if (((numPtr++)[0] & 0xff000000) != 0xff000000)
                            return true;
                    }

                }
            }
            return false;
        }

        public static Bitmap ResizeBitmap(Bitmap image, Size size, InterpolationMode mode)
        {
            Bitmap bitmap = new Bitmap(size.Width, size.Height, image.PixelFormat);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = mode;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, 0, 0, bitmap.Width, bitmap.Height);
            }
            return bitmap;
        }

        public static Bitmap ResizeBitmap(Bitmap image, float magnification, InterpolationMode mode)
        {
            if (magnification <= 0f)
            {
                throw new ArgumentException("Parameter cannot be zero", "magnification");
            }
            return ResizeBitmap(image, new Size((int)(image.Width * magnification), (int)(image.Height * magnification)), mode);
        }

        private static List<Rectangle> UnionRectangles(List<Rectangle> res)
        {
            bool flag;
            do
            {
                List<Rectangle> list = new List<Rectangle>();
                foreach (Rectangle rectangle in res)
                {
                    bool flag2 = false;
                    for (int i = 0; i < list.Count; i++)
                    {
                        Rectangle rect = list[i];
                        if (rectangle.IntersectsWith(rect))
                        {
                            list[i] = Rectangle.Union(rect, rectangle);
                            flag2 = true;
                        }
                    }
                    if (!flag2)
                    {
                        list.Add(rectangle);
                    }
                }
                flag = res.Count == list.Count;
                res = list;
            }
            while (!flag);
            return res;
        }

        public static void UnlockBits(Bitmap bmp, BitmapData bd)
        {
            bmp.UnlockBits(bd);
        }
    }
}

