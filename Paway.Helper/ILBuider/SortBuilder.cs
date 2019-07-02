﻿using Paway.Helper;
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
        /// <summary>
        /// 返回类型
        /// </summary>
        private Type returnType;
        private Delegate handler;
        private SortBuilder() { }
        /// <summary>
        /// 转化
        /// </summary>
        public dynamic Build(object obj)
        {
            switch (returnType.Name)
            {
                case nameof(Int32):
                case nameof(Int16):
                case nameof(Byte):
                case nameof(Boolean):
                case nameof(Image):
                case nameof(Bitmap):
                    return ((Func<object, int>)handler)(obj);
                case nameof(Int64):
                case nameof(DateTime):
                    return ((Func<object, long>)handler)(obj);
                case nameof(Double):
                case nameof(Single):
                case nameof(Decimal):
                    return ((Func<object, double>)handler)(obj);
                default:
                    return ((Func<object, string>)handler)(obj);
            }
        }
        /// <summary>
        /// 生成动态代码，创建委托
        /// </summary>
        public static SortBuilder CreateBuilder(Type type, string name, out bool iString)
        {
            iString = false;
            var property = type.Property(name);
            if (property == null) throw new ArgumentNullException(name);
            if (!property.CanRead) throw new ArgumentException("无法读取值");

            var getTCompareString = typeof(ConverHelper).GetMethod(nameof(ConverHelper.TCompareString), new Type[] { typeof(object) });
            var getTCompareInt = typeof(ConverHelper).GetMethod(nameof(ConverHelper.TCompareInt), new Type[] { typeof(object) });
            var getTCompareLong = typeof(ConverHelper).GetMethod(nameof(ConverHelper.TCompareLong), new Type[] { typeof(object) });
            var getTCompareDouble = typeof(ConverHelper).GetMethod(nameof(ConverHelper.TCompareDouble), new Type[] { typeof(object) });

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

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, type);//类型转化
            generator.Emit(OpCodes.Callvirt, property.GetGetMethod());//获取值
            generator.Box(property);//值数据转引用数据
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
                    generator.Emit(OpCodes.Call, getTCompareString);//调用静态方法
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
            builder.returnType = dbType;
            return builder;
        }
    }
}
