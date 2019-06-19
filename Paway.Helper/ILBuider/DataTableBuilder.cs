using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// IL动态代码(Emit)，List转DataTable
    /// </summary>
    internal class DataTableBuilder
    {
        private Delegate handler;
        private DataTableBuilder() { }
        /// <summary>
        /// 转化
        /// </summary>
        public void Build(object t, DataRow dr)
        {
            ((Action<object, DataRow>)handler)(t, dr);
        }
        /// <summary>
        /// IL动态代码，创建委托
        /// </summary>
        public static DataTableBuilder CreateBuilder(Type type, bool iExcel = false)
        {
            var setValueMethod = typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) });

            var dymMethod = new DynamicMethod(type.Name + "DataTableBuilder", null, new Type[] { typeof(object), typeof(DataRow) }, true);
            ILGenerator generator = dymMethod.GetILGenerator();
            foreach (var property in type.GetProperties())
            {
                if (iExcel && !property.IExcel()) continue;
                Label endIfLabel = generator.DefineLabel();
                if (property.PropertyType.IsGenericType)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
                    generator.Emit(OpCodes.Callvirt, property.GetGetMethod());
                    //实体数据使用需要装箱
                    if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                        generator.Emit(OpCodes.Box, property.PropertyType);//装箱                                                        
                    else
                        generator.Emit(OpCodes.Castclass, property.PropertyType);
                    generator.Emit(OpCodes.Brfalse, endIfLabel);
                }
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldstr, property.Column());
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
                generator.Emit(OpCodes.Callvirt, property.GetGetMethod());//获取
                //实体数据使用需要装箱
                if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                    generator.Emit(OpCodes.Box, property.PropertyType);//装箱                                                        
                else
                    generator.Emit(OpCodes.Castclass, property.PropertyType);
                generator.Emit(OpCodes.Callvirt, setValueMethod);
                if (property.PropertyType.IsGenericType)
                {
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ret);

            var builder = new DataTableBuilder
            {
                handler = dymMethod.CreateDelegate(typeof(Action<object, DataRow>))
            };
            return builder;
        }
    }
}
