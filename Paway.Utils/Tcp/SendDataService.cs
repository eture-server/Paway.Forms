using Paway.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Paway.Utils.Tcp
{
    /// <summary>
    /// 数据发送类
    /// </summary>
    public class SendDataService : IDisposable
    {
        /// <summary>
        /// Socket连接实例
        /// </summary>
        private SocketBase Socket = null;
        /// <summary>
        /// 强制退出数据发送
        /// </summary>
        private volatile bool SendStop;
        /// <summary>
        /// 发送数据队列
        /// </summary>
        private volatile ConcurrentQueue<byte[]> MessageQueue = new ConcurrentQueue<byte[]>();
        /// <summary>
        /// 发送数据间隔(ms)
        /// </summary>
        private int interval = 5;

        /// <summary>
        /// 传入Socket实例初始化 数据发送类
        /// </summary>
        public SendDataService(SocketBase socket)
        {
            Licence.Checking();
            if (socket == null) return;
            Socket = socket;
            Socket.IPPort = socket.Socket.RemoteEndPoint.ToString();
            ThreadPool.QueueUserWorkItem(new WaitCallback(IntervalSendData), Socket.IPPort);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="byteData"></param>
        public void SendData(byte[] byteData)
        {
            MessageQueue.Enqueue(byteData);
        }

        /// <summary>
        /// 线程发送消息
        /// </summary>
        protected void IntervalSendData(object state)
        {
            if (state == null) return;
            SocketConfig.ThreadList.TryAdd(Thread.CurrentThread.ManagedThreadId, state.ToString());
            while (!SendStop)
            {
                if (!MessageQueue.IsEmpty)
                {
                    byte[] byteData;
                    if (!MessageQueue.TryDequeue(out byteData)) continue;
                    if (SendStop) return;
                    Socket.SendData(byteData);
                    Thread.Sleep(interval);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            string name;
            SocketConfig.ThreadList.TryRemove(Thread.CurrentThread.ManagedThreadId, out name);
        }

        #region Dispose
        /// <summary>
        /// Disposes the instance of SocketClient.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// 释放资源
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
                    this.SendStop = true;

                    MessageQueue = null;
                    Socket = null;
                }
            }
            disposed = true;
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~SendDataService()
        {
            Dispose(false);
        }
        #endregion
    }
}
