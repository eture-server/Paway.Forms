using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 自定义任务栏右键菜单
    /// </summary>
    public enum MenuType : int
    {
        /// <summary>
        /// 横线
        /// </summary>
        None = 3000,
        /// <summary>
        /// 关于
        /// </summary>
        About = 3001,
        /// <summary>
        /// 还原
        /// </summary>
        Restore = 3002,
        /// <summary>
        /// 最大化
        /// </summary>
        MaxSize = 3003,
    }
}
