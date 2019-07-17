using System.Collections.Concurrent;

namespace Paway.Utils
{
    /// <summary>
    /// 通讯相关
    /// </summary>
    public abstract class SocketConfig
    {
        /// <summary>
        /// 10分钟内的限制连接数
        /// 默认100
        /// </summary>
        public static int Current { get; set; } = 100;

        /// <summary>
        /// 总的限制连接数
        /// </summary>
        public static int Limit { get; set; }

        /// <summary>
        /// 消息序列化标记
        /// </summary>
        public static bool IStruct { get; set; } = true;

        /// <summary>
        /// 当前连接数
        /// </summary>
        public static int Count
        {
            get { return ClientList.Count; }
        }

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