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
        public static Delegate GetCloneFunc<T>()
        {
            var type = typeof(T);
            // Create ILGenerator
            var dymMethod = new DynamicMethod(type.Name + "CloneBuilder", type, new Type[] { type }, true);
            ILGenerator generator = dymMethod.GetILGenerator();

            LocalBuilder result = generator.DeclareLocal(type);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            GetCloneFunc(generator, type);
            while (type.BaseType != null)
            {//获取所有父类属性
                type = type.BaseType;
                GetCloneFunc(generator, type);
            }

            // Load new constructed obj on eval stack -> 1 item on stack
            generator.Emit(OpCodes.Ldloc, result);
            // Return constructed object.   --> 0 items on stack
            generator.Emit(OpCodes.Ret);
            return dymMethod.CreateDelegate(typeof(Func<T, T>));
        }
        /// <summary>
        /// 获取指定Type属性
        /// </summary>
        private static void GetCloneFunc(ILGenerator generator, Type type)
        {
            foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (field.FieldType.IsGenericType && Nullable.GetUnderlyingType(field.FieldType) == null) continue;
                var match = new Regex(@"<(?<name>\w+)>").Match(field.Name);
                if (match.Success)
                {
                    PropertyInfo property = type.GetProperty(match.Groups["name"].ToString());
                    if (property != null && !property.IClone()) continue;
                }

                // Load the new object on the eval stack... (currently 1 item on eval stack)
                generator.Emit(OpCodes.Ldloc_0);
                // Load initial object (parameter)          (currently 2 items on eval stack)
                generator.Emit(OpCodes.Ldarg_0);
                // Replace value by field value             (still currently 2 items on eval stack)
                generator.Emit(OpCodes.Ldfld, field);
                // Store the value of the top on the eval stack into the object underneath that value on the value stack.
                //  (0 items on eval stack)
                generator.Emit(OpCodes.Stfld, field);
            }
        }
        /// <summary>
        /// 生成动态代码，仅复制公有属性
        /// </summary>
        public static Delegate GetCloneAction<T>()
        {
            var type = typeof(T);
            // Create ILGenerator
            var dymMethod = new DynamicMethod(type.Name + "CloneBuilder", null, new Type[] { type, type });
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
        /// <summary>
        /// 表达式树，仅复制公有属性
        /// </summary>
        public static Func<T, T> GetCloneLambda<T>()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(T).Properties())
            {
                if (!item.CanWrite || !item.IClone()) continue;

                MemberExpression property = Expression.Property(parameterExpression, typeof(T).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(T)), memberBindingList.ToArray());
            Expression<Func<T, T>> lambda = Expression.Lambda<Func<T, T>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

            return lambda.Compile();
        }
    }
}