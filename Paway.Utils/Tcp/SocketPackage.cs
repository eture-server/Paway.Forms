﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Paway.Utils
{
    /// <summary>
    /// 为每个连接到服务端的Socket连接创建实例
    /// </summary>
    public class SocketPackage : SocketBase
    {
        /// <summary>
        /// 线程锁
        /// </summary>
        private readonly object Lock = new object();

        /// <summary>
        /// 为每个客户端连接创建实例
        /// </summary>
        /// <param name="socket">连接的Socket实例</param>
        /// <param name="heard">头部字节长度</param>
        public SocketPackage(Socket socket, int heard)
        {
            base.heardLength = heard;
            Socket = socket;
            SendDataService = new SendDataService(this);
        }

        /// <summary>
        /// 重写实例 socket异常事件
        /// </summary>
        internal override void OnDisConnectEvent(SocketError type)
        {
            ClearClientSocket();
            base.OnDisConnectEvent(type);
        }

        /// <summary>
        /// 清除无效客户端连接
        /// </summary>
        private void ClearClientSocket()
        {
            try
            {
                List<SocketBase> list = new List<SocketBase>();
                lock (Lock)
                {
                    SocketBase socket = null;
                    while (SocketConfig.ClientList.Count > 0)
                    {
                        if (!SocketConfig.ClientList.TryTake(out socket))
                        {
                            continue;
                        }
                        if (socket.IPPoint == this.IPPoint || !socket.IConnected)
                        {
                            socket.Disconnect();
                            socket.Dispose();
                        }
                        else
                        {
                            list.Add(socket);
                        }
                    }
                    foreach (var item in list) SocketConfig.ClientList.TryAdd(item);
                }
            }
            catch (Exception ex)
            {
                OnClientEvent(string.Format("清理连接列表异常：{0}", ex));
            }
        }
    }
}