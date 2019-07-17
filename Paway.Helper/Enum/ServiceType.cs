using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// </summary>
        None,

        /// <summary>
        /// 超过最大连接数
        /// </summary>
        Limit,

        /// <summary>
        /// 一定时间内连接过多
        /// </summary>
        Current,

        /// <summary>
        /// 连接
        /// </summary>
        Connect,

        /// <summary>
        /// 断开
        /// </summary>
        DisConnect,

        /// <summary>
        /// 客户端消息
        /// </summary>
        Client,

        /// <summary>
        /// 错误
        /// </summary>
        Error
    }
}
