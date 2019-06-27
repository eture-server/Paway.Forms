using log4net;
using Paway.Helper;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace Paway.Utils
{
    /// <summary>
    ///     封装Socket通讯客户端
    /// </summary>
    public class Client
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IConnected { get { return client.IConnected; } }
        /// <summary>
        /// 本地端口
        /// </summary>
        public string IpPort { get { return client.Socket.LocalEndPoint.ToString(); } }
        /// <summary>
        /// 连接事件
        /// </summary>
        public event Action<bool> ConnectEvent;
        /// <summary>
        /// 接收消息事件
        /// </summary>
        public event Action<object> MessageEvent;

        private string Host;
        private int Port;
        private SocketClient client;
        /// <summary>
        /// 头部数据长度
        /// </summary>
        private int heardLength;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="heard">头部字节长度</param>
        public Client(string host, int port, int heard = 2)
        {
            this.Host = host;
            this.Port = port;
            this.heardLength = heard;
        }
        /// <summary>
        /// 重置并重连
        /// </summary>
        public void Reset(string host, int port)
        {
            this.Host = host;
            this.Port = port;
            this.Stop();
            this.Connect();
        }

        #region Test
        /// <summary>
        /// 连接测试
        /// </summary>
        public void TestConnection(int timeout = 500)
        {
            if (!this.IConnected) throw new WarningException("Not Connected, Please Wait..");
            TcpClient client = new TcpClient();
            try
            {
                var ar = client.BeginConnect(Host, Port, null, null);
                ar.AsyncWaitHandle.WaitOne(timeout);
                if (!client.Connected)
                {
                    this.client.Disconnect();
                    throw new WarningException("Connect Test Falied, Please Wait..");
                }
            }
            finally
            {
                client.Close();
            }
        }

        #endregion

        #region 连接服务端
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (client != null)
            {
                client.ConnectFinished -= Client_ConnectFinished;
                client.ClientEvent -= Client_ClientEvent;
                client.MessageEvent -= Client_MessageEvent;
                client.Disconnect();
                client.Dispose();
            }
        }
        /// <summary>
        /// 连接
        /// </summary>
        public void Connect()
        {
            Thread.Sleep(100);
            Stop();
            if (string.IsNullOrEmpty(Host)) Host = HardWareHandler.GetIpAddress();
            client = new SocketClient(Host, Port, heardLength);
            client.ConnectFinished += Client_ConnectFinished;
            client.ClientEvent += Client_ClientEvent;
            client.MessageEvent += Client_MessageEvent;
            client.Connect();
        }
        /// <summary>
        /// 发送
        /// </summary>
        public void Send(object msg)
        {
            this.client.Send(msg);
        }
        /// <summary>
        /// 同步直接发送
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="iPackage">true=封装</param>
        public void SendSync(byte[] buffer, bool iPackage = true)
        {
            this.client.SendSync(buffer, iPackage);
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        protected virtual void Client_MessageEvent(object sender)
        {
            MessageEvent?.Invoke(sender);
        }

        private void Client_ClientEvent(ServiceEventArgs e)
        {
            try
            {
                if (e.Type == ServiceType.DisConnect)
                {
                    ConnectEvent?.Invoke(false);
                    switch (e.SocketError)
                    {
                        case SocketError.NotConnected:
                            Thread.Sleep(1000);
                            break;
                        default:
                            Thread.Sleep(125);
                            break;
                    }
                    Connect();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void Client_ConnectFinished(IPEndPoint sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                ConnectEvent?.Invoke(true);
            }
            else
            {
                ConnectEvent?.Invoke(false);
                switch (e.SocketError)
                {
                    case SocketError.HostUnreachable:
                    case SocketError.ConnectionAborted:
                    case SocketError.NetworkUnreachable:
                    case SocketError.ConnectionRefused:
                        Thread.Sleep(3000);
                        break;
                    default:
                        Thread.Sleep(1000);
                        break;
                }
                Connect();
            }
        }

        #endregion
    }
}