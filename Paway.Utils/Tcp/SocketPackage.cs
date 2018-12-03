using System;
using System.Linq;
using System.Net.Sockets;

namespace Paway.Utils.Tcp
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
            IConnected = true;
            IRegisted = false;
            IUNRegisted = false;
            SendDataService = new SendDataService(this);
        }

        /// <summary>
        ///     重写实例 socket异常事件
        /// </summary>
        protected override void OnDisConnectEvent(SocketError type)
        {
            var socketList =
                SocketConfig.ClientList.Where(
                    c => (c.IUNRegisted || !c.IConnected) && !c.Disposed && c.IPPoint == IPPoint).ToArray();
            ClearClientSocket(socketList);
            base.OnDisConnectEvent(type);
        }

        /// <summary>
        ///     清除无效客户端连接
        /// </summary>
        public void ClearClientSocket(SocketBase[] socketList)
        {
            SocketBase socket = null;
            for (var i = 0; i < socketList.Length; i++)
            {
                try
                {
                    for (var j = 0; j < SocketConfig.ClientList.Count; j++)
                    {
                        lock (Lock)
                        {
                            if (!SocketConfig.ClientList.TryTake(out socket))
                            {
                                j--;
                                continue;
                            }
                            if (!socket.IPPoint.Equals(socketList[i].IPPoint))
                            {
                                SocketConfig.ClientList.Add(socket);
                            }
                            else break;
                        }
                    }
                    if (socket == null) return;
                    socket.Disconnect();
                    socket.Dispose();
                }
                catch (Exception ex)
                {
                    OnClientEvent(string.Format("清理连接列表异常：{0}", ex));
                }
            }
        }
    }
}