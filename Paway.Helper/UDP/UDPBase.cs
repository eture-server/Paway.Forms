using System;
using System.Net;

namespace Paway.Helper
{
    /// <summary>
    ///     UDPClient基础类
    /// </summary>
    public class UDPBase
    {
        /// <summary>
        ///     广播地址
        /// </summary>
        protected readonly IPAddress BroadcastAddress = IPAddress.Parse("224.224.224.224");

        /// <summary>
        ///     标示
        /// </summary>
        protected readonly byte Preamble = 0xFA;

        /// <summary>
        ///     通信端口
        /// </summary>
        protected int Port = 2008;

        /// <summary>
        ///     服务器的地址
        /// </summary>
        protected IPEndPoint ServerAddress
        {
            get { return new IPEndPoint(BroadcastAddress, Port); }
        }

        /// <summary>
        ///     接收到的消息
        /// </summary>
        public event Action<UDPEventArgs> MessageEvent;

        /// <summary>
        ///     抛出异常
        /// </summary>
        protected void OnError(string msg, IPEndPoint ipAddress)
        {
            MessageEvent?.Invoke(new UDPEventArgs(false, msg, ipAddress));
        }

        /// <summary>
        ///     抛出消息
        /// </summary>
        protected void OnMessage(object msg, IPEndPoint ipAddress)
        {
            try
            {
                MessageEvent?.Invoke(new UDPEventArgs(true, msg, ipAddress));
            }
            catch
            {
            }
        }
    }
}