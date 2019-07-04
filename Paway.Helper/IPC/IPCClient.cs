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
        #region 字段与属性
        private IpcChannel channel;

        /// <summary>
        ///     接口实例
        /// </summary>
        public T Obj { get; private set; }

        /// <summary>
        ///     连接状态
        /// </summary>
        public bool IConnected { get; private set; }

        #endregion

        #region public method
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
                throw;
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
                channel = null;
            }
            Obj = null;
            IConnected = false;
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
                Stop();
            }
        }
        /// <summary>
        /// 析构，释放非托管资源
        /// </summary>
        ~IPCClient()
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