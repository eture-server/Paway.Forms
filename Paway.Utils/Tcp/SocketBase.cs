using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using log4net;
using Paway.Helper;

namespace Paway.Utils
{
    /// <summary>
    /// Tcp通讯：Socket通讯基础类
    /// </summary>
    public class SocketBase : IDisposable
    {
        /// <summary>
        /// 内部日志
        /// </summary>
        internal static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region 字段与属性
        /// <summary>
        /// 头部数据长度
        /// </summary>
        protected int heardLength = 2;

        /// <summary>
        /// 线程通知，停止运行。
        /// </summary>
        internal volatile bool SendStop;

        /// <summary>
        /// 数据发送服务类
        /// </summary>
        internal SendDataService SendDataService { get; set; }

        /// <summary>
        /// 客户端数据
        /// </summary>
        public object Client { get; set; }

        /// <summary>
        /// 有关的数据对象
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 获取远程终结点
        /// </summary>
        public IPEndPoint IPPoint { get; internal set; }

        /// <summary>
        /// 客户端socket连接(used to send/receive messages.)
        /// </summary>
        public Socket Socket { get; internal set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; internal set; }

        /// <summary>
        /// 是否连接客户端
        /// </summary>
        public bool IConnected { get { return Socket != null && Socket.Connected; } }

        #endregion

        #region 事件
        /// <summary>
        /// 外部事件
        /// </summary>
        public event Action<EventArgs> ChangeEvent;

        /// <summary>
        /// 客户端事件
        /// </summary>
        public event Action<ServiceEventArgs> ClientEvent;

        /// <summary>
        /// 接收到的消息
        /// </summary>
        public event Action<object> MessageEvent;

        #endregion

        #region public methord
        /// <summary>
        /// 缓冲发送内部数据的接口
        /// </summary>
        /// <param name="message"></param>
        public void Send(object message)
        {
            Send(message, true);
        }
        /// <summary>
        /// 缓冲发送内部数据的接口
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ithrow">失败是否抛出异常</param>
        public void Send(object message, bool ithrow)
        {
            if (ithrow && !IConnected)
            {
                throw new PawayException("Not Connected.");
            }
            if (SendDataService != null && message != null)
            {
                var byteData = message is byte[]? message as byte[] : StructHelper.SerializeObject(message);
                SendDataService.SendData(byteData);
            }
            else if (ithrow)
            {
                if (SendDataService == null)
                    throw new PawayException("Send Service is null.");
                if (message == null)
                    throw new PawayException("Message Argument can not be empty.");
            }
        }
        /// <summary>
        /// 从主机断开
        /// </summary>
        public void Disconnect()
        {
            if (IConnected)
            {
                Socket.Disconnect(false);
            }
        }
        /// <summary>
        /// 引发外部事件方法
        /// </summary>
        public void OnChange(EventArgs e)
        {
            ChangeEvent?.Invoke(e);
        }

        #endregion

        #region virtual methord
        /// <summary>
        /// 触发socker异常事件->断开
        /// </summary>
        internal virtual void OnDisConnectEvent(SocketError type)
        {
            try
            {
                var msg = new ServiceEventArgs(ServiceType.DisConnect)
                {
                    SocketError = type,
                    Message = type.ToString(),
                    Ip = IPPoint.Address.ToString(),
                    Port = IPPoint.Port
                };
                ClientEvent?.Invoke(msg);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion

        #region internal methord
        /// <summary>
        /// 等待客户端发送过来的数据
        /// </summary>
        internal void WaitForData(AsynSocketArg state)
        {
            try
            {
                if (state != null && state.WorkSocket.Connected)
                {
                    state.InitializeState();
                    state.WorkSocket.BeginReceive(state.GetBuffer(), 0, state.AutoBufferSize, 0, ReceiveCallback, state);
                }
            }
            catch (SocketException e)
            {
                OnSocketException(e.SocketErrorCode);
            }
        }

        /// <summary>
        /// 触发socker异常事件
        /// </summary>
        internal void OnSocketException(SocketError type)
        {
            if (disposed) return;
            OnDisConnectEvent(type);
        }

        /// <summary>
        /// 触发客户端日志
        /// </summary>
        /// <param name="message"></param>
        internal void OnClientEvent(string message)
        {
            try
            {
                var msg = new ServiceEventArgs(ServiceType.Client)
                {
                    Ip = IPPoint.ToString(),
                    Port = IPPoint.Port,
                    Message = message
                };
                ClientEvent?.Invoke(msg);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="iPackage">true=封装</param>
        internal void SendSync(byte[] buffer, bool iPackage = true)
        {
            if (!IConnected) throw new PawayException("Not Connected.");
            if (iPackage) SendData(buffer);
            else if (!SendStop) Socket.Send(buffer);
        }

        /// <summary>
        /// 直接发送消息对象
        /// </summary>
        internal void SendData(byte[] buffer)
        {
            try
            {
                //检查连接作相应处理
                if (IConnected)
                {
                    SendMessage(buffer);
                }
                else
                {
                    OnSocketException(SocketError.NotConnected);
                }
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }

        #endregion

        #region private methord
        /// <summary>
        /// 异步执行接受数据函数
        /// </summary>
        private void ReceiveCallback(IAsyncResult arg)
        {
            if (SendStop) return;
            if (!IConnected)
            {
                //用于异常情况下的触发通知
                OnSocketException(SocketError.NotConnected);
                return;
            }

            if (arg == null) return;
            var state = (AsynSocketArg)arg.AsyncState;
            var handler = state.WorkSocket;

            // Read data from the client socket.
            try
            {
                var read = handler.EndReceive(arg);
                if (read == 0)
                {
                    OnSocketException(SocketError.NotConnected);
                    return;
                }
                state.AddBufferData(state.GetBuffer(), read);

                if (state.WorkSocket.Connected)
                {
                    if (state.LstBuffer.Count < state.ReceiveDataLength || state.ReceiveDataLength < 0)
                    {
                        state.ResetBuffer();
                        handler.BeginReceive(state.GetBuffer(), 0, state.AutoBufferSize, 0,
                            ReceiveCallback, state);
                    }
                    else
                    {
                        //先处理接受的数据，去掉前heardLength个字节
                        var buffer = state.LstBuffer.ToArray();
                        var data = new byte[buffer.Length - heardLength];
                        for (var i = 0; i < data.Length; i++)
                        {
                            data[i] = (byte)buffer[i + heardLength];
                        }
                        HandleMessage(data, state.IServer);
                        WaitForData(state);
                    }
                }
            }
            catch (SocketException e)
            {
                OnSocketException(e.SocketErrorCode);
            }
        }

        /// <summary>
        /// 反序列化并处理消息
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="iServer">服务端标记</param>
        private void HandleMessage(byte[] buffer, bool iServer)
        {
            object message;
            try
            {
                if (buffer.Length == 5 && Encoding.UTF8.GetString(buffer) == "Hello")
                {//心跳，客户端需要回复
                    if (!iServer) Send(buffer);
                    return;
                }
                try
                {
                    if (SocketConfig.IStruct) message = StructHelper.DeserializeObject(buffer);
                    else message = buffer;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    message = buffer;
                }
                MessageEvent?.Invoke(message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// 写入缓冲,发送数据
        /// </summary>
        private void SendMessage(byte[] byteData)
        {
            //先处理发送的数据，加上四字节长度
            var msgBuffer = new byte[byteData.Length + heardLength];
            for (int i = 0; i < heardLength; i++)
            {
                msgBuffer[i] = (byte)(byteData.Length >> (8 * (heardLength - 1 - i)));
            }
            for (var i = 0; i < byteData.Length; i++)
            {
                msgBuffer[i + heardLength] = byteData[i];
            }
            if (!SendStop) Socket.Send(msgBuffer);
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// 标识此对象已释放
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// 参数为true表示释放所有资源，只能由使用者调用
        /// 参数为false表示释放非托管资源，只能由垃圾回收器自动调用
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                if (disposing)
                {
                    // TODO: 释放托管资源(托管的对象)。
                }
                // TODO: 释放未托管资源(未托管的对象)
                if (SendDataService != null)
                {
                    SendDataService.Dispose();
                    SendDataService = null;
                }
                if (IConnected && Socket != null)
                {
                    Socket.Close();
                    Socket = null;
                }
            }
        }
        /// <summary>
        /// 析构，释放非托管资源
        /// </summary>
        ~SocketBase()
        {
            Dispose(false);
        }
        /// <summary>
        /// 释放资源
        /// 由类的使用者，在外部显示调用，释放类资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}