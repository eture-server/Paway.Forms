using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

namespace Paway.Helper
{
    /// <summary>
    ///     服务端
    /// </summary>
    /// <typeparam name="T">子类型</typeparam>
    public class IPCServer<T> : MarshalByRefObject, IDisposable
    {
        #region 字段与属性
        private IpcChannel serverChannel;
        /// <summary>
        ///     服务状态
        /// </summary>
        public bool IBusy { get; private set; }

        #endregion

        #region public method
        /// <summary>
        ///     启动
        ///     默认单实例
        /// </summary>
        public virtual void Start()
        {
            Start(WellKnownObjectMode.Singleton);
        }

        /// <summary>
        ///     启动
        /// </summary>
        public virtual void Start(WellKnownObjectMode mode)
        {
            var serverProvider = new BinaryServerFormatterSinkProvider();
            var clientProvider = new BinaryClientFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable
            {
                ["portName"] = string.Format("ServerChannel-Server.{0}", typeof(T).Name)
            };
            serverChannel = new IpcChannel(props, clientProvider, serverProvider);
            // 注册这个IPC信道.
            ChannelServices.RegisterChannel(serverChannel, true);
            // 向信道暴露一个远程对象.
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(T), "IPCObject", mode);
            IBusy = true;
        }

        /// <summary>
        ///     停止服务
        /// </summary>
        public virtual void Stop()
        {
            IBusy = false;
            if (serverChannel != null)
            {
                ChannelServices.UnregisterChannel(serverChannel);
                serverChannel.StopListening(null);
                serverChannel = null;
            }
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
        ~IPCServer()
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