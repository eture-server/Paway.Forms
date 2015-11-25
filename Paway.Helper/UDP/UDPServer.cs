using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// UdpClient 接收数据
    /// </summary>
    public class UDPServer : UDPBase, IDisposable
    {
        private AsyncCallback OnDataReceiveCallBack;
        private volatile bool IsStarting = false;
        private UdpClient udpServer = null;

        private static UDPServer instance = null;
        /// <summary>
        /// </summary>
        public static UDPServer Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new UDPServer();
                }
                return instance;
            }
        }

        /// <summary>
        /// </summary>
        public UDPServer()
        {
            if (udpServer == null)
            {
                udpServer = new UdpClient(Port, AddressFamily.InterNetwork);
            }
            this.OnDataReceiveCallBack = new AsyncCallback(OnDataReceive);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            if (IsStarting) return;
            IsStarting = true;
            this.udpServer.JoinMulticastGroup(BroadcastAddress);
            this.udpServer.BeginReceive(this.OnDataReceiveCallBack, this.udpServer);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (!IsStarting) return;
            IsStarting = false;
            this.udpServer.DropMulticastGroup(BroadcastAddress);
            this.udpServer.Close();
        }

        /// <summary>
        /// 接收数据的事件
        /// </summary>
        private void OnDataReceive(IAsyncResult async)
        {
            IPEndPoint ipAddress = null;
            if (!this.IsStarting) return;//已关闭服务
            UdpClient server = null;
            try
            {
                server = (UdpClient)async.AsyncState;
                ipAddress = new IPEndPoint(System.Net.IPAddress.Any, 0);
                byte[] buffer = server.EndReceive(async, ref ipAddress);
                if (buffer != null || buffer.Length != 0 && buffer[0] == this.Preamble)
                {
                    byte[] data = new byte[buffer.Length - 5];
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = (byte)buffer[i + 5];
                    }
                    HandleMessage(data, ipAddress);
                }
            }
            catch (Exception ex)
            {
                OnError(ex.Message, ipAddress);
            }
            finally
            {
                server.BeginReceive(this.OnDataReceiveCallBack, server);
            }
        }
        /// <summary>
        /// 反序列化并处理消息
        /// </summary>
        private void HandleMessage(byte[] buffer, IPEndPoint ipAddress)
        {
            object message = null;
            try
            {
                try
                {
                    message = SctructHelper.GetObjectFromByte(buffer);
                }
                catch
                {
                    message = buffer;
                }
                OnMessage(message, ipAddress);
            }
            catch { }
        }

        #region Dispose
        /// <summary>
        /// Disposes the instance of SocketClient.
        /// </summary>
        public bool Disposed = false;
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                Disposed = true;
                if (disposing)
                {
                    Stop();
                }
            }
            Disposed = true;
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~UDPServer()
        {
            Dispose(false);
        }

        #endregion
    }
}
