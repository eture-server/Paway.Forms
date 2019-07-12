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
    /// 时间搜索接口
    /// </summary>
    public interface ITime
    {
        /// <summary>
        /// 日期时间
        /// </summary>
        DateTime DateTime { get; set; }
    }
    /// <summary>
    /// 名称搜索接口
    /// </summary>
    public interface IName : IId
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
    }
    /// <summary>
    /// 主键接口
    /// </summary>
    public interface IId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        long Id { get; set; }
    }
    /// <summary>
    /// 父接点接口(树控件)
    /// </summary>
    public interface IParent : IId
    {
        /// <summary>
        /// 父接点Id
        /// </summary>
        long ParentId { get; set; }
    }
}