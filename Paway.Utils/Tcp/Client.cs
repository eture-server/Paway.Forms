using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Paway.Utils.Tcp
{
    /// <summary>
    /// 封装Socket通讯客户端
    /// </summary>
    public class Client : SocketClient
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="IpAddress"></param>
        /// <param name="port"></param>
        public Client(String IpAddress, Int32 port)
            : base(IpAddress, port)
        {
        }
        /// <summary>
        /// 连接完成
        /// </summary>
        /// <param name="e"></param>
        protected override void OnConnectEvent(SocketAsyncEventArgs e)
        {
            base.OnConnectEvent(e);
        }
        /// <summary>
        /// 连接错误
        /// </summary>
        protected override void OnSocketException()
        {
            base.OnSocketException();
        }
    }
}
