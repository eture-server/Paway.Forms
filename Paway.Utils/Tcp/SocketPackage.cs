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
            IsConnected = true;
            IsRegisted = false;
            IsUNRegisted = false;
            SendDataService = new SendDataService(this);
        }

        /// <summary>
        ///     重写实例 socket异常事件
        /// </summary>
        protected override void OnSocketException(string msg)
        {
            var socketList =
                SocketConfig.ClientList.Where(
                    c => (c.IsUNRegisted || !c.IsConnected) && !c.Disposed && c.IPPoint == IPPoint).ToArray();
            ClearClientSocket(socketList);
            base.OnSocketException(msg);
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

        /// <summary>
        ///     缓冲发送外部数据的接口
        /// </summary>
        public void SendOutSideData(object msg, bool ithrow = true)
        {
            Host.InsertSendData(msg, ithrow);
        }
    }
}