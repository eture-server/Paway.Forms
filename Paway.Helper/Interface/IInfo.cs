using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    ///     普通数据接口
    /// </summary>
    public interface IInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        long Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        string Value { get; set; }
        /// <summary>
        /// DateTime
        /// </summary>
        DateTime DateTime { get; set; }
    }
}