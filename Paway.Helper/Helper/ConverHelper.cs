using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 将一个基本数据类型转换为另一个基本数据类型。
    /// </summary>
    public static class ConverHelper
    {
        /// <summary>
        /// 五笔编码字符
        /// </summary>
        private static readonly List<string> wbList = new List<string>(26);

        #region 枚举
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        public static string Description(this Enum e)
        {
            if (e == null) return string.Empty;
            var value = e.ToString();
            foreach (var field in e.GetType().GetFields(TConfig.Flags))
            {
                if (value == field.Name)
                {
                    return field.Description() ?? value;
                }
            }
            return value;
        }
        /// <summary>
        /// 获取枚举默认值(非枚举值)
        /// </summary>
        public static object DefaultValue(this Enum e)
        {
            if (e == null) return string.Empty;
            var value = e.ToString();
            foreach (var field in e.GetType().GetFields(TConfig.Flags))
            {
                if (value == field.Name)
                {
                    return field.DefaultValue() ?? value;
                }
            }
            return value;
        }
        /// <summary>
        /// 获取字段描述
        /// </summary>
        public static string Description(this MemberInfo type)
        {
            var list = type.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (list.Length > 0)
                return ((DescriptionAttribute)list[0]).Description;
            return null;
        }
        /// <summary>
        /// 将枚举常数的名称或数字值的字符串表示转换成等效的枚举对象
        /// </summary>
        public static T Parse<T>(this string value)
        {
            Type type = typeof(T);
            foreach (FieldInfo field in type.GetFields())
            {
                string name = field.Name;
                if (name.Equals(value, StringComparison.OrdinalIgnoreCase))
                    return (T)field.GetRawConstantValue();
                name = field.Description() ?? field.Name;
                if (name.Equals(value, StringComparison.OrdinalIgnoreCase))
                    return (T)field.GetRawConstantValue();
            }
            return default;
        }
        /// <summary>
        /// 将枚举常数的名称或数字值的字符串表示转换成等效的枚举对象
        /// </summary>
        public static int Parse(this Type type, string value)
        {
            foreach (FieldInfo field in type.GetFields())
            {
                string name = field.Name;
                if (name.Equals(value, StringComparison.OrdinalIgnoreCase))
                    return (int)field.GetRawConstantValue();
                name = field.Description() ?? field.Name;
                if (name.Equals(value, StringComparison.OrdinalIgnoreCase))
                    return (int)field.GetRawConstantValue();
            }
            return default;
        }

        #endregion

        #region 颜色亮度变化
        /// <summary>
        /// RGB空间与HSL空间的转换(Color亮度变化)
        /// </summary>
        public static Color AddLight(this Color color, int value = 30)
        {
            var result = BitmapHelper.RGBToHSL(color.R / 255.0, color.G / 255.0, color.B / 255.0);
            return BitmapHelper.HSLToRGB(result[0], result[1], result[2] + value * 1.0 / 240);
        }
        /// <summary>
        /// 混合颜色
        /// </summary>
        /// <param name="color">原颜色</param>
        /// <param name="mixColor">混合颜色</param>
        /// <param name="percent">原颜色比例</param>
        public static Color AddColor(this Color color, Color mixColor, int percent = 90)
        {
            decimal percentA = 1 - percent / 100m;

            decimal R = color.R - (color.R - mixColor.R) * percentA;
            decimal G = color.G - (color.G - mixColor.G) * percentA;
            decimal B = color.B - (color.B - mixColor.B) * percentA;

            Color target = Color.FromArgb((int)R, (int)G, (int)B);
            return target;
        }

        #endregion

        #region 关于异常
        /// <summary>
        /// 获取异常中的所有描述
        /// </summary>
        public static string Message(this Exception ex)
        {
            var msg = ex.Message;
            while (ex.InnerException != null)
            {
                string innerMsg = ex.InnerException.Message;
                if (!string.IsNullOrEmpty(innerMsg) && !msg.Contains(innerMsg))
                {
                    msg = string.Format("{0}\r\n{1}", msg, innerMsg);
                }
                ex = ex.InnerException;
            }
            var list = new List<string>();
            var strs = msg.Split(new[] { "\r\n", "&&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in strs)
            {
                if (str.Equals("发生一个或多个错误。")) continue;
                if (!list.Contains(str)) list.Add(str);
            }
            return string.Join("\r\n", list.ToArray());
        }
        /// <summary>
        /// 获取内部异常
        /// </summary>
        public static Exception InnerException(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        #endregion

        #region 字符串转换
        #region 中文转拼音(取汉字、数字、字母，其它自动过滤)
        /// <summary>
        /// 在指定的字符串列表cnStr中检索符合拼音索引字符串
        /// <para>使用NPinyin获取拼音，失败再使用微软PinYinConverter</para>
        /// </summary>
        /// <param name="str">汉字字符串</param>
        /// <param name="args">换格字符</param>
        /// <returns>相对应的汉语拼音首字母串</returns>
        public static string ToSpell(this string str, params char[] args)
        {
            return ToCode(ToSpell, str, args);
        }
        /// <summary>
        /// 在指定的字符串列表cnStr中检索符合拼音索引字符串(仅取一级汉字共3755个)
        /// </summary>
        /// <param name="str">汉字字符串</param>
        /// <param name="args">换格字符</param>
        /// <returns>相对应的汉语拼音首字母串</returns>
        public static string ToSpellOld(this string str, params char[] args)
        {
            return ToCode(ToSpellOld, str, args);
        }
        private static string ToCode(Func<char, Tuple<bool, char>> action, string str, params char[] args)
        {
            if (str.IsNullOrEmpty()) return string.Empty;
            string strTemp = string.Empty;
            bool last = true;
            for (int i = 0; i < str.Length; i++)
            {
                Tuple<bool, char> result = action(str[i]);
                if ((result.Item1 || last) && !IContains(result.Item2, args))
                {
                    if (args.Length > 0) { }

                    strTemp += result.Item2;
                }
                last = result.Item1 | IContains(result.Item2, args);
            }
            return strTemp.ToUpper();
        }
        /// <summary>
        /// 判断非汉字时取值问题
        /// 参数为空时仅取连续字母或数字的首位
        /// </summary>
        private static bool IContains(char c, params char[] args)
        {
            if (args.Length > 0) return args.Contains(c);
            if (c >= 65 && c <= 90) return false;//A-Z
            if (c >= 97 && c <= 122) return false;//a-z
            if (c >= 48 && c <= 57) return false;//0-9
            return true;
        }
        /// <summary>
        /// 根据一个汉字获得其首拼音字符
        /// </summary>
        private static Tuple<bool, char> ToSpellOld(char chr)
        {
            char result = chr;
            //获得其ASSIC码
            byte[] arrCN = Encoding.Default.GetBytes(new char[] { chr });
            if (arrCN.Length > 1)
            {
                //int code = (area << 8) + pos;
                int code = arrCN[0] * 256 + arrCN[1];
                //0~65535
                //a~Z的数字表示
                int[] areacode = {45217, 45253, 45761, 46318, 46826, 47010, 47297,
                                    47614, 48119, 48119, 49062, 49324, 49896, 50371,
                                    50614, 50622, 50906, 51387, 51446, 52218,
                                    52698, 52698, 52698, 52980, 53689, 54481,55290 };
                //判断其值在那2个数字之间
                for (int i = 0; i < areacode.Length - 1; i++)
                {
                    //最后一个汉字的值
                    int max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        //转为字母返回
                        return new Tuple<bool, char>(true, (char)(65 + i));
                    }
                }
                result = '?';
            }
            return new Tuple<bool, char>(false, result);
        }
        /// <summary>
        /// 根据一个汉字获得其首拼音字符
        /// <para>使用NPinyin获取拼音，失败再使用微软PinYinConverter</para>
        /// </summary>
        private static Tuple<bool, char> ToSpell(char chr)
        {
            var coverchr = NPinyin.Pinyin.GetPinyin(chr);
            bool isChineses = ChineseChar.IsValidChar(coverchr[0]);
            if (isChineses)
            {
                ChineseChar chineseChar = new ChineseChar(coverchr[0]);
                foreach (string value in chineseChar.Pinyins)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        isChineses = false;
                        coverchr = value.Remove(value.Length - 1, 1);
                    }
                }
            }
            return new Tuple<bool, char>(!isChineses, coverchr[0]);
        }

        /// <summary>
        /// 在字符串中搜索正则表达式的第一个匹配项
        /// </summary>
        public static Match Regex(this string text, string pattern, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline)
        {
            var regex = new Regex(pattern, options);
            return regex.Match(text);
        }
        /// <summary>
        /// 在字符串中搜索正则表达式的所有匹配项
        /// </summary>
        public static MatchCollection Regexs(this string text, string pattern, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline)
        {
            var regex = new Regex(pattern, options);
            return regex.Matches(text);
        }

        #endregion

        #region 中文转五笔(取汉字、数字、字母，其它自动过滤)
        /// <summary>
        /// 在指定的字符串列表cnStr中检索符合五笔索引字符串
        /// </summary>
        /// <param name="str">汉字字符串</param>
        /// <param name="args">换格字符</param>
        /// <returns>相对应的汉语五笔首字母串</returns>
        public static string ToWbCode(this string str, params char[] args)
        {
            return ToCode(ToWbCode, str, args);
        }
        /// <summary>
        /// 根据一个汉字获得其首个五笔字符
        /// </summary>
        private static Tuple<bool, char> ToWbCode(char chr)
        {
            InitWbCode();
            var result = chr;
            //获得其ASSIC码
            byte[] arrCN = Encoding.Default.GetBytes(new char[] { chr });
            if (arrCN.Length > 1)
            {
                for (int i = 0; i < wbList.Count; i++)
                {
                    if (wbList[i].Contains(chr))
                    {
                        return new Tuple<bool, char>(true, (char)(65 + i));
                    }
                }
                result = '?';
            }
            return new Tuple<bool, char>(false, result);
        }
        private static void InitWbCode()
        {
            if (wbList.Count > 0) return;
            wbList.Add(TConst.A);
            wbList.Add(TConst.B);
            wbList.Add(TConst.C);
            wbList.Add(TConst.D);
            wbList.Add(TConst.E);
            wbList.Add(TConst.F);
            wbList.Add(TConst.G);
            wbList.Add(TConst.H);
            wbList.Add(TConst.I);
            wbList.Add(TConst.J);
            wbList.Add(TConst.K);
            wbList.Add(TConst.L);
            wbList.Add(TConst.M);
            wbList.Add(TConst.N);
            wbList.Add(TConst.O);
            wbList.Add(TConst.P);
            wbList.Add(TConst.Q);
            wbList.Add(TConst.R);
            wbList.Add(TConst.S);
            wbList.Add(TConst.T);
            wbList.Add(TConst.U);
            wbList.Add(TConst.V);
            wbList.Add(TConst.W);
            wbList.Add(TConst.X);
            wbList.Add(TConst.Y);
            wbList.Add(TConst.Z);
        }

        #endregion

        #region 全角半角转换
        /// <summary> 转半角的函数(DBC case) </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        /// 全角空格为12288，半角空格为32
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToHalfAngle(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] == 12290)
                {
                    c[i] = (char)46;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375) c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        #endregion

        #region 解压缩
        /// <summary>
        /// 压缩字符串(返回Base64数字编码的等效字符串)
        /// </summary>
        public static string CompressBase64(this string str)
        {
            var buffer = Compress(str);
            string result = Convert.ToBase64String(buffer);
            return result;
        }
        /// <summary>
        /// 压缩字符串(返回字节流)
        /// </summary>
        public static byte[] Compress(this string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            return CompressBuffer(buffer);
        }
        /// <summary>
        /// 解压字符串(Base64数字编码的等效字符串)
        /// </summary>
        public static string Decompress(this string str)
        {
            var buffer = Convert.FromBase64String(str);
            return Decompress(buffer);
        }
        /// <summary>
        /// 解压字节流
        /// </summary>
        public static string Decompress(this byte[] buffer)
        {
            var temp = DecompressBuffer(buffer);
            string result = Encoding.UTF8.GetString(temp);
            return result;
        }
        /// <summary>
        /// 压缩字节流
        /// </summary>
        private static byte[] CompressBuffer(byte[] data)
        {
            var ms = new MemoryStream();
            using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(data, 0, data.Length);
            }
            var buffer = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        /// <summary>
        /// 解压字节流
        /// </summary>
        private static byte[] DecompressBuffer(byte[] data)
        {
            var ms = new MemoryStream(data);
            using (var zip = new GZipStream(ms, CompressionMode.Decompress, true))
            {
                using (var msreader = new MemoryStream())
                {
                    var buffer = new byte[0x1000];
                    while (true)
                    {
                        var reader = zip.Read(buffer, 0, buffer.Length);
                        if (reader <= 0)
                        {
                            break;
                        }
                        msreader.Write(buffer, 0, reader);
                    }
                    msreader.Position = 0;
                    buffer = msreader.ToArray();
                    return buffer;
                }
            }
        }

        #endregion

        #endregion

        #region 数据转换
        /// <summary>
        /// Int32转换
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
        /// Bool转换
        /// </summary>
        public static bool ToBool(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return false;

            if (obj is int iObj) return iObj != 0;
            if (bool.TryParse(obj.ToString(), out bool result)) return result;
            return false;
        }
        /// <summary>
        /// Long转换
        /// </summary>
        public static long ToLong(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;
            if (obj.GetType() == typeof(long)) return (long)obj;

            if (long.TryParse(obj.ToString(), out long value)) return value;
            return 0;
        }
        /// <summary>
        /// Double转换
        /// </summary>
        public static double ToDouble(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            if (double.TryParse(obj.ToString(), out double value)) return value;
            return 0;
        }
        /// <summary>
        /// float
        /// </summary>
        public static float ToFloat(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            if (float.TryParse(obj.ToString(), out float value)) return value;
            return 0;
        }
        /// <summary>
        /// decimal
        /// </summary>
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            if (decimal.TryParse(obj.ToString(), out decimal value)) return value;
            return 0;
        }
        /// <summary>
        /// String转换
        /// </summary>
        public static string ToStrs(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return string.Empty;

            return obj.ToString().Trim();
        }
        /// <summary>
        /// DateTime转换
        /// </summary>
        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == string.Empty) return DateTime.MinValue;

            if (obj is DateTime time) return time;
            if (DateTime.TryParse(obj.ToString(), out DateTime result)) return result;
            return DateTime.MinValue;
        }
        /// <summary>
        /// 检测obj,如果为DBNUll或空字符串 返回true
        /// </summary>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return true;

            return string.IsNullOrEmpty(obj.ToString());
        }
        /// <summary>
        /// 消除算术计算误差(decimal转换)
        /// </summary>
        public static double Clear(this double value)
        {
            return decimal.ToDouble(value.ToDecimal());
        }

        #endregion

        #region IList 与　DataTable 互转
        /// <summary>
        /// 获取接口所有属性
        /// PropertyInfo.SetValue不能设置只读接口
        /// </summary>
        internal static List<PropertyInfo> Properties(this Type type)
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
            var properties = type.PropertiesCache();
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
        public static PropertyInfo Property<T>(this T t, string name)
        {
            return t.GetType().Property(name);
        }
        /// <summary>
        /// 获取指定名称属性
        /// </summary>
        public static PropertyInfo Property(this Type type, string name)
        {
            var properties = type.PropertiesCache();
            return Property(properties, name);
        }
        /// <summary>
        /// 获取指定属性值
        /// </summary>
        public static object GetValue<T>(this PropertyInfo property, T obj)
        {
            return obj.GetValue(property.Name);
        }
        /// <summary>
        /// 获取指定名称属性
        /// </summary>
        public static PropertyInfo Property(this List<PropertyInfo> properties, string name)
        {
            var property = properties.Find(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (property == null)
            {
                property = properties.Find(c => c.Column().Equals(name, StringComparison.OrdinalIgnoreCase));
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
        /// 将指定类型转为DataTable
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
        public static void TrySetValue<T>(this T obj, string name, object value)
        {
            Type type = typeof(T);
            var descriptors = type.Descriptors();
            var descriptor = descriptors.Find(c => c.Name == name);
            if (descriptor != null)
            {
                obj.SetValue(descriptor, value);
            }
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
            if (type.IsEnum)
            {
                var iValue = type.Parse(value.ToString());
                if (iValue != 0)
                {
                    pro.SetValue(obj, iValue);
                    return;
                }
                type = type.GetEnumUnderlyingType();
            }
            switch (type.Name)
            {
                case nameof(Image):
                    if (value is byte[] buffer)
                    {
                        pro.SetValue(obj, StructHelper.DeserializeImage(buffer));
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
                case nameof(Point):
                    var match = value.ToStrs().Regex(@"{X=(?<x>\d+).+Y=(?<y>\d+)}");
                    if (match.Success)
                    {
                        pro.SetValue(obj, new Point(match.Groups["x"].ToInt(), match.Groups["y"].ToInt()));
                    }
                    break;
                case nameof(Size):
                    match = value.ToStrs().Regex(@"{Width=(?<w>\d+).+Height=(?<h>\d+)}");
                    if (match.Success)
                    {
                        pro.SetValue(obj, new Size(match.Groups["w"].ToInt(), match.Groups["h"].ToInt()));
                    }
                    break;
                case nameof(TimeSpan):
                    if (TimeSpan.TryParse(value.ToStrs(), out TimeSpan timeSpan))
                    {
                        pro.SetValue(obj, timeSpan);
                    }
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
        public static string Table(this Type type)
        {
            var list = type.GetCustomAttributes(typeof(TableAttribute), false) as TableAttribute[];
            if (list.Length == 1 && list[0].Name != null)
            {
                return list[0].Name;
            }
            throw new ArgumentException("No table name.");
        }
        /// <summary>
        /// 获取主键(Key.Mark.Id)
        /// <para>优先取Key</para>
        /// <para>存在Mark时取Mark</para>
        /// <para>存在Id时取Id</para>
        /// <para>否则抛出空参数异常</para>
        /// </summary>
        public static string TableKeys(this Type type)
        {
            var properties = type.PropertiesCache();
            foreach (var property in properties)
            {
                if (property.IKey()) return property.Column();
            }
            foreach (var property in properties)
            {
                if (property.IMark()) return property.Column();
            }
            foreach (var property in properties)
            {
                var name = property.Column();
                if (name.Equals(nameof(IId.Id), StringComparison.OrdinalIgnoreCase)) return name;
            }
            throw new ArgumentException("No primary key.");
        }
        /// <summary>
        /// 获取自更新唯一主键(Key.null.Id)
        /// <para>取Key</para>
        /// <para>存在Mark时返回null</para>
        /// <para>存在Id时取Id</para>
        /// <para>否则返回null</para>
        /// </summary>
        public static string TableKey(this Type type)
        {
            var properties = type.PropertiesCache();
            foreach (var property in properties)
            {
                if (property.IKey()) return property.Column();
            }
            foreach (var property in properties)
            {
                if (property.IMark()) return null;
            }
            foreach (var property in properties)
            {
                var name = property.Column();
                if (name.Equals(nameof(IId.Id), StringComparison.OrdinalIgnoreCase)) return name;
            }
            return null;
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
        /// 自定义特性-数据库列名称
        /// 兼容视图多表查询、自定义列名
        /// </summary>
        public static string Column(this MemberInfo pro, bool sql = false)
        {
            var list = pro.GetCustomAttributes(typeof(ColumnAttribute), false) as ColumnAttribute[];
            var column = pro.Name;
            if (list.Length == 1 && list[0].Name != null)
            {
                column = list[0].Name;
                if (column.Contains(" "))
                {
                    column = column.Substring(column.LastIndexOf(" ") + 1);
                    if (sql) column = $"{list[0].Name.Remove(list[0].Name.LastIndexOf(" "))} [{column}]";
                }
                else if (column.Contains("."))
                {
                    column = column.Substring(column.LastIndexOf(".") + 1);
                    if (sql) column = $"{list[0].Name} [{column}]";
                }
                else if (sql) column = $"[{column}]";
            }
            else if (sql) column = $"[{column}]";
            return column;
        }
        /// <summary>
        /// 自定义特性-文本
        /// </summary>
        public static string Text(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(TextAttribute), false) as TextAttribute[];
            if (list.Length == 1 && list[0].Name != null)
            {
                return list[0].Name;
            }
            return pro.Name;
        }
        /// <summary>
        /// 自定义特性-标识列(非自增)
        /// </summary>
        public static bool IMark(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(MarkAttribute), false) as MarkAttribute[];
            return list.Length == 1;
        }
        /// <summary>
        /// 自定义特性-主键列(自增)
        /// </summary>
        public static bool IKey(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(KeyAttribute), false) as KeyAttribute[];
            return list.Length == 1;
        }
        /// <summary>
        /// 自定义特性-不显示列
        /// </summary>
        public static bool IShow(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(NoShowAttribute), false) as NoShowAttribute[];
            return list.Length == 0;
        }
        /// <summary>
        /// 自定义特性-不生成数据列
        /// </summary>
        public static SelectType ISelect(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(NoSelectAttribute), false) as NoSelectAttribute[];
            return list.Length == 0 ? SelectType.Normal : list[0].Type;
        }
        /// <summary>
        /// 自定义特性-不生成ExcelTable列
        /// </summary>
        public static bool IExcel(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(NoExcelAttribute), false) as NoExcelAttribute[];
            return list.Length == 0;
        }
        /// <summary>
        /// 自定义特性-不复制列
        /// </summary>
        public static bool IClone(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(NoCloneAttribute), false) as NoCloneAttribute[];
            return list.Length == 0;
        }

        /// <summary>
        /// 自定义特性-TDataGridView全选框
        /// </summary>
        public static bool ICheckBox(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(ICheckBoxAttribute), false) as ICheckBoxAttribute[];
            return list.Length != 0;
        }
        /// <summary>
        /// 自定义特性-TDataGridView按钮
        /// </summary>
        public static bool IButton(this MemberInfo pro, out IButtonAttribute button)
        {
            var list = pro.GetCustomAttributes(typeof(IButtonAttribute), false) as IButtonAttribute[];
            button = list.Length > 0 ? list[0] : null;
            return list.Length > 0;
        }
        /// <summary>
        /// 系统特性-获取默认值
        /// </summary>
        public static object DefaultValue(this MemberInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(DefaultValueAttribute), false) as DefaultValueAttribute[];
            if (list.Length > 0) return list[0].Value;
            return Activator.CreateInstance(pro.DeclaringType);
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
            var properties = parent.PropertiesCache();
            foreach (var property in properties)
            {
                if (!property.IClone()) continue;

                var value = property.GetValue(t);
                var tempValue = property.GetValue(temp);
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
        /// 返回泛型实参数类型
        /// </summary>
        public static Type GenericType(this IList obj)
        {
            return obj.GetType().GenericType();
        }
        /// <summary>
        /// 返回泛型实参数类型
        /// </summary>
        public static Type GenericType(this Type type)
        {
            var types = type.GetGenericArguments();
            if (types.Length == 1) return types[0];
            return null;
        }
        /// <summary>
        /// 返回泛型实参实例
        /// </summary>
        public static IList GenericList(this Type type)
        {
            var listType = typeof(List<>);
            listType = listType.MakeGenericType(type);
            return Activator.CreateInstance(listType) as IList;
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

        #endregion
    }
}