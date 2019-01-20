using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    ///     将一个基本数据类型转换为另一个基本数据类型。
    /// </summary>
    public static class ConverHelper
    {
        #region 关于异常

        /// <summary>
        ///     获取异常中的所有描述
        /// </summary>
        public static string InnerMessage(this Exception ex)
        {
            var msg = ex.Message;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                if (!string.IsNullOrEmpty(ex.Message))
                    msg = string.Format("{0}\r\n{1}", msg, ex.Message);
            }
            return msg;
        }

        #endregion

        #region 中文转拼音
        /// <summary>
        /// 在指定的字符串列表cnStr中检索符合拼音索引字符串
        /// </summary>
        /// <param name="cnStr">汉字字符串</param>
        /// <param name="args">换行字符</param>
        /// <returns>相对应的汉语拼音首字母串</returns>
        public static string ToSpellCode(this string cnStr, params string[] args)
        {
            if (cnStr.IsNullOrEmpty()) return null;
            string strTemp = null;
            bool last = true;
            for (int i = 0; i < cnStr.Length; i++)
            {
                string v = null;
                bool c = GetCharSpellCode(cnStr.Substring(i, 1), ref v);
                if ((c || last) && !args.Contains(v))
                {
                    strTemp += v;
                }
                last = c | args.Contains(v);
            }
            return strTemp;
        }
        /// <summary>
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回小写字母
        /// </summary>
        /// <param name="cnChar">单个汉字</param>
        /// <param name="result">单个小写字母</param>
        /// <returns>如果是字母，返回false</returns>
        private static bool GetCharSpellCode(string cnChar, ref string result)
        {
            long lenCnChar;
            byte[] zw = Encoding.Default.GetBytes(cnChar);
            //如果是字母，则直接返回
            if (zw.Length == 1)
            {
                result = cnChar.ToLower();
                return false;
            }
            else
            {
                // get the array of byte from the single char
                int i1 = zw[0];
                int i2 = (zw[1]);
                lenCnChar = i1 * 256 + i2;
            }
            // iCnChar match the constant
            if (lenCnChar >= 45217 && lenCnChar <= 55289)
            {
                if (lenCnChar <= 45252)
                {
                    result = "a";
                }
                else if (lenCnChar <= 45760)
                {
                    result = "b";
                }
                else if (lenCnChar <= 46317)
                {
                    result = "c";
                }
                else if (lenCnChar <= 46825)
                {
                    result = "d";
                }
                else if (lenCnChar <= 47009)
                {
                    result = "e";
                }
                else if (lenCnChar <= 47296)
                {
                    result = "f";
                }
                else if (lenCnChar <= 47613)
                {
                    result = "g";
                }
                else if (lenCnChar <= 48118)
                {
                    result = "h";
                }
                else if (lenCnChar <= 49061)
                {
                    result = "j";
                }
                else if (lenCnChar <= 49323)
                {
                    result = "k";
                }
                else if (lenCnChar <= 49895)
                {
                    result = "l";
                }
                else if (lenCnChar <= 50370)
                {
                    result = "m";
                }
                else if (lenCnChar <= 50613)
                {
                    result = "n";
                }
                else if (lenCnChar <= 50621)
                {
                    result = "o";
                }
                else if (lenCnChar <= 50905)
                {
                    result = "p";
                }
                else if (lenCnChar <= 51386)
                {
                    result = "q";
                }
                else if (lenCnChar <= 51445)
                {
                    result = "r";
                }
                else if (lenCnChar <= 52217)
                {
                    result = "s";
                }
                else if (lenCnChar <= 52697)
                {
                    result = "t";
                }
                else if (lenCnChar <= 52979)
                {
                    result = "w";
                }
                else if (lenCnChar <= 53640)
                {
                    result = "x";
                }
                else if (lenCnChar <= 54480)
                {
                    result = "y";
                }
                else if (lenCnChar >= 54481)
                {
                    result = "z";
                }
            }
            else
            {
                result = ("?");
            }
            return true;
        }

        #endregion

        #region 数据转换
        /// <summary>
        ///     Int32转换
        /// </summary>
        public static int ToInt(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;

            if (obj.ToString().ToUpper() == "TRUE")
                return 1;
            if (obj.ToString().ToUpper() == "FALSE")
                return 0;
            if (int.TryParse(obj.ToString(), out int data))
            {
                return data;
            }

            if (double.TryParse(obj.ToString(), out double result))
            {
                result = Math.Round(result, MidpointRounding.AwayFromZero);
                if (int.TryParse(result.ToString(), out data))
                {
                    return data;
                }
            }
            return 0;
        }
        /// <summary>
        ///     Long转换
        /// </summary>
        public static long ToLong(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;

            if (long.TryParse(obj.ToString(), out long value))
                return value;
            return 0;
        }
        /// <summary>
        ///     Double转换
        /// </summary>
        public static double ToDouble(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;

            if (double.TryParse(obj.ToString(), out double value))
                return value;
            return 0;
        }
        /// <summary>
        ///     float
        /// </summary>
        public static float ToFloat(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;

            if (float.TryParse(obj.ToString(), out float value))
                return value;
            return 0;
        }
        /// <summary>
        ///     Bool转换
        /// </summary>
        public static bool ToBool(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return false;

            if (obj.ToString() == "1")
                return true;
            if (obj.ToString() == "0")
                return false;
            if (bool.TryParse(obj.ToString(), out bool value))
                return value;
            return false;
        }
        /// <summary>
        ///     String转换
        /// </summary>
        public static string ToString2(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return string.Empty;

            return obj.ToString().Trim();
        }
        /// <summary>
        ///     DateTime转换
        /// </summary>
        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == string.Empty)
                return new DateTime();

            return Convert.ToDateTime(obj);
        }
        /// <summary>
        ///     检测obj,如果为DBNUll或空字符串 返回true
        /// </summary>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return true;
            }
            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        ///     拼音转换
        /// </summary>
        public static string ToSpell(this object obj)
        {
            string text = obj.ToString2();
            string str = "";
            for (int i = 0; i < text.Length; i++)
            {
                //取单个汉字
                str += GetSpell(text.Substring(i, 1));
            }
            return str;
        }
        /// <summary>
        /// 根据一个汉字获得其首拼音
        /// </summary>
        private static string GetSpell(string chart)
        {
            //获得其ASSIC码
            byte[] arrCN = Encoding.Default.GetBytes(chart);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];

                //int code = (area << 8) + pos;
                int code = area * 256 + pos;
                //0~65535
                //a~Z的数字表示
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297,
                                     47614, 48119, 48119, 49062, 49324, 49896, 50371,
                                     50614, 50622, 50906, 51387, 51446, 52218,
                                     52698, 52698, 52698, 52980, 53689, 54481 };
                //判断其值在那2个数字之间
                for (int i = 0; i < 26; i++)
                {
                    //最后一个汉字的值
                    int max = 55290;
                    if (i != 25)
                        max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        //转为字母返回
                        return Encoding.Default.GetString(new byte[] { (byte)(97 + i) });
                    }
                }
                return "*";
            }
            else return chart;
        }

        /// <summary>
        /// 消除算术计算误差(double转decimal)
        /// </summary>
        public static double ClearError(this double value)
        {
            try
            {
                return Decimal.ToDouble(new Decimal(value));
            }
            catch
            {
                return value;
            }
        }

        #endregion

        #region IList 与　DataTable 互转
        /// <summary>
        /// 获取接口所有属性
        /// PropertyInfo.SetValue不能设置只读接口
        /// </summary>
        public static List<PropertyInfo> Properties(this Type type)
        {
            var list = new List<Type>();
            list.Add(type);
            if (type.IsInterface)
            {
                list.AddRange(type.GetInterfaces());
            }
            var pList = new List<PropertyInfo>();
            for (int i = 0; i < list.Count; i++)
            {
                pList.AddRange(list[i].GetProperties());
            }
            return pList;
        }
        /// <summary>
        /// 获取接口所有属性
        /// </summary>
        public static List<PropertyDescriptor> Descriptors(this Type type)
        {
            var list = new List<Type>();
            list.Add(type);
            if (type.IsInterface)
            {
                list.AddRange(type.GetInterfaces());
            }
            var pList = new List<PropertyDescriptor>();
            for (int i = 0; i < list.Count; i++)
            {
                foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(list[i]))
                    pList.Add(item);
            }
            return pList;
        }


        /// <summary>
        /// 多行加到新的DataTable
        /// </summary>
        public static DataTable ToDataTable(this DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            // 复制DataRow的表结构
            DataTable table = rows[0].Table.Clone();
            foreach (DataRow row in rows)
            {
                // 将DataRow添加到DataTable中
                table.ImportRow(row);
            }
            return table;
        }
        /// <summary>
        ///     将指定类型转为DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateTable(this Type type)
        {
            var table = new DataTable(type.Name);
            var properties = type.Properties();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType) continue;
                if (!property.ITable(out string name)) continue;
                table.Columns.Add(name, property.PropertyType);
            }
            return table;
        }
        /// <summary>
        ///     更新表列名
        ///     实体类中列名与表名一一对应，无则Excel=false
        /// </summary>
        public static void UpdateColumn<T>(DataTable dt)
        {
            var type = typeof(T);
            var index = 0;
            var properties = type.Properties();
            foreach (var property in properties)
            {
                if (property.ITable(out string name))
                {
                    dt.Columns[index++].ColumnName = name;
                }
            }
        }
        /// <summary>
        ///     IList转为 Excel标记的DataTable
        /// </summary>
        public static DataTable ToExcelTable<T>(this List<T> list)
        {
            return typeof(T).ToExcelTable(list);
        }
        /// <summary>
        ///     IList转为 Excel标记的DataTable
        /// </summary>
        public static DataTable ToExcelTable(this Type type, IList list)
        {
            var table = type.CreateExcelTable();
            var properties = type.Properties();
            var descriptors = type.Descriptors();
            for (int i = 0; i < list.Count; i++)
            {
                table.Rows.Add(table.NewRow());
            }
            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType) continue;
                if (!property.IExcel(out string name)) continue;
                var descriptor = descriptors.Find(c => c.Name == property.Name);
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    var value = descriptor.GetValue(list[j]);
                    lock (table)
                        table.Rows[j][name] = value;
                }
            }
            return table;
        }
        /// <summary>
        ///     将指定类型转为DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateExcelTable(this Type type)
        {
            var table = new DataTable(type.Name);
            var properties = type.Properties();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType) continue;
                if (!property.IExcel(out string name)) continue;
                table.Columns.Add(name, property.PropertyType);
            }
            return table;
        }
        /// <summary>
        ///     IList转为 DataTable
        /// </summary>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            return typeof(T).ToDataTable(list);
        }
        /// <summary>
        ///     IList转为 DataTable
        /// </summary>
        public static DataTable ToDataTable(this Type type, IList list)
        {
            var table = type.CreateTable();
            var properties = type.Properties();
            var descriptors = type.Descriptors();
            for (int i = 0; i < list.Count; i++)
            {
                table.Rows.Add(table.NewRow());
            }
            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType) continue;
                if (!property.ITable(out string name)) continue;
                var descriptor = descriptors.Find(c => c.Name == property.Name);
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    var value = descriptor.GetValue(list[j]);
                    lock (table)
                        table.Rows[j][name] = value;
                }
            }
            return table;
        }
        /// <summary>
        ///     IList转为 DataTable
        ///     指定列名的值
        /// </summary>
        public static DataTable ToDataTable(this Type type, IList list, string name, object value)
        {
            var table = type.CreateTable();
            var descriptors = type.Descriptors();
            var descriptor = descriptors.Find(c => c.Name == name);
            for (int i = 0; i < list.Count; i++)
            {
                if (descriptor.GetValue(list[i]).Equals(value))
                {
                    table.Rows.Add(type.CreateItem(list[i], table));
                }
            }
            return table;
        }
        /// <summary>
        ///     从类创建行
        /// </summary>
        public static DataRow CreateItem<T>(this Type type, T t, DataTable table = null)
        {
            if (table == null)
                table = type.CreateTable();
            DataRow row = table.NewRow();
            var properties = type.Properties();
            var descriptors = type.Descriptors();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType) continue;
                if (!property.ITable(out string name)) continue;
                var descriptor = descriptors.Find(c => c.Name == property.Name);
                row[name] = descriptor.GetValue(t);
            }
            return row;
        }

        /// <summary>
        ///     将DataTable转为IList
        /// </summary>
        public static List<T> ToList<T>(this DataTable table, int count = int.MaxValue)
        {
            if (table == null)
            {
                return null;
            }
            if (count > table.Rows.Count) count = table.Rows.Count;
            List<T> list = new List<T>();
            var type = typeof(T);
            var properties = type.Properties();
            var descriptors = type.Descriptors();
            for (int i = 0; i < count; i++)
            {
                list.Add(Activator.CreateInstance<T>());
            }
            foreach (var property in properties)
            {
                if (!property.ITable(out string name)) continue;

                var descriptor = descriptors.Find(c => c.Name == property.Name);
                foreach (DataColumn column in table.Columns)
                {
                    if (name != column.ColumnName) continue;
                    for (var j = 0; j < list.Count; j++)
                    {
                        var value = table.Rows[j][column.ColumnName];
                        list[j].SetValue(descriptor, value);
                    }
                    break;
                }
            }
            return list;
        }

        /// <summary>
        ///     从行创建类
        /// </summary>
        public static T CreateItem<T>(this DataRow row)
        {
            var type = typeof(T);
            var obj = Activator.CreateInstance<T>(); //string 类型不支持无参的反射
            if (row == null) return obj;
            var properties = type.Properties();
            var descriptors = type.Descriptors();
            foreach (var property in properties)
            {
                if (!property.ITable(out string name)) continue;

                var descriptor = descriptors.Find(c => c.Name == property.Name);
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (name != column.ColumnName) continue;
                    var value = row[column.ColumnName];
                    obj.SetValue(descriptor, value);
                    break;
                }
            }

            return obj;
        }

        /// <summary>
        /// 赋值
        /// </summary>
        public static void SetValue<T>(this T obj, PropertyDescriptor pro, object value)
        {
            if (value == null || value == DBNull.Value)
            {
                pro.SetValue(obj, null);
            }
            else if (pro.PropertyType == typeof(Image) && value is byte[])
            {
                pro.SetValue(obj, SctructHelper.BytesToImage(value as byte[]));
            }
            else if (pro.PropertyType == typeof(int) || pro.PropertyType == typeof(int?))
            {
                pro.SetValue(obj, value.ToInt());
            }
            else if (pro.PropertyType == typeof(long) || pro.PropertyType == typeof(long?))
            {
                pro.SetValue(obj, value.ToLong());
            }
            else if (pro.PropertyType == typeof(double) || pro.PropertyType == typeof(double?))
            {
                pro.SetValue(obj, value.ToDouble());
            }
            else if (pro.PropertyType == typeof(float) || pro.PropertyType == typeof(float?))
            {
                pro.SetValue(obj, value.ToFloat());
            }
            else if (pro.PropertyType == typeof(bool) || pro.PropertyType == typeof(bool?))
            {
                pro.SetValue(obj, value.ToBool());
            }
            else if (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?))
            {
                var time = value.ToDateTime();
                if (TConfig.IUtcTime && time.Kind != DateTimeKind.Local)
                {
                    time = TimeZoneInfo.ConvertTimeFromUtc(time, TimeZoneInfo.Local);
                }
                pro.SetValue(obj, time);
            }
            else if (pro.PropertyType == typeof(string))
            {
                pro.SetValue(obj, value.ToString2());
            }
            else if (pro.PropertyType.IsEnum)
            {
                Type type = pro.PropertyType.GetEnumUnderlyingType();
                if (type == typeof(byte) || type == typeof(sbyte) ||
                    type == typeof(short) || type == typeof(ushort) ||
                    type == typeof(int) || type == typeof(uint))
                {
                    pro.SetValue(obj, value.ToInt());
                }
                else if (type == typeof(long) || type == typeof(ulong))
                {
                    pro.SetValue(obj, value.ToLong());
                }
            }
            else
            {
                pro.SetValue(obj, value);
            }
        }

        #endregion

        #region 特性
        /// <summary>
        /// 获取表名
        /// </summary>
        public static string TableName(this Type type)
        {
            return type.Table().Table;
        }
        /// <summary>
        /// 获取主键
        /// </summary>
        public static string TableKey(this Type type)
        {
            var attr = type.Table();
            return attr.Keys;
        }

        /// <summary>
        ///     返回特性，检查表名及主键或主列
        /// </summary>
        public static PropertyAttribute Table(this Type type)
        {
            var attr = AttrTable(type);
            if (attr.Keys == null) throw new ArgumentException("没有指定主键或主列名称");
            return attr;
        }
        /// <summary>
        ///     返回特性，检查表名
        /// </summary>
        public static PropertyAttribute AttrTable(Type type)
        {
            var attrList = type.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (attrList.Length != 1) throw new ArgumentException(string.Format("类型 {0} 特性错误", type));
            if (attrList[0].Table == null) throw new ArgumentException("没有指定表名称");
            return attrList[0];
        }
        /// <summary>
        ///     获取描述
        /// </summary>
        public static string Description(this MemberInfo type)
        {
            var list = type.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (list.Length > 0)
                return ((DescriptionAttribute)list[0]).Description;
            return null;
        }
        /// <summary>
        /// 生成数据列标记
        /// </summary>
        public static bool ISelect(this MemberInfo pro, out string column)
        {
            column = pro.Name;
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (list.Length == 1 && list[0].Column != null)
            {
                column = list[0].Column;
            }
            return list.Length == 0 || list[0].ISelect;
        }
        /// <summary>
        /// 生成Table标记
        /// </summary>
        public static bool ITable(this MemberInfo pro, out string column)
        {
            column = pro.Name;
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (list.Length == 1 && list[0].Column != null)
            {
                column = list[0].Column;
            }
            return list.Length == 0 || list[0].ITable;
        }
        /// <summary>
        /// 生成到ExcelTable标记
        /// </summary>
        public static bool IExcel(this MemberInfo pro, out string column)
        {
            column = pro.Name;
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (list.Length == 1 && list[0].Column != null)
            {
                column = list[0].Column;
            }
            return list.Length == 0 || list[0].IExcel;
        }
        /// <summary>
        /// 获取列名
        /// </summary>
        public static string Column(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (list.Length == 1 && list[0].Column != null)
            {
                return list[0].Column;
            }
            return pro.Name;
        }
        /// <summary>
        /// 显示列标记
        /// </summary>
        public static bool IShow(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            return list.Length == 0 || list[0].IShow;
        }
        /// <summary>
        /// 显示属性
        /// </summary>
        public static bool IBrowsable(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(BrowsableAttribute), false) as BrowsableAttribute[];
            return list.Length == 0 || list[0].Browsable;
        }
        /// <summary>
        /// 自定义排序列标记
        /// </summary>
        public static bool ISort(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            return list.Length == 1 && list[0].ISort;
        }
        private static bool IClone(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            return list.Length == 0 || list[0].IClone;
        }

        #endregion

        #region Clone
        /// <summary>
        ///     返回泛型实参数类型
        /// </summary>
        public static Type GenericType(this object obj)
        {
            var types = obj.GetType().GetGenericArguments();
            if (types.Length == 1) return types[0];
            return null;
        }

        /// <summary>
        ///     返回泛型实参实例
        /// </summary>
        public static IList CreateList(this Type type)
        {
            var listType = typeof(List<>);
            listType = listType.MakeGenericType(type);
            var list = Activator.CreateInstance(listType) as IList;
            return list;
        }

        /// <summary>
        ///     一般复制
        /// </summary>
        public static T Clone<T>(this T t, T copy = default(T))
        {
            return t.Clone(false, copy);
        }

        /// <summary>
        ///     深度复制：引用、IList子级。
        ///     不要使用嵌套参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="child">是否深度复制 引用、IList子级</param>
        /// <param name="copy"></param>
        /// <returns></returns>
        public static T Clone<T>(this T t, bool child, object copy = null)
        {
            var type = typeof(T);
            var asmb = Assembly.GetAssembly(type);
            if (copy == null)
                copy = asmb.CreateInstance(type.FullName);
            type.Clone(ref copy, t, child);

            return (T)copy;
        }

        /// <summary>
        ///     复制子级
        /// </summary>
        public static void Clone(this Type parent, ref object copy, object t, bool child)
        {
            var properties = parent.Properties();
            var descriptors = parent.Descriptors();
            foreach (var property in properties)
            {
                if (!property.IClone()) continue;

                var descriptor = descriptors.Find(c => c.Name == property.Name);
                var value = descriptor.GetValue(t);
                descriptor.SetValue(copy, value);
                if (!child) continue;
                if (value is IList)
                {
                    var clist = descriptor.GetValue(copy) as IList;
                    var list = value as IList;
                    var type = list.GenericType();
                    var asmb = Assembly.GetAssembly(type);
                    for (var j = 0; j < list.Count; j++)
                    {
                        if (!type.IsValueType && type != typeof(string))
                        {
                            var obj = asmb.CreateInstance(type.FullName);
                            type.Clone(ref obj, list[j], child);
                            clist.Add(obj);
                        }
                        else
                        {
                            clist.Add(list[j]);
                        }
                    }
                }
                else if (value != null && !value.GetType().IsValueType && value.GetType() != typeof(string))
                {
                    var type = value.GetType();
                    var asmb = Assembly.GetAssembly(type);
                    var obj = asmb.CreateInstance(type.FullName);
                    type.Clone(ref obj, value, child);
                }
            }
        }

        /// <summary>
        ///     从实体类复制到DataRow
        ///     是否同步复制开关
        /// </summary>
        public static void Clone<T>(this DataRow row, DataRow copy, bool aysc = true)
        {
            var type = typeof(T);
            var properties = type.Properties();
            foreach (var property in properties)
            {
                if (aysc && !property.IClone()) continue;
                copy[property.Name] = row[property.Name];
            }
        }

        #endregion

        #region 在窗体上固定控件位置

        private static List<LocateInfo> tList;
        private static Size normal = Size.Empty;

        #region private class

        /// <summary>
        ///     定位属性
        /// </summary>
        private class LocateInfo
        {
            /// <summary>
            ///     构造
            /// </summary>
            public LocateInfo(Control control, Point point, StringAlignment xLocation, StringAlignment yLocation)
            {
                Control = control;
                Point = point;
                XLocation = xLocation;
                YLocation = yLocation;
            }

            /// <summary>
            ///     控件
            /// </summary>
            public Control Control { get; set; }

            /// <summary>
            ///     原坐标
            /// </summary>
            public Point Point { get; set; }

            /// <summary>
            ///     x点对齐方式
            /// </summary>
            public StringAlignment XLocation { get; set; }

            /// <summary>
            ///     y点对齐方式
            /// </summary>
            public StringAlignment YLocation { get; set; }
        }

        #endregion

        /// <summary>
        ///     在窗体上固定控件位置
        /// </summary>
        public static void AddLocate(this ContainerControl form, Control control)
        {
            form.AddLocate(control, StringAlignment.Center, StringAlignment.Center);
        }

        /// <summary>
        ///     在窗体上固定控件位置
        /// </summary>
        public static void AddLocate(this ContainerControl form, Control control, StringAlignment lLocation)
        {
            form.AddLocate(control, lLocation, lLocation);
        }

        /// <summary>
        ///     在窗体上固定控件位置
        /// </summary>
        public static void AddLocate(this ContainerControl form, Control control, StringAlignment xLocation,
            StringAlignment yLocation)
        {
            form.SizeChanged -= Form_SizeChanged;
            form.SizeChanged += Form_SizeChanged;
            if (normal == Size.Empty)
            {
                normal = form.Size;
            }
            if (tList == null)
            {
                tList = new List<LocateInfo>();
            }
            tList.Add(new LocateInfo(control, control.Location, xLocation, yLocation));
        }

        private static void Form_SizeChanged(object sender, EventArgs e)
        {
            if (tList == null) return;
            var form = sender as ContainerControl;
            for (var i = 0; i < tList.Count; i++)
            {
                var left = 0;
                var top = 0;
                switch (tList[i].XLocation)
                {
                    case StringAlignment.Near:
                        left = 0;
                        break;
                    case StringAlignment.Center:
                        left = tList[i].Control.Width / 2;
                        break;
                    case StringAlignment.Far:
                        left = tList[i].Control.Width;
                        break;
                }
                switch (tList[i].YLocation)
                {
                    case StringAlignment.Near:
                        top = 0;
                        break;
                    case StringAlignment.Center:
                        top = tList[i].Control.Height / 2;
                        break;
                    case StringAlignment.Far:
                        top = tList[i].Control.Height;
                        break;
                }
                var x = form.Width * (tList[i].Point.X + left) / normal.Width;
                var y = form.Height * (tList[i].Point.Y + top) / normal.Height;
                tList[i].Control.Location = new Point(x - left, y - top);
            }
        }

        #endregion

        #region 含数字的字符串比较数字
        /// <summary>
        /// 含数字的字符串比较数字
        /// </summary>
        public static int TCompare(this object obj1, object obj2)
        {
            string x = obj1 as string;
            string y = obj2 as string;
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            char[] arr1 = x.ToCharArray();
            char[] arr2 = y.ToCharArray();
            int i = 0, j = 0;
            while (i < arr1.Length && j < arr2.Length)
            {
                if (char.IsDigit(arr1[i]) && char.IsDigit(arr2[j]))
                {
                    string s1 = "", s2 = "";
                    while (i < arr1.Length && char.IsDigit(arr1[i]))
                    {
                        s1 += arr1[i];
                        i++;
                    }
                    while (j < arr2.Length && char.IsDigit(arr2[j]))
                    {
                        s2 += arr2[j];
                        j++;
                    }
                    if (long.Parse(s1) > long.Parse(s2))
                    {
                        return 1;
                    }
                    if (long.Parse(s1) < long.Parse(s2))
                    {
                        return -1;
                    }
                }
                else
                {
                    if (arr1[i] > arr2[j])
                    {
                        return 1;
                    }
                    if (arr1[i] < arr2[j])
                    {
                        return -1;
                    }
                    i++;
                    j++;
                }
            }
            if (arr1.Length == arr2.Length)
            {
                return 0;
            }
            else
            {
                return arr1.Length > arr2.Length ? 1 : -1;
            }
        }
        /// <summary>
        /// </summary>
        public static int TCompare(this PropertyDescriptor pro, object obj1, object obj2)
        {
            if ((obj1 == null || obj1 == DBNull.Value) && (obj2 == null || obj2 == DBNull.Value)) return 0;
            if (obj1 == null || obj1 == DBNull.Value) return -1;
            if (obj2 == null || obj2 == DBNull.Value) return 1;
            if (pro.PropertyType == typeof(Image))
            {
                return 0;
            }
            else if (pro.PropertyType == typeof(int) || pro.PropertyType == typeof(int?))
            {
                return ((int)obj1).CompareTo((int)obj2);
            }
            else if (pro.PropertyType == typeof(long) || pro.PropertyType == typeof(long?))
            {
                return ((long)obj1).CompareTo((long)obj2);
            }
            else if (pro.PropertyType == typeof(double) || pro.PropertyType == typeof(double?))
            {
                return ((double)obj1).CompareTo((double)obj2);
            }
            else if (pro.PropertyType == typeof(float) || pro.PropertyType == typeof(float?))
            {
                return ((float)obj1).CompareTo((float)obj2);
            }
            else if (pro.PropertyType == typeof(bool) || pro.PropertyType == typeof(bool?))
            {
                return ((bool)obj1).CompareTo((bool)obj2);
            }
            else if (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?))
            {
                return (obj1.ToDateTime()).CompareTo(obj2.ToDateTime());
            }
            else if (pro.PropertyType == typeof(string))
            {
                return obj1.TCompare(obj2);
            }
            else if (pro.PropertyType.IsEnum)
            {
                Type type = pro.PropertyType.GetEnumUnderlyingType();
                if (type == typeof(byte))
                {
                    return ((byte)obj1).CompareTo((byte)obj2);
                }
                else if (type == typeof(sbyte))
                {
                    return ((sbyte)obj1).CompareTo((sbyte)obj2);
                }
                else if (type == typeof(short))
                {
                    return ((short)obj1).CompareTo((short)obj2);
                }
                else if (type == typeof(ushort))
                {
                    return ((ushort)obj1).CompareTo((ushort)obj2);
                }
                else if (type == typeof(int))
                {
                    return ((int)obj1).CompareTo((int)obj2);
                }
                else if (type == typeof(uint))
                {
                    return ((uint)obj1).CompareTo((uint)obj2);
                }
                else if (type == typeof(long))
                {
                    return ((long)obj1).CompareTo((long)obj2);
                }
                else if (type == typeof(ulong))
                {
                    return ((ulong)obj1).CompareTo((ulong)obj2);
                }
            }
            return 0;
        }

        #endregion
    }
}