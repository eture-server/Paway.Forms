﻿using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Reflection.Emit;

namespace Paway.Helper
{
    /// <summary>
    /// IL动态代码(Emit)，DataTable转List
    /// </summary>
    internal class EntityBuilder<T> where T : new()
    {
        private Delegate handler;
        private EntityBuilder() { }
        /// <summary>
        /// 转化
        /// </summary>
        public void Build(DataRow dr, T t)
        {
            ((Action<DataRow, T>)handler)(dr, t);
        }

        private static DynamicMethod CreateMethod(Type type)
        {
            return new DynamicMethod(type.Name + nameof(EntityBuilder), null, new Type[] { typeof(DataRow), type }, true);
        }
        /// <summary>
        /// IL动态代码，创建委托(DataRow,T)
        /// </summary>
        public static EntityBuilder<T> CreateBuilder(DataRow dataRecord)
        {
            var dymMethod = CreateDynamicMethod.Create(typeof(T), dataRecord, CreateMethod);
            var handlerTo = new EntityBuilder<T>
            {
                handler = dymMethod.CreateDelegate(typeof(Action<DataRow, T>))
            };
            return handlerTo;
        }
    }
    /// <summary>
    /// IL动态代码(Emit)，DataTable转List(非泛型)
    /// </summary>
    internal class EntityBuilder
    {
        private Delegate handler;
        private EntityBuilder() { }
        /// <summary>
        /// 转化
        /// </summary>
        public void Build(DataRow dr, object t)
        {
            ((Action<DataRow, object>)handler)(dr, t);
        }

        private static DynamicMethod CreateMethod(Type type)
        {
            return new DynamicMethod(type.Name + nameof(EntityBuilder), null, new Type[] { typeof(DataRow), typeof(object) }, true);
        }
        private static void ConvertType(ILGenerator generator, Type type)
        {
            generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
        }
        /// <summary>
        /// IL动态代码，创建委托(DataRow,T)
        /// </summary>
        public static EntityBuilder CreateBuilder(Type type, DataRow dataRecord)
        {
            var dymMethod = CreateDynamicMethod.Create(type, dataRecord, CreateMethod, ConvertType);
            var handlerTo = new EntityBuilder
            {
                handler = dymMethod.CreateDelegate(typeof(Action<DataRow, object>))
            };
            return handlerTo;
        }
    }
    internal class CreateDynamicMethod
    {
        /// <summary>
        /// IL动态代码，创建委托(DataRow,T)
        /// </summary>
        public static DynamicMethod Create(Type type, DataRow dataRecord,
            Func<Type, DynamicMethod> createMethod, Action<ILGenerator, Type> convertType = null)
        {
            var getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
            var isDBNullMethod = typeof(DataRow).GetMethod(nameof(DataRow.IsNull), new Type[] { typeof(int) });
            var toImage = typeof(StructHelper).GetMethod(nameof(StructHelper.DeserializeImage), new Type[] { typeof(byte[]) });
            var toDouble = typeof(decimal).GetMethod(nameof(Decimal.ToDouble), new Type[] { typeof(decimal) });
            var toBool = typeof(ConverHelper).GetMethod(nameof(ConverHelper.ToBool), new Type[] { typeof(object) });

            var dymMethod = createMethod(type);
            ILGenerator generator = dymMethod.GetILGenerator();

            //LocalBuilder result = generator.DeclareLocal(type);
            //generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            //generator.Emit(OpCodes.Stloc, result);
            var properties = type.PropertiesCache();
            for (int i = 0; i < dataRecord.ItemArray.Length; i++)
            {
                PropertyInfo property = properties.Property(dataRecord.Table.Columns[i].ColumnName);
                if (property == null) continue;
                Type dbType = property.PropertyType;
                if (dbType.IsGenericType)
                {
                    if (Nullable.GetUnderlyingType(dbType) == null) continue;
                    dbType = Nullable.GetUnderlyingType(dbType);
                }
                if (dbType.IsClass && dbType != typeof(string) && dbType != typeof(byte[]) && dbType != typeof(Image) && dbType != typeof(Bitmap)) continue;
                if (property != null && property.CanWrite)
                {
                    var endIfLabel = generator.DefineLabel();
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);

                    generator.Emit(OpCodes.Ldarg_1);
                    convertType?.Invoke(generator, type);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    //DataTable数据object使用需要拆箱
                    if ((dbType == typeof(Image) || dbType == typeof(Bitmap)) && dataRecord.Table.Columns[i].DataType == typeof(byte[]))
                    {
                        generator.Emit(OpCodes.Unbox_Any, typeof(byte[]));
                        //静态方法byte[]转Image
                        generator.Emit(OpCodes.Call, toImage);
                    }
                    else if (dbType == typeof(bool) && dataRecord.Table.Columns[i].DataType == typeof(int))
                    {//兼容数据库数据=>int转bool
                        generator.Emit(OpCodes.Call, toBool);
                    }
                    else if (dbType == typeof(double) && dataRecord.Table.Columns[i].DataType == typeof(decimal))
                    {//兼容数据库数据=>decimal转double
                        generator.Emit(OpCodes.Unbox_Any, typeof(decimal));
                        generator.Emit(OpCodes.Call, toDouble);
                    }
                    else generator.UnBox(property);//引用转值
                    generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            //generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            return dymMethod;
        }
    }
}