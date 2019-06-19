﻿using System;
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
    internal class EntityBuilder<T>
    {
        private Delegate handler;
        private EntityBuilder() { }
        /// <summary>
        /// 转化
        /// </summary>
        public T Build(DataRow dr)
        {
            return ((Func<DataRow, T>)handler)(dr);
        }
        /// <summary>
        /// IL动态代码，创建委托
        /// </summary>
        public static EntityBuilder<T> CreateBuilder(DataRow dataRecord)
        {
            var getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
            var isDBNullMethod = typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });
            var getImage = typeof(StructHelper).GetMethod("BytesToImage", new Type[] { typeof(byte[]) });

            var type = typeof(T);
            var dymMethod = new DynamicMethod(type.Name + "EntityBuilder", type, new Type[] { typeof(DataRow) }, type, true);
            ILGenerator generator = dymMethod.GetILGenerator();

            LocalBuilder result = generator.DeclareLocal(type);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);
            var properties = type.Properties();
            for (int i = 0; i < dataRecord.ItemArray.Length; i++)
            {
                PropertyInfo property = type.GetProperty(dataRecord.Table.Columns[i].ColumnName);
                if (property == null)
                {
                    foreach (var item in properties)
                    {
                        if (item.Column() == dataRecord.Table.Columns[i].ColumnName)
                        {
                            property = item;
                            break;
                        }
                    }
                }
                var endIfLabel = generator.DefineLabel();
                if (property != null && property.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    //DataTable数据使用需要拆箱
                    if (property.PropertyType == typeof(Image))
                    {
                        generator.Emit(OpCodes.Unbox_Any, typeof(byte[]));
                        //静态方法byte[]转Image
                        generator.Emit(OpCodes.Call, getImage);
                    }
                    else generator.Emit(OpCodes.Unbox_Any, property.PropertyType);
                    generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            var builder = new EntityBuilder<T>
            {
                handler = dymMethod.CreateDelegate(typeof(Func<DataRow, T>))
            };
            return builder;
        }
    }
}