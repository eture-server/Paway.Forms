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
        /// 裁剪指定区域图像
        /// </summary>
        public static Bitmap CutBitmap(Image image, Rectangle rect)
        {
            Bitmap temp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(temp))
            {
                graphics.DrawImage(image, new Rectangle(Point.Empty, rect.Size), rect, GraphicsUnit.Pixel);
            }
            return temp;
        }

        /// <summary>
        /// 高质量缩放图像，显示像素点
        /// </summary>
        public static Image HighImage(Image image, Rectangle rect)
        {
            Image temp = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(temp))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(image, new Rectangle(rect.Location, rect.Size), new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel);
                g.PixelOffsetMode = PixelOffsetMode.Default;
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
        public static Bitmap ConvertTo(Image image, TConvertType type, object param = null)
        {
            if (image == null) throw new ArgumentNullException("image");
            switch (type)
            {
                case TConvertType.LeftRight:
                    return RevPicLR(image);
                case TConvertType.UpDown:
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
                    case TConvertType.Relief:
                    case TConvertType.Brightness:
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
                                gray = (byte)((float)(p[0] + p[1] + p[2]) / 3.0f);
                                p[2] = p[1] = p[0] = gray;
                                break;
                            case TConvertType.Grayscale:
                                gray = (byte)((float)(p[0] * 0.114f + p[1] * 0.587f + p[2] * 0.299f) / 3.0f);
                                p[2] = p[1] = p[0] = gray;
                                break;
                            case TConvertType.Trans:
                                if (p[3] != 0)
                                {
                                    p[3] = (byte)val;
                                }
                                break;
                            case TConvertType.Replace:
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

        #region 指定角度旋转
        /// <summary>
        /// 指定角度旋转图片
        /// </summary>
        public static Bitmap RotateImage(Bitmap bmp, float angle, Color bkColor)
        {
            // 获得图片的高度和宽度 
            int width = bmp.Width;
            int height = bmp.Height;
            // PixelFormat指定图像中每个像素的颜色数据的格式 
            PixelFormat pixelFormat = default(PixelFormat);
            if (bkColor == Color.Transparent)
            {
                pixelFormat = PixelFormat.Format32bppArgb;
            }
            else
            {
                // 获取图像像素格式 
                pixelFormat = bmp.PixelFormat;
            }

            Bitmap tempImg = new Bitmap(width, height, pixelFormat);
            // 一个 GDI+ 绘图图面 
            // 创建画布对象 
            Graphics g = Graphics.FromImage(tempImg);
            g.Clear(bkColor);

            // 在由坐标对指定的位置，使用图像的原始物理大小绘制指定的图像 
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            // 画布路径 
            GraphicsPath path = new GraphicsPath();
            // 向路径添加一个矩形 
            path.AddRectangle(new RectangleF(0f, 0f, width, height));
            // 创建一个单位矩阵 
            Matrix matrix = new Matrix();
            // 沿原点并按指定角度顺时针旋转 
            matrix.Rotate(angle);

            RectangleF rct = path.GetBounds(matrix);
            Bitmap newImg = new Bitmap(Convert.ToInt32(rct.Width), Convert.ToInt32(rct.Height), pixelFormat);
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
        public static Bitmap TrapezoidTransformation(Bitmap src, double compressH, double compressW, T3Direction direction, bool isCenter = true)
        {
            Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
            using (Bitmap resultH = new Bitmap(rect.Width, rect.Height))
            {
                Bitmap resultW = new Bitmap(rect.Width, rect.Height);

                #region 指针算法，高速
                //LockBits将Bitmap锁定到内存中
                BitmapData srcData = src.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                BitmapData resultHData = resultH.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                BitmapData resultWData = resultW.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                switch (direction)
                {
                    case T3Direction.Left:
                    case T3Direction.Right:
                        #region 左右
                        unsafe
                        {
                            //指向地址（分量顺序是BGRA）
                            byte* srcP = (byte*)srcData.Scan0;//原图地址
                            byte* resultHP = (byte*)resultHData.Scan0;//侧面压缩结果图像地址
                            byte* resultWP = (byte*)resultWData.Scan0;//纵向压缩
                            int dataW = srcData.Stride;//每行的数据量

                            double changeY = (1.0 - compressH) * rect.Height / 2 / rect.Width;//Y变化率
                            for (int y = 0; y < rect.Height; y++)
                            {
                                for (int x = 0; x < rect.Width; x++)
                                {
                                    double h = 0;
                                    double nH = 0;
                                    switch (direction)
                                    {
                                        case T3Direction.Left:
                                            if (y >= changeY * (rect.Width - x))
                                            {
                                                h = rect.Height - 2.0 * changeY * (rect.Width - x);//变化之后的每竖像素高度
                                                nH = y - (changeY * (rect.Width - x));//当前像素在变化之后的高度
                                            }
                                            break;
                                        case T3Direction.Right:
                                            if (y >= changeY * x)
                                            {
                                                h = rect.Height - 2.0 * changeY * x;//变化之后的每竖像素高度
                                                nH = y - (changeY * x);//当前像素在变化之后的高度
                                            }
                                            break;
                                    }

                                    double p = 1.0 * nH / h;//当前像素在变化之后的位置高度百分比
                                    int nY = (int)(rect.Height * p);//变化之后像素在原图片中的Y轴位置
                                    if (nY < rect.Height && nY > -1)
                                    {
                                        byte* sp = srcP + nY * dataW + x * 4;//原图的像素偏移的数据位置

                                        resultHP[0] = sp[0];
                                        resultHP[1] = sp[1];
                                        resultHP[2] = sp[2];
                                        resultHP[3] = sp[3];
                                    }
                                    resultHP += 4;
                                } // x
                            } // y

                            resultHP = (byte*)resultHData.Scan0;//重置地址
                            //纵向压缩
                            int offsetX = 0;//居中偏移
                            if (isCenter)
                            {
                                offsetX = (int)((rect.Width - compressW * rect.Width) / 2);
                            }
                            else if (direction == T3Direction.Right)
                            {
                                offsetX = (int)((rect.Width - compressW * rect.Width));
                            }

                            for (int y = 0; y < rect.Height; y++)
                            {
                                for (int x = 0; x < rect.Width; x++)
                                {
                                    int nX = (int)(1.0 * x / compressW);//纵向压缩后像素在原图片中的X轴位置
                                    if (nX > -1 && nX < rect.Width)
                                    {
                                        byte* hp = resultHP + nX * 4 + dataW * y;
                                        byte* wp = resultWP + offsetX * 4;

                                        wp[0] = hp[0];
                                        wp[1] = hp[1];
                                        wp[2] = hp[2];
                                        wp[3] = hp[3];
                                    }
                                    resultWP += 4;
                                }
                            }

                            src.UnlockBits(srcData);//从内存中解除锁定
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
                            byte* srcP = (byte*)srcData.Scan0;//原图地址
                            byte* resultHP = (byte*)resultHData.Scan0;//侧面压缩结果图像地址
                            byte* resultWP = (byte*)resultWData.Scan0;//纵向压缩
                            int dataW = srcData.Stride;//每行的数据量

                            double changeX = (1.0 - compressH) * rect.Width / 2 / rect.Height;//X变化率
                            for (int y = 0; y < rect.Height; y++)
                            {
                                for (int x = 0; x < rect.Width; x++)
                                {
                                    double w = 0;
                                    double nW = 0;
                                    switch (direction)
                                    {
                                        case T3Direction.Left:
                                        case T3Direction.Up:
                                            if (x >= changeX * (rect.Height - y))
                                            {
                                                w = rect.Width - 2.0 * changeX * (rect.Height - y);//变化之后的每横像素高度
                                                nW = x - (changeX * (rect.Height - y));//当前像素在变化之后的宽度
                                            }
                                            break;
                                        case T3Direction.Right:
                                        case T3Direction.Down:
                                            if (x >= changeX * y)
                                            {
                                                w = rect.Width - 2.0 * changeX * y;//变化之后的每横像素高度
                                                nW = x - (changeX * y);//当前像素在变化之后的宽度
                                            }
                                            break;
                                    }

                                    double p = 1.0 * nW / w;//当前像素在变化之后的位置宽度百分比
                                    int nX = (int)(rect.Width * p);//变化之后像素在原图片中的X轴位置
                                    if (nX < rect.Width && nX > -1)
                                    {
                                        byte* sp = srcP + nX * 4 + y * dataW;//原图的像素偏移的数据位置

                                        resultHP[0] = sp[0];
                                        resultHP[1] = sp[1];
                                        resultHP[2] = sp[2];
                                        resultHP[3] = sp[3];
                                    }
                                    resultHP += 4;
                                } // x
                            } // y

                            resultHP = (byte*)resultHData.Scan0;//重置地址
                            //横向压缩
                            int offsetY = 0;//居中偏移
                            if (isCenter)
                            {
                                offsetY = (int)((rect.Height - compressW * rect.Height) / 2);
                            }
                            else if (direction == T3Direction.Down)
                            {
                                offsetY = (int)((rect.Height - compressW * rect.Height));
                            }

                            for (int y = 0; y < rect.Height; y++)
                            {
                                for (int x = 0; x < rect.Width; x++)
                                {
                                    int nY = (int)(1.0 * y / compressW);//横向压缩后像素在原图片中的Y轴位置
                                    if (nY > -1 && nY < rect.Height)
                                    {
                                        byte* hp = resultHP + nY * dataW + x * 4;
                                        byte* wp = resultWP + offsetY * dataW;

                                        wp[0] = hp[0];
                                        wp[1] = hp[1];
                                        wp[2] = hp[2];
                                        wp[3] = hp[3];
                                    }
                                    resultWP += 4;
                                }
                            }

                            src.UnlockBits(srcData);//从内存中解除锁定
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

        #region RGB<->HSB
        /// <summary>
        /// RGB空间到HSL空间的转换
        /// 色调-饱和度-亮度(HSB) 转 Color
        /// </summary>
        public static Color HSLToRGB(int H, int S, int L)
        {
            if (H > 360) H = 360; if (H < 0) H = 0;
            if (S > 100) S = 100; if (S < 0) S = 0;
            if (L > 100) L = 100; if (L < 0) L = 0;
            H = H * 255 / 360;
            S = S * 255 / 100;
            L = L * 255 / 100;

            decimal fractionalSector;
            decimal sectorNumber;
            decimal sectorPos;
            sectorPos = (Convert.ToDecimal(H) / 255 * 360) / 60;
            sectorNumber = Convert.ToInt32(Math.Floor(Convert.ToDouble(sectorPos)));
            fractionalSector = sectorPos - sectorNumber;

            decimal p;
            decimal q;
            decimal t;

            decimal r = 0;
            decimal g = 0;
            decimal b = 0;
            decimal ss = Convert.ToDecimal(S) / 255;
            decimal vv = Convert.ToDecimal(L) / 255;

            p = vv * (1 - ss);
            q = vv * (1 - (ss * fractionalSector));
            t = vv * (1 - (ss * (1 - fractionalSector)));

            switch (Convert.ToInt32(sectorNumber))
            {
                case 0:
                    r = vv;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = vv;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = vv;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = vv;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = vv;
                    break;
                case 5:
                    r = vv;
                    g = p;
                    b = q;
                    break;
            }
            return Color.FromArgb((r * 255).ToInt(), (g * 255).ToInt(), (b * 255).ToInt());
        }

        /// <summary>
        /// RGB空间到HSL空间的转换
        /// Color 转 色调-饱和度-亮度(HSB)
        /// </summary>
        public static int[] RGBToHSL(Color color)
        {
            double hue = 0;

            double Red = (double)color.R / 255;
            double Green = (double)color.G / 255;
            double Blue = (double)color.B / 255;
            double max = Math.Max(Red, Math.Max(Green, Blue));
            double min = Math.Min(Red, Math.Min(Green, Blue));

            if (max == Red && Green >= Blue)
            {
                if (max - min == 0) hue = 0.0;
                else hue = 60 * (Green - Blue) / (max - min);
            }
            else if (max == Red && Green < Blue)
            {
                hue = 60 * (Green - Blue) / (max - min) + 360;
            }
            else if (max == Green)
            {
                hue = 60 * (Blue - Red) / (max - min) + 120;
            }
            else if (max == Blue)
            {
                hue = 60 * (Red - Green) / (max - min) + 240;
            }
            double sat = (max == 0) ? 0 : (1 - (min / max));

            int[] HSL = new int[3] { hue.ToInt(), (sat * 100).ToInt(), (max * 100).ToInt() };
            return HSL;
        }

        #endregion
    }
    /// <summary>
    /// 图片转换类型
    /// </summary>
    public enum TConvertType
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
    /// <summary>
    /// 梯形方向(3D旋转)类型
    /// </summary>
    public enum T3Direction
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 左侧翻转
        /// </summary>
        Left,
        /// <summary>
        /// 右侧翻转
        /// </summary>
        Right,
        /// <summary>
        /// 上侧翻转
        /// </summary>
        Up,
        /// <summary>
        /// 下侧翻转
        /// </summary>
        Down,
    }
}
