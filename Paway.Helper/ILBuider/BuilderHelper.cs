﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        #region Sort
        /// <summary>
        /// 并行排序，并返回排序后的List列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="name">排序列名称</param>
        /// <param name="sort">true:升序，false:倒序</param>
        public static List<T> Sort<T>(this List<T> list, string name, bool sort = true)
        {
            var dic = new Dictionary<string, bool>();
            dic.Add(name, sort);
            return list.Sort(dic);
        }
        /// <summary>
        /// 多列并行升序排序，并返回排序后的List列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="names">排序列名称组</param>
        public static List<T> Sort<T>(this List<T> list, params string[] names)
        {
            if (names.Length == 0) throw new ArgumentException("未设置排序列", "names");
            var dic = new Dictionary<string, bool>();
            foreach (var name in names) dic.Add(name, true);
            return list.Sort(dic);
        }
        /// <summary>
        /// 并行排序，并返回排序后的List列表
        /// 自定义列与升倒序键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="sort">Key:名称，值:true:升序，false:倒序</param>
        public static List<T> Sort<T>(this List<T> list, Dictionary<string, bool> sort)
        {
            if (sort == null || sort.Count == 0) throw new ArgumentException("未设置排序列", "sort");
            var parallel = list.AsParallel();
            var item = sort.ElementAt(0);
            var builder = SortBuilder.CreateBuilder(typeof(T), item.Key);
            var orderBy = item.Value ? list.AsParallel().OrderBy(c => builder.Build(c)) : list.AsParallel().OrderByDescending(c => builder.Build(c));
            for (int i = 1; i < sort.Count; i++)
            {
                item = sort.ElementAt(i);
                builder = SortBuilder.CreateBuilder(typeof(T), item.Key);
                orderBy = item.Value ? orderBy.ThenBy(c => builder.Build(c)) : orderBy.ThenByDescending(c => builder.Build(c));
            }
            return orderBy.ToList();
        }

        #endregion
    }
}