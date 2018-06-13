using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 等级
    /// </summary>
    public enum LeveType : int
    {
        /// <summary>
        /// 默认
        /// </summary>
        None = 0,
        /// <summary>
        /// 信息
        /// </summary>
        Info,
        /// <summary>
        /// 调试
        /// </summary>
        Debug,
        /// <summary>
        /// 警告
        /// </summary>
        Warn,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
    }
}
