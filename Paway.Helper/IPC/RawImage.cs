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
        private readonly PixelFormat format;
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
                format = image.PixelFormat;
                ByteData = StructHelper.ToRawData(image);
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
                    _Image = StructHelper.FromRawData(rawData, width, height, format);
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
    }
}