using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// IL动态代码(Emit)，排序
    /// </summary>
    internal class SortBuilder
    {
        private Delegate handler;
        private SortBuilder() { }
        /// <summary>
        /// 转化
        /// </summary>
        public dynamic Build(object obj)
        {
            switch (handler.Method.ReturnType.Name)
            {
                case nameof(Int32):
                    return ((Func<object, int>)handler)(obj);
                case nameof(Int64):
                    return ((Func<object, long>)handler)(obj);
                case nameof(Double):
                    return ((Func<object, double>)handler)(obj);
                case nameof(String):
                    return ((Func<object, string>)handler)(obj);
                default:
                    return 0;
            }
        }
        /// <summary>
        /// 生成动态代码，创建委托
        /// </summary>
        public static SortBuilder CreateBuilder(Type type, string name, out bool iString)
        {
            iString = false;
            var property = type.Property(name);
            if (property == null) throw new ArgumentException(name + " Argument can not be empty");
            if (!property.CanRead) throw new ArgumentException("Cannot read value");

            var getTCompareInt = typeof(BuilderHelper).GetMethod(nameof(BuilderHelper.TCompareInt),
                TConfig.Flags, null, new Type[] { typeof(object) }, null);
            var getTCompareLong = typeof(BuilderHelper).GetMethod(nameof(BuilderHelper.TCompareLong),
                TConfig.Flags, null, new Type[] { typeof(object) }, null);
            var getTCompareDouble = typeof(BuilderHelper).GetMethod(nameof(BuilderHelper.TCompareDouble),
                TConfig.Flags, null, new Type[] { typeof(object) }, null);

            var dbType = property.PropertyType;
            if (dbType.IsGenericType && Nullable.GetUnderlyingType(dbType) != null) dbType = Nullable.GetUnderlyingType(dbType);
            if (dbType.IsEnum) dbType = dbType.GetEnumUnderlyingType();
            var dymMethod = new DynamicMethod(type.Name + nameof(SortBuilder), typeof(string), new Type[] { typeof(object) }, true);
            switch (dbType.Name)
            {
                case nameof(Int32):
                case nameof(Int16):
                case nameof(Byte):
                case nameof(Boolean):
                case nameof(Image):
                case nameof(Bitmap):
                    dymMethod = new DynamicMethod(type.Name + nameof(SortBuilder), typeof(int), new Type[] { typeof(object) }, true);
                    break;
                case nameof(Int64):
                case nameof(DateTime):
                    dymMethod = new DynamicMethod(type.Name + nameof(SortBuilder), typeof(long), new Type[] { typeof(object) }, true);
                    break;
                case nameof(Double):
                case nameof(Single):
                case nameof(Decimal):
                    dymMethod = new DynamicMethod(type.Name + nameof(SortBuilder), typeof(double), new Type[] { typeof(object) }, true);
                    break;
            }
            var generator = dymMethod.GetILGenerator();// Create ILGenerator

            generator.GetValue(property, type);//获取引用值
            switch (dbType.Name)
            {
                case nameof(Int32):
                case nameof(Int16):
                case nameof(Byte):
                case nameof(Boolean):
                case nameof(Image):
                case nameof(Bitmap):
                    generator.Emit(OpCodes.Call, getTCompareInt);//调用静态方法
                    break;
                case nameof(Int64):
                case nameof(DateTime):
                    generator.Emit(OpCodes.Call, getTCompareLong);//调用静态方法
                    break;
                case nameof(Double):
                case nameof(Single):
                case nameof(Decimal):
                    generator.Emit(OpCodes.Call, getTCompareDouble);//调用静态方法
                    break;
                default:
                    //String不用再次转化
                    //generator.Emit(OpCodes.Call, getTCompareString);//调用静态方法
                    break;
            }
            generator.Emit(OpCodes.Ret);

            var builder = new SortBuilder();
            switch (dbType.Name)
            {
                case nameof(Int32):
                case nameof(Int16):
                case nameof(Byte):
                case nameof(Boolean):
                case nameof(Image):
                case nameof(Bitmap):
                    builder.handler = dymMethod.CreateDelegate(typeof(Func<object, int>));
                    break;
                case nameof(Int64):
                case nameof(DateTime):
                    builder.handler = dymMethod.CreateDelegate(typeof(Func<object, long>));
                    break;
                case nameof(Double):
                case nameof(Single):
                case nameof(Decimal):
                    builder.handler = dymMethod.CreateDelegate(typeof(Func<object, double>));
                    break;
                default:
                    iString = true;
                    builder.handler = dymMethod.CreateDelegate(typeof(Func<object, string>));
                    break;
            }
            return builder;
        }
    }
}
