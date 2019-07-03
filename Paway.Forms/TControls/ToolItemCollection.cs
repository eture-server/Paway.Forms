using System;
using System.ComponentModel;

namespace Paway.Forms
{
    /// <summary>
    ///     代表 ToolBar 中项的集合。
    /// </summary>
    [ListBindable(false)]
    public class ToolItemCollection : BindingList<ToolItem>, IDisposable
    {
        #region 构造函数

        /// <summary>
        ///     初始化 Paway.Forms.ToolItemCollection 新的实例。
        /// </summary>
        public ToolItemCollection() { }

        #endregion

        #region 方法

        /// <summary>
        ///     返回该项在集合中的索引值
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

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用
        /// <summary>
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    for (var i = 0; i < Count; i++)
                    {
                        base[i].Dispose();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~TProperties() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}