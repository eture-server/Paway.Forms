﻿using System.Collections;
using System.Net.Sockets;

namespace Paway.Utils
{
    /// <summary>
    ///     数据接收方法
    /// </summary>
    internal class AsynSocketArg
    {
        private const int BufferSize = 1024;

        private byte[] buffer = new byte[BufferSize];

        private int length = -1;

        private ArrayList lstBuffer = new ArrayList();

        /// <summary>
        ///     接收的Socket实例对象
        /// </summary>
        public Socket WorkSocket { get; private set; }

        /// <summary>
        ///     自动调节的缓冲区大小
        /// </summary>
        public int AutoBufferSize
        {
            get
            {
                int size;
                // 先获取4字节数据
                if (length == -1)
                {
                    size = 4;
                }
                // 中间数据设置1024缓冲区
                else if (length - lstBuffer.Count > BufferSize)
                {
                    size = BufferSize;
                }
                // 最后的剩余数据根据长度设置缓冲区
                else
                {
                    size = length - lstBuffer.Count;
                }
                return size;
            }
        }

        /// <summary>
        ///     数据长度
        /// </summary>
        public int ReceiveDataLength
        {
            get
            {
                if (length == -1 && lstBuffer.Count >= 4)
                {
                    length = ((byte)lstBuffer[0] << 24 | (byte)lstBuffer[1] << 16 | (byte)lstBuffer[2] << 8 |
                              (byte)lstBuffer[3] << 0) + 4;
                }
                return length;
            }
        }

        /// <summary>
        ///     接收到的数据缓冲区
        /// </summary>
        public ArrayList LstBuffer
        {
            get
            {
                if (lstBuffer == null)
                    lstBuffer = new ArrayList();

                return lstBuffer;
            }
        }

        /// <summary>
        ///     数据缓冲区
        /// </summary>
        public byte[] GetBuffer()
        {
            return buffer;
        }

        /// <summary>
        ///     向数据缓冲区添加接收到的有效数据
        /// </summary>
        /// <param name="bufferValue"></param>
        /// <param name="valueLength"></param>
        public void AddBufferData(byte[] bufferValue, int valueLength)
        {
            if (bufferValue == null) return;
            for (var i = 0; i < valueLength; i++)
            {
                lstBuffer.Add(bufferValue[i]);
            }
        }

        /// <summary>
        ///     初始化state对象
        /// </summary>
        public void InitializeState()
        {
            length = -1;
            lstBuffer = new ArrayList();
            buffer = new byte[AutoBufferSize];
        }

        /// <summary>
        ///     初始化state对象
        /// </summary>
        public void InitializeState(Socket socket)
        {
            InitializeState();
            WorkSocket = socket;
            socket.SetSocketKeepAliveValues(1000, 1000);
        }

        /// <summary>
        ///     重置缓冲区大小
        /// </summary>
        public void ResetBuffer()
        {
            buffer = new byte[AutoBufferSize];
        }
    }
}