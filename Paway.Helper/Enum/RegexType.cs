using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    ///     已写好的正则表达式
    /// </summary>
    public enum RegexType
    {
        /// <summary>
        ///     无
        /// </summary>
        None,

        /// <summary>
        ///     自定义
        /// </summary>
        Custom,

        /// <summary>
        ///     一般规则，不允许特殊字符
        ///     允许中文汉字+符号+密码规则
        /// </summary>
        Normal,

        /// <summary>
        ///     密码
        ///     不允许单引号 '
        /// </summary>
        Password,

        /// <summary>
        ///     IP地址
        /// </summary>
        Ip,

        /// <summary>
        ///     正整数
        /// </summary>
        PosInt,

        /// <summary>
        ///     数字
        /// </summary>
        Number
    }
}
