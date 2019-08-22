using System;
using System.ComponentModel;
using System.Reflection;

namespace Paway.Helper
{
    /// <summary>
    /// 特性.字段Sql属性设置
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class PropertyAttribute : Attribute
    {
        /// <summary>
        /// 是否生成数据列,
        /// 默认true
        /// </summary>
        public bool ISelect { get; set; } = true;

        /// <summary>
        /// 是否显示列(GridView)
        /// 默认默认true
        /// </summary>
        public bool IShow { get; set; } = true;

        /// <summary>
        /// 列名称
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// 文本名称
        /// </summary>
        public string Text { get; set; }
    }
    /// <summary>
    /// 特性.不导出Excel
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class NoExcelAttribute : Attribute
    {
        /// <summary>
        /// </summary>
        public NoExcelAttribute() { }
    }
    /// <summary>
    /// 特性.不复制
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NoCloneAttribute : Attribute
    {
        /// <summary>
        /// </summary>
        public NoCloneAttribute() { }
    }
    /// <summary>
    /// 特性.类Table设置
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 主键名称，不可更新
        /// </summary>
        public string Key { get; set; } = nameof(IId.Id);
        /// <summary>
        /// 标识(唯一)列名称，可更新
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Keys { get { return Mark ?? Key; } }
        /// <summary>
        /// 表名称
        /// </summary>
        public string Table { get; set; }
    }
}