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
                case nameof(Double):
                case nameof(Single):
                case nameof(Decimal):
                    return ((Func<object, double>)handler)(obj);
                default:
                    return ((Func<object, long>)handler)(obj);
            }
        }
        /// <summary>
        /// 生成动态代码，创建委托
        /// </summary>
        public static SortBuilder CreateBuilder(Type type, string name)
        {
            var properties = type.Properties();
            var property = properties.Find(c => c.Name == name);
            if (property == null) throw new ArgumentException("名称不存在", name);

            var getTCompare = typeof(ConverHelper).GetMethod("TCompare", new Type[] { typeof(object) });
            var getTCompareInt = typeof(ConverHelper).GetMethod("TCompareInt", new Type[] { typeof(object) });
            var getTCompareDouble = typeof(ConverHelper).GetMethod("TCompareDouble", new Type[] { typeof(object) });
            var dymMethod = new DynamicMethod(type.Name + "SortBuilder" + name, typeof(long), new Type[] { typeof(object) }, true);
            switch (property.PropertyType.Name)
            {
                case nameof(Int32):
                case nameof(Int16):
                case nameof(Byte):
                case nameof(Boolean):
                case nameof(Image):
                case nameof(Bitmap):
                    dymMethod = new DynamicMethod(type.Name + "SortBuilderInt" + name, typeof(int), new Type[] { typeof(object) }, true);
                    break;
                case nameof(Double):
                case nameof(Single):
                case nameof(Decimal):
                    dymMethod = new DynamicMethod(type.Name + "SortBuilderDouble" + name, typeof(double), new Type[] { typeof(object) }, true);
                    break;
            }
            var generator = dymMethod.GetILGenerator();// Create ILGenerator

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, type);//类型转化
            generator.Emit(OpCodes.Callvirt, property.GetGetMethod());//调用非静态方法
            //实体数据使用需要装箱
            if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                generator.Emit(OpCodes.Box, property.PropertyType);//装箱                                                        
            else
                generator.Emit(OpCodes.Castclass, property.PropertyType);
            switch (property.PropertyType.Name)
            {
                case nameof(Int32):
                case nameof(Int16):
                case nameof(Byte):
                case nameof(Boolean):
                case nameof(Image):
                case nameof(Bitmap):
                    generator.Emit(OpCodes.Call, getTCompareInt);//调用静态方法
                    break;
                case nameof(Double):
                case nameof(Single):
                case nameof(Decimal):
                    generator.Emit(OpCodes.Call, getTCompareDouble);//调用静态方法
                    break;
                default:
                    generator.Emit(OpCodes.Call, getTCompare);//调用静态方法
                    break;
            }
            generator.Emit(OpCodes.Ret);

            var builder = new SortBuilder();
            switch (property.PropertyType.Name)
            {
                case nameof(Int32):
                case nameof(Int16):
                case nameof(Byte):
                case nameof(Boolean):
                case nameof(Image):
                case nameof(Bitmap):
                    builder.handler = dymMethod.CreateDelegate(typeof(Func<object, int>));
                    break;
                case nameof(Double):
                case nameof(Single):
                case nameof(Decimal):
                    builder.handler = dymMethod.CreateDelegate(typeof(Func<object, double>));
                    break;
                default:
                    builder.handler = dymMethod.CreateDelegate(typeof(Func<object, long>));
                    break;
            }
            builder.returnType = property.PropertyType;
            return builder;
        }
    }
}
