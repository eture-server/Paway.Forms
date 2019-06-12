using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

namespace Paway.Helper
{
    /// <summary>
    ///     IPC客户端
    /// </summary>
    /// <typeparam name="T">接口</typeparam>
    public class IPCClient<T> : IDisposable where T : class
    {
        private IpcChannel channel;

        /// <summary>
        ///     接口实例
        /// </summary>
        public T Obj { get; private set; }

        /// <summary>
        ///     连接状态
        /// </summary>
        public bool IConnected { get; private set; }

        /// <summary>
        ///     创建一个IPC信道。
        /// </summary>
        public virtual void Connect(string name)
        {
            try
            {
                var serverProvider = new BinaryServerFormatterSinkProvider();
                var clientProvider = new BinaryClientFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
                IDictionary props = new Hashtable
                {
                    ["name"] = string.Format("ServerChannel.{0}", name),
                    ["portName"] = string.Format("ServerChannel-Client.{0}", name)
                };
                channel = new IpcChannel(props, clientProvider, serverProvider);
                ChannelServices.RegisterChannel(channel, true);
                Obj = Activator.GetObject(typeof(T), string.Format("ipc://ServerChannel-Server.{0}/IPCObject", name)) as T;
                IConnected = Obj != null;
            }
            catch (Exception)
            {
                Stop();
            }
        }

        /// <summary>
        ///     断开IPC连接
        /// </summary>
        public virtual void Stop()
        {
            if (channel != null)
            {
                ChannelServices.UnregisterChannel(channel);
                channel.StopListening(null);
            }
            channel = null;
            Obj = null;
            IConnected = false;
        }

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
                    Stop();
                }
            }
            Disposed = true;
        }

        /// <summary>
        ///     析构
        /// </summary>
        ~IPCClient()
        {
            Dispose(false);
        }

        #endregion
    }
}