using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace Paway.Helper
{
    /// <summary>
    /// 图像处理类
    /// </summary>
    public abstract class BitmapHelper
    {
        #region 方法

        /// <summary>
        /// 图片无损缩放
        /// </summary>
        /// <param name="source">源图片</param>
        /// <param name="destHeight">缩放后图片高度</param>
        /// <param name="destWidth">缩放后图片宽度</param>
        /// <returns></returns>
        public static Bitmap GetThumbnail(Image source, int destWidth, int destHeight)
        {
            Bitmap bitmap = null;
            try
            {
                System.Drawing.Imaging.ImageFormat sourceFormat = source.RawFormat;
                int sW = 0, sH = 0;
                // 按比例缩放
                int sWidth = source.Width;
                int sHeight = source.Height;

                if (sHeight > destHeight || sWidth > destWidth)
                {
                    if ((sWidth * destHeight) > (sHeight * destWidth))
                    {
                        sW = destWidth;
                        sH = (destWidth * sHeight) / sWidth;
                    }
                    else
                    {
                        sH = destHeight;
                        sW = (sWidth * destHeight) / sHeight;
                    }
                }
                else
                {
                    sW = sWidth;
                    sH = sHeight;
                }

                bitmap = new Bitmap(destWidth, destHeight);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.WhiteSmoke);

                    // 设置画布的描绘质量
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    Rectangle rect = new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH);
                    g.DrawImage(source, rect, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ImageProcess.GEtThumbnail(Image, int, int) :: " + ex.Message);
                throw;
            }
            return bitmap;
        }

        /// <summary>
        /// 图片无损缩放
        /// </summary>
        /// <param name="filePath">图片源路径</param>
        /// <param name="destHeight">缩放后图片高度</param>
        /// <param name="destWidth">缩放后图片宽度</param>
        /// <returns></returns>
        public static Bitmap GetThumbnail(string filePath, int destWidth, int destHeight)
        {
            Bitmap bitmap = null;
            try
            {
                using (Image source = Image.FromFile(filePath))
                {
                    bitmap = BitmapHelper.GetThumbnail(source, destWidth, destHeight);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ImageProcess.GetThumbnail(string, int, int) :: " + ex.Message);
                throw;
            }
            return bitmap;
        }

        /// <summary>
        /// 截取屏幕图像
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetScreenPic()
        {
            Bitmap bitmap = null;
            try
            {
                Size size = Screen.PrimaryScreen.WorkingArea.Size;
                if (bitmap == null)
                    bitmap = new Bitmap(size.Width, size.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.CopyFromScreen(Point.Empty, Point.Empty, size);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ImageProcess.GetScreenPic() :: " + ex.Message);
                throw;
            }
            return bitmap;
        }

        /// <summary>
        /// 灰度
        /// </summary>
        /// <param name="image">需要处理成灰度的图片对象</param>
        /// <returns></returns>
        public static Bitmap ToGray(Image image)
        {
            Bitmap bitmap = new Bitmap(image);
            try
            {
                int width = bitmap.Width;
                int height = bitmap.Height;
                Rectangle rect = new Rectangle(0, 0, width, height);
                //用可读写的方式锁定全部位图像素
                BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                byte gray = 0;
                unsafe//启用不安全模式
                {
                    byte* p = (byte*)bmpData.Scan0;//获取首地址
                    int offset = bmpData.Stride - width * 3;
                    //二维图像循环
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            //gray = (byte)((float)p[0] * 0.114f + (float)p[1] * 0.587f + (float)p[2] * 0.299f);
                            gray = (byte)((float)(p[0] + p[1] + p[2]) / 3.0f);
                            p[2] = p[1] = p[0] = (byte)gray;
                            p += 3;
                        }
                        p += offset;
                    }
                }
                bitmap.UnlockBits(bmpData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ImageProcess.ToGray(Image) :: " + ex.Message);
                throw;
            }
            #region 效果不如前者

            ////得到首地址
            //IntPtr ptr = bmpData.Scan0;
            ////24位bmp位图字节数
            //int bytes = width * height * 3;
            //byte[] rgbValues = new byte[bytes];
            //Marshal.Copy(ptr, rgbValues, 0, bytes);
            ////灰度化
            //double colorTemp = 0;
            //for (int i = 0; i < bytes; i += 3)
            //{
            //    //colorTemp = (byte)(rgbValues[i] * 0.114f + rgbValues[i + 1] * 0.587f + rgbValues[i + 2] * 0.299f);
            //    colorTemp = (rgbValues[i] + rgbValues[i + 1] + rgbValues[i + 2]) / 3;
            //    rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = (byte)colorTemp;
            //}
            ////还原位图
            //Marshal.Copy(rgbValues, 0, ptr, bytes);
            //bitmap.UnlockBits(bmpData);
            #endregion

            return bitmap;
        }

        /// <summary>
        /// 从指定的文件初始化图像
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Bitmap GetBitmapFormFile(string fileName)
        {
            Bitmap bitmap = new Bitmap(fileName);
            Bitmap image = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
            }
            bitmap.Dispose();
            return image;
        }

        /// <summary>
        /// 替换图像像素点
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap ReplaceByPixel(Image image, int x, int y, Color color)
        {
            //生成图
            Bitmap bitmap = new Bitmap(image);
            int width = bitmap.Width;
            int height = bitmap.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            //用可读写的方式锁定全部位图像素
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe//启用不安全模式
            {
                byte* p = (byte*)bmpData.Scan0;//获取首地址
                p[y * width * 4 + x * 4 + 0] = color.B;
                p[y * width * 4 + x * 4 + 1] = color.G;
                p[y * width * 4 + x * 4 + 2] = color.R;
                p[y * width * 4 + x * 4 + 3] = color.A;
            }
            bitmap.UnlockBits(bmpData);
            return bitmap;
        }

        #endregion

        #region 获取Gif图片中的各帧
        /// <summary>
        /// 获取Gif图片中的各帧
        /// </summary>
        /// <param name="gPath">图片路径</param>
        /// <param name="sPath">保存路径</param>
        public static void GetFrames(string gPath, string sPath)
        {
            Image gif = Image.FromFile(gPath);
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            int count = gif.GetFrameCount(fd); //获取帧数(gif图片可能包含多帧，其它格式图片一般仅一帧)
            for (int i = 0; i < count; i++)    //以Jpeg格式保存各帧
            {
                gif.SelectActiveFrame(fd, i);
                gif.Save(sPath + "\\frame_" + i + ".jpg", ImageFormat.Jpeg);
            }
        }
        #endregion

        #region 图片转换
        /// <summary>
        /// 图片转换
        /// </summary>
        /// <param name="image">原图</param>
        /// <param name="type">转换类型</param>
        /// <param name="param">参数值</param>
        /// <returns>返回新图片</returns>
        public static Bitmap ConvertTo(Image image, BConvertType type, object param = null)
        {
            if (image == null) throw new ArgumentNullException("image");
            switch (type)
            {
                case BConvertType.LeftRight:
                    return RevPicLR(image);
                case BConvertType.UpDown:
                    return RevPicUD(image);
            }
            //生成图
            Bitmap bitmap = new Bitmap(image);
            int width = bitmap.Width;
            int height = bitmap.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            //用可读写的方式锁定全部位图像素
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte gray = 0;
            int val = param.ToInt();
            unsafe//启用不安全模式
            {
                byte* p = (byte*)bmpData.Scan0;//获取首地址
                int offset = bmpData.Stride - width * 4;
                switch (type)
                {
                    case BConvertType.Relief:
                    case BConvertType.Brightness:
                        width -= 1;
                        height -= 1;
                        break;
                }
                //二维图像循环
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        switch (type)
                        {
                            case BConvertType.Relief:
                                p[0] = CheckPixel(Math.Abs(p[0] - p[(width + 2) * 4] + 128));
                                p[1] = CheckPixel(Math.Abs(p[1] - p[(width + 2) * 4 + 1] + 128));
                                p[2] = CheckPixel(Math.Abs(p[2] - p[(width + 2) * 4 + 2] + 128));
                                break;
                            case BConvertType.Brightness:
                                p[0] = CheckPixel(p[0] + val);
                                p[1] = CheckPixel(p[1] + val);
                                p[2] = CheckPixel(p[2] + val);
                                break;
                            case BConvertType.Anti:
                                p[0] = (byte)(255 - p[0]);
                                p[1] = (byte)(255 - p[1]);
                                p[2] = (byte)(255 - p[2]);
                                break;
                            case BConvertType.Color:
                                p[2] = (byte)(255 - p[0]);
                                break;
                            case BConvertType.BlackWhite:
                                gray = (byte)((float)(p[0] + p[1] + p[2]) / 3.0f);
                                p[2] = p[1] = p[0] = gray;
                                break;
                            case BConvertType.Grayscale:
                                gray = (byte)((float)(p[0] * 0.114f + p[1] * 0.587f + p[2] * 0.299f) / 3.0f);
                                p[2] = p[1] = p[0] = gray;
                                break;
                            case BConvertType.Trans:
                                if (p[3] != 0)
                                {
                                    p[3] = (byte)val;
                                }
                                break;
                            case BConvertType.Replace:
                                if (p[3] != 0)
                                {
                                    p[2] = p[1] = p[0] = (byte)val;
                                }
                                break;
                        }
                        p += 4;
                    }
                    p += offset;
                }
            }
            bitmap.UnlockBits(bmpData);
            return bitmap;
        }

        #region 左右翻转
        /// <summary>
        /// 左右翻转
        /// </summary>
        /// <param name="image">原始图片</param>
        public static Bitmap RevPicLR(Image image)
        {
            //生成图
            Bitmap bitmap = new Bitmap(image);
            //对照图
            Bitmap copy = new Bitmap(image);
            int width = bitmap.Width;
            int height = bitmap.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            //用可读写的方式锁定全部位图像素
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            BitmapData copyData = copy.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe//启用不安全模式
            {
                byte* p = (byte*)bmpData.Scan0;//获取首地址
                byte* f = (byte*)copyData.Scan0;//获取首地址
                int offset = bmpData.Stride - width * 4;
                //二维图像循环
                //for (int y = height - 1; y >= 0; y--)
                for (int y = 0; y < height; y++)
                {
                    for (int x = width - 1; x >= 0; x--)
                    {
                        p[y * width * 4 + (width - 1 - x) * 4 + 0] = f[y * width * 4 + x * 4];
                        p[y * width * 4 + (width - 1 - x) * 4 + 1] = f[y * width * 4 + x * 4 + 1];
                        p[y * width * 4 + (width - 1 - x) * 4 + 2] = f[y * width * 4 + x * 4 + 2];
                    }
                }
            }
            bitmap.UnlockBits(bmpData);
            return bitmap;
        }
        #endregion
        #region 上下翻转
        /// <summary>
        /// 上下翻转
        /// </summary>
        /// <param name="image">原始图片</param>
        public static Bitmap RevPicUD(Image image)
        {
            //生成图
            Bitmap bitmap = new Bitmap(image);
            //对照图
            Bitmap copy = new Bitmap(image);
            int width = bitmap.Width;
            int height = bitmap.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            //用可读写的方式锁定全部位图像素
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            BitmapData copyData = copy.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe//启用不安全模式
            {
                byte* p = (byte*)bmpData.Scan0;//获取首地址
                byte* f = (byte*)copyData.Scan0;//获取首地址
                int offset = bmpData.Stride - width * 4;
                //二维图像循环
                for (int x = 0; x < width; x++)
                {
                    for (int y = height - 1; y >= 0; y--)
                    {
                        p[(height - 1 - y) * width * 4 + x * 4 + 0] = f[y * width * 4 + x * 4];
                        p[(height - 1 - y) * width * 4 + x * 4 + 1] = f[y * width * 4 + x * 4 + 1];
                        p[(height - 1 - y) * width * 4 + x * 4 + 2] = f[y * width * 4 + x * 4 + 2];
                    }
                }
            }
            bitmap.UnlockBits(bmpData);
            return bitmap;
        }
        /// <summary>
        /// 顺时针90度
        /// </summary>
        private Bitmap Rotate90(Image image)
        {
            //原图
            Bitmap bitmap = new Bitmap(image);
            //生成图
            int width = bitmap.Width;
            int height = bitmap.Height;
            Bitmap copy = new Bitmap(height, width);
            Rectangle rect = new Rectangle(0, 0, height, width);
            //用可读写的方式锁定全部位图像素
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            BitmapData copyData = copy.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe//启用不安全模式
            {
                byte* p = (byte*)copyData.Scan0;//获取首地址
                byte* f = (byte*)bmpData.Scan0;//获取首地址
                //二维图像循环
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        p[y * 4 + x * height * 4 + 0] = f[x * 4 + (height - 1 - y) * width * 4 + 0];
                        p[y * 4 + x * height * 4 + 1] = f[x * 4 + (height - 1 - y) * width * 4 + 1];
                        p[y * 4 + x * height * 4 + 2] = f[x * 4 + (height - 1 - y) * width * 4 + 2];
                    }
                }
            }
            copy.UnlockBits(copyData);
            return copy;
        }
        /// <summary>
        /// 顺时针270度（逆时针90度）
        /// </summary>
        private Bitmap Rotate270(Image image)
        {
            //原图
            Bitmap bitmap = new Bitmap(image);
            //生成图
            int width = bitmap.Width;
            int height = bitmap.Height;
            Bitmap copy = new Bitmap(height, width);
            Rectangle rect = new Rectangle(0, 0, height, width);
            //用可读写的方式锁定全部位图像素
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            BitmapData copyData = copy.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe//启用不安全模式
            {
                byte* p = (byte*)copyData.Scan0;//获取首地址
                byte* f = (byte*)bmpData.Scan0;//获取首地址
                //二维图像循环
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        p[y * 4 + (width - 1 - x) * height * 4 + 0] = f[x * 4 + y * width * 4 + 0];
                        p[y * 4 + (width - 1 - x) * height * 4 + 1] = f[x * 4 + y * width * 4 + 1];
                        p[y * 4 + (width - 1 - x) * height * 4 + 2] = f[x * 4 + y * width * 4 + 2];
                    }
                }
            }
            copy.UnlockBits(copyData);
            return copy;
        }
        #endregion

        /// <summary>
        /// 检查像素值会不会超出[0, 255]
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static byte CheckPixel(int val)
        {
            if (val < 0) return 0;
            if (val > 255) return 25; ;
            return (byte)val;
        }

        #endregion
    }
    /// <summary>
    /// 图片转换类型
    /// </summary>
    public enum BConvertType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 光暗
        /// </summary>
        Brightness,
        /// <summary>
        /// 反色
        /// </summary>
        Anti,
        /// <summary>
        /// 浮雕
        /// </summary>
        Relief,
        /// <summary>
        /// 滤色
        /// </summary>
        Color,
        /// <summary>
        /// 左右
        /// </summary>
        LeftRight,
        /// <summary>
        /// 上下
        /// </summary>
        UpDown,
        /// <summary>
        /// 黑白
        /// </summary>
        BlackWhite,
        /// <summary>
        /// 灰度
        /// </summary>
        Grayscale,
        /// <summary>
        /// 透明
        /// </summary>
        Trans,
        /// <summary>
        /// 替换
        /// </summary>
        Replace,
    }
}
