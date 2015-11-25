using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 接收消息体
    /// </summary>
    public class UDPEventArgs : EventArgs
    {
        /// <summary>
        /// 错误标记
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 客户端地址
        /// </summary>
        public IPEndPoint IpAddress { get; set; }

        /// <summary>
        /// 消息事件
        /// </summary>
        public UDPEventArgs(bool result, object msg, IPEndPoint ipAddress)
        {
            this.Result = result;
            this.IpAddress = IpAddress;
            this.Data = msg;
        }
    }
}
