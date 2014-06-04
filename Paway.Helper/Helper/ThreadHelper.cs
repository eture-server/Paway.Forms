using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 线程安全队列
    /// </summary>
    public abstract class ThreadHelper
    {
        private static ConcurrentDictionary<int, string> _dictlist = null;
        /// <summary>
        /// 表示可由多个线程同时访问的键值对的线程安全集合
        /// </summary>
        public static ConcurrentDictionary<int, string> Dictlist
        {
            get
            {
                if (_dictlist == null) _dictlist = new ConcurrentDictionary<int, string>();
                return _dictlist;
            }
        }

        private static ConcurrentQueue<string> _queueList = null;
        /// <summary>
        /// 表示线程安全的先进先出 (FIFO) 集合。
        /// </summary>
        public static ConcurrentQueue<string> QueueList
        {
            get
            {
                if (_queueList == null) _queueList = new ConcurrentQueue<string>();
                return _queueList;
            }
        }
    }
}
