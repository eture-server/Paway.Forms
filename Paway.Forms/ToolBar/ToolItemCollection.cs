using System;
using System.ComponentModel;

namespace Paway.Forms
{
    /// <summary>
    /// 代表 ToolBar 中项的集合。
    /// </summary>
    [ListBindable(false)]
    public class ToolItemCollection : BindingList<ToolItem>, IDisposable
    {
        #region public method
        /// <summary>
        /// 初始化 Paway.Forms.ToolItemCollection 新的实例。
        /// </summary>
        public ToolItemCollection() { }

        /// <summary>
        /// 返回该项在集合中的索引值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetIndexOfRange(ToolItem item)
        {
            var result = -1;
            for (var i = 0; i < Count; i++)
            {
                if (item == base[i])
                {
                    result = i;
                    break;
                }
            }
            return result;
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
                for (var i = 0; i < Count; i++)
                {
                    base[i].Dispose();
                }
                Clear();
            }
        }
        /// <summary>
        /// 析构，释放非托管资源
        /// </summary>
        ~ToolItemCollection()
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