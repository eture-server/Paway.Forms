using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Paway.Helper;
using System.Net;

namespace Paway.Utils.Tcp
{
    /// <summary>
    /// Tcp通讯：Socket通讯基础类
    /// </summary>
    public class SocketBase : IDisposable
    {
        /// <summary>
        /// 错误日志
        /// </summary>
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 外部数据与事件
        /// <summary>
        /// 客户端数据
        /// </summary>
        public object Client { get; set; }
        /// <summary>
        /// 有关的数据对象
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// 自定义标记
        /// </summary>
        public volatile bool IFlag;

        /// <summary>
        /// 外部事件
        /// </summary>
        public event EventHandler ChangeEvent;
        /// <summary>
        /// 引发外部事件方法
        /// </summary>
        public void OnChange(object sender, EventArgs e)
        {
            try
            {
                if (ChangeEvent != null)
                {
                    ChangeEvent(sender, EventArgs.Empty);
                }
            }
            catch { }
        }

        #endregion

        #region fields
        /// <summary>
        /// 获取远程终结点
        /// </summary>
        public IPEndPoint IPPoint { get; internal set; }

        private Socket socket;
        /// <summary>
        /// 客户端socket连接(used to send/receive messages.)
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
            internal set { socket = value; }
        }

        /// <summary>
        /// 数据发送服务类
        /// </summary>
        protected SendDataService SendDataService { get; set; }

        /// <summary>
        /// 线程通知，停止运行。
        /// </summary>
        public volatile bool SendStop;

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; internal set; }

        /// <summary>
        /// 是否连接客户端
        /// </summary>
        public bool IsConnected { get; internal set; }

        /// <summary>
        /// 是否注册客户端
        /// </summary>
        public bool IsRegisted { get; internal set; }

        /// <summary>
        /// 是否注销客户端
        /// </summary>
        public bool IsUNRegisted { get; internal set; }

        /// <summary>
        /// 客户端事件
        /// </summary>
        public event EventHandler<ServiceEventArgs> ClientEvent;
        /// <summary>
        /// 接收到的消息
        /// </summary>
        public event EventHandler MessageEvent;

        #endregion

        #region private methord
        /// <summary>
        /// 异步执行接受数据函数
        /// </summary>
        private void ReceiveCallback(IAsyncResult arg)
        {
            if (SendStop) return;
            if (IsUNRegisted || !IsConnected || !this.socket.Connected)
            {
                if (IsConnected && !this.socket.Connected)
                {
                    //用于异常情况下的触发通知
                    IsConnected = false;
                    this.OnSocketException(SocketError.NotConnected);
                }
                return;
            }

            if (arg == null) return;
            AsynSocketArg state = (AsynSocketArg)arg.AsyncState;
            Socket handler = state.WorkSocket;

            // Read data from the client socket.
            try
            {
                int read = handler.EndReceive(arg);
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
                            new AsyncCallback(ReceiveCallback), state);
                    }
                    else
                    {
                        //先处理接受的数据，去掉前四个字节
                        object[] buffer = state.LstBuffer.ToArray();
                        byte[] data = new byte[buffer.Length - 4];
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (byte)buffer[i + 4];
                        }
                        HandleMessage(data);
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
        /// <param name="buffer"></param>
        private void HandleMessage(byte[] buffer)
        {
            object message = null;
            try
            {
                try
                {
                    message = SctructHelper.GetObjectFromByte(buffer);
                }
                catch
                {
                    message = buffer;
                }
                if (MessageEvent != null)
                {
                    MessageEvent(message, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                OnClientEvent(ex.Message);
            }
        }
        /// <summary>
        /// 写入缓冲,发送数据
        /// </summary>
        private void SendMessage(byte[] byteData)
        {
            //先处理发送的数据，加上四字节长度
            byte[] msgBuffer = new byte[byteData.Length + 4];
            msgBuffer[0] = (byte)(byteData.Length >> 24);
            msgBuffer[1] = (byte)(byteData.Length >> 16);
            msgBuffer[2] = (byte)(byteData.Length >> 8);
            msgBuffer[3] = (byte)(byteData.Length);
            for (int i = 0; i < byteData.Length; i++)
            {
                msgBuffer[i + 4] = byteData[i];
            }
            if (!SendStop) this.socket.Send(msgBuffer);
        }
        #endregion

        #region abstract methord
        /// <summary>
        /// 触发socker异常事件
        /// </summary>
        protected void OnSocketException(SocketError socketError)
        {
            if (Disposed) return;
            this.IsConnected = false;
            OnSocketException();
        }
        /// <summary>
        /// 触发客户端日志
        /// </summary>
        /// <param name="message"></param>
        protected void OnClientEvent(string message)
        {
            try
            {
                ServiceEventArgs msg = new ServiceEventArgs(ServiceType.Client);
                msg.Ip = this.IPPoint.ToString();
                msg.Port = this.IPPoint.Port;
                msg.Message = message;
                if (ClientEvent != null)
                {
                    ClientEvent(this, msg);
                }
            }
            catch { }
        }
        /// <summary>
        /// 触发socker异常事件->断开
        /// </summary>
        protected virtual void OnSocketException()
        {
            try
            {
                ServiceEventArgs msg = new ServiceEventArgs(ServiceType.DisConnect);
                msg.Ip = this.IPPoint.Address.ToString();
                msg.Port = this.IPPoint.Port;
                if (ClientEvent != null)
                {
                    ClientEvent(this, msg);
                }
            }
            catch { }
        }

        #endregion

        #region public methord
        /// <summary>
        /// 等待客户端发送过来的数据
        /// </summary>
        public void WaitForData(AsynSocketArg state)
        {
            try
            {
                if (state != null && state.WorkSocket.Connected)
                {
                    state.InitializeState();
                    state.WorkSocket.BeginReceive(state.GetBuffer(), 0, state.AutoBufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch (SocketException e)
            {
                OnSocketException(e.SocketErrorCode);
            }
        }

        /// <summary>
        /// 缓冲发送内部数据的接口
        /// </summary>
        /// <param name="message"></param>
        public void InsertSendData(object message)
        {
            InsertSendData(message, false);
        }
        /// <summary>
        /// 缓冲发送内部数据的接口
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ithrow">失败是否抛出异常</param>
        public void InsertSendData(object message, bool ithrow)
        {
            if (SendDataService != null && message != null)
            {
                byte[] byteData = message is byte[] ? message as byte[] : SctructHelper.GetByteFromObject(message);
                SendDataService.SendData(byteData);
            }
            else if (ithrow) throw new ArgumentException("SendDataService is null or message is null");
        }

        /// <summary>
        /// 直接发送消息对象
        /// </summary>
        public void SendData(byte[] msgBuffer)
        {
            try
            {
                //检查连接作相应处理
                if (socket != null && socket.Connected)
                {
                    SendMessage(msgBuffer);
                }
                else
                {
                    OnSocketException(SocketError.NotConnected);
                }
            }
            catch (SocketException)
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// 从主机断开
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (socket != null && socket.Connected)
                {
                    socket.Disconnect(false);
                }
            }
            catch { }
            finally
            {
                this.IsConnected = false;
            }
        }

        #endregion

        #region Dispose
        /// <summary>
        /// Disposes the instance of SocketClient.
        /// </summary>
        public volatile bool Disposed = false;

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                Disposed = true;
                if (disposing)
                {
                    if (this.socket != null && this.socket.Connected)
                    {
                        this.socket.Close();
                    }
                    if (SendDataService != null)
                    {
                        SendDataService.Dispose();
                    }
                    socket = null;
                    SendDataService = null;
                }
            }
            Disposed = true;
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~SocketBase()
        {
            Dispose(false);
        }
        #endregion
    }
}
