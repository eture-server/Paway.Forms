using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// Sql操作
    /// </summary>
    public enum SqlType : int
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
        /// Replace
        /// </summary>
        Replace
    }
}
