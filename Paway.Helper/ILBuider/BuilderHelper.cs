using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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
        private static Dictionary<string, Delegate> CloneFunc { set; get; } = new Dictionary<string, Delegate>();
        private static Dictionary<string, Delegate> CloneAction { set; get; } = new Dictionary<string, Delegate>();
        /// <summary>
        /// IL动态代码(Emit)，复制（实体及列表）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="depth">深度克隆Class及IList列表</param>
        /// <returns></returns>
        public static T Clone<T>(this T obj, bool depth = false)
        {
            return (T)CloneObject(obj, depth);
        }
        /// <summary>
        /// IL动态代码(Emit)，复制（实体及列表）
        /// </summary>
        public static object CloneObject(object obj, bool depth)
        {
            if (obj is IEnumerable list)
            {
                var type = list.GenericType();
                if (!CloneFunc.TryGetValue(type.FullName + "." + depth, out Delegate func))
                {
                    func = CloneBuilder.CloneFunc(type, depth);
                    CloneFunc.Add(type.FullName + "." + depth, func);
                }
                var iList = type.GenericList();
                var action = (Func<object, object>)func;
                foreach (var item in list)
                {
                    iList.Add(action(item));
                }
                return iList;
            }
            else
            {
                var type = obj.GetType();
                if (!CloneFunc.TryGetValue(type.FullName + "." + depth, out Delegate func))
                {
                    func = CloneBuilder.CloneFunc(type, depth);
                    CloneFunc.Add(type.FullName + "." + depth, func);
                }
                return ((Func<object, object>)func)(obj);
            }
        }
        /// <summary>
        /// IL动态代码(Emit)，复制（到已有实体及列表）
        /// </summary>
        public static void Clone<T>(this T obj, T copy, bool depth = false)
        {
            CloneObject(obj, copy, depth);
        }
        /// <summary>
        /// IL动态代码(Emit)，复制（到已有实体及列表）
        /// </summary>
        public static void CloneObject(object obj, object copy, bool depth)
        {
            if (obj is IList list)
            {
                var type = list.GenericType();
                if (!CloneFunc.TryGetValue(type.FullName + "." + depth, out Delegate func))
                {
                    func = CloneBuilder.CloneFunc(type, depth);
                    CloneFunc.Add(type.FullName + "." + depth, func);
                }
                var copyList = copy as IList;
                copyList.Clear();
                var action = (Func<object, object>)func;
                foreach (var item in list)
                {
                    copyList.Add(action(item));
                }
            }
            else
            {
                var type = obj.GetType();
                if (!CloneAction.TryGetValue(type.FullName + "." + depth, out Delegate action))
                {
                    action = CloneBuilder.CloneAction(type, depth);
                    CloneAction.Add(type.FullName + "." + depth, action);
                }
                ((Action<object, object>)action)(obj, copy);
            }
        }

        #endregion

        #region ToList
        /// <summary>
        /// IL动态代码(Emit)，DataTable转List
        /// </summary>
        public static List<T> ToList<T>(this DataTable table, int? count = null) where T : new()
        {
            if (table == null) return new List<T>();

            if (count > table.Rows.Count) count = table.Rows.Count;
            List<T> list = new List<T>(count ?? 0);
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
        /// 并行排序（指定类型，非泛型），并返回排序后的List列表
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <param name="list">object列表</param>
        /// <param name="name">排序列名称</param>
        /// <param name="sort">true:升序，false:倒序</param>
        public static ParallelQuery Sort(this Type type, List<object> list, string name, bool sort = true)
        {
            var builder = SortBuilder.CreateBuilder(type, name, out bool iString);
            var property = type.Property(name);
            if (iString)
            {
                return sort ? list.AsParallel().OrderBy(c => builder.Build(c) as string, new StringComparer()) :
                    list.AsParallel().OrderByDescending(c => builder.Build(c) as string, new StringComparer());
            }
            else
            {
                return sort ? list.AsParallel().OrderBy(c => builder.Build(c)) : list.AsParallel().OrderByDescending(c => builder.Build(c));
            }
        }
        /// <summary>
        /// 并行排序，并返回排序后的List列表
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name">排序列名称</param>
        /// <param name="sort">true:升序，false:倒序</param>
        public static void Sort<T>(this List<T> list, string name, bool sort = true)
        {
            var dic = new Dictionary<string, bool>
            {
                { name, sort }
            };
            list.Sort(dic);
        }
        /// <summary>
        /// 多列并行升序排序，并返回排序后的List列表
        /// </summary>
        /// <param name="list"></param>
        /// <param name="names">排序列名称组</param>
        public static void Sort<T>(this List<T> list, params string[] names)
        {
            var dic = new Dictionary<string, bool>();
            foreach (var name in names) dic.Add(name, true);
            list.Sort(dic);
        }
        /// <summary>
        /// 并行排序，并返回排序后的List列表
        /// 自定义列与升倒序键值对
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sort">Key:名称，值:true:升序，false:倒序</param>
        public static void Sort<T>(this List<T> list, Dictionary<string, bool> sort)
        {
            if (sort == null || sort.Count == 0) throw new ArgumentException("未设置排序列");
            var item = sort.ElementAt(0);
            var builder = SortBuilder.CreateBuilder(typeof(T), item.Key, out bool iString);
            var property = typeof(T).Property(item.Key);
            OrderedParallelQuery<T> orderBy;
            if (iString)
            {
                orderBy = item.Value ? list.AsParallel().OrderBy(c => builder.Build(c) as string, new StringComparer()) :
                   list.AsParallel().OrderByDescending(c => builder.Build(c) as string, new StringComparer());
            }
            else
            {
                orderBy = item.Value ? list.AsParallel().OrderBy(c => builder.Build(c)) : list.AsParallel().OrderByDescending(c => builder.Build(c));
            }
            for (int i = 1; i < sort.Count; i++)
            {
                item = sort.ElementAt(i);
                var builder2 = SortBuilder.CreateBuilder(typeof(T), item.Key, out iString);
                property = typeof(T).Property(item.Key);
                if (iString)
                {
                    orderBy = item.Value ? orderBy.ThenBy(c => builder2.Build(c) as string, new StringComparer()) :
                        orderBy.ThenByDescending(c => builder2.Build(c) as string, new StringComparer());
                }
                else
                {
                    orderBy = item.Value ? orderBy.ThenBy(c => builder2.Build(c)) : orderBy.ThenByDescending(c => builder2.Build(c));
                }
            }
            var temp = orderBy.ToList();
            list.Clear();
            list.AddRange(temp);
        }

        #endregion

        #region SQL
        /// <summary>
        /// 创建SQL参数
        /// </summary>
        public static DbParameter AddParameter(string column, object value, Type type, Type ptype)
        {
            var param = (DbParameter)Activator.CreateInstance(ptype);
            param.ParameterName = string.Format("@{0}", column);
            switch (type.Name)
            {
                case nameof(DateTime):
                    var time = (DateTime)value;
                    if (TConfig.IUtcTime) time = time.ToUniversalTime();
                    if (time == DateTime.MinValue) param.Value = DBNull.Value;
                    else param.Value = time;
                    break;
                case nameof(Image):
                case nameof(Bitmap):
                    param.Value = StructHelper.ImageToBytes((Image)value);
                    param.DbType = DbType.Binary;
                    break;
                default:
                    param.Value = value;
                    break;
            }
            if (param.Value == null) param.Value = DBNull.Value;
            return param;
        }

        #endregion

        #region GetValue、SetValue
        private static Dictionary<string, Delegate> GetValueFunc { set; get; } = new Dictionary<string, Delegate>();
        private static Dictionary<string, Delegate> SetValueFunc { set; get; } = new Dictionary<string, Delegate>();
        /// <summary>
        /// IL动态代码(Emit)，获取值
        /// </summary>
        public static object GetValue<T>(this T t, string name)
        {
            return typeof(T).GetValue(t, name);
        }
        /// <summary>
        /// IL动态代码(Emit)，获取值
        /// </summary>
        public static object GetValue(this Type type, object obj, string name)
        {
            if (!GetValueFunc.TryGetValue(type.FullName + "." + name, out Delegate func))
            {
                func = ValueBuilder.GetValueFunc(type, name);
                GetValueFunc.Add(type.FullName + "." + name, func);
            }
            return ((Func<object, object>)func)(obj);
        }
        /// <summary>
        /// IL动态代码(Emit)，设置值
        /// </summary>
        public static void SetValue<T>(this T t, string name, object value)
        {
            typeof(T).SetValue(t, name, value);
        }
        /// <summary>
        /// IL动态代码(Emit)，GetValue
        /// </summary>
        public static void SetValue(this Type type, object obj, string name, object value)
        {
            if (!SetValueFunc.TryGetValue(type.FullName + "." + name, out Delegate func))
            {
                func = ValueBuilder.SetValueFunc(type, name);
                SetValueFunc.Add(type.FullName + "." + name, func);
            }
            ((Action<object, object>)func)(obj, value);
        }

        #endregion

        #region 静态
        private static Dictionary<string, Type> GetTypeFunc { set; get; } = new Dictionary<string, Type>();
        /// <summary>
        /// 缓存反射类型
        /// </summary>
        public static Type GetType<T>(this T t, string name)
        {
            return t.GetType().GetType(name);
        }
        /// <summary>
        /// 缓存反射类型
        /// </summary>
        public static Type GetType(this Type type, string name)
        {
            if (!GetTypeFunc.TryGetValue(type.FullName + "." + name, out Type propertyType))
            {
                var property = type.Property(name);
                if (property == null) propertyType = null;
                else propertyType = property.PropertyType;
                GetTypeFunc.Add(type.FullName + "." + name, propertyType);
            }
            return propertyType;
        }

        #endregion

        #region 动态代码
        /// <summary>
        /// 值数据转引用数据(Value->object)
        /// GetGetMethod()获取到的是值引用
        /// </summary>
        internal static void Box(this ILGenerator generator, PropertyInfo property)
        {
            if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                generator.Emit(OpCodes.Box, property.PropertyType);
            else
                generator.Emit(OpCodes.Castclass, property.PropertyType);
        }
        /// <summary>
        /// 拆箱 引用数据转值数据(object->Value)
        /// 实体.SetGetMethod()需要值数据
        /// DataRow.SetGetMethod()需要引用数据
        /// </summary>
        internal static void UnBox(this ILGenerator generator, PropertyInfo property)
        {
            generator.Emit(OpCodes.Unbox_Any, property.PropertyType);
        }

        #endregion
    }
}