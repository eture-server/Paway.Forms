﻿using System;
using System.Net;
using System.Net.Sockets;
using Paway.Helper;

namespace Paway.Utils.Tcp
{
    /// <summary>
    ///     Tcp通讯：Socket通讯基础类
    /// </summary>
    public class SocketBase : IDisposable
    {
        #region 外部数据与事件

        /// <summary>
        ///     客户端数据
        /// </summary>
        public object Client { get; set; }

        /// <summary>
        ///     有关的数据对象
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        ///     自定义标记
        /// </summary>
        public volatile bool IFlag;

        /// <summary>
        ///     外部事件
        /// </summary>
        public event Action ChangeEvent;

        /// <summary>
        ///     引发外部事件方法
        /// </summary>
        public void OnChange(object sender, EventArgs e)
        {
            try
            {
                ChangeEvent?.Invoke();
            }
            catch
            {
            }
        }

        #endregion

        #region fields

        /// <summary>
        ///     获取远程终结点
        /// </summary>
        public IPEndPoint IPPoint { get; internal set; }

        /// <summary>
        ///     客户端socket连接(used to send/receive messages.)
        /// </summary>
        public Socket Socket { get; internal set; }

        /// <summary>
        ///     数据发送服务类
        /// </summary>
        protected SendDataService SendDataService { get; set; }

        /// <summary>
        ///     线程通知，停止运行。
        /// </summary>
        public volatile bool SendStop;

        /// <summary>
        ///     连接时间
        /// </summary>
        public DateTime ConnectTime { get; internal set; }

        /// <summary>
        ///     是否连接客户端
        /// </summary>
        public bool IConnected { get; internal set; }

        /// <summary>
        ///     是否注册客户端
        /// </summary>
        public bool IRegisted { get; internal set; }

        /// <summary>
        ///     是否注销客户端
        /// </summary>
        public bool IUNRegisted { get; internal set; }

        /// <summary>
        ///     客户端事件
        /// </summary>
        public event Action<ServiceEventArgs> ClientEvent;

        /// <summary>
        ///     接收到的消息
        /// </summary>
        public event Action<object> MessageEvent;

        #endregion

        #region private methord

        /// <summary>
        ///     异步执行接受数据函数
        /// </summary>
        private void ReceiveCallback(IAsyncResult arg)
        {
            if (SendStop) return;
            if (IUNRegisted || !IConnected || !Socket.Connected)
            {
                if (IConnected && !Socket.Connected)
                {
                    //用于异常情况下的触发通知
                    OnSocketException(SocketError.NotConnected);
                }
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
                        //先处理接受的数据，去掉前四个字节
                        var buffer = state.LstBuffer.ToArray();
                        var data = new byte[buffer.Length - 4];
                        for (var i = 0; i < data.Length; i++)
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
        ///     反序列化并处理消息
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
                MessageEvent?.Invoke(message);
            }
            catch (Exception ex)
            {
                OnClientEvent(ex.Message);
            }
        }

        /// <summary>
        ///     写入缓冲,发送数据
        /// </summary>
        private void SendMessage(byte[] byteData)
        {
            //先处理发送的数据，加上四字节长度
            var msgBuffer = new byte[byteData.Length + 4];
            msgBuffer[0] = (byte)(byteData.Length >> 24);
            msgBuffer[1] = (byte)(byteData.Length >> 16);
            msgBuffer[2] = (byte)(byteData.Length >> 8);
            msgBuffer[3] = (byte)byteData.Length;
            for (var i = 0; i < byteData.Length; i++)
            {
                msgBuffer[i + 4] = byteData[i];
            }
            if (!SendStop) Socket.Send(msgBuffer);
        }

        #endregion

        #region abstract methord

        /// <summary>
        ///     触发socker异常事件
        /// </summary>
        protected void OnSocketException(SocketError type)
        {
            if (Disposed) return;
            IConnected = false;
            OnDisConnectEvent(type);
        }

        /// <summary>
        ///     触发客户端日志
        /// </summary>
        /// <param name="message"></param>
        protected void OnClientEvent(string message)
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
            catch
            {
            }
        }
        /// <summary>
        ///     触发socker异常事件->断开
        /// </summary>
        protected virtual void OnDisConnectEvent(SocketError type)
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
            catch
            {
            }
        }

        #endregion

        #region public methord

        /// <summary>
        ///     等待客户端发送过来的数据
        /// </summary>
        public void WaitForData(AsynSocketArg state)
        {
            try
            {
                if (state != null && state.WorkSocket.Connected)
                {
                    state.InitializeState();
                    state.WorkSocket.BeginReceive(state.GetBuffer(), 0, state.AutoBufferSize, 0,
                        ReceiveCallback, state);
                }
            }
            catch (SocketException e)
            {
                OnSocketException(e.SocketErrorCode);
            }
        }

        /// <summary>
        ///     缓冲发送内部数据的接口
        /// </summary>
        /// <param name="message"></param>
        public void InsertSendData(object message)
        {
            InsertSendData(message, true);
        }

        /// <summary>
        ///     缓冲发送内部数据的接口
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ithrow">失败是否抛出异常</param>
        public void InsertSendData(object message, bool ithrow)
        {
            if (SendDataService != null && message != null)
            {
                var byteData = message is byte[] ? message as byte[] : SctructHelper.GetByteFromObject(message);
                SendDataService.SendData(byteData);
            }
            else if (Socket == null || !Socket.Connected)
            {
                throw new ArgumentException("Is DisConnected");
            }
            else if (ithrow)
            {
                if (SendDataService == null)
                    throw new ArgumentException("SendDataService is null");
                if (message == null)
                    throw new ArgumentException("message is null");
            }
        }

        /// <summary>
        ///     直接发送消息对象
        /// </summary>
        public void SendData(byte[] msgBuffer)
        {
            try
            {
                //检查连接作相应处理
                if (Socket != null && Socket.Connected)
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
                Disconnect();
            }
        }

        /// <summary>
        ///     从主机断开
        /// </summary>
        public void Disconnect()
        {
            if (Socket != null && Socket.Connected)
            {
                Socket.Disconnect(false);
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        ///     Disposes the instance of SocketClient.
        /// </summary>
        public bool Disposed;

        /// <summary>
        ///     释放
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
                    if (Socket != null && Socket.Connected)
                    {
                        Socket.Close();
                    }
                    if (SendDataService != null)
                    {
                        SendDataService.Dispose();
                    }
                    Socket = null;
                    SendDataService = null;
                }
            }
            Disposed = true;
        }

        /// <summary>
        ///     析构
        /// </summary>
        ~SocketBase()
        {
            Dispose(false);
        }

        #endregion
    }
}