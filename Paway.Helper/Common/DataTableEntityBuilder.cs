using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Reflection.Emit;

namespace Paway.Helper
{
    /// <summary>
    /// DataTable转List
    /// 动态生成代码Emit技术(IL生成)
    /// </summary>
    public class DataTableEntityBuilder<T>
    {
        private static readonly MethodInfo getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod = typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });
        private static readonly MethodInfo getImage = typeof(StructHelper).GetMethod("BytesToImage", new Type[] { typeof(byte[]) });
        private Delegate handler;
        private DataTableEntityBuilder() { }
        /// <summary>
        /// 转化
        /// </summary>
        public T Build(DataRow dataRecord)
        {
            return ((Func<DataRow, T>)handler)(dataRecord);
        }
        /// <summary>
        /// IL动态代码，创建委托
        /// </summary>
        public static DataTableEntityBuilder<T> CreateBuilder(DataRow dataRecord)
        {
            DataTableEntityBuilder<T> dynamicBuilder = new DataTableEntityBuilder<T>();
            var type = typeof(T);
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity", type, new Type[] { typeof(DataRow) }, type, true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(type);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);
            //byte[]转Image非静态方法转变示例
            var imageHelper = generator.DeclareLocal(typeof(StructHelper));
            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, imageHelper);
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
                    if (property.PropertyType == typeof(Image))
                    {
                        //加载参数
                        generator.Emit(OpCodes.Ldloc, imageHelper);
                    }
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    if (property.PropertyType == typeof(Image))
                    {
                        generator.Emit(OpCodes.Unbox_Any, typeof(byte[]));
                        //转化为Image
                        generator.Emit(OpCodes.Callvirt, getImage);
                    }
                    else generator.Emit(OpCodes.Unbox_Any, property.PropertyType);
                    generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            dynamicBuilder.handler = method.CreateDelegate(typeof(Func<DataRow, T>));
            return dynamicBuilder;
        }
    }
}