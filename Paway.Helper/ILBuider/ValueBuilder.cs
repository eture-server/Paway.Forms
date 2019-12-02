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
            var dymMethod = new DynamicMethod(type.Name + nameof(ValueBuilder), typeof(object), new Type[] { typeof(object) }, true);
            ILGenerator generator = dymMethod.GetILGenerator();
            var property = type.Property(name);
            if (property == null) throw new ArgumentException(name + " Argument can not be empty");
            if (!property.CanRead) throw new ArgumentException("Cannot read value");
            {
                generator.GetValue(property, type);//获取引用值
            }
            generator.Emit(OpCodes.Ret);

            return dymMethod.CreateDelegate(typeof(Func<object, object>));
        }
        /// <summary>
        /// IL动态代码，创建委托（获取所有值）
        /// </summary>
        public static Delegate GetValuesFunc(Type type)
        {
            var dymMethod = new DynamicMethod(type.Name + nameof(ValueBuilder), typeof(object[]), new Type[] { typeof(object) }, true);
            ILGenerator generator = dymMethod.GetILGenerator();

            var properties = type.PropertiesValue();
            LocalBuilder result = generator.DeclareLocal(typeof(object[]));
            generator.Emit(OpCodes.Ldc_I4, properties.Count);
            generator.Emit(OpCodes.Newobj, typeof(object[]).GetConstructor(new[] { typeof(int) }));
            generator.Emit(OpCodes.Stloc, result);
            int index = 0;
            foreach (var property in properties)
            {
                generator.Emit(OpCodes.Ldloc, result);
                generator.Emit(OpCodes.Ldc_I4, index);
                generator.GetValue(property, type);//获取引用值
                generator.Emit(OpCodes.Stelem_Ref);
                index++;
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            return dymMethod.CreateDelegate(typeof(Func<object, object[]>));
        }
        /// <summary>
        /// IL动态代码，创建委托（设置值）
        /// </summary>
        public static Delegate SetValueFunc(Type type, string name)
        {
            var dymMethod = new DynamicMethod(type.Name + nameof(ValueBuilder), null, new Type[] { typeof(object), typeof(object) }, true);
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
        /// <summary>
        /// IL动态代码，创建委托（获取类型）
        /// 无参数，可直接缓存。
        /// </summary>
        public static Delegate GetTypeFunc(Type type, string name)
        {
            var dymMethod = new DynamicMethod(type.Name + nameof(ValueBuilder), typeof(Type), new Type[] { }, true);
            ILGenerator generator = dymMethod.GetILGenerator();
            var property = type.Property(name);
            if (property == null) throw new ArgumentException(name + " Argument can not be empty");
            {
                generator.Emit(OpCodes.Ldtoken, property.PropertyType);
                generator.Emit(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle), new Type[] { typeof(RuntimeTypeHandle) }));
            }
            generator.Emit(OpCodes.Ret);

            return dymMethod.CreateDelegate(typeof(Func<Type>));
        }
    }
}