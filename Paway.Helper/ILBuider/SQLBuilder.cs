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
    public class SQLBuilder
    {
        private Delegate handler;
        private SQLBuilder() { }

        #region 创建参数
        /// <summary>
        /// 转化（创建参数）
        /// </summary>
        public List<DbParameter> Build(object t)
        {
            return ((Func<object, List<DbParameter>>)handler)(t);
        }
        /// <summary>
        /// IL动态代码，创建委托（创建参数）
        /// </summary>
        public static SQLBuilder CreateBuilder(Type type, Type ptype, params string[] args)
        {
            var valueType = typeof(List<DbParameter>);
            var key = type.TableKey();
            var addParameter = typeof(BuilderHelper).GetMethod(nameof(BuilderHelper.AddParameter), new Type[] { typeof(string), typeof(object), typeof(Type), typeof(Type) });
            var add = valueType.GetMethod(nameof(List<DbParameter>.Add), new Type[] { typeof(DbParameter) });

            var dymMethod = new DynamicMethod(type.Name + nameof(SQLBuilder), valueType, new Type[] { typeof(object) }, type, true);
            ILGenerator generator = dymMethod.GetILGenerator();

            LocalBuilder result = generator.DeclareLocal(valueType);
            generator.Emit(OpCodes.Newobj, valueType.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);
            foreach (var property in type.PropertiesValue())
            {
                if (!property.ISelect(out string column)) continue;
                if (args.Length > 0 && key != column &&
                    args.FirstOrDefault(c => c == column) == null &&
                    args.FirstOrDefault(c => c == property.Name) == null) continue;

                generator.Emit(OpCodes.Ldloc, result);
                {//参数
                    generator.Emit(OpCodes.Ldstr, column);
                }
                generator.GetValue(property, type);//获取引用值
                {//参数
                    generator.Emit(OpCodes.Ldtoken, property.PropertyType);
                    generator.Emit(OpCodes.Ldtoken, ptype);
                }
                generator.Emit(OpCodes.Call, addParameter);
                generator.Emit(OpCodes.Callvirt, add);
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            var builder = new SQLBuilder
            {
                handler = dymMethod.CreateDelegate(typeof(Func<object, List<DbParameter>>))
            };
            return builder;
        }

        #endregion
    }
}