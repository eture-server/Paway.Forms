using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace Paway.Helper
{
    /// <summary>
    /// IL动态代码(Emit)，复制（不复制子级）
    /// </summary>
    internal abstract class CloneBuilder
    {
        /// <summary>
        /// 生成动态代码，复制属性字段等所有
        /// </summary>
        public static Delegate CloneFunc(Type type)
        {
            // Create ILGenerator
            var dymMethod = new DynamicMethod(type.Name + "CloneBuilder", typeof(object), new Type[] { typeof(object) }, true);
            ILGenerator generator = dymMethod.GetILGenerator();

            LocalBuilder result = generator.DeclareLocal(type);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            CloneFunc(generator, type);
            while (type.BaseType != null)
            {//获取所有父类属性
                type = type.BaseType;
                CloneFunc(generator, type);
            }

            // Load new constructed obj on eval stack -> 1 item on stack
            generator.Emit(OpCodes.Ldloc, result);
            // Return constructed object.   --> 0 items on stack
            generator.Emit(OpCodes.Ret);
            return dymMethod.CreateDelegate(typeof(Func<object, object>));
        }
        /// <summary>
        /// 获取指定Type属性
        /// </summary>
        private static void CloneFunc(ILGenerator generator, Type type)
        {
            foreach (var property in type.Properties().FindAll(c => c.CanRead && c.CanWrite))
            {
                if (property.PropertyType.IsGenericType && Nullable.GetUnderlyingType(property.PropertyType) == null) continue;
                if (!property.IClone()) continue;

                // Load the new object on the eval stack... (currently 1 item on eval stack)
                generator.Emit(OpCodes.Ldloc_0);
                // Load initial object (parameter)          (currently 2 items on eval stack)
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
                generator.Emit(OpCodes.Callvirt, property.GetGetMethod());
                generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
            }
        }
        /// <summary>
        /// 生成动态代码，仅复制公有属性
        /// </summary>
        public static Delegate CloneAction<T>()
        {
            var type = typeof(T);
            // Create ILGenerator
            var dymMethod = new DynamicMethod(type.Name + "CloneBuilder", null, new Type[] { type, type }, true);
            ILGenerator generator = dymMethod.GetILGenerator();

            foreach (var property in type.Properties().FindAll(c => c.CanRead && c.CanWrite))
            {
                //不复制静态类属性
                if (property.GetAccessors(true)[0].IsStatic) continue;
                if (property.PropertyType.IsGenericType && Nullable.GetUnderlyingType(property.PropertyType) == null) continue;
                if (!property.IClone()) continue;

                generator.Emit(OpCodes.Ldarg_1);// los
                generator.Emit(OpCodes.Ldarg_0);// s
                generator.Emit(OpCodes.Callvirt, property.GetGetMethod());
                generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
            }
            generator.Emit(OpCodes.Ret);
            return dymMethod.CreateDelegate(typeof(Action<T, T>));
        }
    }
}