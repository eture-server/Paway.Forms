using System;
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
                {
                    ret = ((EnumTextValueAttribute)attrs[0]).Text;
                }
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
            EnumTextValueAttribute[] attribs = field.GetCustomAttributes(typeof(EnumTextValueAttribute), false) as EnumTextValueAttribute[];
            string name = field.Name;
            if (attribs.Length > 0)
                name = ((EnumTextValueAttribute)attribs[0]).Text;
            return name;
        }
    }

    /// <summary>
    ///     特性.自动生成Sql语句
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class PropertyAttribute : Attribute
    {
        private bool _clone = true;
        private bool _excel = true;
        private bool _select = true;
        private bool _show = true;
        private bool _sort = false;

        /// <summary>
        ///     是否自定义排序数字字符列,
        /// </summary>
        public bool Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }

        /// <summary>
        ///     是否生成列,
        ///     默认生成，在Db中
        /// </summary>
        public bool Select
        {
            get { return _select; }
            set { _select = value; }
        }

        /// <summary>
        ///     是否复制列
        ///     默认复制
        /// </summary>
        public bool Clone
        {
            get { return _clone; }
            set { _clone = value; }
        }

        /// <summary>
        ///     是否导入列
        ///     默认导入
        /// </summary>
        public bool Excel
        {
            get { return _excel; }
            set { _excel = value; }
        }

        /// <summary>
        ///     是否显示列
        ///     默认显示，在GridView中
        /// </summary>
        public bool Show
        {
            get { return _show; }
            set { _show = value; }
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