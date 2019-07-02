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

            var dymMethod = new DynamicMethod(type.Name + nameof(DataTableBuilder), null, new Type[] { typeof(object), typeof(DataRow) }, true);
            ILGenerator generator = dymMethod.GetILGenerator();
            foreach (var property in type.Properties())
            {
                if (iExcel && !property.IExcel()) continue;
                if (!property.CanRead) continue;
                Type dbType = property.PropertyType;
                if (dbType.IsClass && dbType != typeof(string) && dbType != typeof(byte[]) && dbType != typeof(Image) && dbType != typeof(Bitmap)) continue;
                Label endIfLabel = generator.DefineLabel();
                LocalBuilder value = generator.DeclareLocal(dbType);
                if (dbType.IsGenericType)
                {
                    if (Nullable.GetUnderlyingType(dbType) == null) continue;
                    GetValue(type, generator, property);
                    //generator.Emit(OpCodes.Stloc, value);
                    //generator.Emit(OpCodes.Ldloc, value);
                    generator.Emit(OpCodes.Brfalse, endIfLabel);
                }
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldstr, property.Column());

                //if (dbType.IsGenericType) generator.Emit(OpCodes.Ldloc, value);
                GetValue(type, generator, property);
                generator.Emit(OpCodes.Callvirt, setValueMethod);
                if (dbType.IsGenericType)
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
        private static void GetValue(Type type, ILGenerator generator, PropertyInfo property)
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
            generator.Emit(OpCodes.Callvirt, property.GetGetMethod());//获取值
            generator.Box(property);//值数据转引用数据
        }
    }
}
