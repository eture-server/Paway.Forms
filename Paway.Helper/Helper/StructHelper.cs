using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Paway.Helper
{
    /// <summary>
    /// 结构体-内存
    /// </summary>
    public abstract class StructHelper
    {
        /// <summary>
        /// 反序列化所提供流中的数据并重新组成对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object GetObjectFromByte(byte[] data)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(data))
            {
                return formatter.Deserialize(stream);
            }
        }
        /// <summary>
        /// 将对象或具有给定根的对象序列化为所提供的流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetByteFromObject(object data)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, data);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// byte[]转图片(无编码)
        /// </summary>
        public static Image BytesToImage(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return null;
            using (var ms = new MemoryStream(buffer))
            {
                Image image = Image.FromStream(ms);
                return image;
            }
        }
        /// <summary>
        /// 图片转byte[](无编码)
        /// </summary>
        public static byte[] ImageToBytes(Image image)
        {
            if (image == null) return null;
            using (MemoryStream ms = new MemoryStream())
            {
                ImageFormat format = image.RawFormat;
                if (image.RawFormat.Equals(ImageFormat.MemoryBmp)) format = ImageFormat.Png;
                image.Save(ms, format);
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// 创建图像并锁定内存，写入byte[]
        /// </summary>
        public static Bitmap FromRawData(byte[] rawData, int width, int height, PixelFormat format = PixelFormat.Format32bppArgb)
        {
            if (width < 1 || height < 1) throw new ArgumentException("width和height必须大于零。");
            if (rawData == null) throw new ArgumentNullException("rawData");

            var image = new Bitmap(width, height, format);
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
        /// 将图像锁定到内存，复制到byte[]
        /// </summary>
        public static byte[] ToRawData(Bitmap image)
        {
            if (image == null) throw new ArgumentNullException("image");

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