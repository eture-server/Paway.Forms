using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// IPC通信中的图像转换方法
    /// </summary>
    [Serializable]
    public class RawImage
    {
        private int width;
        private int height;
        private int format;

        private Bitmap _Image = null;
        /// <summary>
        /// 图像
        /// </summary>
        public Bitmap Image
        {
            get
            {
                if (_Image == null && this.width > 0 && this.height > 0)
                    _Image = this.FromRawData();
                return _Image;
            }
        }

        private byte[] rawData = null;
        /// <summary>
        /// 图像流
        /// </summary>
        public byte[] ByteData
        {
            get { return rawData; }
            set { rawData = value; }
        }

        /// <summary>
        /// 构造初始化图像流
        /// </summary>
        public RawImage(Bitmap image)
        {
            if (image != null)
            {
                this.width = image.Width;
                this.height = image.Height;
                this.format = (int)image.PixelFormat;
                this.ByteData = ToRawData(image);
                image = null;
            }
        }

        /// <summary>
        /// 创建图像并锁定内存，写入byte[]
        /// </summary>
        public Bitmap FromRawData()
        {
            if (width < 1 || height < 1)
                throw new ArgumentException("width和height必须大于零。");
            if (this.rawData == null)
                throw new ArgumentNullException("rawData", "rawData 不能为空。");

            Bitmap image = new Bitmap(this.width, this.height, (PixelFormat)this.format);
            BitmapData bitmapData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.WriteOnly,
                image.PixelFormat);
            int byteCount = bitmapData.Stride * bitmapData.Height;
            Marshal.Copy(rawData, 0, bitmapData.Scan0, byteCount);
            image.UnlockBits(bitmapData);

            return image;
        }
        /// <summary>
        /// 将图像锁定到内存，复制到byte[]
        /// </summary>
        public byte[] ToRawData(Bitmap image)
        {
            if (image == null)
                throw new ArgumentNullException("image", "image 不能为空。");

            BitmapData bitmapData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                image.PixelFormat);
            int byteCount = bitmapData.Stride * bitmapData.Height;
            byte[] rawData = new byte[byteCount];
            Marshal.Copy(bitmapData.Scan0, rawData, 0, rawData.Length);
            image.UnlockBits(bitmapData);

            return rawData;
        }
    }
}
