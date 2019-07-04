using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using Paway.Helper;
using System.Reflection;

namespace Paway.Utils
{
    /// <summary>
    ///     数据发送类
    /// </summary>
    internal class SendDataService : IDisposable
    {
        #region 字段与属性
        /// <summary>
        ///     发送数据间隔(ms)
        /// </summary>
        private readonly int interval = 5;

        /// <summary>
        ///     发送数据队列
        /// </summary>
        private volatile ConcurrentQueue<byte[]> MessageQueue = new ConcurrentQueue<byte[]>();

        /// <summary>
        ///     强制退出数据发送
        /// </summary>
        private volatile bool SendStop;

        /// <summary>
        ///     Socket连接实例
        /// </summary>
        private SocketBase Socket;

        #endregion

        #region public method
        /// <summary>
        ///     传入Socket实例初始化 数据发送类
        /// </summary>
        public SendDataService(SocketBase socket)
        {
            Licence.Checking();
            if (socket == null) return;
            Socket = socket;
            Socket.IPPoint = socket.Socket.RemoteEndPoint as IPEndPoint;
            ThreadPool.QueueUserWorkItem(IntervalSendData, Socket.IPPoint);
        }

        /// <summary>
        ///     发送数据
        /// </summary>
        /// <param name="byteData"></param>
        public void SendData(byte[] byteData)
        {
            MessageQueue.Enqueue(byteData);
        }

        #endregion

        #region private method
        /// <summary>
        ///     线程发送消息
        /// </summary>
        private void IntervalSendData(object state)
        {
            if (state == null) return;
            SocketConfig.ThreadList.TryAdd(Thread.CurrentThread.ManagedThreadId, state.ToString());
            while (!SendStop)
            {
                if (!MessageQueue.IsEmpty)
                {
                    if (!MessageQueue.TryDequeue(out byte[] byteData)) continue;
                    if (SendStop) return;
                    Socket.SendData(byteData);
                    Thread.Sleep(interval);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            SocketConfig.ThreadList.TryRemove(Thread.CurrentThread.ManagedThreadId, out _);
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
                SendStop = true;
                if (Socket != null)
                {
                    Socket.SendStop = true;
                    Socket = null;
                }
                if (MessageQueue != null)
                {
                    MessageQueue = null;
                }
            }
        }
        /// <summary>
        /// 析构，释放非托管资源
        /// </summary>
        ~SendDataService()
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