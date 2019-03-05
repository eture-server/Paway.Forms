using System;
using System.Net;
using System.Net.Sockets;

namespace Paway.Helper
{
    /// <summary>
    ///     UdpClient 接收数据
    /// </summary>
    public class UDPServer : UDPBase, IDisposable
    {
        private static UDPServer instance;
        private readonly AsyncCallback OnDataReceiveCallBack;
        private readonly UdpClient udpServer;
        private volatile bool IsStarting;

        /// <summary>
        /// </summary>
        public UDPServer()
        {
            if (udpServer == null)
            {
                udpServer = new UdpClient(Port, AddressFamily.InterNetwork);
            }
            OnDataReceiveCallBack = OnDataReceive;
        }

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
        ///     启动服务
        /// </summary>
        public void Start()
        {
            if (IsStarting) return;
            IsStarting = true;
            udpServer.JoinMulticastGroup(BroadcastAddress);
            udpServer.BeginReceive(OnDataReceiveCallBack, udpServer);
        }

        /// <summary>
        ///     停止服务
        /// </summary>
        public void Stop()
        {
            if (!IsStarting) return;
            IsStarting = false;
            udpServer.DropMulticastGroup(BroadcastAddress);
            udpServer.Close();
        }

        /// <summary>
        ///     接收数据的事件
        /// </summary>
        private void OnDataReceive(IAsyncResult async)
        {
            IPEndPoint ipAddress = null;
            if (!IsStarting) return; //已关闭服务
            UdpClient server = null;
            try
            {
                server = (UdpClient)async.AsyncState;
                ipAddress = new IPEndPoint(IPAddress.Any, 0);
                var buffer = server.EndReceive(async, ref ipAddress);
                if (buffer != null || buffer.Length != 0 && buffer[0] == Preamble)
                {
                    var data = new byte[buffer.Length - 5];
                    for (var i = 0; i < data.Length; i++)
                    {
                        data[i] = buffer[i + 5];
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
                server.BeginReceive(OnDataReceiveCallBack, server);
            }
        }

        /// <summary>
        ///     反序列化并处理消息
        /// </summary>
        private void HandleMessage(byte[] buffer, IPEndPoint ipAddress)
        {
            object message = null;
            try
            {
                message = StructHelper.GetObjectFromByte(buffer);
            }
            catch
            {
                message = buffer;
            }
            OnMessage(message, ipAddress);
        }

        #region Dispose

        /// <summary>
        ///     Disposes the instance of SocketClient.
        /// </summary>
        public bool Disposed;

        /// <summary>
        ///     释放
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
        ///     析构
        /// </summary>
        ~UDPServer()
        {
            Dispose(false);
        }

        #endregion
    }
}