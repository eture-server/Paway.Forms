using System;
using System.ComponentModel;
using System.Reflection;

namespace Paway.Helper
{
    /// <summary>
    ///     枚举属性设置
    /// </summary>
    public abstract class EntityHelper
    {
        /// <summary>
        /// 获取指定枚举特性值
        /// </summary>
        public static string GetValue(Enum e)
        {
            var value = string.Empty;
            if (e == null) return value;

            value = e.ToString();
            var members = e.GetType().GetMember(value);
            if (members != null && members.Length == 1)
            {
                return members[0].Description() ?? value;
            }
            return value;
        }
        /// <summary>
        /// 获取指定字段特性值
        /// </summary>
        public static string GetValue(FieldInfo field)
        {
            return field.Description() ?? field.Name;
        }
        /// <summary>
        /// 将枚举常数的名称或数字值的字符串表示转换成等效的枚举对象
        /// </summary>
        /// <returns></returns>
        public static T Parse<T>(string value)
        {
            Type type = typeof(T);
            foreach (FieldInfo field in type.GetFields())
            {
                string name = field.Name;
                if (string.Equals(name, value, StringComparison.CurrentCultureIgnoreCase))
                    return (T)field.GetRawConstantValue();
                name = GetValue(field);
                if (string.Equals(name, value, StringComparison.CurrentCultureIgnoreCase))
                    return (T)field.GetRawConstantValue();
            }
            return default(T);
        }
    }

    /// <summary>
    ///     特性.字段Sql属性设置
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class PropertyAttribute : Attribute
    {
        private bool _iSelect = true;
        private bool _iShow = true;

        /// <summary>
        ///     是否生成数据列,
        ///     默认true
        /// </summary>
        public bool ISelect
        {
            get { return _iSelect; }
            set { _iSelect = value; }
        }

        /// <summary>
        ///     是否显示列(GridView)
        ///     默认默认true
        /// </summary>
        public bool IShow
        {
            get { return _iShow; }
            set { _iShow = value; }
        }

        /// <summary>
        ///     列名称
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        ///     文本名称
        /// </summary>
        public string Text { get; set; }
    }
    /// <summary>
    ///     特性.不导出Excel
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class NoExcelAttribute : Attribute
    {
        /// <summary>
        /// </summary>
        public NoExcelAttribute() { }
    }
    /// <summary>
    ///     特性.排序
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class SortAttribute : Attribute
    {
        /// <summary>
        /// </summary>
        public SortAttribute() { }
    }
    /// <summary>
    ///     特性.不复制
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NoCloneAttribute : Attribute
    {
        /// <summary>
        /// </summary>
        public NoCloneAttribute() { }
    }
    /// <summary>
    ///     特性.类Table设置
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class TableAttribute : Attribute
    {
        /// <summary>
        ///     主键名称，不可更新
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        ///     标识(唯一)列名称，可更新
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Keys { get { return Mark ?? Key; } }
        /// <summary>
        ///     表名称
        /// </summary>
        public string Table { get; set; }
    }
}