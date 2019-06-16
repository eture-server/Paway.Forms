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
    ///     复制相关
    /// </summary>
    public static class CloneHelper
    {
        #region 动态生成代码Emit技术(IL生成)复制（不复制子级）
        private static Dictionary<Type, Delegate> CachedFunc { set; get; } = new Dictionary<Type, Delegate>();
        private static Dictionary<Type, Delegate> CachedAction { set; get; } = new Dictionary<Type, Delegate>();
        /// <summary>
        /// 动态生成代码Emit技术(IL生成)复制（不复制子级，复制属性字段等所有）
        /// </summary>
        public static T Clone<T>(this T t)
        {
            if (!CachedFunc.TryGetValue(typeof(T), out Delegate func))
            {
                func = GetFunc<T>();
                CachedFunc.Add(typeof(T), func);
            }
            return ((Func<T, T>)func)(t);
        }
        /// <summary>
        /// 动态生成代码Emit技术(IL生成)复制（不复制子级，仅复制公有属性）
        /// </summary>
        public static void Clone<T>(this T t, T copy)
        {
            if (!CachedAction.TryGetValue(typeof(T), out Delegate action))
            {
                action = GetAction<T>();
                CachedAction.Add(typeof(T), action);
            }
             ((Action<T, T>)action)(t, copy);
        }
        /// <summary>
        /// 生成动态代码，复制属性字段等所有
        /// </summary>
        private static Delegate GetFunc<T>()
        {
            var type = typeof(T);
            // Create ILGenerator
            DynamicMethod dymMethod = new DynamicMethod("DoClone", type, new Type[] { type }, true);
            ConstructorInfo cInfo = type.GetConstructor(new Type[] { });

            ILGenerator generator = dymMethod.GetILGenerator();

            LocalBuilder lbf = generator.DeclareLocal(type);
            //lbf.SetLocalSymInfo("_temp");

            generator.Emit(OpCodes.Newobj, cInfo);
            generator.Emit(OpCodes.Stloc_0);
            foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                Match match = new Regex(@"<(?<name>\w+)>").Match(field.Name);
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

            // Load new constructed obj on eval stack -> 1 item on stack
            generator.Emit(OpCodes.Ldloc_0);
            // Return constructed object.   --> 0 items on stack
            generator.Emit(OpCodes.Ret);

            return dymMethod.CreateDelegate(typeof(Func<T, T>));
        }
        /// <summary>
        /// 生成动态代码，仅复制公有属性
        /// </summary>
        private static Delegate GetAction<T>()
        {
            var type = typeof(T);
            // Create ILGenerator
            var dymMethod = new DynamicMethod("Clone", null, new[] { type, type });
            ILGenerator generator = dymMethod.GetILGenerator();
            foreach (var temp in type.Properties().FindAll(temp => temp.CanRead && temp.CanWrite))
            {
                //不复制静态类属性
                if (temp.GetAccessors(true)[0].IsStatic) continue;
                if (!temp.IClone()) continue;

                generator.Emit(OpCodes.Ldarg_1);// los
                generator.Emit(OpCodes.Ldarg_0);// s
                generator.Emit(OpCodes.Callvirt, temp.GetGetMethod());
                generator.Emit(OpCodes.Callvirt, temp.GetSetMethod());
            }
            generator.Emit(OpCodes.Ret);
            return dymMethod.CreateDelegate(typeof(Action<T, T>));
        }
        /// <summary>
        /// 表达式树，仅复制公有属性
        /// </summary>
        private static Func<T, T> GetFuncLambda<T>()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(T).GetProperties())
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

        #endregion

        #region 普通复制
        /// <summary>
        /// 深度复制：引用、IList列表（禁止复制链结构，会造成死循环）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="child">复制子级标记</param>
        /// <returns></returns>
        public static T Clone<T>(this T t, bool child)
        {
            var type = typeof(T);
            var asmb = Assembly.GetAssembly(type);
            var copy = asmb.CreateInstance(type.FullName);
            return t.Clone(copy, child);
        }
        /// <summary>
        /// 深度复制：引用、IList列表（禁止复制链结构，会造成死循环）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="copy">已有实体</param>
        /// <param name="child">复制子级标记</param>
        /// <returns></returns>
        public static T Clone<T>(this T t, object copy, bool child)
        {
            var type = typeof(T);
            var asmb = Assembly.GetAssembly(type);
            if (copy == null)
                copy = asmb.CreateInstance(type.FullName);
            type.Clone(copy, t, child);

            return (T)copy;
        }
        /// <summary>
        ///     复制子级
        /// </summary>
        private static void Clone(this Type parent, object copy, object t, bool child)
        {
            var properties = parent.Properties();
            var descriptors = parent.Descriptors();
            foreach (var property in properties)
            {
                if (!property.IClone()) continue;

                var descriptor = descriptors.Find(c => c.Name == property.Name);
                var value = descriptor.GetValue(t);
                descriptor.SetValue(copy, value);
                if (child && value is IList)
                {
                    var list = value as IList;
                    var type = list.GenericType();
                    var clist = type.CreateList();
                    descriptor.SetValue(copy, clist);
                    var asmb = Assembly.GetAssembly(type);
                    for (var j = 0; j < list.Count; j++)
                    {
                        if (!type.IsValueType && type != typeof(string))
                        {
                            var obj = asmb.CreateInstance(type.FullName);
                            type.Clone(obj, list[j], child);
                            clist.Add(obj);
                        }
                        else
                        {
                            clist.Add(list[j]);
                        }
                    }
                }
                else if (child && value != null && !value.GetType().IsValueType && value.GetType() != typeof(string) && !(value is Image))
                {
                    var type = value.GetType();
                    var asmb = Assembly.GetAssembly(type);
                    var obj = asmb.CreateInstance(type.FullName);
                    descriptor.SetValue(copy, obj);
                    type.Clone(obj, value, child);
                }
            }
        }

        #endregion
    }
}