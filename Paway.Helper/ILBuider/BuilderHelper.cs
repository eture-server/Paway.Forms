using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace Paway.Helper
{
    /// <summary>
    /// IL动态代码创建方法
    /// </summary>
    public static class BuilderHelper
    {
        #region Clone
        private static Dictionary<Type, Delegate> CloneFunc { set; get; } = new Dictionary<Type, Delegate>();
        private static Dictionary<Type, Delegate> CloneAction { set; get; } = new Dictionary<Type, Delegate>();
        /// <summary>
        /// IL动态代码(Emit)，复制（不复制包含类，复制基类、属性、字段）
        /// </summary>
        public static T Clone<T>(this T t)
        {
            if (!CloneFunc.TryGetValue(typeof(T), out Delegate func))
            {
                func = CloneBuilder.GetCloneFunc<T>();
                CloneFunc.Add(typeof(T), func);
            }
            return ((Func<T, T>)func)(t);
        }
        /// <summary>
        /// IL动态代码(Emit)，复制（不复制包含类，复制基类、公有属性）
        /// </summary>
        public static void Clone<T>(this T t, T copy)
        {
            if (!CloneAction.TryGetValue(typeof(T), out Delegate action))
            {
                action = CloneBuilder.GetCloneAction<T>();
                CloneAction.Add(typeof(T), action);
            }
             ((Action<T, T>)action)(t, copy);
        }

        #endregion

        #region ToList
        /// <summary>
        /// IL动态代码(Emit)，DataTable转List
        /// </summary>
        public static List<T> ToList<T>(this DataTable table, int? count = null)
        {
            List<T> list = new List<T>();
            if (table == null) return list;
            if (count > table.Rows.Count) count = table.Rows.Count;
            if (table.Rows.Count > 0)
            {
                var builder = EntityBuilder<T>.CreateBuilder(table.Rows[0]);
                foreach (DataRow dr in table.Rows)
                {
                    list.Add(builder.Build(dr));
                    if (count != null && list.Count == count) break;
                }
            }
            return list;
        }

        #endregion

        #region ToDataTable
        /// <summary>
        /// IL动态代码(Emit)，List转DataTable
        /// </summary>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            return typeof(T).ToDataTable(list);
        }
        /// <summary>
        /// IL动态代码(Emit)，List转DataTable
        /// </summary>
        public static DataTable ToDataTable(this IEnumerable list)
        {
            var type = list.GenericType();
            return type.ToDataTable(list);
        }
        /// <summary>
        /// IL动态代码(Emit)，List转DataTable
        /// </summary>
        internal static DataTable ToDataTable(this Type type, IEnumerable list)
        {
            var table = type.CreateTable();
            var builder = DataTableBuilder.CreateBuilder(type);
            foreach (var item in list)
            {
                var dr = table.NewRow();
                builder.Build(item, dr);
                table.Rows.Add(dr);
            }
            return table;
        }
        /// <summary>
        /// IL动态代码(Emit)，List转DataTable（Excel）
        /// </summary>
        public static DataTable ToExcelTable<T>(this List<T> list)
        {
            return typeof(T).ToExcelTable(list);
        }
        /// <summary>
        /// IL动态代码(Emit)，List转DataTable（Excel）
        /// </summary>
        public static DataTable ToExcelTable(this Type type, IList list)
        {
            var table = type.CreateExcelTable();
            var builder = DataTableBuilder.CreateBuilder(type, true);
            foreach (var item in list)
            {
                var dr = table.NewRow();
                builder.Build(item, dr);
                table.Rows.Add(dr);
            }
            return table;
        }

        #endregion
    }
}