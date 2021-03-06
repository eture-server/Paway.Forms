﻿using System;
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
        public static object DeserializeObject(byte[] data)
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
        public static byte[] SerializeObject(object data)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, data);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 反序列化所提供流中的数据并重新组成Image对象(无编码)
        /// </summary>
        public static Image DeserializeImage(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return null;
            using (var ms = new MemoryStream(buffer))
            {
                Image image = Image.FromStream(ms);
                return image;
            }
        }
        /// <summary>
        /// 将Image对象序列化为所提供的流(无编码)
        /// </summary>
        public static byte[] SerializeImage(Image image)
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
        public static Bitmap ImageRawData(byte[] rawData, int width, int height, PixelFormat format = PixelFormat.Format32bppArgb)
        {
            if (width < 1 || height < 1) throw new ArgumentException("Width and Height Must Be Greater Than Zero.");
            if (rawData == null) throw new ArgumentException("RawData Argument can not be empty.");

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
        public static byte[] ImageRawData(Bitmap image)
        {
            if (image == null) throw new ArgumentException("Image Argument can not be empty.");

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