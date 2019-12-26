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
    /// IL动态代码(Emit)，复制（实体及列表），支持深度复制
    /// </summary>
    internal abstract class CloneBuilder
    {
        /// <summary>
        /// IL动态代码(Emit)，复制（实体及列表）
        /// </summary>
        public static Delegate CloneFunc(Type type, bool depth)
        {
            // Create ILGenerator
            var dymMethod = new DynamicMethod(type.Name + nameof(CloneBuilder), typeof(object), new Type[] { typeof(object) }, true);
            ILGenerator generator = dymMethod.GetILGenerator();

            LocalBuilder result = generator.DeclareLocal(type);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            foreach (var property in type.PropertiesCache().FindAll(c => c.CanRead))
            {
                if (!property.IClone()) continue;
                Type dbType = property.PropertyType;

                if ((!dbType.IsValueType && dbType.IsGenericType) ||
                    (dbType.IsClass && dbType != typeof(string) && dbType != typeof(byte[]) && dbType != typeof(Image) && dbType != typeof(Bitmap)))
                {
                    if (!depth) continue;
                    if (property.CanWrite) CloneFuncWrite(type, generator, property);
                    else CloneFuncRead(type, generator, property);
                }
                else if (property.CanWrite)
                {
                    // Load the new object on the eval stack... (currently 1 item on eval stack)
                    generator.Emit(OpCodes.Ldloc_0);
                    // Load initial object (parameter)          (currently 2 items on eval stack)
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
                    generator.Emit(OpCodes.Callvirt, property.GetGetMethod());
                    generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
                }
            }

            // Load new constructed obj on eval stack -> 1 item on stack
            generator.Emit(OpCodes.Ldloc_0, result);
            // Return constructed object.   --> 0 items on stack
            generator.Emit(OpCodes.Ret);
            return dymMethod.CreateDelegate(typeof(Func<object, object>));
        }
        /// <summary>
        /// 只读
        /// </summary>
        private static void CloneFuncRead(Type type, ILGenerator generator, PropertyInfo property)
        {
            var method = typeof(BuilderHelper).GetMethod(nameof(BuilderHelper.CloneObject),
                TConfig.Flags, null, new Type[] { typeof(object), typeof(object), typeof(bool) }, null);
            // Load initial object (parameter)          (currently 2 items on eval stack)
            generator.GetValue(property, type);//获取引用值

            // Load the new object on the eval stack... (currently 1 item on eval stack)
            generator.Emit(OpCodes.Ldloc_0);
            generator.Box(property);//值数据转引用数据

            {//参数
                generator.Emit(OpCodes.Ldc_I4, 1);
            }
            generator.Emit(OpCodes.Call, method);//调用静态方法
        }
        /// <summary>
        /// 可写
        /// </summary>
        private static void CloneFuncWrite(Type type, ILGenerator generator, PropertyInfo property)
        {
            var method = typeof(BuilderHelper).GetMethod(nameof(BuilderHelper.CloneObject),
                TConfig.Flags, null, new Type[] { typeof(object), typeof(bool) }, null);

            Label endIfLabel = generator.DefineLabel();
            generator.GetValue(property, type);//获取引用值

            //保存值到变量（变量需要主动创建后可用），以免重复获取
            LocalBuilder value = generator.DeclareLocal(property.PropertyType);
            generator.Emit(OpCodes.Stloc, value);
            generator.Emit(OpCodes.Ldloc, value);
            generator.Emit(OpCodes.Brfalse, endIfLabel);

            // Load the new object on the eval stack... (currently 1 item on eval stack)
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Ldloc, value);
            {//参数
                generator.Emit(OpCodes.Ldc_I4, 1);
            }
            generator.Emit(OpCodes.Call, method);//调用静态方法
            generator.UnBox(property);//值数据转引用数据
            generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
            generator.MarkLabel(endIfLabel);
        }
        /// <summary>
        /// IL动态代码(Emit)，复制（到已有实体及列表）
        /// </summary>
        public static Delegate CloneAction(Type type, bool depth)
        {
            // Create ILGenerator
            var dymMethod = new DynamicMethod(type.Name + nameof(CloneBuilder), null, new Type[] { typeof(object), typeof(object) }, true);
            ILGenerator generator = dymMethod.GetILGenerator();

            foreach (var property in type.PropertiesCache().FindAll(c => c.CanRead))
            {
                if (!property.IClone()) continue;
                Type dbType = property.PropertyType;

                if ((!dbType.IsValueType && dbType.IsGenericType) ||
                    (dbType.IsClass && dbType != typeof(string) && dbType != typeof(byte[]) && dbType != typeof(Image) && dbType != typeof(Bitmap)))
                {
                    if (!depth) continue;
                    if (property.CanWrite) CloneActionWrite(type, generator, property);
                    else if ((!dbType.IsValueType && dbType.IsGenericType)) CloneActionRead(type, generator, property);
                }
                else if (property.CanWrite)
                {
                    generator.Emit(OpCodes.Ldarg_1);// los
                    generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
                    generator.Emit(OpCodes.Ldarg_0);// s
                    generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
                    generator.Emit(OpCodes.Callvirt, property.GetGetMethod());
                    generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
                }
            }
            generator.Emit(OpCodes.Ret);
            return dymMethod.CreateDelegate(typeof(Action<object, object>));
        }
        /// <summary>
        /// 只读
        /// </summary>
        private static void CloneActionRead(Type type, ILGenerator generator, PropertyInfo property)
        {
            var method = typeof(BuilderHelper).GetMethod(nameof(BuilderHelper.CloneObject),
                TConfig.Flags, null, new Type[] { typeof(object), typeof(object), typeof(bool) }, null);

            generator.GetValue(property, type);//获取引用值

            generator.Emit(OpCodes.Ldarg_1);// los
            generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
            generator.Box(property);//值数据转引用数据

            {//参数
                generator.Emit(OpCodes.Ldc_I4, 1);
            }
            generator.Emit(OpCodes.Call, method);//调用静态方法
        }
        /// <summary>
        /// 可写
        /// </summary>
        private static void CloneActionWrite(Type type, ILGenerator generator, PropertyInfo property)
        {
            var method = typeof(BuilderHelper).GetMethod(nameof(BuilderHelper.CloneObject),
                TConfig.Flags, null, new Type[] { typeof(object), typeof(bool) }, null);

            Label endIfLabel = generator.DefineLabel();
            generator.GetValue(property, type);//获取引用值

            //保存值到变量（变量需要主动创建后可用），以免重复获取
            LocalBuilder value = generator.DeclareLocal(property.PropertyType);
            generator.Emit(OpCodes.Stloc, value);
            generator.Emit(OpCodes.Ldloc, value);
            generator.Emit(OpCodes.Brfalse, endIfLabel);

            generator.Emit(OpCodes.Ldarg_1);// los
            generator.Emit(OpCodes.Castclass, type);//未使用泛类，要转化为指定type类型
            generator.Emit(OpCodes.Ldloc, value);
            {//参数
                generator.Emit(OpCodes.Ldc_I4, 1);
            }
            generator.Emit(OpCodes.Call, method);//调用静态方法
            generator.UnBox(property);//值数据转引用数据
            generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
            generator.MarkLabel(endIfLabel);
        }
    }
}