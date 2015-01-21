// Copyright 2011 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.ExceptionServices;
using System.Reflection;
using System.IO;
using System.Linq;
using System.ComponentModel;

namespace Mobot.Imaging
{
    /// <summary>
    /// 提供对imgcomp.dll的托管封装。
    /// </summary>
    internal class ImgCompInterop
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string lpFileName);

        private static string tempFolder = "";
        const string imgcompDllName = "imgcomp.dll";
        static ImgCompInterop()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();

            // The temporary folder holds one or more of the temporary DLLs
            // It is made "unique" to avoid different versions of the DLL or architectures.
            tempFolder = String.Format("{0}.{1}.{2}", assemblyName.Name, assemblyName.ProcessorArchitecture, assemblyName.Version);

            string dirName = Path.Combine(Path.GetTempPath(), tempFolder);
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);

            // Add the temporary dirName to the PATH environment variable (at the head!)
            string path = Environment.GetEnvironmentVariable("PATH");
            string[] pathPieces = path.Split(';');
            bool found = false;
            foreach (string pathPiece in pathPieces)
            {
                if (pathPiece == dirName)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Environment.SetEnvironmentVariable("PATH", dirName + ";" + path);
            }

            byte[] resourceBytes = Properties.Resources.imgcomp_dll;

            // See if the file exists, avoid rewriting it if not necessary
            string dllPath = Path.Combine(dirName, imgcompDllName);
            bool rewrite = true;
            if (File.Exists(dllPath))
            {
                byte[] existing = File.ReadAllBytes(dllPath);
                if (resourceBytes.SequenceEqual(existing))
                {
                    rewrite = false;
                }
            }
            if (rewrite)
            {
                File.WriteAllBytes(dllPath, resourceBytes);
            }

            //载入到内存
            IntPtr h = LoadLibrary(imgcompDllName);
            if (h == IntPtr.Zero)
            {
                throw new DllNotFoundException("Unable to load library: " + imgcompDllName + " from " + tempFolder);
            }
        }


        const long max_Len = 1024 * 768;
        [StructLayout(LayoutKind.Sequential)]
        struct SearchRange
        {
            Int32 Left, Top, Right, Bottom;
            public SearchRange(Int32 Left, Int32 Top, Int32 Right, Int32 Bottom)
            {
                this.Left = Left;
                this.Top = Top;
                this.Right = Right;
                this.Bottom = Bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CompareOut
        {
            public Int32 Left, Top;
            public double Similarity;
            public CompareOut(Int32 Left, Int32 Top, double Similarity)
            {
                this.Left = Left;
                this.Top = Top;
                this.Similarity = Similarity;
            }
        }

        /// <summary>
        /// do_Image_Compare 函数
        /// </summary>
        /// <param name="A0">待比对大图A0</param>
        /// <param name="H0">Image Size (Width, Height)该图像的行数H0</param>
        /// <param name="W0">该图像的列数W0</param>
        /// <param name="T0">用于比对的小图标T0</param>
        /// <param name="H1">subImage Size (Width, Height)图标的行数H1</param>
        /// <param name="W1">列数W1</param>
        /// <param name="R0">Search Range Size (Width, Height)搜索范围R0</param>
        /// <returns>比对结果 － 见 Call_Image_Compare.h 文件的说明 Compare Result: (Left, Top, Similarity)</returns>
        [DllImport(imgcompDllName, EntryPoint = "do_Image_Compare", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        static extern CompareOut do_Image_Compare(byte[] A0, Int32 H0, Int32 W0, byte[] T0, Int32 H1, Int32 W1, SearchRange R0);

        /// <summary>
        /// 8位灰度图像的比较，数据为一维数组，先行后列。
        /// </summary>
        [HandleProcessCorruptedStateExceptions]
        static void CompareImageRaw8(
            byte[] imageRawData, Size imageSize,
            byte[] subImageRawData, Size subImageSize,
            Rectangle area,
            out Point location, out double similarity)
        {
            byte[] imageMatrix = Raw2Matrix(imageRawData, imageSize);
            byte[] subImageMatrix = Raw2Matrix(subImageRawData, subImageSize);

            try
            {
                CompareOut result = do_Image_Compare(
                    imageMatrix, imageSize.Height, imageSize.Width,
                    subImageMatrix, subImageSize.Height, subImageSize.Width,
                    new SearchRange(area.X, area.Y, area.Right, area.Bottom));

                location = new Point(result.Left, result.Top);

                similarity = result.Similarity;
            }
            catch (Exception ex)
            {
                log.Error("图像比对失败", ex);
                location = Point.Empty;
                similarity = 0.0;
            }
            imageMatrix = null;
            subImageMatrix = null;
        }

        /// <summary>
        /// 获取位图的原始数据。
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ToRawData(Bitmap image)
        {
            BitmapData bmpData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                image.PixelFormat);
            IntPtr ptr = bmpData.Scan0;

            byte[] rawData = new byte[bmpData.Stride * image.Height];
            Marshal.Copy(ptr, rawData, 0, rawData.Length);
            image.UnlockBits(bmpData);

            return rawData;
        }

        /// <summary>
        /// 8bit位图RAW转换为Matlab矩阵：行列互换，并去掉原BMP的4字节补齐。
        /// </summary>
        /// <returns></returns>
        static byte[] Raw2Matrix(byte[] imageRawData, Size imageSize)
        {
            byte[] imageMatrix = new byte[imageSize.Width * imageSize.Height];

            int rawWidth = (int)Math.Ceiling((double)imageSize.Width / 4.0) * 4;
            int rawHeight = imageSize.Height;
            for (int row = 0; row < imageSize.Height; row++)
                for (int col = 0; col < imageSize.Width; col++)
                    imageMatrix[col * imageSize.Height + row] = imageRawData[row * rawWidth + col];

            return imageMatrix;
        }

        private static Object thisLock = new Object();
        /// <summary>
        /// 在图片中的指定区域内查找图标。
        /// </summary>
        /// <param name="image">要处理的图片</param>
        /// <param name="subImage">图标（子图）</param>
        /// <param name="area">可能出现的区域</param>
        /// <param name="location">输出：最佳位置</param>
        /// <param name="similarity">输出：相似度（0.0 - 1.0）</param>
        public static void CompareImage(Bitmap image, Bitmap subImage, Rectangle area, out Point location, out double similarity)
        {
            if (image == null)
                throw new ArgumentNullException("image", "image 不能为空。");
            if (subImage == null)
                throw new ArgumentNullException("subImage", "subImage 不能为空。");

            //if (image.Width * image.Height > max_Len)
            //    throw new ArgumentException(
            //        string.Format("图像尺寸过大（总像素数{0}x{1}={2}，超过{3}）。", image.Width, image.Height, image.Width * image.Height, max_Len),
            //        "");
            //if (image.Width * image.Height > max_Len)
            //    throw new ArgumentException(
            //        string.Format("图像尺寸过大（总像素数{0}x{1}={2}，超过{3}）。", image.Width, image.Height, image.Width * image.Height, max_Len),
            //        "");

            Size imageSize, subImageSize;
            byte[] imageRawData, subImageRawData;
            lock (image)
            {
                //先转换为8位灰度图像
                Bitmap image8bit = image.Clone() as Bitmap;
                if (image8bit.PixelFormat != PixelFormat.Format8bppIndexed)
                {
                    image8bit = AForge.Imaging.Image.Clone(image8bit, PixelFormat.Format8bppIndexed);
                    AForge.Imaging.Filters.IFilter filterGrayscale = AForge.Imaging.Filters.Grayscale.CommonAlgorithms.BT709;
                    image8bit = filterGrayscale.Apply(image8bit);
                }
                imageRawData = ToRawData(image8bit);
                imageSize = image8bit.Size;

                image8bit.Dispose();
                image8bit = null;
            }
            lock (subImage)
            {
                Bitmap subImage8bit = subImage.Clone() as Bitmap;
                if (subImage8bit.PixelFormat != PixelFormat.Format8bppIndexed)
                {
                    subImage8bit = AForge.Imaging.Image.Clone(subImage8bit, PixelFormat.Format8bppIndexed);
                    AForge.Imaging.Filters.IFilter filterGrayscale = AForge.Imaging.Filters.Grayscale.CommonAlgorithms.BT709;
                    subImage8bit = filterGrayscale.Apply(subImage8bit);
                }

                subImageRawData = ToRawData(subImage8bit);
                subImageSize = subImage8bit.Size;

                subImage8bit.Dispose();
                subImage8bit = null;
            }
            //整个图片区域
            Rectangle imageRect = new Rectangle(0, 0, imageSize.Width, imageSize.Height);
            if (area == Rectangle.Empty)
                area = imageRect;
            else
            {//整理该区域，确保区域正确，且未超出边界
                area.Intersect(imageRect);
            }
            lock (thisLock)
            {
                CompareImageRaw8(
                    imageRawData, imageSize,
                    subImageRawData, subImageSize,
                    area,
                    out location,
                    out similarity);
            }
            imageRawData = null;
            subImageRawData = null;
        }
    }
}
