﻿using System;
using System.Net.Sockets;

namespace Paway.Utils.Tcp
{
    /// <summary>
    ///     系统消息
    /// </summary>
    public class ServiceEventArgs : EventArgs
    {
        /// <summary>
        ///     事件类型构造
        /// </summary>
        public ServiceEventArgs(ServiceType type)
        {
            Type = type;
        }

        /// <summary>
        ///     客户端Ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        ///     客户端Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///     结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        ///     消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Socket连接错误
        /// </summary>
        public SocketError SocketError { get; set; }

        /// <summary>
        ///     消息类型
        /// </summary>
        public ServiceType Type { get; set; }
    }

    /// <summary>
    ///     消息类型
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// </summary>
        None,

        /// <summary>
        ///     超过最大连接数
        /// </summary>
        Limit,

        /// <summary>
        ///     一定时间内连接过多
        /// </summary>
        Current,

        /// <summary>
        ///     连接
        /// </summary>
        Connect,

        /// <summary>
        ///     断开
        /// </summary>
        DisConnect,

        /// <summary>
        ///     客户端消息
        /// </summary>
        Client,

        /// <summary>
        ///     错误
        /// </summary>
        Error
    }
}