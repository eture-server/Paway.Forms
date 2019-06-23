using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace Paway.Helper
{
    /// <summary>
    /// IL动态代码(Emit)，实体值
    /// </summary>
    internal abstract class ValueBuilder
    {
        /// <summary>
        /// IL动态代码，创建委托（获取值）
        /// </summary>
        public static Delegate GetValueFunc(Type type, string name)
        {
            var dymMethod = new DynamicMethod(type.Name + "GetValueBuilder", typeof(object), new Type[] { typeof(object) }, type, true);
            ILGenerator generator = dymMethod.GetILGenerator();
            var property = type.Property(name);
            if (property != null && property.CanRead)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Castclass, type);//类型转化
                generator.Emit(OpCodes.Callvirt, property.GetGetMethod());//获取值
                generator.Box(property);//值数据转引用数据
            }
            generator.Emit(OpCodes.Ret);

            return dymMethod.CreateDelegate(typeof(Func<object, object>));
        }
        /// <summary>
        /// IL动态代码，创建委托（设置值）
        /// </summary>
        public static Delegate SetValueFunc(Type type, string name)
        {
            var dymMethod = new DynamicMethod(type.Name + "SetValueBuilder", null, new Type[] { typeof(object), typeof(object) }, type, true);
            ILGenerator generator = dymMethod.GetILGenerator();
            var property = type.Property(name);
            if (property != null && property.CanWrite)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Castclass, type);//类型转化
                generator.Emit(OpCodes.Ldarg_1);
                generator.UnBox(property);//引用转值
                generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
            }
            generator.Emit(OpCodes.Ret);

            return dymMethod.CreateDelegate(typeof(Action<object, object>));
        }
    }
}