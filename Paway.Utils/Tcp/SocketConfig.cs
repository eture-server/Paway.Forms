﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Utils.Tcp
{
    /// <summary>
    /// Tcp通讯：Socket通讯公共数据
    /// </summary>
    public abstract class SocketConfig
    {
        private static int _current = 100;
        /// <summary>
        /// 10分钟内的限制连接数
        /// 默认100
        /// </summary>
        public static int Current
        {
            get { return _current; }
            set { _current = value; }
        }
        /// <summary>
        /// 总的限制连接数
        /// </summary>
        public static int Limit { get; set; }

        /// <summary>
        /// 当前连接数
        /// </summary>
        public static int Count { get { return ClientList.Count; } }

        private static ConcurrentDictionary<int, string> threadList;
        /// <summary>
        /// 线程列表
        /// </summary>
        public static ConcurrentDictionary<int, string> ThreadList
        {
            get
            {
                if (threadList == null) threadList = new ConcurrentDictionary<int, string>();
                return threadList;
            }
        }

        private static BlockingCollection<SocketBase> _clientList;
        /// <summary>
        /// 客户端列表
        /// </summary>
        public static BlockingCollection<SocketBase> ClientList
        {
            get
            {
                if (_clientList == null) _clientList = new BlockingCollection<SocketBase>();
                return _clientList;
            }
        }
    }
}
