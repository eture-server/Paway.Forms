using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperType
    {
        /// <summary>
        /// 默认
        /// </summary>
        None = 0,
        /// <summary>
        /// Insert
        /// </summary>
        Insert,
        /// <summary>
        /// Update
        /// </summary>
        Update,
        /// <summary>
        /// Delete
        /// </summary>
        Delete,
        /// <summary>
        /// Clear+Insert
        /// </summary>
        Reset,
        /// <summary>
        /// Refresh
        /// </summary>
        Query,
    }
}
