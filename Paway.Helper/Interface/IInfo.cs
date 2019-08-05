using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 普通数据接口
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
    public interface ITime : IId
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
        /// 当初设置为long的原因未知,现统一全部使用int
        /// </summary>
        int Id { get; set; }
    }
    /// <summary>
    /// 父接点接口(树控件)
    /// </summary>
    public interface IParent : IId
    {
        /// <summary>
        /// 父接点Id
        /// </summary>
        int ParentId { get; set; }
    }
}