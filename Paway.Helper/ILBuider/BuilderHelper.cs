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
using System.Threading;
using System.Threading.Tasks;

namespace Paway.Helper
{
    /// <summary>
    /// IL动态代码创建方法
    /// </summary>
    public static class BuilderHelper
    {
        #region 锁
        private static object _syncRoot;
        /// <summary>
        /// 原子锁
        /// </summary>
        private static object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }
                return _syncRoot;
            }
        }

        #endregion

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
        /// 并行处理列表复制
        /// </summary>
        internal static object CloneObject(object obj, bool depth)
        {
            if (obj is IList list)
            {
                var type = list.GenericType();
                Delegate action;
                lock (SyncRoot)
                {
                    if (!CloneAction.TryGetValue(type.FullName + "." + depth, out action))
                    {
                        action = CloneBuilder.CloneAction(type, depth);
                        CloneAction.Add(type.FullName + "." + depth, action);
                    }
                }
                var iList = type.GenericList();
                for (int i = 0; i < list.Count; i++)
                {
                    iList.Add(Activator.CreateInstance(type));
                }
                Parallel.For(0, list.Count, (i) =>
                {
                    ((Action<object, object>)action)(list[i], iList[i]);
                });
                return iList;
            }
            else if (obj != null)
            {
                var type = obj.GetType();
                Delegate func;
                lock (SyncRoot)
                {
                    if (!CloneFunc.TryGetValue(type.FullName + "." + depth, out func))
                    {
                        func = CloneBuilder.CloneFunc(type, depth);
                        CloneFunc.Add(type.FullName + "." + depth, func);
                    }
                }
                return ((Func<object, object>)func)(obj);
            }
            return null;
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
        /// 并行处理列表复制
        /// </summary>
        internal static void CloneObject(object obj, object copy, bool depth)
        {
            if (obj is IList list)
            {
                var type = list.GenericType();
                Delegate action;
                lock (SyncRoot)
                {
                    if (!CloneAction.TryGetValue(type.FullName + "." + depth, out action))
                    {
                        action = CloneBuilder.CloneAction(type, depth);
                        CloneAction.Add(type.FullName + "." + depth, action);
                    }
                }
                var copyList = copy as IList;
                copyList.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    copyList.Add(Activator.CreateInstance(type));
                }
                Parallel.For(0, list.Count, (i) =>
                {
                    ((Action<object, object>)action)(list[i], copyList[i]);
                });
            }
            else if (obj != null)
            {
                var type = obj.GetType();
                if (!type.IsInstanceOfType(copy)) type = copy.GetType();
                Delegate action;
                lock (SyncRoot)
                {
                    if (!CloneAction.TryGetValue(type.FullName + "." + depth, out action))
                    {
                        action = CloneBuilder.CloneAction(type, depth);
                        CloneAction.Add(type.FullName + "." + depth, action);
                    }
                }
                ((Action<object, object>)action)(obj, copy);
            }
        }

        #endregion

        #region ToList
        /// <summary>
        /// IL动态代码(Emit)，DataTable转List
        /// 并行处理
        /// </summary>
        public static List<T> ToList<T>(this DataTable table, int count = 0) where T : new()
        {
            if (table == null) return new List<T>();

            if (count <= 0 || count > table.Rows.Count) count = table.Rows.Count;
            List<T> list = new List<T>(count);
            if (count > 0)
            {
                var type = typeof(T);
                for (int i = 0; i < count; i++)
                {
                    list.Add((T)Activator.CreateInstance(type));
                }
                var builder = EntityBuilder<T>.CreateBuilder(table.Rows[0]);
                Parallel.For(0, count, (i) =>
                {
                    builder.Build(table.Rows[i], list[i]);
                });
            }
            return list;
        }

        #endregion

        #region ToDataTable
        /// <summary>
        /// IL动态代码(Emit)，List转DataTable
        /// </summary>
        public static DataTable ToDataTable(this IList list)
        {
            var type = list.GenericType();
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
        /// IL动态代码(Emit)，T转DataRow
        /// </summary>
        public static DataRow ToDataRow(this object obj)
        {
            var type = obj.GetType();
            var table = type.CreateTable();
            var builder = DataTableBuilder.CreateBuilder(type);
            {
                var dr = table.NewRow();
                builder.Build(obj, dr);
                return dr;
            }
        }
        /// <summary>
        /// IL动态代码(Emit)，List转DataTable（Excel）
        /// </summary>
        public static DataTable ToExcelTable(this IList list)
        {
            var type = list.GenericType();
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
            if (list.Count == 0) return;
            var item = sort.ElementAt(0);
            var type = list[0].GetType();
            var builder = SortBuilder.CreateBuilder(type, item.Key, out bool iString);
            var property = type.Property(item.Key);
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
                var builder2 = SortBuilder.CreateBuilder(type, item.Key, out iString);
                property = type.Property(item.Key);
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
        /// <summary>
        /// 将值拆箱为Int值以进行比较
        /// </summary>
        internal static int TCompareInt(this object obj)
        {
            if (obj == null) return 0;
            Type type = obj.GetType();
            if (type.IsEnum) type = type.GetEnumUnderlyingType();
            switch (type.Name)
            {
                case nameof(Int32):
                    return (int)obj;
                case nameof(Int16):
                    return (short)obj;
                case nameof(Byte):
                    return (byte)obj;
                case nameof(Boolean):
                    return (bool)obj ? 1 : 0;
                case nameof(Image):
                case nameof(Bitmap):
                    var image = (Image)obj;
                    return image.Width * image.Height;
            }
            return obj.ToInt();
        }
        /// <summary>
        /// 将值拆箱为Double值以进行比较
        /// </summary>
        internal static double TCompareDouble(this object obj)
        {
            if (obj == null) return 0;
            Type type = obj.GetType();
            switch (type.Name)
            {
                case nameof(Double):
                    return (double)obj;
                case nameof(Single):
                    return (float)obj;
                case nameof(Decimal):
                    return (double)(decimal)obj;
                default:
                    return obj.ToDouble();
            }
        }
        /// <summary>
        /// 将值拆箱为Long值以进行比较
        /// </summary>
        internal static long TCompareLong(this object obj)
        {
            if (obj == null) return 0;
            Type type = obj.GetType();
            switch (type.Name)
            {
                case nameof(Int64):
                    return (long)obj;
                case nameof(DateTime):
                    return ((DateTime)obj).Ticks;
                default:
                    return obj.ToLong();
            }
        }

        #endregion

        #region SQL
        /// <summary>
        /// 创建SQL参数
        /// </summary>
        internal static DbParameter AddParameter(string column, object value, Type type, Type ptype)
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
        private static Dictionary<Type, Delegate> GetValuesFunc { set; get; } = new Dictionary<Type, Delegate>();
        private static Dictionary<string, Delegate> SetValueFunc { set; get; } = new Dictionary<string, Delegate>();
        /// <summary>
        /// IL动态代码(Emit)，获取值
        /// </summary>
        public static object GetValue(this object obj, string name)
        {
            var type = obj.GetType();
            if (!GetValueFunc.TryGetValue(type.FullName + "." + name, out Delegate func))
            {
                func = ValueBuilder.GetValueFunc(type, name);
                GetValueFunc.Add(type.FullName + "." + name, func);
            }
            return ((Func<object, object>)func)(obj);
        }
        /// <summary>
        /// 泛型值组
        /// </summary>
        public static object[] GetValues(this object obj)
        {
            var type = obj.GetType();
            if (!GetValuesFunc.TryGetValue(type, out Delegate func))
            {
                func = ValueBuilder.GetValuesFunc(type);
                GetValuesFunc.Add(type, func);
            }
            return ((Func<object, object[]>)func)(obj);
        }
        /// <summary>
        /// IL动态代码(Emit)，GetValue
        /// </summary>
        public static void SetValue(this object obj, string name, object value)
        {
            var type = obj.GetType();
            if (!SetValueFunc.TryGetValue(type.FullName + "." + name, out Delegate func))
            {
                func = ValueBuilder.SetValueFunc(type, name);
                SetValueFunc.Add(type.FullName + "." + name, func);
            }
            ((Action<object, object>)func)(obj, value);
        }
        /// <summary>
        /// 泛型查找 
        /// </summary>
        public static IList FindAll(this IList list, string name, object value)
        {
            var type = list.GenericType();
            var vList = type.GenericList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetValue(name).Equals(value))
                {
                    vList.Add(list[i]);
                }
            }
            return vList;
        }
        /// <summary>
        /// 泛型查找 
        /// </summary>
        public static object Find(this IList list, string name, object value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetValue(name).Equals(value))
                {
                    return list[i];
                }
            }
            return null;
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
        /// 转化参数到类型，获取参数值，返回引用数据（指定类型时，调用转化）
        /// </summary>
        internal static void GetValue(this ILGenerator generator, PropertyInfo property, Type type)
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
            generator.Box(property);//值数据转引用数据
        }
        /// <summary>
        /// 获取参数值，并将值数据转引用数据(Value->object)
        /// GetGetMethod()获取到的是值
        /// </summary>
        internal static void Box(this ILGenerator generator, PropertyInfo property)
        {
            generator.Emit(OpCodes.Callvirt, property.GetGetMethod());//获取值
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