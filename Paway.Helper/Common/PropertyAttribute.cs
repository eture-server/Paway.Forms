using System;
using System.ComponentModel;
using System.Reflection;

namespace Paway.Helper
{
    /// <summary>
    /// 特性.列名称(对应数据库列)
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 列名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 列名称(对应数据库列)
        /// </summary>
        public ColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
    /// <summary>
    /// 特性.文本名称(GridView)
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TextAttribute : Attribute
    {
        /// <summary>
        /// 文本名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示文本名称(GridView)
        /// </summary>
        public TextAttribute(string name)
        {
            this.Name = name;
        }
    }
    /// <summary>
    /// 特性.不显示列(GridView)
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NoShowAttribute : Attribute { }
    /// <summary>
    /// 特性.生成指定类型查询数据列
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NoSelectAttribute : Attribute
    {
        /// <summary>
        /// 数据库字段Sql执行类型
        /// </summary>
        public SelectType Type { get; set; }
        /// <summary>
        /// 特性.不生成数据列
        /// </summary>
        public NoSelectAttribute()
        {
            this.Type = SelectType.None;
        }
        /// <summary>
        /// 特性.生成指定类型查询数据列
        /// </summary>
        public NoSelectAttribute(SelectType type)
        {
            this.Type = type;
        }
    }
    /// <summary>
    /// 特性.不生成ExcelTable列
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NoExcelAttribute : Attribute { }
    /// <summary>
    /// 特性.不复制列
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NoCloneAttribute : Attribute { }
    /// <summary>
    /// 特性.主键列
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class KeyAttribute : Attribute { }
    /// <summary>
    /// 特性.标识(唯一)列.可更新
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MarkAttribute : Attribute { }
    /// <summary>
    /// 特性.数据库表名称
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 数据库表名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据库表名称
        /// </summary>
        public TableAttribute(string name)
        {
            this.Name = name;
        }
    }
}