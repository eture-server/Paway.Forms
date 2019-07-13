using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
        /// <param name="str">汉字字符串</param>
        /// <param name="args">换行字符</param>
        /// <returns>相对应的汉语拼音首字母串</returns>
        public static string ToSpell(this string str, params string[] args)
        {
            if (str.IsNullOrEmpty()) return string.Empty;
            string strTemp = null;
            bool last = true;
            for (int i = 0; i < str.Length; i++)
            {
                string v = null;
                bool c = ToSpell(str.Substring(i, 1), ref v);
                if ((c || last) && !IContains(v, args))
                {
                    if (args.Length > 0) { }

                    strTemp += v;
                }
                last = c | IContains(v, args);
            }
            return strTemp.ToLower();
        }
        /// <summary>
        /// 判断非汉字时取值问题
        /// 参数为空时仅取连续字母或数字的首位
        /// </summary>
        private static bool IContains(string str, params string[] args)
        {
            if (args.Length > 0) return args.Contains(str);
            int c = str[0];
            if (c >= 65 && c <= 90) return false;//A-Z
            if (c >= 97 && c <= 122) return false;//a-z
            if (c >= 48 && c <= 57) return false;//0-9
            return true;
        }
        /// <summary>
        /// 根据一个汉字获得其首拼音
        /// </summary>
        private static bool ToSpell(string chart, ref string result)
        {
            //获得其ASSIC码
            byte[] arrCN = Encoding.Default.GetBytes(chart);
            if (arrCN.Length > 1)
            {
                int area = arrCN[0];
                int pos = arrCN[1];

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
                        result = Encoding.Default.GetString(new byte[] { (byte)(97 + i) });
                        return true;
                    }
                }
                result = "?";
            }
            else result = chart;
            return false;
        }

        #endregion

        #region 数据转换
        /// <summary>
        ///     Int32转换
        /// </summary>
        public static int ToInt(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;
            if (obj.GetType() == typeof(int)) return (int)obj;

            var value = obj.ToString();
            if (value.Equals("true", StringComparison.OrdinalIgnoreCase)) return 1;
            if (value.Equals("false", StringComparison.OrdinalIgnoreCase)) return 0;
            if (int.TryParse(value, out int data)) return data;

            if (double.TryParse(value, out double result))
            {
                result = Math.Round(result, MidpointRounding.AwayFromZero);
                return (int)result;
            }
            return 0;
        }
        /// <summary>
        ///     Long转换
        /// </summary>
        public static long ToLong(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;
            if (obj.GetType() == typeof(long)) return (long)obj;

            if (long.TryParse(obj.ToString(), out long value)) return value;
            return 0;
        }
        /// <summary>
        ///     Double转换
        /// </summary>
        public static double ToDouble(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            if (double.TryParse(obj.ToString(), out double value)) return value;
            return 0;
        }
        /// <summary>
        ///     float
        /// </summary>
        public static float ToFloat(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            if (float.TryParse(obj.ToString(), out float value)) return value;
            return 0;
        }
        /// <summary>
        ///     decimal
        /// </summary>
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            if (decimal.TryParse(obj.ToString(), out decimal value)) return value;
            return 0;
        }
        /// <summary>
        ///     Bool转换
        /// </summary>
        public static bool ToBool(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return false;

            var value = obj.ToString();
            if (value == "1") return true;
            if (value == "0") return false;
            if (bool.TryParse(value, out bool result)) return result;
            return false;
        }
        /// <summary>
        ///     String转换
        /// </summary>
        public static string ToStrs(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return string.Empty;

            return obj.ToString().Trim();
        }
        /// <summary>
        ///     DateTime转换
        /// </summary>
        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == string.Empty) return DateTime.MinValue;

            if (DateTime.TryParse(obj.ToString(), out DateTime result)) return result;
            return DateTime.MinValue;
        }
        /// <summary>
        ///     检测obj,如果为DBNUll或空字符串 返回true
        /// </summary>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return true;

            return string.IsNullOrEmpty(obj.ToString());
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
            catch (Exception ex)
            {
                log.Error(ex);
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
            var list = new List<Type> { type };
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
        /// 获取接口属性（可读、基础类型非空的泛型、值类型（含string、byte[]、Image、Bitmap））
        /// </summary>
        public static List<PropertyInfo> PropertiesValue(this Type type)
        {
            var properties = type.Properties();
            var vList = new List<PropertyInfo>();
            foreach (var property in properties)
            {
                if (!property.CanRead) continue;
                Type dbType = property.PropertyType;
                if (dbType.IsGenericType)
                {
                    if (Nullable.GetUnderlyingType(dbType) == null) continue;
                    dbType = Nullable.GetUnderlyingType(dbType);
                }
                if (dbType.IsClass && dbType != typeof(string) && dbType != typeof(byte[]) && dbType != typeof(Image) && dbType != typeof(Bitmap)) continue;
                vList.Add(property);
            }
            return vList;
        }
        /// <summary>
        /// 获取指定名称属性
        /// </summary>
        public static PropertyInfo Property(this Type type, string name)
        {
            var property = type.GetProperty(name);
            if (property == null)
            {
                var properties = type.Properties();
                property = properties.Find(c => c.Name == name);
                if (property == null) property = properties.Find(c => c.Column() == name);
            }
            return property;
        }
        /// <summary>
        /// 获取接口所有属性
        /// </summary>
        public static List<PropertyDescriptor> Descriptors(this Type type)
        {
            var list = new List<Type> { type };
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
        /// 将指定类型转为DataTable
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql">从数据取值时，Image存储格式为byte[]</param>
        /// <returns></returns>
        public static DataTable CreateTable(this Type type, bool sql = false)
        {
            var table = new DataTable(type.Name);
            foreach (var property in type.PropertiesValue())
            {
                Type dbType = property.PropertyType;
                if (dbType.IsGenericType) dbType = Nullable.GetUnderlyingType(dbType);
                if (sql && (dbType == typeof(Image) || dbType == typeof(Bitmap))) dbType = typeof(byte[]);
                table.Columns.Add(property.Column(), dbType);
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
            foreach (var property in type.PropertiesValue())
            {
                if (!property.IExcel()) continue;
                Type dbType = property.PropertyType;
                if (dbType.IsGenericType) dbType = Nullable.GetUnderlyingType(dbType);
                table.Columns.Add(property.Name, dbType);
            }
            return table;
        }

        /// <summary>
        /// 赋值（string数据转指定类型）
        /// </summary>
        public static void SetValue<T>(this T obj, PropertyDescriptor pro, object value)
        {
            if (value == null || value == DBNull.Value)
            {
                pro.SetValue(obj, null);
                return;
            }
            var type = pro.PropertyType;
            if (type.IsGenericType && Nullable.GetUnderlyingType(type) != null) type = Nullable.GetUnderlyingType(type);
            if (type.IsEnum) type = type.GetEnumUnderlyingType();
            switch (type.Name)
            {
                case nameof(Image):
                    if (value is byte[] buffer)
                    {
                        pro.SetValue(obj, StructHelper.BytesToImage(buffer));
                    }
                    else if (value is Image image)
                    {
                        pro.SetValue(obj, image);
                    }
                    break;
                case nameof(Int64):
                    pro.SetValue(obj, value.ToLong());
                    break;
                case nameof(Int32):
                    pro.SetValue(obj, value.ToInt());
                    break;
                case nameof(Int16):
                    pro.SetValue(obj, (short)value.ToInt());
                    break;
                case nameof(Byte):
                    pro.SetValue(obj, (byte)value.ToInt());
                    break;
                case nameof(Boolean):
                    pro.SetValue(obj, value.ToBool());
                    break;
                case nameof(Double):
                    pro.SetValue(obj, value.ToDouble());
                    break;
                case nameof(Single):
                    pro.SetValue(obj, value.ToFloat());
                    break;
                case nameof(Decimal):
                    pro.SetValue(obj, value.ToDecimal());
                    break;
                case nameof(DateTime):
                    var time = value.ToDateTime();
                    if (TConfig.IUtcTime) time = time.ToLocalTime();
                    pro.SetValue(obj, time);
                    break;
                case nameof(String):
                    pro.SetValue(obj, value.ToStrs());
                    break;
                default:
                    pro.SetValue(obj, value);
                    break;
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
        public static TableAttribute Table(this Type type)
        {
            var attr = AttrTable(type);
            if (attr.Keys == null) throw new ArgumentException("没有指定主键或主列名称");
            return attr;
        }
        /// <summary>
        ///     返回特性，检查表名
        /// </summary>
        public static TableAttribute AttrTable(Type type)
        {
            var attrList = type.GetCustomAttributes(typeof(TableAttribute), false) as TableAttribute[];
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
        public static bool ISelect(this PropertyInfo pro, out string column)
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
        /// 显示列标记
        /// </summary>
        public static bool IShow(this MemberInfo pro, out string text)
        {
            text = pro.Name;
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (list.Length == 1 && list[0].Text != null)
            {
                text = list[0].Text;
            }
            return list.Length == 0 || list[0].IShow;
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
        /// 显示属性
        /// </summary>
        public static bool IBrowsable(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(BrowsableAttribute), false) as BrowsableAttribute[];
            return list.Length == 0 || list[0].Browsable;
        }
        /// <summary>
        /// 生成到ExcelTable标记
        /// </summary>
        public static bool IExcel(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(NoExcelAttribute), false) as NoExcelAttribute[];
            return list.Length == 0;
        }
        /// <summary>
        /// 自定义不复制列标记
        /// </summary>
        public static bool IClone(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(NoCloneAttribute), false) as NoCloneAttribute[];
            return list.Length == 0;
        }

        #endregion

        #region Equals
        /// <summary>
        /// 判断值是否相等
        /// </summary>
        public static bool TEquals<T>(this T t, T temp, bool child = false)
        {
            var type = typeof(T);
            return type.TEquals(t, temp, child);
        }
        /// <summary>
        /// 判断值是否相等
        /// </summary>
        private static bool TEquals(this Type parent, object t, object temp, bool child)
        {
            var properties = parent.Properties();
            foreach (var property in properties)
            {
                if (!property.IClone()) continue;

                var value = parent.GetValue(t, property.Name);
                var tempValue = parent.GetValue(temp, property.Name);
                if (!value.Equals(tempValue)) return false;
                if (!child) continue;
                if (value is IList)
                {
                    var tlist = tempValue as IList;
                    var list = value as IList;
                    if (tlist.Count != list.Count) return false;
                    for (var i = 0; i < list.Count; i++)
                    {
                        if (!list[i].TEquals(tlist[i], child)) return false;
                    }
                }
                else if (value != null && !value.GetType().IsValueType && value.GetType() != typeof(string))
                {
                    var type = value.GetType();
                    if (!type.TEquals(value, tempValue, child)) return false;
                }
            }
            return true;
        }

        #endregion

        #region 泛型
        /// <summary>
        ///     返回泛型实参数类型
        /// </summary>
        public static Type GenericType(this IList obj)
        {
            return obj.GetType().GenericType();
        }
        /// <summary>
        ///     返回泛型实参数类型
        /// </summary>
        public static Type GenericType(this Type type)
        {
            var types = type.GetGenericArguments();
            if (types.Length == 1) return types[0];
            return null;
        }
        /// <summary>
        ///     返回泛型实参实例
        /// </summary>
        public static IList GenericList(this Type type)
        {
            var listType = typeof(List<>);
            listType = listType.MakeGenericType(type);
            return Activator.CreateInstance(listType) as IList;
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
                    string s1 = string.Empty, s2 = string.Empty;
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
        /// 将值拆箱为Int值以进行比较
        /// </summary>
        public static int TCompareInt(this object obj)
        {
            if (obj == null) return 0;
            Type type = obj.GetType();
            if (type.IsEnum) type = type.GetEnumUnderlyingType();
            switch (type.Name)
            {
                case nameof(Int32):
                    return (int)obj;
                case nameof(Int16):
                    return (short)obj;
                case nameof(Byte):
                    return (byte)obj;
                case nameof(Boolean):
                    return (bool)obj ? 1 : 0;
                case nameof(Image):
                case nameof(Bitmap):
                    var image = (Image)obj;
                    return image.Width * image.Height;
            }
            return obj.ToInt();
        }
        /// <summary>
        /// 将值拆箱为Double值以进行比较
        /// </summary>
        public static double TCompareDouble(this object obj)
        {
            if (obj == null) return 0;
            Type type = obj.GetType();
            switch (type.Name)
            {
                case nameof(Double):
                    return (double)obj;
                case nameof(Single):
                    return (float)obj;
                case nameof(Decimal):
                    return (double)(decimal)obj;
                default:
                    return obj.ToDouble();
            }
        }
        /// <summary>
        /// 将值拆箱为Long值以进行比较
        /// </summary>
        public static long TCompareLong(this object obj)
        {
            if (obj == null) return 0;
            Type type = obj.GetType();
            switch (type.Name)
            {
                case nameof(Int64):
                    return (long)obj;
                case nameof(DateTime):
                    return ((DateTime)obj).Ticks;
                default:
                    return obj.ToLong();
            }
        }

        #endregion
    }
}