using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// UDPClient基础类
    /// </summary>
    public class UDPBase
    {
        /// <summary>
        /// 标示
        /// </summary>
        protected readonly byte Preamble = 0xFA;
        /// <summary>
        /// 广播地址
        /// </summary>
        protected readonly IPAddress BroadcastAddress = IPAddress.Parse("224.224.224.224");
        /// <summary>
        /// 通信端口
        /// </summary>
        protected int Port = 2008;
        /// <summary>
        /// 接收到的消息
        /// </summary>
        public event EventHandler<UDPEventArgs> MessageEvent;
        /// <summary>
        /// 服务器的地址
        /// </summary>
        protected System.Net.IPEndPoint ServerAddress
        {
            get
            {
                return new IPEndPoint(BroadcastAddress, Port);
            }
        }
        /// <summary>
        /// 抛出异常
        /// </summary>
        protected void OnError(string msg, IPEndPoint ipAddress)
        {
            if (MessageEvent != null)
                MessageEvent(null, new UDPEventArgs(false, msg, ipAddress));
        }
        /// <summary>
        /// 抛出消息
        /// </summary>
        protected void OnMessage(object msg, IPEndPoint ipAddress)
        {
            if (MessageEvent != null)
                MessageEvent(null, new UDPEventArgs(true, msg, ipAddress));
        }
    }
}
