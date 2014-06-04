using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Forms
{
    /// <summary>
    /// 代表 QQTabControl 中继承于TabPage的项的集合。
    /// </summary>
    public class QQTabPageCollection : List<QQTabPage>
    {
        #region 变量
        /// <summary>
        /// QQTabControl
        /// </summary>
        private QQTabControl _owner = null;

        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化 Paway.Forms.ToolItemCollection 新的实例。
        /// </summary>
        /// <param name="owner">ToolBar</param>
        public QQTabPageCollection(QQTabControl owner)
        {
            this._owner = owner;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 返回该项在集合中的索引值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetIndexOfRange(QQTabPage item)
        {
            int result = -1;
            for (int i = 0; i < base.Count; i++)
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
    }
}
