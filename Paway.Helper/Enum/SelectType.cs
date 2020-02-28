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
        /// 组合类型(自动查询+插入+更新，默认类型)
        /// </summary>
        Normal = Find + Insert + Update,
        /// <summary>
        /// 组合类型(手动查询+更新、自动插入，如缓存图片)
        /// </summary>
        Image = ManualFind + Insert + ManualUpdate,
        /// <summary>
        /// 查询(View)
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
        /// <summary>
        /// 手动查询
        /// </summary>
        ManualFind = Find + (1 << 3),
        /// <summary>
        /// 手动插入
        /// </summary>
        ManualInsert = Insert + (1 << 4),
        /// <summary>
        /// 手动更新
        /// </summary>
        ManualUpdate = Update + (1 << 5),
    }
}
