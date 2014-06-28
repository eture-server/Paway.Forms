using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections;
using System.Drawing;
using System.Data.Common;

namespace Paway.Helper
{
    /// <summary>
    /// 将一个基本数据类型转换为另一个基本数据类型。
    /// </summary>
    public static class ConverHelper
    {
        #region 常量
        /// <summary>
        /// 每毫米等于的英寸数值
        /// </summary>
        private const float MM_OF_INCH = 0.039370078740157f;

        #endregion

        #region 方法

        #region 对 英寸 的转换

        /// <summary>
        /// 将毫米转换为英寸
        /// </summary>
        /// <param name="mm">毫米</param>
        /// <returns></returns>
        public static float MmToInch(this float mm)
        {
            float d = (float)(mm * ConverHelper.MM_OF_INCH);
            return d * 100;
        }

        #endregion

        #region 数据转换助手类

        /// <summary>
        /// Double转换
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ToDouble(this object obj)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(obj.ToString()))
                return 0;
            else
            {
                double value;
                if (double.TryParse(obj.ToString(), out value))
                    return value;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Int32转换
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(this object obj)
        {
            try
            {
                if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(obj.ToString()))
                    return -1;
                else
                {
                    if (obj.ToString().ToUpper() == "TRUE")
                        return 1;
                    else if (obj.ToString().ToUpper() == "FALSE")
                        return 0;
                    int data = -1;
                    if (int.TryParse(obj.ToString(), out data))
                    {
                        return data;
                    }
                    else
                    {
                        double result;
                        if (double.TryParse(obj.ToString(), out result))
                        {
                            result = Math.Round(result);
                            if (int.TryParse(result.ToString(), out data))
                            {
                                return data;
                            }
                        }
                        return -1;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bool转换
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBool(this object obj)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(obj.ToString()))
                return false;
            else
            {
                bool value;
                if (obj.ToString() == "1")
                    return true;
                else if (obj.ToString() == "0")
                    return false;
                else if (bool.TryParse(obj.ToString(), out value))
                    return value;
                else
                    return false;
            }
        }

        /// <summary>
        /// Long转换
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ToLong(this object obj)
        {
            try
            {
                if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(obj.ToString()))
                    return -1;
                else
                {
                    long value;
                    if (long.TryParse(obj.ToString(), out value))
                    {
                        return value;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// String转换
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(this object obj)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                    return string.Empty;
                else
                    return obj.ToString().Trim();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DateTime转换
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == string.Empty)
                return new DateTime();
            else
                return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// List int泛型转换为','隔开的字符串
        /// </summary>
        /// <param name="lstObj"></param>
        /// <returns></returns>
        public static string ToListString(this List<int> lstObj)
        {
            if (lstObj == null)
                return string.Empty;

            string strValue = string.Empty;
            foreach (int value in lstObj)
            {
                if (string.IsNullOrEmpty(strValue))
                    strValue += value.ToString();
                else
                    strValue += string.Format(",{0}", value);
            }
            return strValue;
        }

        /// <summary>
        /// 字符串转换为List int泛型
        /// </summary>
        /// <param name="strObj"></param>
        /// <returns></returns>
        public static List<int> ToIntList(this string strObj)
        {
            List<int> lstValue = new List<int>();
            string[] arrString = strObj.Split(',');
            if (arrString == null || arrString.Length == 0)
                return lstValue;

            foreach (string value in arrString)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int intBcch;
                    if (int.TryParse(value, out intBcch))
                    {
                        lstValue.Add(Convert.ToInt32(intBcch));
                    }
                }
            }
            return lstValue;
        }

        /// <summary>
        /// Int List集合转换为字符串：1-3,6,9
        /// </summary>
        /// <param name="lstFreq"></param>
        /// <returns></returns>
        public static string ToRangeString(this IList<int> lstFreq)
        {
            if (lstFreq == null || lstFreq.Count <= 0)
                return "";

            lstFreq = lstFreq.OrderBy(item => item).ToList();
            int begin = lstFreq[0];
            int end = lstFreq[0];
            string txtFreq = string.Empty;

            for (int i = 1; i <= lstFreq.Count; i++)
            {
                if (i == lstFreq.Count)
                {
                    if (lstFreq[i - 1] != begin)
                    {
                        txtFreq += string.Format("{0}-{1}", begin, lstFreq[i - 1]);
                    }
                    else
                    {
                        txtFreq += string.Format("{0}", lstFreq[i - 1]);
                    }
                }
                else if (lstFreq[i] != (lstFreq[i - 1] + 1))
                {
                    end = lstFreq[i - 1];

                    if (begin != end)
                    {
                        txtFreq += string.Format("{0}-{1},", begin, end);
                    }
                    else
                    {
                        txtFreq += string.Format("{0},", end);
                    }
                    begin = lstFreq[i];
                    end = lstFreq[i];
                }
            }
            return txtFreq;
        }

        /// <summary>
        /// 字符串转换为频点集合
        /// </summary>
        /// <param name="strPlan"></param>
        /// <returns></returns>
        public static List<int> RangeToList(this string strPlan)
        {
            string[] arrPlan = strPlan.Split(',');
            List<int> lstPlan = new List<int>();
            for (int i = 0; i < arrPlan.Length; i++)
            {
                if (arrPlan[i].Contains('-'))
                {
                    string[] data = arrPlan[i].Split('-');
                    if (data.Length == 2)
                    {
                        int first = data[0].ToInt();
                        int last = data[1].ToInt();
                        if (first <= last && first != -1)
                        {
                            for (int x = first; x <= last; x++)
                            {
                                if (!lstPlan.Contains(x))
                                {
                                    lstPlan.Add(x);
                                }
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(arrPlan[i]))
                {

                    int data = arrPlan[i].ToInt();
                    if (!lstPlan.Contains(data) && data != -1)
                    {
                        lstPlan.Add(data);
                    }
                }

            }
            return lstPlan;
        }

        /// <summary>
        /// 逗号分割字符串
        /// </summary>
        /// <param name="strObj"></param>
        /// <returns></returns>
        public static List<string> SplitByComma(this string strObj)
        {
            if (string.IsNullOrEmpty(strObj))
                return new List<string>();
            string[] str = strObj.Trim().Split(',');
            return str.ToList();
        }

        /// <summary>
        /// 检测obj,如果为DBNUll或空字符串 返回true
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool CheckIsDBNullOREmpty(this object obj)
        {
            if (obj == null)
            {
                return true;
            }
            else if (obj.ToString().Trim() == string.Empty)
            {
                return true;
            }
            return false;
        }

        #region IList 与　DataTable 互转
        /// <summary>
        /// IList转为 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }
            return table;
        }
        /// <summary>
        /// IList转为 DataTable
        /// </summary>
        /// <param name="type"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this Type type, IList list)
        {
            DataTable table = type.CreateTable();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
            for (int i = 0; i < list.Count; i++)
            {
                DataRow row = table.NewRow();
                for (int j = 0; j < properties.Count; j++)
                {
                    row[properties[j].Name] = properties[j].GetValue(list[i]);
                }
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// 将指定类型转为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
        /// <summary>
        /// 将指定类型转为DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateTable(this Type type)
        {
            DataTable table = new DataTable(type.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
            for (int i = 0; i < properties.Count; i++)
            {
                table.Columns.Add(properties[i].Name, properties[i].PropertyType);
            }
            return table;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// 将DataTable转为IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(this DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T CreateItem<T>(this DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();//string 类型不支持无参的反射
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

                foreach (DataColumn column in row.Table.Columns)
                {
                    for (int i = 0; i < properties.Count; i++)
                    {
                        PropertyInfo pro = typeof(T).GetProperty(properties[i].Name, properties[i].PropertyType);
                        PropertyAttribute[] itemList = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                        if (itemList == null || itemList.Length == 0 || itemList[0].Select)
                        {
                            string name = properties[i].Name;
                            if (itemList != null && itemList.Length == 1 && itemList[0].Column != null)
                            {
                                name = itemList[0].Column;
                            }
                            if (name != column.ColumnName) continue;
                            try
                            {
                                object value = row[column.ColumnName];
                                if (value != DBNull.Value)
                                {
                                    if (pro.PropertyType == typeof(Image) && value is byte[])
                                    {
                                        pro.SetValue(obj, SctructHelper.GetObjectFromByte(value as byte[]) as Image, null);
                                    }
                                    else
                                    {
                                        pro.SetValue(obj, value, null);
                                    }
                                }
                                break;
                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// 该方法仅能转换DataTable的首列
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<string> ConvertToListWithFirstColumn(this DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<string> list = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(row[0].ToString());
            }
            return list;
        }
        #endregion
        /// <summary>
        /// </summary>
        /// <param name="oldDic"></param>
        /// <returns></returns>
        public static Dictionary<int, int> DeepCopy(this Dictionary<int, int> oldDic)
        {
            var newDic = oldDic.ToDictionary(entry => entry.Key, entry => entry.Value);
            return newDic;
        }

        /// <summary> 
        /// Invokes a transform function on each element of a sequence and returns the minimum Double value  
        /// if the sequence is not empty; otherwise returns the specified default value.  
        /// </summary> 
        /// <typeparam name="TSource">The type of the elements of source.</typeparam> 
        /// <param name="source">A sequence of values to determine the minimum value of.</param> 
        /// <param name="selector">A transform function to apply to each element.</param> 
        /// <param name="defaultValue">The default value.</param> 
        /// <returns>The minimum value in the sequence or default value if sequence is empty.</returns> 
        public static double MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double defaultValue)
        {
            if (source.Any<TSource>())
                return source.Min<TSource>(selector);

            return defaultValue;
        }

        /// <summary> 
        /// Invokes a transform function on each element of a sequence and returns the maximum Double value 
        /// if the sequence is not empty; otherwise returns the specified default value.  
        /// </summary> 
        /// <typeparam name="TSource">The type of the elements of source.</typeparam> 
        /// <param name="source">A sequence of values to determine the maximum value of.</param> 
        /// <param name="selector">A transform function to apply to each element.</param> 
        /// <param name="defaultValue">The default value.</param> 
        /// <returns>The maximum value in the sequence or default value if sequence is empty.</returns> 
        public static double MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double defaultValue)
        {
            if (source.Any<TSource>())
                return source.Max<TSource>(selector);

            return defaultValue;
        }

        /// <summary>
        /// Sql语句中的in关键字最多支持1000，查询条件过多时分为多个In操作
        /// </summary>
        private const int QUERY_EVERY_SIZE = 990;
        /// <summary>
        /// </summary>
        /// <param name="lstFileNames"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string FormatListToSqlStr(List<string> lstFileNames, string fieldName)
        {
            if (lstFileNames == null || lstFileNames.Count == 0)
            {
                return "and " + fieldName + " in ('')";
            }
            StringBuilder sb = new StringBuilder();
            int totalCount = GetTotalCount(lstFileNames);
            for (int i = 1; i <= totalCount; i++)
            {
                List<String> pckFileName = GetEveryQueryList(i, totalCount, lstFileNames);
                if (i == 1)
                {
                    sb.Append(" AND(");
                }
                else
                {
                    sb.Append(" OR ");
                }
                sb.Append(fieldName);
                sb.Append(" in(");
                sb.Append(ConverToSqlStr(pckFileName));
                sb.Append(")");

            }
            sb.Append(")");
            return sb.ToString();
        }
        /// <summary>
        /// 格式化时间字符串
        /// </summary>
        /// <param name="time">时间参数</param>
        /// <returns></returns>
        public static string ConvertDataTimeString(double time)
        {
            string strTime = string.Empty;
            double hour = Math.Floor(time * 24);
            double minutes = Math.Floor((time * 24 - hour) * 60);
            double seconds = Math.Floor(((time * 24 - hour) * 60 - minutes) * 60);
            if (hour < 10)
                strTime += "0" + hour.ToInt() + ":";
            else
                strTime += hour.ToInt() + ":";

            if (minutes < 10)
                strTime += "0" + minutes.ToInt() + ":";
            else
                strTime += minutes.ToInt() + ":";

            if (seconds < 10)
                strTime += "0" + seconds.ToInt();
            else
                strTime += seconds.ToInt();
            return strTime;
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T StringToEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }

        /// <summary>
        /// 根据文字换算成TimeSpan类型
        /// 文字由数据和中文单位组成
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <returns></returns>
        public static TimeSpan StringToTimeSpan(string strDateTime)
        {
            TimeSpan timeSpan = TimeSpan.Zero;
            int span = 0;
            if (int.TryParse(strDateTime, out span)) strDateTime += "分钟";
            if (strDateTime.EndsWith("天"))
            {
                span = strDateTime.Replace("天", string.Empty).ToInt();
                timeSpan = TimeSpan.FromDays(span);
            }
            else if (strDateTime.EndsWith("小时"))
            {
                span = strDateTime.Replace("小时", string.Empty).ToInt();
                timeSpan = TimeSpan.FromHours(span);
            }
            else if (strDateTime.EndsWith("分钟"))
            {
                span = strDateTime.Replace("分钟", string.Empty).ToInt();
                timeSpan = TimeSpan.FromMinutes(span);
            }
            else if (strDateTime.EndsWith("秒"))
            {
                span = strDateTime.Replace("秒", string.Empty).ToInt();
                timeSpan = TimeSpan.FromSeconds(span);
            }
            return timeSpan;
        }

        /// <summary>
        /// UInt32转枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="number"></param>
        /// <returns></returns>
        public static T UIntToEnum<T>(uint number)
        {
            return (T)Enum.ToObject(typeof(T), number);
        }
        #region Private Method
        /// <summary>
        /// 获取查询次数
        /// </summary>
        /// <param name="lstFileNames"></param>
        /// <returns></returns>
        private static int GetTotalCount(List<String> lstFileNames)
        {
            int total = 0;
            if (lstFileNames.Count % QUERY_EVERY_SIZE == 0)
            {
                total = lstFileNames.Count / QUERY_EVERY_SIZE;
            }
            else
            {
                total = lstFileNames.Count / QUERY_EVERY_SIZE + 1;
            }
            return total;
        }

        private static List<string> GetEveryQueryList(int queryTimes, int totalTimes, List<string> totalList)
        {
            List<string> pckList = new List<string>();
            if (queryTimes == totalTimes)
            {
                pckList = totalList.GetRange((queryTimes - 1) * QUERY_EVERY_SIZE, totalList.Count - ((queryTimes - 1) * QUERY_EVERY_SIZE));
            }
            else
            {
                pckList = totalList.GetRange((queryTimes - 1) * QUERY_EVERY_SIZE, QUERY_EVERY_SIZE);
            }
            return pckList;
        }

        /// <summary>
        /// 文件名分隔.
        /// </summary>
        /// <param name="lstLogName"></param>
        /// <returns></returns>
        private static string ConverToSqlStr(List<string> lstLogName)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lstLogName.Count; i++)
            {
                string logName = string.Empty;
                if (lstLogName[i].Contains("."))
                    logName = lstLogName[i].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];
                else
                    logName = lstLogName[i];
                if (i == 0)
                {
                    sb.Append("'" + logName + "'");
                }
                else
                {
                    sb.Append(",'" + logName + "'");
                }
            }
            return sb.ToString();
        }

        #endregion

        #endregion

        #region 将 数值 转换为中文大写字符串

        /// <summary>
        /// 转换数字金额主函数（包括小数）
        /// 数字字符串
        /// 转换成中文大写后的字符串或者出错信息提示字符串
        /// </summary>
        public static string ToChineseStr(this string str)
        {
            string result = string.Empty;
            try
            {
                if (!ConverHelper.IsDecimal(str))                // 判断是否为正整数
                    return result;
                if (Double.Parse(str) > double.MaxValue)    // 判断数值是否太大
                    return result;

                char sign = '.';                            //小数点
                string[] splitstr = str.Split(sign);        //按小数点分割字符串
                if (splitstr.Length == 1)                   //只有整数部分
                {
                    result = ConverHelper.ToData(str) + "圆整";
                }
                else                                        //有小数部分
                {
                    result = ConverHelper.ToData(splitstr[0]) + "圆";//转换整数部分
                    result += ConverHelper.ToDecimalStr(splitstr[1]);//转换小数部分
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Convert.ToChineseStr(string) :: " + ex.Message);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 判断是否是正数字字符串
        /// <remarks>如果是数字，返回true，否则返回false</remarks>
        /// </summary>
        private static bool IsDecimal(this string str)
        {
            Decimal d = -1;
            Decimal.TryParse(str, out d);
            if (d < 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 转换数字（整数）
        /// </summary>
        private static string ToData(this string str)
        {
            string rstr = "";
            try
            {
                string tmpstr = "";
                int strlen = str.Length;
                if (strlen <= 4)
                {
                    rstr = ToDigit(str);
                }
                else
                {

                    if (strlen <= 8)//数字长度大于四位，小于八位
                    {
                        tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字
                        rstr = ToDigit(tmpstr);//转换最后四位数字
                        tmpstr = str.Substring(0, strlen - 4);//截取其余数字
                        //将两次转换的数字加上萬后相连接
                        rstr = String.Concat(ToDigit(tmpstr) + "萬", rstr);
                        rstr = rstr.Replace("零萬", "萬");
                        rstr = rstr.Replace("零零", "零");

                    }
                    else
                        if (strlen <= 12)//数字长度大于八位，小于十二位
                        {
                            tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字
                            rstr = ToDigit(tmpstr);//转换最后四位数字
                            tmpstr = str.Substring(strlen - 8, 4);//再截取四位数字
                            rstr = String.Concat(ToDigit(tmpstr) + "萬", rstr);
                            tmpstr = str.Substring(0, strlen - 8);
                            rstr = String.Concat(ToDigit(tmpstr) + "億", rstr);
                            rstr = rstr.Replace("零億", "億");
                            rstr = rstr.Replace("零萬", "零");
                            rstr = rstr.Replace("零零", "零");
                            rstr = rstr.Replace("零零", "零");
                        }
                }
                strlen = rstr.Length;
                if (strlen >= 2)
                {
                    switch (rstr.Substring(strlen - 2, 2))
                    {
                        case "佰零": rstr = rstr.Substring(0, strlen - 2) + "佰"; break;
                        case "仟零": rstr = rstr.Substring(0, strlen - 2) + "仟"; break;
                        case "萬零": rstr = rstr.Substring(0, strlen - 2) + "萬"; break;
                        case "億零": rstr = rstr.Substring(0, strlen - 2) + "億"; break;

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Convert.ToData(string) :: " + ex.Message);
                throw;
            }

            return rstr;
        }

        /// <summary>
        /// 转换数字（小数部分）
        /// </summary>
        private static string ToDecimalStr(this string str)
        {
            string result = string.Empty;
            try
            {
                int strlen = str.Length;
                if (strlen == 1)
                {
                    result = ConverHelper.To1Digit(str) + "角";
                }
                else
                {
                    string tmpstr = str.Substring(0, 1);
                    result = ConverHelper.To1Digit(tmpstr) + "角";
                    tmpstr = str.Substring(1, 1);
                    result += ConverHelper.To1Digit(tmpstr) + "分";
                    result = result.Replace("零分", "");
                    result = result.Replace("零角", "");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Convert.ToDecimalStr(string) :: " + ex.Message);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 转换的字符串（四位以内）
        /// </summary>
        private static string ToDigit(this string str)
        {
            string result = string.Empty;
            try
            {
                switch (str.Length)
                {
                    case 1: result = To1Digit(str); break;
                    case 2: result = To2Digit(str); break;
                    case 3: result = To3Digit(str); break;
                    case 4: result = To4Digit(str); break;
                }
                result = result.Replace("拾零", "拾");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Convert.ToDigit(string) :: " + ex.Message);
                throw;
            }

            return result;
        }

        /// <summary>
        /// 转换四位数字
        /// </summary>
        private static string To4Digit(this string str)
        {
            string result = string.Empty;
            try
            {
                string str1 = str.Substring(0, 1);
                string str2 = str.Substring(1, 1);
                string str3 = str.Substring(2, 1);
                string str4 = str.Substring(3, 1);

                result += ConverHelper.To1Digit(str1) + "仟";
                result += ConverHelper.To1Digit(str2) + "佰";
                result += ConverHelper.To1Digit(str3) + "拾";
                result += ConverHelper.To1Digit(str4);
                result = result.Replace("零仟", "零");
                result = result.Replace("零佰", "零");
                result = result.Replace("零拾", "零");
                result = result.Replace("零零", "零");
                result = result.Replace("零零", "零");
                result = result.Replace("零零", "零");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Convert.To4Digit(string) :: " + ex.Message);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 转换三位数字
        /// </summary>
        private static string To3Digit(this string str)
        {
            string result = string.Empty;
            try
            {
                string str1 = str.Substring(0, 1);
                string str2 = str.Substring(1, 1);
                string str3 = str.Substring(2, 1);
                result += ConverHelper.To1Digit(str1) + "佰";
                result += ConverHelper.To1Digit(str2) + "拾";
                result += ConverHelper.To1Digit(str3);
                result = result.Replace("零佰", "零");
                result = result.Replace("零拾", "零");
                result = result.Replace("零零", "零");
                result = result.Replace("零零", "零");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Convert.To3Digit(string) :: " + ex.Message);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 转换二位数字
        /// </summary>
        private static string To2Digit(this string str)
        {
            string result = string.Empty;
            try
            {
                string str1 = str.Substring(0, 1);
                string str2 = str.Substring(1, 1);

                result += ConverHelper.To1Digit(str1) + "拾";
                result += ConverHelper.To1Digit(str2);
                result = result.Replace("零拾", "零");
                result = result.Replace("零零", "零");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Convert.To2Digit(string) :: " + ex.Message);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 将一位数字转换成中文大写数字
        /// </summary>
        private static string To1Digit(this string str)
        {
            try
            {
                //"零壹贰叁肆伍陆柒捌玖拾佰仟萬億圆整角分"
                switch (str)
                {
                    case "1": return "壹";
                    case "2": return "贰";
                    case "3": return "叁";
                    case "4": return "肆";
                    case "5": return "伍";
                    case "6": return "陆";
                    case "7": return "柒";
                    case "8": return "捌";
                    case "9": return "玖";
                    default: return "零";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Convert.To1Digit(string) :: " + ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #region 基本Sql语句
        /// <summary>
        /// 将指定类型转为Select语句
        /// </summary>
        public static string Select<T>(this T t)
        {
            return Select<T>(t, null);
        }
        /// <summary>
        /// 将指定类型转为Select语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="find">查询语句</param>
        /// <returns></returns>
        public static string Select<T>(this T t, string find)
        {
            PropertyAttribute[] attrList = typeof(T).GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (attrList == null || attrList.Length != 1) throw new ArgumentException(string.Format("类型 {0} 特性错误", typeof(T)));

            string sql = "select";
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyInfo pro = typeof(T).GetProperty(properties[i].Name, properties[i].PropertyType);
                PropertyAttribute[] itemList = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                if (itemList == null || itemList.Length == 0 || itemList[0].Select)
                {
                    string column = properties[i].Name;
                    if (itemList != null && itemList.Length == 1 && itemList[0].Column != null)
                    {
                        column = itemList[0].Column;
                    }
                    sql = string.Format("{0} {1},", sql, column);
                }
            }
            sql = sql.TrimEnd(',');
            sql = string.Format("{0} from '{1}'", sql, attrList[0].Table);
            if (find != null)
            {
                sql = string.Format("{0} where {1}", sql, find);
            }
            return sql;
        }
        /// <summary>
        /// 将指定类型转为Delete语句
        /// </summary>
        public static string Delete<T>(this T t)
        {
            PropertyAttribute[] attrList = typeof(T).GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (attrList == null || attrList.Length != 1) throw new ArgumentException(string.Format("类型 {0} 特性错误", typeof(T)));
            if (attrList[0].Table == null) throw new ArgumentException("没有指定表名称");
            if (attrList[0].Key == null && attrList[0].Mark == null) throw new ArgumentException("没有指定主键或主列名称");

            string sql = "delete from '{0}' where {1}=@{1}";
            sql = string.Format(sql, attrList[0].Table, attrList[0].Key ?? attrList[0].Mark);
            return sql;
        }
        /// <summary>
        /// 将指定类型转为Update语句
        /// </summary>
        public static string Update<T>(this T t)
        {
            PropertyAttribute[] attrList = typeof(T).GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (attrList == null || attrList.Length != 1) throw new ArgumentException(string.Format("类型 {0} 特性错误", typeof(T)));
            if (attrList[0].Table == null) throw new ArgumentException("没有指定表名称");
            if (attrList[0].Key == null && attrList[0].Mark == null) throw new ArgumentException("没有指定主键或主列名称");

            string sql = "update {0} set";
            sql = string.Format(sql, attrList[0].Table);

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < properties.Count; i++)
            {
                if (attrList[0].Key != null && properties[i].Name == attrList[0].Key) continue;
                if (properties[i].GetValue(t) == null) continue;

                PropertyInfo pro = typeof(T).GetProperty(properties[i].Name, properties[i].PropertyType);
                PropertyAttribute[] itemList = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                if (itemList == null || itemList.Length == 0 || itemList[0].Select)
                {
                    string column = properties[i].Name;
                    if (itemList != null && itemList.Length == 1 && itemList[0].Column != null)
                    {
                        column = itemList[0].Column;
                    }
                    sql = string.Format("{0} {1}=@{1},", sql, column);
                }
            }
            sql = sql.TrimEnd(',');
            sql = string.Format("{0} where {1}=@{1}", sql, attrList[0].Key ?? attrList[0].Mark);
            return sql;
        }
        /// <summary>
        /// 将指定类型转为Insert语句
        /// </summary>
        public static string Insert<T>(this T t, string getId)
        {
            PropertyAttribute[] attrList = typeof(T).GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (attrList == null || attrList.Length != 1) throw new ArgumentException(string.Format("类型 {0} 特性错误", typeof(T)));
            if (attrList[0].Table == null) throw new ArgumentException("没有指定表名称");

            string insert = null;
            string values = null;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyInfo pro = typeof(T).GetProperty(properties[i].Name, properties[i].PropertyType);
                PropertyAttribute[] itemList = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                if (itemList == null || itemList.Length == 0 || itemList[0].Select)
                {
                    if (attrList[0].Key != null && properties[i].Name == attrList[0].Key) continue;
                    if (properties[i].GetValue(t) == null) continue;

                    string column = properties[i].Name;
                    if (itemList != null && itemList.Length == 1 && itemList[0].Column != null)
                    {
                        column = itemList[0].Column;
                    }
                    insert = string.Format("{0}{1},", insert, column);
                    values = string.Format("{0}@{1},", values, column);
                }
            }
            insert = insert.TrimEnd(',');
            values = values.TrimEnd(',');
            string sql = string.Format("insert into {0}({1}) values({2})", attrList[0].Table, insert, values);
            sql = string.Format("{0};{1}", sql, getId);
            return sql;
        }
        /// <summary>
        /// 将指定类型转为UpdateOrInsert语句
        /// </summary>
        public static string UpdateOrInsert<T>(this T t, string getid)
        {
            PropertyAttribute[] attrList = typeof(T).GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (attrList == null || attrList.Length != 1) throw new ArgumentException(string.Format("类型 {0} 特性错误", typeof(T)));
            if (attrList[0].Table == null) throw new ArgumentException("没有指定表名称");
            if (attrList[0].Key == null && attrList[0].Mark == null) throw new ArgumentException("没有指定主键或主列名称");

            string sql = "if exists(select 0 from '{1}' where {0}=@{0})";
            sql = string.Format(sql, attrList[0].Key ?? attrList[0].Mark, attrList[0].Table);

            string update = null;
            string insert = null;
            string values = null;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < properties.Count; i++)
            {
                if (attrList[0].Key != null && properties[i].Name == attrList[0].Key) continue;
                if (properties[i].GetValue(t) == null) continue;

                PropertyInfo pro = typeof(T).GetProperty(properties[i].Name, properties[i].PropertyType);
                PropertyAttribute[] itemList = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                if (itemList == null || itemList.Length == 0 || itemList[0].Select)
                {
                    string column = properties[i].Name;
                    if (itemList != null && itemList.Length == 1 && itemList[0].Column != null)
                    {
                        column = itemList[0].Column;
                    }
                    update = string.Format("{0}{1}=@{1},", update, column);
                    insert = string.Format("{0}{1},", insert, column);
                    values = string.Format("{0}@{1},", values, column);
                }
            }
            update = update.TrimEnd(',');
            insert = insert.TrimEnd(',');
            values = values.TrimEnd(',');
            sql = string.Format("{0} update {1} set {2} where {3}=@{3} else insert into {1}({4}) values({5})",
                sql, attrList[0].Table, update, attrList[0].Key ?? attrList[0].Mark, insert, values);

            sql = string.Format("{0};{1}", sql, getid);
            return sql;
        }

        /// <summary>
        /// 设置主键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="value"></param>
        public static bool SetMark<T>(this T t, object value)
        {
            if (value == null || value == DBNull.Value) return false;

            PropertyAttribute[] attrList = typeof(T).GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (attrList == null || attrList.Length != 1) throw new ArgumentException(string.Format("类型 {0} 特性错误", typeof(T)));
            if (attrList[0].Key == null && attrList[0].Mark == null) throw new ArgumentException("没有指定主键或主列名称");

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i].Name == attrList[0].Key)
                {
                    object result = properties[i].GetValue(t);
                    if (properties[i].PropertyType == typeof(int))
                    {
                        properties[i].SetValue(t, value.ToInt());
                    }
                    if (properties[i].PropertyType == typeof(long))
                    {
                        properties[i].SetValue(t, value.ToLong());
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 添加参数值到参数列表
        /// 通用型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="ptype"></param>
        /// <returns></returns>
        public static List<DbParameter> AddParameters<T>(this T t, Type ptype)
        {
            List<DbParameter> pList = new List<DbParameter>();
            PropertyAttribute[] attrList = typeof(T).GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (attrList == null || attrList.Length != 1) throw new ArgumentException(string.Format("类型 {0} 特性错误", typeof(T)));

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyInfo pro = typeof(T).GetProperty(properties[i].Name, properties[i].PropertyType);
                PropertyAttribute[] itemList = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                if (itemList == null || itemList.Length == 0 || itemList[0].Select)
                {
                    object value = properties[i].GetValue(t);
                    if (value == null || value == DBNull.Value) continue;

                    if (properties[i].PropertyType == typeof(Image) && value is Image)
                    {
                        value = SctructHelper.GetByteFromObject(value);
                    }
                    Assembly asmb = Assembly.GetAssembly(ptype);
                    DbParameter param = asmb.CreateInstance(ptype.FullName) as DbParameter;

                    string column = properties[i].Name;
                    if (itemList != null && itemList.Length == 1 && itemList[0].Column != null)
                    {
                        column = itemList[0].Column;
                    }
                    param.ParameterName = string.Format("@{0}", column);
                    param.Value = value;
                    pList.Add(param);
                }
            }
            return pList;
        }

        #endregion
    }
}
