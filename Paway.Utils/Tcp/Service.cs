using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Paway.Utils.Tcp
{
    /// <summary>
    /// 封装Socket通讯服务端
    /// </summary>
    public class Service
    {
        /// <summary>
        /// 错误日志
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 心跳检测时长，默认3
        /// </summary>
        private int heartTime = 3;
        /// <summary>
        ///服务端监听Socket
        /// </summary>
        private Socket socketListener;
        /// <summary>
        /// 允许线程通过发信号互相通信。通常，此通信涉及一个线程在其他线程进行之前必须完成的任务。
        /// </summary>
        private ManualResetEvent allDone = new ManualResetEvent(false);
        /// <summary>
        /// 线程标记
        /// </summary>
        private volatile bool ForceStop = false;
        /// <summary>
        /// 连接错误
        /// </summary>
        public event EventHandler ConnectError;
        /// <summary>
        /// 客户端连接
        /// </summary>
        public event EventHandler ConnectFinished;
        /// <summary>
        /// 监听端口
        /// </summary>
        public IPEndPoint IpPort { get; set; }
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Listener(string host, int port)
        {
            this.IpPort = new IPEndPoint(IPAddress.Parse(host), port);
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定本端IP地址
            socketListener.Bind(IpPort);
            //启动监听
            socketListener.Listen(10);
            //启动监听服务
            ThreadPool.QueueUserWorkItem(new WaitCallback(SocketServerListener));
            //启动心跳服务
            ThreadPool.QueueUserWorkItem(new WaitCallback(HeartListener));
        }
        /// <summary>
        /// 停止监听
        /// </summary>
        public void Stop()
        {
            ForceStop = true;
        }

        #region Method
        /// <summary>
        /// 心跳监听服务
        /// </summary>
        private void HeartListener(object state)
        {
            try
            {
                SocketConfig.ThreadList.TryAdd(Thread.CurrentThread.ManagedThreadId, "Server Heart Listener");
                while (!ForceStop)
                {
                    InsertSendData("hello,world");
                    Thread.Sleep(heartTime * 1000);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("心跳服务失败：{0}", ex);
            }
            finally
            {
                string name;
                SocketConfig.ThreadList.TryRemove(Thread.CurrentThread.ManagedThreadId, out name);
            }
        }
        /// <summary>
        /// 监听服务
        /// </summary>
        private void SocketServerListener(object state)
        {
            try
            {
                SocketConfig.ThreadList.TryAdd(Thread.CurrentThread.ManagedThreadId, "Server Socket Listener");
                while (!ForceStop)
                {
                    allDone.Reset();
                    socketListener.BeginAccept(new AsyncCallback(OnClientConnect), null);
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("监听服务失败：{0}", ex);
            }
            finally
            {
                string name;
                SocketConfig.ThreadList.TryRemove(Thread.CurrentThread.ManagedThreadId, out name);
            }
        }
        /// <summary>
        /// 接受客户端连接
        /// </summary>
        private void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                Socket socket = null;
                try
                {
                    socket = socketListener.EndAccept(asyn);
                }
                finally
                {
                    allDone.Set();
                }
                SocketPackage client = new SocketPackage(this, socket);
                OnClientConnect(client);
                SocketConfig.ClientList.Add(client);
                client.ConnectError += client_ConnectError;
                OnClientFinished(client.IPPort);

                //等待客户端发送来的数据
                AsynSocketArg state = new AsynSocketArg();
                state.InitializeState(socket);
                client.WaitForData(state);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("客户端连接失败：{0}", ex);
            }
        }
        /// <summary>
        /// 抛出连接完成事件
        /// </summary>
        /// <param name="ipport"></param>
        protected virtual void OnClientFinished(string ipport)
        {
            try
            {
                if (ConnectFinished != null)
                {
                    ConnectFinished(ipport, EventArgs.Empty);
                }
            }
            catch { }
        }
        /// <summary>
        /// 抛出单个连接对象，用于消息发送
        /// </summary>
        /// <param name="client"></param>
        protected virtual void OnClientConnect(SocketPackage client) { }

        void client_ConnectError(object sender, EventArgs e)
        {
            try
            {
                if (ConnectError != null)
                {
                    ConnectError(sender, EventArgs.Empty);
                }
            }
            catch { }
        }

        #endregion

        /// <summary>
        /// 根据消息类型及源客户端地址过滤发送对象
        /// </summary>
        public void InsertSendData(object msg)
        {
            try
            {
                foreach (SocketBase socket in SocketConfig.ClientList)
                {
                    socket.InsertSendData(msg);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
