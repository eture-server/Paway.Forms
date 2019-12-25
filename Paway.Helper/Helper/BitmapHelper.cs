using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 图像处理类
    /// </summary>
    public abstract class BitmapHelper
    {
        #region 获取Gif图片中的各帧
        /// <summary>
        /// 获取Gif图片中的各帧
        /// </summary>
        /// <param name="gPath">图片路径</param>
        /// <param name="sPath">保存路径</param>
        public static void GetFrames(string gPath, string sPath)
        {
            var gif = Image.FromFile(gPath);
            var fd = new FrameDimension(gif.FrameDimensionsList[0]);
            var count = gif.GetFrameCount(fd); //获取帧数(gif图片可能包含多帧，其它格式图片一般仅一帧)
            for (var i = 0; i < count; i++) //以Jpeg格式保存各帧
            {
                gif.SelectActiveFrame(fd, i);
                gif.Save(sPath + "\\frame_" + i + ".jpg", ImageFormat.Jpeg);
            }
        }

        #endregion

        #region 3D左右翻转
        /// <summary>
        /// 矩形图片变换成梯形图片，用于QQ的3D翻转特效
        /// </summary>
        /// <param name="src">原图</param>
        /// <param name="compressH">纵向缩放的比例</param>
        /// <param name="compressW">横向缩放的比例</param>
        /// <param name="direction">缩放位置</param>
        /// <param name="isCenter">是否水平居中</param>
        /// <returns></returns>
        public static Bitmap TrapezoidTransformation(Bitmap src, double compressH, double compressW,
            T3Direction direction, bool isCenter = true)
        {
            var rect = new Rectangle(0, 0, src.Width, src.Height);
            using (var resultH = new Bitmap(rect.Width, rect.Height))
            {
                var resultW = new Bitmap(rect.Width, rect.Height);

                #region 指针算法，高速

                //LockBits将Bitmap锁定到内存中
                var srcData = src.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                var resultHData = resultH.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                var resultWData = resultW.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                switch (direction)
                {
                    case T3Direction.Left:
                    case T3Direction.Right:

                        #region 左右

                        unsafe
                        {
                            //指向地址（分量顺序是BGRA）
                            var srcP = (byte*)srcData.Scan0; //原图地址
                            var resultHP = (byte*)resultHData.Scan0; //侧面压缩结果图像地址
                            var resultWP = (byte*)resultWData.Scan0; //纵向压缩
                            var dataW = srcData.Stride; //每行的数据量

                            var changeY = (1.0 - compressH) * rect.Height / 2 / rect.Width; //Y变化率
                            for (var y = 0; y < rect.Height; y++)
                            {
                                for (var x = 0; x < rect.Width; x++)
                                {
                                    double h = 0;
                                    double nH = 0;
                                    switch (direction)
                                    {
                                        case T3Direction.Left:
                                            if (y >= changeY * (rect.Width - x))
                                            {
                                                h = rect.Height - 2.0 * changeY * (rect.Width - x); //变化之后的每竖像素高度
                                                nH = y - changeY * (rect.Width - x); //当前像素在变化之后的高度
                                            }
                                            break;
                                        case T3Direction.Right:
                                            if (y >= changeY * x)
                                            {
                                                h = rect.Height - 2.0 * changeY * x; //变化之后的每竖像素高度
                                                nH = y - changeY * x; //当前像素在变化之后的高度
                                            }
                                            break;
                                    }

                                    var p = 1.0 * nH / h; //当前像素在变化之后的位置高度百分比
                                    var nY = (int)(rect.Height * p); //变化之后像素在原图片中的Y轴位置
                                    if (nY < rect.Height && nY > -1)
                                    {
                                        var sp = srcP + nY * dataW + x * 4; //原图的像素偏移的数据位置

                                        resultHP[0] = sp[0];
                                        resultHP[1] = sp[1];
                                        resultHP[2] = sp[2];
                                        resultHP[3] = sp[3];
                                    }
                                    resultHP += 4;
                                } // x
                            } // y

                            resultHP = (byte*)resultHData.Scan0; //重置地址
                            //纵向压缩
                            var offsetX = 0; //居中偏移
                            if (isCenter)
                            {
                                offsetX = (int)((rect.Width - compressW * rect.Width) / 2);
                            }
                            else if (direction == T3Direction.Right)
                            {
                                offsetX = (int)(rect.Width - compressW * rect.Width);
                            }

                            for (var y = 0; y < rect.Height; y++)
                            {
                                for (var x = 0; x < rect.Width; x++)
                                {
                                    var nX = (int)(1.0 * x / compressW); //纵向压缩后像素在原图片中的X轴位置
                                    if (nX > -1 && nX < rect.Width)
                                    {
                                        var hp = resultHP + nX * 4 + dataW * y;
                                        var wp = resultWP + offsetX * 4;

                                        wp[0] = hp[0];
                                        wp[1] = hp[1];
                                        wp[2] = hp[2];
                                        wp[3] = hp[3];
                                    }
                                    resultWP += 4;
                                }
                            }

                            src.UnlockBits(srcData); //从内存中解除锁定
                            resultH.UnlockBits(resultHData);
                            resultW.UnlockBits(resultWData);
                        }

                        #endregion

                        break;
                    case T3Direction.Up:
                    case T3Direction.Down:

                        #region 上下

                        unsafe
                        {
                            //指向地址（分量顺序是BGRA）
                            var srcP = (byte*)srcData.Scan0; //原图地址
                            var resultHP = (byte*)resultHData.Scan0; //侧面压缩结果图像地址
                            var resultWP = (byte*)resultWData.Scan0; //纵向压缩
                            var dataW = srcData.Stride; //每行的数据量

                            var changeX = (1.0 - compressH) * rect.Width / 2 / rect.Height; //X变化率
                            for (var y = 0; y < rect.Height; y++)
                            {
                                for (var x = 0; x < rect.Width; x++)
                                {
                                    double w = 0;
                                    double nW = 0;
                                    switch (direction)
                                    {
                                        case T3Direction.Left:
                                        case T3Direction.Up:
                                            if (x >= changeX * (rect.Height - y))
                                            {
                                                w = rect.Width - 2.0 * changeX * (rect.Height - y); //变化之后的每横像素高度
                                                nW = x - changeX * (rect.Height - y); //当前像素在变化之后的宽度
                                            }
                                            break;
                                        case T3Direction.Right:
                                        case T3Direction.Down:
                                            if (x >= changeX * y)
                                            {
                                                w = rect.Width - 2.0 * changeX * y; //变化之后的每横像素高度
                                                nW = x - changeX * y; //当前像素在变化之后的宽度
                                            }
                                            break;
                                    }

                                    var p = 1.0 * nW / w; //当前像素在变化之后的位置宽度百分比
                                    var nX = (int)(rect.Width * p); //变化之后像素在原图片中的X轴位置
                                    if (nX < rect.Width && nX > -1)
                                    {
                                        var sp = srcP + nX * 4 + y * dataW; //原图的像素偏移的数据位置

                                        resultHP[0] = sp[0];
                                        resultHP[1] = sp[1];
                                        resultHP[2] = sp[2];
                                        resultHP[3] = sp[3];
                                    }
                                    resultHP += 4;
                                } // x
                            } // y

                            resultHP = (byte*)resultHData.Scan0; //重置地址
                            //横向压缩
                            var offsetY = 0; //居中偏移
                            if (isCenter)
                            {
                                offsetY = (int)((rect.Height - compressW * rect.Height) / 2);
                            }
                            else if (direction == T3Direction.Down)
                            {
                                offsetY = (int)(rect.Height - compressW * rect.Height);
                            }

                            for (var y = 0; y < rect.Height; y++)
                            {
                                for (var x = 0; x < rect.Width; x++)
                                {
                                    var nY = (int)(1.0 * y / compressW); //横向压缩后像素在原图片中的Y轴位置
                                    if (nY > -1 && nY < rect.Height)
                                    {
                                        var hp = resultHP + nY * dataW + x * 4;
                                        var wp = resultWP + offsetY * dataW;

                                        wp[0] = hp[0];
                                        wp[1] = hp[1];
                                        wp[2] = hp[2];
                                        wp[3] = hp[3];
                                    }
                                    resultWP += 4;
                                }
                            }

                            src.UnlockBits(srcData); //从内存中解除锁定
                            resultH.UnlockBits(resultHData);
                            resultW.UnlockBits(resultWData);
                        }

                        #endregion

                        break;
                }

                #endregion

                return resultW;
            }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 截取屏幕图像
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetScreen()
        {
            Bitmap bitmap = null;
            {
                var size = Screen.PrimaryScreen.WorkingArea.Size;
                if (bitmap == null)
                    bitmap = new Bitmap(size.Width, size.Height);
                var g = Graphics.FromImage(bitmap);
                g.CopyFromScreen(Point.Empty, Point.Empty, size);
            }
            return bitmap;
        }

        /// <summary>
        /// 从指定的文件初始化图像，并指定像素格式
        /// </summary>
        public static Bitmap GetBitmapFormFile(string fileName, PixelFormat format = PixelFormat.Format32bppArgb)
        {
            var bitmap = new Bitmap(fileName);
            var image = new Bitmap(bitmap.Width, bitmap.Height, format);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
            }
            bitmap.Dispose();
            return image;
        }

        /// <summary>
        /// 裁剪指定区域图像
        /// </summary>
        public static Bitmap CutBitmap(Image image, Rectangle rect)
        {
            var temp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(temp))
            {
                graphics.DrawImage(image, new Rectangle(Point.Empty, rect.Size), rect, GraphicsUnit.Pixel);
            }
            return temp;
        }

        /// <summary>
        /// 高质量缩放图像，默认线性收缩
        /// </summary>
        public static Image HighImage(Image image, InterpolationMode mode = InterpolationMode.HighQualityBilinear)
        {
            if (image == null) return null;
            int width = image.Width;
            if (image.Height > width)
                width = image.Height;
            return HighImage(image, new Size(width, width), mode);
        }
        /// <summary>
        /// 高质量缩放图像，默认线性收缩
        /// </summary>
        public static Image HighImage(Image image, Size size, InterpolationMode mode = InterpolationMode.HighQualityBilinear)
        {
            if (image == null) return null;
            Image temp = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(temp))
            {
                g.InterpolationMode = mode;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                Rectangle rect = Rectangle.Empty;
                if (size.Width * 1.0 / size.Height > image.Width * 1.0 / image.Height)
                {
                    int width = image.Width * size.Height / image.Height;
                    rect = new Rectangle((size.Width - width) / 2, 0, width, size.Height);
                }
                else
                {
                    int height = image.Height * size.Width / image.Width;
                    rect = new Rectangle(0, (size.Height - height) / 2, size.Width, height);
                }
                g.DrawImage(image, rect, new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel);
            }
            return temp;
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
            var bitmap = new Bitmap(image);
            var width = bitmap.Width;
            var height = bitmap.Height;
            var rect = new Rectangle(0, 0, width, height);
            //用可读写的方式锁定全部位图像素
            var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe //启用不安全模式
            {
                var p = (byte*)bmpData.Scan0; //获取首地址
                p[y * width * 4 + x * 4 + 0] = color.B;
                p[y * width * 4 + x * 4 + 1] = color.G;
                p[y * width * 4 + x * 4 + 2] = color.R;
                p[y * width * 4 + x * 4 + 3] = color.A;
            }
            bitmap.UnlockBits(bmpData);
            return bitmap;
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
        public static Bitmap ConvertTo(Image image, TConvertType type, object param = null)
        {
            if (image == null) throw new ArgumentException("Image Argument can not be empty");
            switch (type)
            {
                case TConvertType.LeftRight:
                    return RevPicLR(image);
                case TConvertType.UpDown:
                    return RevPicUD(image);
            }
            //生成图
            var bitmap = new Bitmap(image);
            var width = bitmap.Width;
            var height = bitmap.Height;
            var rect = new Rectangle(0, 0, width, height);
            //用可读写的方式锁定全部位图像素
            var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte gray;
            var val = param.ToInt();
            unsafe //启用不安全模式
            {
                var p = (byte*)bmpData.Scan0; //获取首地址
                var offset = bmpData.Stride - width * 4;
                switch (type)
                {
                    case TConvertType.Relief:
                    case TConvertType.Brightness:
                        width -= 1;
                        height -= 1;
                        break;
                }
                //二维图像循环
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        switch (type)
                        {
                            case TConvertType.Relief:
                                p[0] = CheckPixel(Math.Abs(p[0] - p[(width + 2) * 4] + 128));
                                p[1] = CheckPixel(Math.Abs(p[1] - p[(width + 2) * 4 + 1] + 128));
                                p[2] = CheckPixel(Math.Abs(p[2] - p[(width + 2) * 4 + 2] + 128));
                                break;
                            case TConvertType.Brightness:
                                p[0] = CheckPixel(p[0] + val);
                                p[1] = CheckPixel(p[1] + val);
                                p[2] = CheckPixel(p[2] + val);
                                break;
                            case TConvertType.Anti:
                                p[0] = (byte)(255 - p[0]);
                                p[1] = (byte)(255 - p[1]);
                                p[2] = (byte)(255 - p[2]);
                                break;
                            case TConvertType.Color:
                                p[2] = (byte)(255 - p[0]);
                                break;
                            case TConvertType.BlackWhite:
                                gray = (byte)((p[0] + p[1] + p[2]) / 3.0f);
                                p[2] = p[1] = p[0] = gray;
                                break;
                            case TConvertType.Grayscale:
                                gray = (byte)((p[0] * 0.114f + p[1] * 0.587f + p[2] * 0.299f) / 3.0f);
                                p[2] = p[1] = p[0] = gray;
                                break;
                            case TConvertType.Trans:
                                if (p[3] > val)
                                {
                                    p[3] = (byte)val;
                                }
                                break;
                            case TConvertType.Replace:
                                if (p[3] > 0)
                                {
                                    p[2] = p[1] = p[0] = (byte)val;
                                }
                                break;
                            case TConvertType.HSL:
                                if (p[3] > 0)
                                {
                                    if (lastColor.R != p[0] || lastColor.G != p[1] || lastColor.B != p[2])
                                    {
                                        lastColor = Color.FromArgb(p[2], p[1], p[0]);
                                        var result = RGBToHSL(p[2] / 255.0, p[1] / 255.0, p[0] / 255.0);
                                        var temp = HSLToRGB(result[0], result[1], result[2] + val * 1.0 / 240);
                                        lastHsl[2] = temp.R;
                                        lastHsl[1] = temp.G;
                                        lastHsl[0] = temp.B;
                                    }
                                    p[0] = lastHsl[0];
                                    p[1] = lastHsl[1];
                                    p[2] = lastHsl[2];
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

        private static Color lastColor = Color.Transparent;
        private static readonly byte[] lastHsl = new byte[3];

        #region 左右翻转

        /// <summary>
        /// 左右翻转
        /// </summary>
        /// <param name="image">原始图片</param>
        public static Bitmap RevPicLR(Image image)
        {
            //生成图
            var bitmap = new Bitmap(image);
            //对照图
            using (var copy = new Bitmap(image))
            {
                var width = bitmap.Width;
                var height = bitmap.Height;
                var rect = new Rectangle(0, 0, width, height);
                //用可读写的方式锁定全部位图像素
                var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                var copyData = copy.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                unsafe //启用不安全模式
                {
                    var p = (byte*)bmpData.Scan0; //获取首地址
                    var f = (byte*)copyData.Scan0; //获取首地址
                                                   //二维图像循环
                                                   //for (int y = height - 1; y >= 0; y--)
                    for (var y = 0; y < height; y++)
                    {
                        for (var x = width - 1; x >= 0; x--)
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
            var bitmap = new Bitmap(image);
            //对照图
            using (var copy = new Bitmap(image))
            {
                var width = bitmap.Width;
                var height = bitmap.Height;
                var rect = new Rectangle(0, 0, width, height);
                //用可读写的方式锁定全部位图像素
                var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                var copyData = copy.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                unsafe //启用不安全模式
                {
                    var p = (byte*)bmpData.Scan0; //获取首地址
                    var f = (byte*)copyData.Scan0; //获取首地址
                                                   //二维图像循环
                    for (var x = 0; x < width; x++)
                    {
                        for (var y = height - 1; y >= 0; y--)
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
        }

        /// <summary>
        /// 顺时针90度
        /// </summary>
        public static Bitmap Rotate90(Image image)
        {
            //原图
            using (var bitmap = new Bitmap(image))
            {
                //生成图
                var width = bitmap.Width;
                var height = bitmap.Height;
                var copy = new Bitmap(height, width);
                var rect = new Rectangle(0, 0, height, width);
                //用可读写的方式锁定全部位图像素
                var bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite,
                    PixelFormat.Format32bppArgb);
                var copyData = copy.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                unsafe //启用不安全模式
                {
                    var p = (byte*)copyData.Scan0; //获取首地址
                    var f = (byte*)bmpData.Scan0; //获取首地址
                                                  //二维图像循环
                    for (var x = 0; x < width; x++)
                    {
                        for (var y = 0; y < height; y++)
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
        }

        /// <summary>
        /// 顺时针270度（逆时针90度）
        /// </summary>
        public static Bitmap Rotate270(Image image)
        {
            //原图
            using (var bitmap = new Bitmap(image))
            {
                //生成图
                var width = bitmap.Width;
                var height = bitmap.Height;
                var copy = new Bitmap(height, width);
                var rect = new Rectangle(0, 0, height, width);
                //用可读写的方式锁定全部位图像素
                var bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite,
                    PixelFormat.Format32bppArgb);
                var copyData = copy.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                unsafe //启用不安全模式
                {
                    var p = (byte*)copyData.Scan0; //获取首地址
                    var f = (byte*)bmpData.Scan0; //获取首地址
                                                  //二维图像循环
                    for (var x = 0; x < width; x++)
                    {
                        for (var y = 0; y < height; y++)
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
        }

        #endregion

        #region 指定角度旋转

        /// <summary>
        /// 指定角度旋转图片
        /// </summary>
        public static Bitmap RotateImage(Bitmap bmp, float angle, Color bkColor)
        {
            // 获得图片的高度和宽度 
            var width = bmp.Width;
            var height = bmp.Height;
            // PixelFormat指定图像中每个像素的颜色数据的格式 
            PixelFormat pixelFormat;
            if (bkColor == Color.Transparent)
            {
                pixelFormat = PixelFormat.Format32bppArgb;
            }
            else
            {
                // 获取图像像素格式 
                pixelFormat = bmp.PixelFormat;
            }

            var tempImg = new Bitmap(width, height, pixelFormat);
            // 一个 GDI+ 绘图图面 
            // 创建画布对象 
            var g = Graphics.FromImage(tempImg);
            g.Clear(bkColor);

            // 在由坐标对指定的位置，使用图像的原始物理大小绘制指定的图像 
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            // 画布路径 
            using (var path = new GraphicsPath())
            // 创建一个单位矩阵 
            using (var matrix = new Matrix())
            {
                // 向路径添加一个矩形 
                path.AddRectangle(new RectangleF(0f, 0f, width, height));
                // 沿原点并按指定角度顺时针旋转 
                matrix.Rotate(angle);

                var rct = path.GetBounds(matrix);
                var newImg = new Bitmap(Convert.ToInt32(rct.Width), Convert.ToInt32(rct.Height), pixelFormat);
                g = Graphics.FromImage(newImg);
                g.Clear(bkColor);
                // 平移来更改坐标的原点 
                g.TranslateTransform(-rct.X, -rct.Y);
                g.RotateTransform(angle);
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.DrawImageUnscaled(tempImg, 0, 0);
                g.Dispose();
                tempImg.Dispose();

                return newImg;
            }
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
            if (val > 255) return 25;
            return (byte)val;
        }

        #endregion

        #region RGB<->HSB
        /// <summary>
        /// RGB空间到HSL空间的转换
        /// 色调-饱和度-亮度(HSB) 转 Color
        /// </summary>
        internal static Color HSLToRGB(double h, double s, double l)
        {
            if (l < 0) l = 0;
            if (l > 1) l = 1;
            double R, G, B;
            double var1, var2;
            if (s == 0) //HSL values = 0 ÷ 1
            {
                R = l * 255.0; //RGB results = 0 ÷ 255
                G = l * 255.0;
                B = l * 255.0;
            }
            else
            {
                if (l < 0.5) var2 = l * (1 + s);
                else var2 = l + s - s * l;

                var1 = 2.0 * l - var2;

                R = 255.0 * Hue2RGB(var1, var2, h + 1.0 / 3.0);
                G = 255.0 * Hue2RGB(var1, var2, h);
                B = 255.0 * Hue2RGB(var1, var2, h - 1.0 / 3.0);
            }

            var r = R.ToInt();
            var g = G.ToInt();
            var b = B.ToInt();
            return Color.FromArgb(r, g, b);
        }
        private static double Hue2RGB(double v1, double v2, double vH)
        {
            if (vH < 0) vH += 1;
            if (vH > 1) vH -= 1;
            if (6.0 * vH < 1) return v1 + (v2 - v1) * 6.0 * vH;
            if (2.0 * vH < 1) return v2;
            if (3.0 * vH < 2) return v1 + (v2 - v1) * (2.0 / 3.0 - vH) * 6.0;
            return v1;
        }
        /// <summary>
        /// RGB空间到HSL空间的转换
        /// Color 转 色调-饱和度-亮度(HSB)
        /// </summary>
        internal static double[] RGBToHSL(double r, double g, double b)
        {
            double Max, Min, delR, delG, delB, delMax;

            Min = Math.Min(r, Math.Min(g, b)); //Min. value of RGB
            Max = Math.Max(r, Math.Max(g, b)); //Max. value of RGB
            delMax = Max - Min; //Delta RGB value

            var L = (Max + Min) / 2.0;
            double H = 0, S;
            if (delMax == 0) //This is a gray, no chroma...
            {
                //H = 2.0/3.0;          //Windows下S值为0时，H值始终为160（2/3*240）
                H = 0; //HSL results = 0 ÷ 1
                S = 0;
            }
            else //Chromatic data...
            {
                if (L < 0.5) S = delMax / (Max + Min);
                else S = delMax / (2 - Max - Min);

                delR = ((Max - r) / 6.0 + delMax / 2.0) / delMax;
                delG = ((Max - g) / 6.0 + delMax / 2.0) / delMax;
                delB = ((Max - b) / 6.0 + delMax / 2.0) / delMax;

                if (r == Max) H = delB - delG;
                else if (g == Max) H = 1.0 / 3.0 + delR - delB;
                else if (b == Max) H = 2.0 / 3.0 + delG - delR;

                if (H < 0) H += 1;
                if (H > 1) H -= 1;
            }

            var HSL = new double[3] { H, S, L };
            return HSL;
        }

        #endregion
    }
}