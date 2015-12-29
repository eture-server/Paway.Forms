using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Paway.Helper
{
    /// <summary>
    ///     IPC通信中的图像转换方法
    /// </summary>
    [Serializable]
    public class RawImage
    {
        private readonly int format;
        private readonly int height;
        private readonly int width;
        private Bitmap _Image;

        private byte[] rawData;

        /// <summary>
        ///     构造初始化图像流
        /// </summary>
        public RawImage(Bitmap image)
        {
            if (image != null)
            {
                width = image.Width;
                height = image.Height;
                format = (int)image.PixelFormat;
                ByteData = ToRawData(image);
                image = null;
            }
        }

        /// <summary>
        ///     图像
        /// </summary>
        public Bitmap Image
        {
            get
            {
                if (_Image == null && width > 0 && height > 0)
                    _Image = FromRawData();
                return _Image;
            }
        }

        /// <summary>
        ///     图像流
        /// </summary>
        public byte[] ByteData
        {
            get { return rawData; }
            set { rawData = value; }
        }

        /// <summary>
        ///     创建图像并锁定内存，写入byte[]
        /// </summary>
        public Bitmap FromRawData()
        {
            if (width < 1 || height < 1)
                throw new ArgumentException("width和height必须大于零。");
            if (rawData == null)
                throw new ArgumentNullException("rawData", "rawData 不能为空。");

            var image = new Bitmap(width, height, (PixelFormat)format);
            var bitmapData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.WriteOnly,
                image.PixelFormat);
            var byteCount = bitmapData.Stride * bitmapData.Height;
            Marshal.Copy(rawData, 0, bitmapData.Scan0, byteCount);
            image.UnlockBits(bitmapData);

            return image;
        }

        /// <summary>
        ///     将图像锁定到内存，复制到byte[]
        /// </summary>
        public byte[] ToRawData(Bitmap image)
        {
            if (image == null)
                throw new ArgumentNullException("image", "image 不能为空。");

            var bitmapData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                image.PixelFormat);
            var byteCount = bitmapData.Stride * bitmapData.Height;
            var rawData = new byte[byteCount];
            Marshal.Copy(bitmapData.Scan0, rawData, 0, rawData.Length);
            image.UnlockBits(bitmapData);

            return rawData;
        }
    }
}