﻿using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

namespace Paway.Helper
{
    /// <summary>
    /// IPC客户端
    /// </summary>
    /// <typeparam name="T">接口</typeparam>
    public class IPCClient<T> : IDisposable where T : class
    {
        private IpcChannel channel = null;
        /// <summary>
        /// 接口实例
        /// </summary>
        public T obj { get; private set; }
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected { get; private set; }
        /// <summary>
        /// 创建一个IPC信道。
        /// </summary>
        public void Connect()
        {
            try
            {
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
                IDictionary props = new Hashtable();
                props["name"] = "ServerChannel";
                props["portName"] = string.Format("ServerChannel-Client.{0}", TConfig.Name);
                channel = new IpcChannel(props, clientProvider, serverProvider);
                System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(channel, true);
                obj = Activator.GetObject(typeof(T), string.Format("ipc://ServerChannel-Server.{0}/IPCObject", TConfig.Name)) as T;
                if (obj != null)
                    IsConnected = true;
            }
            catch (Exception)
            {
                Stop();
            }
        }
        /// <summary>
        /// 断开IPC连接
        /// </summary>
        public void Stop()
        {
            if (channel != null)
            {
                ChannelServices.UnregisterChannel(channel);
                channel.StopListening(null);
            }
            channel = null;
            obj = null;
            IsConnected = false;
        }

        #region Dispose
        /// <summary>
        /// Disposes the instance of SocketClient.
        /// </summary>
        public bool Disposed = false;
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
                    Stop();
                }
            }
            Disposed = true;
        }
        /// <summary>
        /// 析构
        /// </summary>
        ~IPCClient()
        {
            Dispose(false);
        }

        #endregion
    }
}