using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Paway.Utils.Tcp
{
    /// <summary>
    /// Tcp通讯：Socket通讯客户端
    /// </summary>
    public class SocketClient : SocketBase
    {
        #region fields
        private IPEndPoint _host;
        /// <summary>
        /// Listener endpoint.
        /// </summary>
        public IPEndPoint EndPoint { get { return _host; } }
        /// <summary>
        /// 连接完成
        /// </summary>
        public event EventHandler<SocketAsyncEventArgs> ConnectFinished;

        #endregion

        #region 构造
        /// <summary>
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public SocketClient(String ipAddress, Int32 port)
        {
            this._host = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            this.IPPort = _host.ToString();
            this.Socket = new Socket(this._host.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion

        #region 异步执行回调函数
        private void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            if (e != null && e.SocketError == SocketError.Success)
            {
                this.IsConnected = true;
                //启动发送数据线程
                this.SendDataService = new SendDataService(this);

                //启动异步接收数据操作
                AsynSocketArg state = new AsynSocketArg();
                state.InitializeState(this.Socket);
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
                if (ConnectFinished != null)
                {
                    ConnectFinished(this.IPPort, e);
                }
            }
            catch { }
        }

        #endregion

        #region public methord
        /// <summary>
        /// 连接主机
        /// </summary>
        public void Connect()
        {
            using (SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs())
            {
                connectArgs.UserToken = this.Socket;
                connectArgs.RemoteEndPoint = this._host;
                connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnect);
                try
                {
                    Socket.ConnectAsync(connectArgs);
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    OnSocketException(ex.SocketErrorCode);
                }
            }
        }

        /// <summary>
        /// 注销连接
        /// </summary>
        public void StopConnect()
        {
            Disconnect();
            Dispose();
        }
        #endregion
    }
}
