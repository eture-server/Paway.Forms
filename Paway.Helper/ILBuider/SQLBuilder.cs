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
    /// IL动态代码(Emit)，SQL相关操作.创建参数
    /// </summary>
    public class SQLBuilder<T>
    {
        private Delegate handler;
        private SQLBuilder() { }
        /// <summary>
        /// 转化
        /// </summary>
        public List<DbParameter> Build(T t)
        {
            return ((Func<T, List<DbParameter>>)handler)(t);
        }
        /// <summary>
        /// IL动态代码，创建委托
        /// </summary>
        public static SQLBuilder<T> CreateBuilder(Type ptype, params string[] args)
        {
            var type = typeof(T);
            var valueType = typeof(List<DbParameter>);
            var key = type.TableKey();
            var addParameter = typeof(BuilderHelper).GetMethod("AddParameter", new Type[] { typeof(string), typeof(object), typeof(Type), typeof(Type) });
            var add = valueType.GetMethod("Add", new Type[] { typeof(DbParameter) });

            var dymMethod = new DynamicMethod(type.Name + "SQLBuilder", valueType, new Type[] { type }, type, true);
            ILGenerator generator = dymMethod.GetILGenerator();

            LocalBuilder result = generator.DeclareLocal(valueType);
            generator.Emit(OpCodes.Newobj, valueType.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);
            foreach (var property in type.GetProperties())
            {
                if (!property.ISelect(out string column)) continue;
                if (args.Length > 0 && key != column && args.FirstOrDefault(c => c == column) == null) continue;

                generator.Emit(OpCodes.Ldloc, result);
                {//参数
                    generator.Emit(OpCodes.Ldstr, column);
                }
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Callvirt, property.GetGetMethod());//获取
                //实体数据使用需要装箱
                if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                    generator.Emit(OpCodes.Box, property.PropertyType);//装箱                                                        
                else
                    generator.Emit(OpCodes.Castclass, property.PropertyType);
                generator.Emit(OpCodes.Ldtoken, property.PropertyType);
                generator.Emit(OpCodes.Ldtoken, ptype);
                generator.Emit(OpCodes.Call, addParameter);
                generator.Emit(OpCodes.Callvirt, add);
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            var builder = new SQLBuilder<T>
            {
                handler = dymMethod.CreateDelegate(typeof(Func<T, List<DbParameter>>))
            };
            return builder;
        }
    }
}