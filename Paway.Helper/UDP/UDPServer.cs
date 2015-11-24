using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// UdpClient 接收数据
    /// </summary>
    public class UDPServer : UDPBase
    {
        private AsyncCallback OnDataReceiveCallBack;
        private volatile bool IsStarting = false;

        private UdpClient udpServer = null;
        private static UDPServer instance = null;
        /// <summary>
        /// </summary>
        public static UDPServer Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new UDPServer();
                }
                return instance;
            }
        }

        /// <summary>
        /// </summary>
        public UDPServer()
        {
            if (udpServer == null)
            {
                udpServer = new UdpClient(Port, AddressFamily.InterNetwork);
            }
            this.OnDataReceiveCallBack = new AsyncCallback(OnDataReceive);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            if (IsStarting) return;
            this.udpServer.JoinMulticastGroup(BroadcastAddress);
            this.udpServer.BeginReceive(this.OnDataReceiveCallBack, this.udpServer);
            IsStarting = true;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (!IsStarting) return;
            IsStarting = false;
            this.udpServer.DropMulticastGroup(BroadcastAddress);
            this.udpServer.Close();
        }

        /// <summary>
        /// 接收数据的事件
        /// </summary>
        /// <param name="async"></param>
        private void OnDataReceive(IAsyncResult async)
        {
            string ipAddress = string.Empty;
            if (!this.IsStarting) return;//已关闭服务
            UdpClient server = null;
            try
            {
                server = (UdpClient)async.AsyncState;
                IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Any, 0);
                byte[] buffer = server.EndReceive(async, ref remoteEP);
                if (buffer != null || buffer.Length != 0 && buffer[0] == this.PackageData[0])
                {
                    ipAddress = remoteEP.Address.ToString();
                    //...
                }
            }
            catch (ObjectDisposedException ex)
            {
                //资源已释放
                server = null;
                throw new Exception("资源已释放", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("UdpClient接收数据错误", ex);
            }
            finally
            {
                server.BeginReceive(this.OnDataReceiveCallBack, server);
            }
        }
    }
}
