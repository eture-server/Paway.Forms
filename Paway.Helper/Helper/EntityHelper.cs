using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 枚举属性设置
    /// </summary>
    public sealed class EnumTextValueAttribute : System.Attribute
    {
        private readonly string enumTextValue;

        ///<summary>
        /// Returns the text representation of the value
        ///</summary>
        public string Text
        {
            get
            {
                return enumTextValue;
            }
        }

        ///<summary>
        /// Allows the creation of a friendly text representation of the enumeration.
        ///</summary>
        /// <param name="text">The reader friendly text to decorate the enum.</param>
        public EnumTextValueAttribute(string text)
        {
            enumTextValue = text;
        }
    }

    /// <summary>
    /// 枚举属性设置
    /// </summary>
    public abstract class EntityHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEnumTextValue(Enum e)
        {
            string ret = string.Empty;
            if (e == null) return ret;

            Type t = e.GetType();
            MemberInfo[] members = t.GetMember(e.ToString());
            if (members != null && members.Length == 1)
            {
                object[] attrs = members[0].GetCustomAttributes(typeof(EnumTextValueAttribute), false);
                if (attrs.Length == 1)
                {
                    ret = ((EnumTextValueAttribute)attrs[0]).Text;
                }
            }
            return ret;
        }
    }

    /// <summary>
    /// 特性.自动生成Sql语句
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class PropertyAttribute : Attribute
    {
        private bool _select = true;
        /// <summary>
        /// 是否生成列, 默认生成，在Db
        /// </summary>
        public bool Select
        {
            get { return _select; }
            set { _select = value; }
        }
        private bool _clone = true;
        /// <summary>
        /// 是否复制列，默认复制
        /// </summary>
        public bool Clone
        {
            get { return _clone; }
            set { _clone = value; }
        }
        private bool _excel = true;
        /// <summary>
        /// 是否复制导入列，默认导入
        /// </summary>
        public bool Excel
        {
            get { return _excel; }
            set { _excel = value; }
        }
        private bool _show = true;
        /// <summary>
        /// 是否显示列
        /// 默认显示，在GridView
        /// </summary>
        public bool Show
        {
            get { return _show; }
            set { _show = value; }
        }
        /// <summary>
        /// 主键名称，不可更新
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 标识列名称，可更新
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// 列中文名称
        /// </summary>
        public string CnName { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string Table { get; set; }
    }
}
