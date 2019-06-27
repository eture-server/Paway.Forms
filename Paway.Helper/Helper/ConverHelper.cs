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
        ///     decimal
        /// </summary>
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;

            if (decimal.TryParse(obj.ToString(), out decimal value))
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
        public static string ToStrs(this object obj)
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
            string text = obj.ToStrs();
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
        /// 将指定类型转为DataTable
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql">从数据取值时，Image存储格式为byte[]</param>
        /// <returns></returns>
        public static DataTable CreateTable(this Type type, bool sql = false)
        {
            var table = new DataTable(type.Name);
            var properties = type.Properties();
            foreach (var property in properties)
            {
                Type dbType = property.PropertyType;
                if (property.PropertyType.IsGenericType)
                {
                    dbType = Nullable.GetUnderlyingType(property.PropertyType);
                }
                if (dbType == null) continue;
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
            var properties = type.Properties();
            foreach (var property in properties)
            {
                if (!property.IExcel()) continue;
                Type dbType = property.PropertyType;
                if (property.PropertyType.IsGenericType)
                {
                    dbType = Nullable.GetUnderlyingType(property.PropertyType);
                }
                if (dbType == null) continue;
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
        ///     返回泛型实参数类型
        /// </summary>
        public static Type GenericType(this IEnumerable obj)
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
            var list = Activator.CreateInstance(listType) as IList;
            return list;
        }

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
        /// 将值拆箱为Int值以进行比较
        /// </summary>
        public static int TCompareInt(this object obj)
        {
            if (obj == null) return 0;
            Type type = obj.GetType();
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
        /// 将字符串转为Long值以进行比较
        /// </summary>
        public static long TCompare(this object obj)
        {
            if (obj is string x)
            {
                char[] arr1 = x.ToCharArray();
                int i = 0, j = 0;
                long value = 0;
                while (i < arr1.Length)
                {
                    if (char.IsDigit(arr1[i]))
                    {
                        string s1 = "";
                        while (i < arr1.Length && char.IsDigit(arr1[i]))
                        {
                            s1 += arr1[i];
                            i++;
                        }
                        value += long.Parse(s1);
                    }
                    else
                    {
                        value += arr1[i];
                        i++;
                        j++;
                    }
                }
                return value;
            }
            if (obj == null) return 0;
            Type type = obj.GetType();
            switch (type.Name)
            {
                case nameof(Int64):
                    return (long)obj;
                case nameof(DateTime):
                    return ((DateTime)obj).Ticks;
            }
            if (type.IsEnum) return (int)obj;
            return obj.ToLong();
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
            foreach (var property in properties)
            {
                if (!property.IClone()) continue;

                var value = parent.GetValue(t, property.Name);
                parent.SetValue(copy, property.Name, value);
                if (child && value is IList list)
                {
                    var type = list.GenericType();
                    var clist = type.GenericList();
                    parent.SetValue(copy, property.Name, clist);
                    clist = parent.GetValue(copy, property.Name) as IList;
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
                    parent.SetValue(copy, property.Name, obj);
                    type.Clone(obj, value, child);
                }
            }
        }

        #endregion
    }
}