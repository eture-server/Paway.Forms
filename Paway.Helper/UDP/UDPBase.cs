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
        private readonly byte Preamble = 0XFA;
        /// <summary>
        /// 广播地址
        /// </summary>
        protected readonly IPAddress BroadcastAddress = IPAddress.Parse("224.224.224.224");
        /// <summary>
        /// 通信端口
        /// </summary>
        protected int Port = 2008;
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
        /// 获取消息包数据 虚函数，以实现多态
        /// </summary>
        protected byte[] PackageData
        {
            get { return new byte[] { this.Preamble }; }
        }
    }
}
