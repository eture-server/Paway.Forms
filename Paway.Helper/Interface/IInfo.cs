using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    ///     普通数据接口
    /// </summary>
    public interface IInfo : IName, ITime
    {
        /// <summary>
        /// Value
        /// </summary>
        string Value { get; set; }
    }
    /// <summary>
    /// </summary>
    public interface ITime
    {
        /// <summary>
        /// </summary>
        DateTime DateTime { get; set; }
    }
    /// <summary>
    /// </summary>
    public interface IName : IId
    {
        /// <summary>
        /// </summary>
        string Name { get; set; }
    }
    /// <summary>
    /// </summary>
    public interface IId
    {
        /// <summary>
        /// </summary>
        long Id { get; set; }
    }
}