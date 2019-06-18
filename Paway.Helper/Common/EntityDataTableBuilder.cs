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
    /// List转DataTable
    /// 动态生成代码Emit技术(IL生成)
    /// </summary>
    public class EntityDataTableBuilder<T>
    {
        private static readonly MethodInfo setValueMethod = typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) });
        private Delegate handler;
        private EntityDataTableBuilder() { }
        /// <summary>
        /// 转化
        /// </summary>
        public void Build(T t, DataRow dr)
        {
            ((Action<T, DataRow>)handler)(t, dr);
        }
        /// <summary>
        /// IL动态代码，创建委托
        /// </summary>
        public static EntityDataTableBuilder<T> CreateBuilder()
        {
            var builder = new EntityDataTableBuilder<T>();
            var method = new DynamicMethod(typeof(T).Name + "ConvertToDataRow", null, new Type[] { typeof(T), typeof(DataRow) }, true);
            var generator = method.GetILGenerator();
            foreach (var p in typeof(T).GetProperties())
            {
                Label endIfLabel = generator.DefineLabel();
                if (p.PropertyType.IsGenericType)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Callvirt, p.GetGetMethod());
                    if (p.PropertyType.IsValueType || p.PropertyType == typeof(string))
                        generator.Emit(OpCodes.Box, p.PropertyType);//装箱                                                        
                    else
                        generator.Emit(OpCodes.Castclass, p.PropertyType);
                    generator.Emit(OpCodes.Brfalse, endIfLabel);
                }
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldstr, p.Name);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Callvirt, p.GetGetMethod());//获取
                if (p.PropertyType.IsValueType || p.PropertyType == typeof(string))
                    generator.Emit(OpCodes.Box, p.PropertyType);//装箱                                                        
                else
                    generator.Emit(OpCodes.Castclass, p.PropertyType);
                generator.Emit(OpCodes.Callvirt, setValueMethod);
                if (p.PropertyType.IsGenericType)
                {
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ret);

            builder.handler = method.CreateDelegate(typeof(Action<T, DataRow>));
            return builder;
        }
    }
}
