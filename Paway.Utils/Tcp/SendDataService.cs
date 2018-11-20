using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using Paway.Helper;
using System.Reflection;
using System.Diagnostics;

namespace Paway.Utils.Tcp
{
    /// <summary>
    ///     数据发送类
    /// </summary>
    public class SendDataService : IDisposable
    {
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

        /// <summary>
        ///     传入Socket实例初始化 数据发送类
        /// </summary>
        public SendDataService(SocketBase socket)
        {
            Licence.Checking(MethodBase.GetCurrentMethod().DeclaringType);
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

        /// <summary>
        ///     线程发送消息
        /// </summary>
        protected void IntervalSendData(object state)
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
                    Debug.WriteLine(MessageQueue.Count);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            SocketConfig.ThreadList.TryRemove(Thread.CurrentThread.ManagedThreadId, out string name);
        }

        #region Dispose

        /// <summary>
        ///     Disposes the instance of SocketClient.
        /// </summary>
        private bool disposed;

        /// <summary>
        ///     释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Socket.SendStop = true;
                    SendStop = true;

                    MessageQueue = null;
                    Socket = null;
                }
            }
            disposed = true;
        }

        /// <summary>
        ///     析构
        /// </summary>
        ~SendDataService()
        {
            Dispose(false);
        }

        #endregion
    }
}