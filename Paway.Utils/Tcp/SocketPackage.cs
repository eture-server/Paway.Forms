using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Paway.Utils.Tcp
{
    /// <summary>
    /// 为每个连接到服务端的Socket连接创建实例
    /// </summary>
    public partial class SocketPackage : SocketBase
    {
        /// <summary>
        /// 线程锁
        /// </summary>
        private object Lock = new object();

        private Service Host;

        /// <summary>
        /// 为每个客户端连接创建实例
        /// </summary>
        /// <param name="host">服务端</param>
        /// <param name="socket">连接的Socket实例</param>
        public SocketPackage(Service host, Socket socket)
        {
            this.Host = host;
            this.Socket = socket;
            this.IsConnected = true;
            this.IsRegisted = false;
            this.IsUNRegisted = false;
            this.SendDataService = new SendDataService(this);
        }

        /// <summary>
        /// 重写实例 socket异常事件
        /// </summary>
        protected override void OnSocketException()
        {
            SocketBase[] socketList = SocketConfig.ClientList.Where(c => (c.IsUNRegisted || !c.IsConnected) && !c.Disposed && c.IPPoint == IPPoint).ToArray();
            ClearClientSocket(socketList);
            base.OnSocketException();
        }

        /// <summary>
        /// 清除无效客户端连接
        /// </summary>
        public void ClearClientSocket(SocketBase[] socketList)
        {
            SocketBase socket = null;
            for (int i = 0; i < socketList.Length; i++)
            {
                try
                {
                    for (int j = 0; j < SocketConfig.ClientList.Count; j++)
                    {
                        lock (Lock)
                        {
                            if (!SocketConfig.ClientList.TryTake(out socket))
                            {
                                j--; continue;
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
        /// 缓冲发送外部数据的接口
        /// </summary>
        public void SendOutSideData(object msg)
        {
            this.Host.InsertSendData(msg);
        }
    }
}
