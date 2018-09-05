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
    ///     结构体-内存
    /// </summary>
    public abstract class SctructHelper
    {
        /// <summary>
        ///     反序列化所提供流中的数据并重新组成对象
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
        ///     将对象或具有给定根的对象序列化为所提供的流
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
            MemoryStream ms = new MemoryStream(buffer);
            Image image = Image.FromStream(ms);
            return image;
        }
        /// <summary>
        /// 图片转byte[](无编码)
        /// </summary>
        public static byte[] ImageToBytes(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (image.RawFormat.Equals(ImageFormat.MemoryBmp)) return null;
                image.Save(ms, image.RawFormat);
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
    }
}