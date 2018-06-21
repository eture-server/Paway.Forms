using System;
using System.ComponentModel;
using System.Reflection;

namespace Paway.Helper
{
    /// <summary>
    ///     枚举属性设置
    /// </summary>
    public sealed class EnumTextValueAttribute : Attribute
    {
        private readonly string enumTextValue;

        /// <summary>
        ///     Allows the creation of a friendly text representation of the enumeration.
        /// </summary>
        /// <param name="text">The reader friendly text to decorate the enum.</param>
        public EnumTextValueAttribute(string text)
        {
            enumTextValue = text;
        }

        /// <summary>
        ///     Returns the text representation of the value
        /// </summary>
        public string Text
        {
            get { return enumTextValue; }
        }
    }

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
            var ret = string.Empty;
            if (e == null) return ret;

            var t = e.GetType();
            var members = t.GetMember(e.ToString());
            if (members != null && members.Length == 1)
            {
                var attrs = members[0].GetCustomAttributes(typeof(EnumTextValueAttribute), false);
                if (attrs.Length == 1)
                    return ((EnumTextValueAttribute)attrs[0]).Text;
                attrs = members[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return ret;
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
                string name = GetText(field);
                if (string.Equals(name, value, StringComparison.CurrentCultureIgnoreCase))
                    return (T)field.GetRawConstantValue();
            }
            foreach (FieldInfo field in type.GetFields())
            {
                string name = field.Name;
                if (string.Equals(name, value, StringComparison.CurrentCultureIgnoreCase))
                    return (T)field.GetRawConstantValue();
            }
            return default(T);
        }
        /// <summary>
        /// 获取指定字段特性值
        /// </summary>
        public static string GetText(FieldInfo field)
        {
            string name = field.Name;
            var attribs = field.GetCustomAttributes(typeof(EnumTextValueAttribute), false);
            if (attribs.Length > 0)
                return ((EnumTextValueAttribute)attribs[0]).Text;
            attribs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attribs.Length > 0)
                return ((DescriptionAttribute)attribs[0]).Description;
            return name;
        }
    }

    /// <summary>
    ///     特性.自动生成Sql语句
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class PropertyAttribute : Attribute
    {
        private bool _iClone = true;
        private bool _iTable = true;
        private bool _iExcel = true;
        private bool _iSelect = true;
        private bool _iShow = true;
        private bool _iSort = false;

        /// <summary>
        ///     是否自定义排序列
        ///     默认false
        /// </summary>
        public bool ISort
        {
            get { return _iSort; }
            set { _iSort = value; }
        }

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
        ///     是否复制列
        ///     默认true
        /// </summary>
        public bool IClone
        {
            get { return _iClone; }
            set { _iClone = value; }
        }

        /// <summary>
        ///     是否生成Table
        ///     默认true
        /// </summary>
        public bool ITable
        {
            get { return _iTable; }
            set { _iTable = value; }
        }

        /// <summary>
        ///     是否生成到ExcelTable
        ///     默认true
        /// </summary>
        public bool IExcel
        {
            get { return _iExcel; }
            set { _iExcel = value; }
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
        ///     列名称
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        ///     文本名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     数据
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        ///     表名称
        /// </summary>
        public string Table { get; set; }
    }
}