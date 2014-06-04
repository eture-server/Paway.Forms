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

                    source.Dispose();
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
                    BitmapHelper.GetThumbnail(source, destWidth, destHeight);
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
                image.Dispose();
                image = null;

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
            return image;
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
        public static Bitmap ConvertTo(Bitmap image, BConvertType type, object param = null)
        {
            if (image == null) throw new ArgumentNullException("image");
            switch (type)
            {
                case BConvertType.Relief:
                    return RevPicFD(image, image.Width, image.Height);
                case BConvertType.LeftRight:
                    return RevPicLR(image, image.Width, image.Height);
                case BConvertType.UpDown:
                    return RevPicUD(image, image.Width, image.Height);
            }
            Bitmap bitmap = new Bitmap(image.Width, image.Height);//初始化一个记录经过处理后的图片对象
            int x, y;//x、y是循环次数，后面三个是记录红绿蓝三个值的
            Color pixel;
            Color result = Color.White;
            for (x = 0; x < image.Width; x++)
            {
                for (y = 0; y < image.Height; y++)
                {
                    pixel = image.GetPixel(x, y);//获取当前像素的值
                    switch (type)
                    {
                        case BConvertType.Brightness:
                            result = Color.FromArgb(
                                CheckPixel(pixel.R + param.ToInt()),
                                CheckPixel(pixel.G + param.ToInt()),
                                CheckPixel(pixel.B + param.ToInt()));
                            break;
                        case BConvertType.Anti:
                            result = Color.FromArgb(
                                255 - pixel.R,
                                255 - pixel.G,
                                255 - pixel.B);
                            break;
                        case BConvertType.Color:
                            result = Color.FromArgb(0, pixel.G, pixel.B);
                            break;
                        case BConvertType.BlackWhite:
                            int rgb = pixel.R + pixel.G + pixel.B;
                            rgb /= 3;
                            result = Color.FromArgb(rgb, rgb, rgb);
                            break;
                        case BConvertType.Grayscale:
                            rgb = Convert.ToInt32((double)(((0.3 * pixel.R) + (0.59 * pixel.G)) + (0.11 * pixel.B)));
                            result = Color.FromArgb(rgb, rgb, rgb);
                            break;
                    }
                    bitmap.SetPixel(x, y, result);//绘图
                }
            }
            return bitmap;

        }

        #region 浮雕处理
        /// <summary>
        /// 浮雕处理
        /// </summary>
        /// <param name="oldBitmap">原始图片</param>
        /// <param name="Width">原始图片的长度</param>
        /// <param name="Height">原始图片的高度</param>
        public static Bitmap RevPicFD(Bitmap oldBitmap, int Width, int Height)
        {
            Bitmap newBitmap = new Bitmap(Width, Height);
            Color color1, color2;
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    color1 = oldBitmap.GetPixel(x, y);
                    color2 = oldBitmap.GetPixel(x + 1, y + 1);
                    r = CheckPixel(Math.Abs(color1.R - color2.R + 128));
                    g = CheckPixel(Math.Abs(color1.G - color2.G + 128));
                    b = CheckPixel(Math.Abs(color1.B - color2.B + 128));
                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return newBitmap;
        }
        #endregion
        #region 左右翻转
        /// <summary>
        /// 左右翻转
        /// </summary>
        /// <param name="mybm">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        public static Bitmap RevPicLR(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, z; //x,y是循环次数,z是用来记录像素点的x坐标的变化的
            Color pixel;
            for (y = height - 1; y >= 0; y--)
            {
                for (x = width - 1, z = 0; x >= 0; x--)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值
                    bm.SetPixel(z++, y, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图
                }
            }
            return bm;
        }
        #endregion
        #region 上下翻转
        /// <summary>
        /// 上下翻转
        /// </summary>
        /// <param name="mybm">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        public static Bitmap RevPicUD(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, z;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = height - 1, z = 0; y >= 0; y--)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值
                    bm.SetPixel(x, z++, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图
                }
            }
            return bm;
        }
        #endregion

        /// <summary>
        /// 检查像素值会不会超出[0, 255]
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static int CheckPixel(int val)
        {
            if (val < 0) return 0;
            if (val > 255) return 25; ;
            return val;
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
    }
}
