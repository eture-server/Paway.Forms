using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Paway.Utils
{
    /// <summary>
    /// Tcp通讯：Socket通讯客户端
    /// </summary>
    internal class SocketClient : SocketBase
    {
        #region 构造

        /// <summary>
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="heard">头部字节长度</param>
        public SocketClient(string host, int port, int heard)
        {
            IPPoint = new IPEndPoint(IPAddress.Parse(host), port);
            Socket = new Socket(IPPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            base.heardLength = heard;
        }

        #endregion

        #region fields

        /// <summary>
        /// 连接完成
        /// </summary>
        public event Action<IPEndPoint, SocketAsyncEventArgs> ConnectFinished;

        #endregion

        #region 异步执行回调函数

        private void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            if (e != null && e.SocketError == SocketError.Success)
            {
                //启动发送数据线程
                SendDataService = new SendDataService(this);

                //启动异步接收数据操作
                var state = new AsynSocketArg();
                state.InitializeState(Socket, base.heardLength, false);
                WaitForData(state);
            }
            //处理其他操作
            OnConnectEvent(e);
        }

        /// <summary>
        /// 连接后事件（成功、失败）
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnConnectEvent(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success)
                {
                    ConnectTime = DateTime.Now;
                }
                ConnectFinished?.Invoke(IPPoint, e);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion

        #region public methord

        /// <summary>
        /// 连接主机
        /// </summary>
        public void Connect()
        {
            using (var connectArgs = new SocketAsyncEventArgs())
            {
                connectArgs.UserToken = Socket;
                connectArgs.RemoteEndPoint = IPPoint;
                connectArgs.Completed += OnConnect;
                try
                {
                    Socket.ConnectAsync(connectArgs);
                }
                catch (SocketException ex)
                {
                    OnSocketException(ex.SocketErrorCode);
                }
            }
        }

        #endregion
    }
}