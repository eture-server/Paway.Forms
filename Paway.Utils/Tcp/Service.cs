using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using log4net;

namespace Paway.Utils.Tcp
{
    /// <summary>
    ///     封装Socket通讯服务端
    /// </summary>
    public class Service
    {
        /// <summary>
        ///     错误日志
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     允许线程通过发信号互相通信。通常，此通信涉及一个线程在其他线程进行之前必须完成的任务。
        /// </summary>
        private readonly ManualResetEvent allDone = new ManualResetEvent(false);

        /// <summary>
        ///     线程标记
        /// </summary>
        private volatile bool ForceStop;

        /// <summary>
        ///     心跳检测时长，默认3
        /// </summary>
        private readonly int heartTime = 3;

        /// <summary>
        ///     服务端监听Socket
        /// </summary>
        private Socket socketListener;

        /// <summary>
        ///     监听端口
        /// </summary>
        public IPEndPoint IpPort { get; private set; }

        /// <summary>
        ///     服务端事件
        /// </summary>
        public event EventHandler<ServiceEventArgs> SystemEvent;

        /// <summary>
        ///     开始监听
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Listener(string host, int port)
        {
            IpPort = new IPEndPoint(IPAddress.Parse(host), port);
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定本端IP地址
            socketListener.Bind(IpPort);
            //启动监听
            socketListener.Listen(10);
            //启动监听服务
            ThreadPool.QueueUserWorkItem(SocketServerListener);
            //启动心跳服务
            ThreadPool.QueueUserWorkItem(HeartListener);
        }

        /// <summary>
        ///     停止监听
        /// </summary>
        public void Stop()
        {
            ForceStop = true;
        }

        /// <summary>
        ///     发送到所有连接对象
        /// </summary>
        public void InsertSendData(object msg)
        {
            InsertSendData(msg, true);
        }

        /// <summary>
        ///     发送到所有连接对象
        /// </summary>
        /// <param name="msg">消息体</param>
        /// <param name="ithrow">失败时中止标记</param>
        public void InsertSendData(object msg, bool ithrow)
        {
            foreach (var socket in SocketConfig.ClientList)
            {
                try
                {
                    socket.InsertSendData(msg, ithrow);
                }
                catch
                {
                    if (ithrow) throw;
                }
            }
        }

        #region Method

        /// <summary>
        ///     心跳监听服务
        /// </summary>
        private void HeartListener(object state)
        {
            try
            {
                SocketConfig.ThreadList.TryAdd(Thread.CurrentThread.ManagedThreadId, "Server Heart Listener");
                while (!ForceStop)
                {
                    InsertSendData("hello,world", false);
                    Thread.Sleep(heartTime * 1000);
                }
            }
            catch (Exception ex)
            {
                OnSystemEvent(string.Format("心跳服务异常中止：{0}", ex));
            }
            finally
            {
                SocketConfig.ThreadList.TryRemove(Thread.CurrentThread.ManagedThreadId, out string name);
            }
        }

        /// <summary>
        ///     监听服务
        /// </summary>
        private void SocketServerListener(object state)
        {
            try
            {
                SocketConfig.ThreadList.TryAdd(Thread.CurrentThread.ManagedThreadId, "Server Socket Listener");
                while (!ForceStop)
                {
                    allDone.Reset();
                    socketListener.BeginAccept(OnClientConnect, null);
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                OnSystemEvent(string.Format("监听服务异常中止：{0}", ex));
            }
            finally
            {
                SocketConfig.ThreadList.TryRemove(Thread.CurrentThread.ManagedThreadId, out string name);
            }
        }

        /// <summary>
        ///     接受客户端连接
        /// </summary>
        private void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                if (SocketConfig.Limit != 0 && SocketConfig.ClientList.Count > SocketConfig.Limit - 1)
                {
                    OnSystemEvent(ServiceType.Limit, string.Format("超出连接限制：{0}，拒绝连接。", SocketConfig.Limit));
                    return;
                }
                if (SocketConfig.ClientList.ToList().FindAll(c => c.ConnectTime > DateTime.Now.AddMinutes(-10)).Count >
                    SocketConfig.Current - 1)
                {
                    OnSystemEvent(ServiceType.Current, string.Format("限定时间内连接数超出限制：{0}，拒绝连接。", SocketConfig.Current));
                    return;
                }
                var socket = socketListener.EndAccept(asyn);
                var client = new SocketPackage(this, socket);
                OnClientConnect(client);
                SocketConfig.ClientList.Add(client);
                client.ClientEvent += Client_ClientEvent;
                client.ConnectTime = DateTime.Now;
                ClientFinished(client.IPPoint);

                //等待客户端发送来的数据
                var state = new AsynSocketArg();
                state.InitializeState(socket);
                client.WaitForData(state);
            }
            catch (Exception ex)
            {
                OnSystemEvent(string.Format("客户端连接失败：{0}", ex));
            }
            finally
            {
                allDone.Set();
            }
        }

        /// <summary>
        ///     系统事件
        /// </summary>
        private void OnSystemEvent(string message)
        {
            OnSystemEvent(ServiceType.Error, message);
        }

        /// <summary>
        ///     系统事件
        /// </summary>
        private void OnSystemEvent(ServiceType type, string message)
        {
            var msg = new ServiceEventArgs(type)
            {
                Message = message
            };
            OnSystemEvent(msg);
        }

        /// <summary>
        ///     系统事件
        /// </summary>
        private void OnSystemEvent(ServiceEventArgs msg)
        {
            try
            {
                if (msg.Message != null)
                {
                    if (msg.Type == ServiceType.Error) log.Error(msg.Message);
                    else log.Warn(msg.Message);
                }
                SystemEvent?.Invoke(this, msg);
            }
            catch
            {
            }
        }

        /// <summary>
        ///     抛出连接完成事件
        /// </summary>
        protected virtual void ClientFinished(IPEndPoint point)
        {
            var msg = new ServiceEventArgs(ServiceType.Connect)
            {
                Ip = point.Address.ToString(),
                Port = point.Port
            };
            OnSystemEvent(msg);
        }

        /// <summary>
        ///     抛出单个连接对象，用于消息发送
        /// </summary>
        protected virtual void OnClientConnect(SocketPackage client)
        {
        }

        /// <summary>
        ///     客户端事件与断开连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_ClientEvent(object sender, ServiceEventArgs e)
        {
            OnSystemEvent(e);
        }

        #endregion
    }
}