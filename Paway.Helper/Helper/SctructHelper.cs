using System;
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
        ///     由结构体分配到内存，返回句柄
        ///     需指定大小
        ///     [MarshalAs(UnmanagedType.LPStr, SizeConst = 1024)]
        /// </summary>
        public static IntPtr StructureToByte<T>(T structure)
        {
            var size = Marshal.SizeOf(typeof(T));
            var buffer = new byte[size];
            var bufferIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, bufferIntPtr, true);
                //Marshal.Copy(bufferIntPtr, buffer, 0, size);
            }
            finally
            {
                //Marshal.FreeHGlobal(bufferIntPtr);
            }
            return bufferIntPtr;
        }

        /// <summary>
        ///     由句柄转换为结构体
        /// </summary>
        public static T ByteToStructure<T>(IntPtr allocIntPtr)
        {
            object structure = null;
            var size = Marshal.SizeOf(typeof(T));
            //IntPtr allocIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                //Marshal.Copy(dataBuffer, 0, allocIntPtr, size);
                structure = Marshal.PtrToStructure(allocIntPtr, typeof(T));
            }
            finally
            {
                //Marshal.FreeHGlobal(allocIntPtr);
            }
            return (T)structure;
        }

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
    }
}