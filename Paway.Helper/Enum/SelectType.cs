using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 数据库字段Sql执行类型
    /// </summary>
    public enum SelectType
    {
        /// <summary>
        /// 默认(不执行任何操作)
        /// </summary>
        None = 0,
        /// <summary>
        /// 全部动作
        /// </summary>
        All = (1 << 0) + (1 << 1) + (1 << 2),
        /// <summary>
        /// 缓存图片(主动查询、更新，自动插入)
        /// </summary>
        Image = All + (1 << 3),
        /// <summary>
        /// 仅查询(View)
        /// </summary>
        Find = 1 << 0,
        /// <summary>
        /// 插入
        /// </summary>
        Insert = 1 << 1,
        /// <summary>
        /// 更新
        /// </summary>
        Update = 1 << 2,
    }
}
