using log4net;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Paway.Helper
{
    /// <summary>
    ///     UdpClient 接收数据
    /// </summary>
    public class UDPServer : UDPBase, IDisposable
    {
        #region 字段与属性
        private volatile bool IsStarting;
        private static UDPServer instance;
        private readonly AsyncCallback OnDataReceiveCallBack;
        private readonly UdpClient udpServer;

        #endregion

        #region public method
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
            if (udpServer != null)
            {
                udpServer.DropMulticastGroup(BroadcastAddress);
                udpServer.Close();
            }
        }

        #endregion

        #region private method
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
            object message;
            try
            {
                message = StructHelper.GetObjectFromByte(buffer);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = buffer;
            }
            OnMessage(message, ipAddress);
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// 标识此对象已释放
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// 参数为true表示释放所有资源，只能由使用者调用
        /// 参数为false表示释放非托管资源，只能由垃圾回收器自动调用
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                if (disposing)
                {
                    // TODO: 释放托管资源(托管的对象)。
                }
                // TODO: 释放未托管资源(未托管的对象)
                Stop();
            }
        }
        /// <summary>
        /// 析构，释放非托管资源
        /// </summary>
        ~UDPServer()
        {
            Dispose(false);
        }
        /// <summary>
        /// 释放资源
        /// 由类的使用者，在外部显示调用，释放类资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}