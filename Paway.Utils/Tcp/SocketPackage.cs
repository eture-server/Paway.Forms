using System;
using System.Collections;
using System.Linq;
using System.Net.Sockets;

namespace Paway.Utils
{
    /// <summary>
    ///     为每个连接到服务端的Socket连接创建实例
    /// </summary>
    public class SocketPackage : SocketBase
    {
        private readonly Service Host;

        /// <summary>
        ///     线程锁
        /// </summary>
        private readonly object Lock = new object();

        /// <summary>
        ///     为每个客户端连接创建实例
        /// </summary>
        /// <param name="host">服务端</param>
        /// <param name="socket">连接的Socket实例</param>
        public SocketPackage(Service host, Socket socket)
        {
            Host = host;
            Socket = socket;
            SendDataService = new SendDataService(this);
        }

        /// <summary>
        ///     重写实例 socket异常事件
        /// </summary>
        internal override void OnDisConnectEvent(SocketError type)
        {
            ClearClientSocket();
            base.OnDisConnectEvent(type);
        }

        /// <summary>
        ///     清除无效客户端连接
        /// </summary>
        private void ClearClientSocket()
        {
            try
            {
                ArrayList array = new ArrayList();
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
                            array.Add(socket);
                        }
                    }
                    for (int i = 0; i < array.Count; i++)
                        SocketConfig.ClientList.TryAdd((SocketBase)array[i]);
                }
            }
            catch (Exception ex)
            {
                OnClientEvent(string.Format("清理连接列表异常：{0}", ex));
            }
        }
    }
}