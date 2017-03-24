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

        #region 数据转换
        /// <summary>
        ///     Int32转换
        /// </summary>
        public static int ToInt(this object obj)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(obj.ToString()))
                return -1;

            if (obj.ToString().ToUpper() == "TRUE")
                return 1;
            if (obj.ToString().ToUpper() == "FALSE")
                return 0;
            var data = -1;
            if (int.TryParse(obj.ToString(), out data))
            {
                return data;
            }

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
        /// <summary>
        ///     Long转换
        /// </summary>
        public static long ToLong(this object obj)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(obj.ToString()))
                return -1;

            long value;
            if (long.TryParse(obj.ToString(), out value))
                return value;
            return -1;
        }
        /// <summary>
        ///     Double转换
        /// </summary>
        public static double ToDouble(this object obj)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(obj.ToString()))
                return 0;

            double value;
            if (double.TryParse(obj.ToString(), out value))
                return value;
            return 0;
        }
        /// <summary>
        ///     float
        /// </summary>
        public static float ToFloat(this object obj)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(obj.ToString()))
                return 0;

            float value;
            if (float.TryParse(obj.ToString(), out value))
                return value;
            return 0;
        }
        /// <summary>
        ///     Bool转换
        /// </summary>
        public static bool ToBool(this object obj)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrEmpty(obj.ToString()))
                return false;

            bool value;
            if (obj.ToString() == "1")
                return true;
            if (obj.ToString() == "0")
                return false;
            if (bool.TryParse(obj.ToString(), out value))
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

        #endregion

        #region IList 与　DataTable 互转
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
            var properties = type.GetProperties();
            for (int i = 0; i < list.Count; i++)
            {
                table.Rows.Add(table.NewRow());
            }
            Parallel.For(0, properties.Length, (i) =>
            {
                if (properties[i].PropertyType.IsGenericType) return;
                var name = properties[i].Name;
                if (!properties[i].ITable(ref name)) return;
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    var value = properties[i].GetValue(list[j], null);
                    lock (table)
                        table.Rows[j][name] = value;
                }
            });
            return table;
        }
        /// <summary>
        ///     从类创建行
        /// </summary>
        public static DataRow CreateItem<T>(this T t, DataTable table = null)
        {
            Type type = typeof(T);
            if (table == null)
                table = type.CreateTable();
            DataRow row = table.NewRow();
            var properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].PropertyType.IsGenericType) continue;
                var name = properties[i].Name;
                if (!properties[i].ITable(ref name)) continue;
                row[name] = properties[i].GetValue(t, null);
            }
            return row;
        }

        /// <summary>
        ///     将指定类型转为DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateTable(this Type type)
        {
            var table = new DataTable(type.Name);
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                if (properties[i].PropertyType.IsGenericType) continue;
                var name = properties[i].Name;
                if (!properties[i].ITable(ref name)) continue;
                table.Columns.Add(name, properties[i].PropertyType);
            }
            return table;
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
            var properties = type.GetProperties();
            for (int i = 0; i < count; i++)
            {
                list.Add(Activator.CreateInstance<T>());
            }
            Parallel.For(0, properties.Length, (i) =>
            {
                var name = properties[i].Name;
                if (!properties[i].ITable(ref name)) return;

                var pro = properties[i];
                foreach (DataColumn column in table.Columns)
                {
                    if (name != column.ColumnName) continue;
                    for (var j = 0; j < list.Count; j++)
                    {
                        var value = table.Rows[j][column.ColumnName];
                        if (value == DBNull.Value) continue;
                        SetValue(list[j], pro, value);
                    }
                    break;
                }
            });
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
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var name = properties[i].Name;
                if (!properties[i].ITable(ref name)) continue;

                var pro = properties[i];
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (name != column.ColumnName) continue;
                    var value = row[column.ColumnName];
                    if (value == DBNull.Value) break;
                    SetValue(obj, pro, value);
                    break;
                }
            }

            return obj;
        }

        /// <summary>
        /// 赋值
        /// </summary>
        public static void SetValue<T>(T obj, PropertyInfo pro, object value)
        {
            if (!pro.CanWrite) return;
            if (pro.PropertyType == typeof(Image) && value is byte[])
            {
                pro.SetValue(obj, SctructHelper.GetObjectFromByte(value as byte[]) as Image, null);
            }
            else if (pro.PropertyType == typeof(int) || pro.PropertyType == typeof(int?))
            {
                pro.SetValue(obj, value.ToInt(), null);
            }
            else if (pro.PropertyType == typeof(long) || pro.PropertyType == typeof(long?))
            {
                pro.SetValue(obj, value.ToLong(), null);
            }
            else if (pro.PropertyType == typeof(double) || pro.PropertyType == typeof(double?))
            {
                pro.SetValue(obj, value.ToDouble(), null);
            }
            else if (pro.PropertyType == typeof(float) || pro.PropertyType == typeof(float?))
            {
                pro.SetValue(obj, value.ToFloat(), null);
            }
            else if (pro.PropertyType == typeof(bool) || pro.PropertyType == typeof(bool?))
            {
                pro.SetValue(obj, value.ToBool(), null);
            }
            else if (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?))
            {
                pro.SetValue(obj, value.ToDateTime(), null);
            }
            else if (pro.PropertyType == typeof(string))
            {
                pro.SetValue(obj, value.ToString2(), null);
            }
            else
            {
                pro.SetValue(obj, value, null);
            }
        }

        #endregion

        #region 基本Sql语句

        #region Select
        /// <summary>
        ///     将指定类型转为Select语句
        ///     指定查询条件为主列
        /// </summary>
        public static string Select<T>(params string[] args)
        {
            var attr = typeof(T).Table();
            var sql = Select<T>(0, args);
            sql = string.Format("{0} from [{1}]", sql, attr.Table);
            sql = string.Format("{0} where [{1}]=@{1}", sql, attr.Keys);
            return sql;
        }

        /// <summary>
        ///     将指定类型转为Select语句
        ///     指定查询条件
        /// </summary>
        public static string Select<T>(string find, params string[] args)
        {
            return Select<T>(find, 0, args);
        }

        /// <summary>
        ///     将指定类型转为Select语句
        ///     指定查询条件
        ///     返回指定行数
        /// </summary>
        public static string Select<T>(string find, int count, params string[] args)
        {
            var attr = AttrTable(typeof(T));
            var sql = Select<T>(count, args);
            sql = string.Format("{0} from [{1}]", sql, attr.Table);
            if (find != null)
            {
                sql = string.Format("{0} where {1}", sql, find);
            }
            return sql;
        }

        private static string Select<T>(int count, params string[] args)
        {
            var type = typeof(T);
            var sql = "select";
            if (count != 0)
            {
                sql = string.Format("{0} Top {1}", sql, count);
            }
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    sql = string.Format("{0} [{1}],", sql, column);
                }
            }
            sql = sql.TrimEnd(',');
            return sql;
        }

        #endregion

        #region Delete
        /// <summary>
        ///     将指定类型转为Delete语句
        ///     指定删除条件为主列
        /// </summary>
        public static string Delete<T>()
        {
            var attr = typeof(T).Table();
            var sql = string.Format("delete from [{0}] where [{1}]=@{1}", attr.Table, attr.Keys);
            return sql;
        }

        /// <summary>
        ///     将指定类型转为Delete语句
        ///     指定删除条件
        /// </summary>
        public static string Delete<T>(string find)
        {
            var attr = AttrTable(typeof(T));
            return string.Format("delete from [{0}] where {1}", attr.Table, find);
        }

        #endregion

        #region Update
        /// <summary>
        ///     将指定类型转为Update语句
        /// </summary>
        public static string Update<T>(this T t, params string[] args)
        {
            return t.Update(false, args);
        }
        /// <summary>
        ///     将指定类型转为Update语句
        /// </summary>
        public static string Update<T>(this DataRow row, params string[] args)
        {
            return row.Update<T>(false, args);
        }

        /// <summary>
        ///     将指定类型转为Update语句
        ///     append=true时为附加,对应Sql语句中的+
        /// </summary>
        public static string Update<T>(this T t, bool append = false, params string[] args)
        {
            var type = typeof(T);
            var attr = type.Table();
            var sql = "update [{0}] set";
            sql = string.Format(sql, attr.Table);
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    if (column == attr.Key) continue;
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    if (t.IsNull(properties[i]))
                    {
                        sql = string.Format("{0} [{1}]=NULL,", sql, column);
                    }
                    else if (append)
                    {
                        sql = string.Format("{0} [{1}]=[{1}]+@{1},", sql, column);
                    }
                    else
                    {
                        sql = string.Format("{0} [{1}]=@{1},", sql, column);
                    }
                }
            }
            sql = sql.TrimEnd(',');
            sql = string.Format("{0} where [{1}]=@{1}", sql, attr.Keys);
            return sql;
        }
        /// <summary>
        ///     将指定类型转为Update语句
        ///     append=true时为附加,对应Sql语句中的+
        /// </summary>
        public static string Update<T>(this DataRow row, bool append = false, params string[] args)
        {
            var type = typeof(T);
            var attr = type.Table();
            var sql = "update [{0}] set";
            sql = string.Format(sql, attr.Table);
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    if (column == attr.Key) continue;
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    if (row.IsNull(properties[i]))
                    {
                        sql = string.Format("{0} [{1}]=NULL,", sql, column);
                    }
                    else if (append)
                    {
                        sql = string.Format("{0} [{1}]=[{1}]+@{1},", sql, column);
                    }
                    else
                    {
                        sql = string.Format("{0} [{1}]=@{1},", sql, column);
                    }
                }
            }
            sql = sql.TrimEnd(',');
            sql = string.Format("{0} where [{1}]=@{1}", sql, attr.Keys);
            return sql;
        }

        #endregion

        #region Insert
        /// <summary>
        ///     将指定类型转为Insert语句
        /// </summary>
        public static string Insert<T>(this T t, string getId, bool Identity)
        {
            var attr = AttrTable(typeof(T));

            string insert = null;
            string value = null;
            t.Insert(attr, typeof(T), false, ref insert, ref value);
            var sql = string.Format("insert into [{0}]({1}) values({2})", attr.Table, insert, value);
            sql = string.Format("{0};{1}", sql, getId);
            if (Identity)
            {
                sql = string.Format("SET IDENTITY_INSERT [{0}] ON;{1}", attr.Table, sql);
            }
            return sql;
        }
        /// <summary>
        ///     将指定类型转为Insert语句
        /// </summary>
        public static string Insert<T>(this DataRow row, string getId, bool Identity)
        {
            var attr = AttrTable(typeof(T));

            string insert = null;
            string value = null;
            row.Insert<T>(attr, typeof(T), false, ref insert, ref value);
            var sql = string.Format("insert into [{0}]({1}) values({2})", attr.Table, insert, value);
            sql = string.Format("{0};{1}", sql, getId);
            if (Identity)
            {
                sql = string.Format("SET IDENTITY_INSERT [{0}] ON;{1}", attr.Table, sql);
            }
            return sql;
        }

        private static void Insert<T>(this T t, PropertyAttribute attr, Type type, bool replace, ref string insert, ref string value, params string[] args)
        {
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                if (t.IsNull(properties[i])) continue;

                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    if (!replace && column == attr.Key) continue;
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    insert = string.Format("{0}[{1}],", insert, column);
                    value = string.Format("{0}@{1},", value, column);
                }
            }
            insert = insert.TrimEnd(',');
            value = value.TrimEnd(',');
        }
        private static void Insert<T>(this DataRow row, PropertyAttribute attr, Type type, bool replace, ref string insert, ref string value, params string[] args)
        {
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                if (row.IsNull(properties[i])) continue;

                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    if (!replace && column == attr.Key) continue;
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    insert = string.Format("{0}[{1}],", insert, column);
                    value = string.Format("{0}@{1},", value, column);
                }
            }
            insert = insert.TrimEnd(',');
            value = value.TrimEnd(',');
        }

        /// <summary>
        ///     设置主键值
        /// </summary>
        public static bool SetMark<T>(this T t, object value)
        {
            if (value == null || value == DBNull.Value) return false;
            if (value.ToInt() == 0) return false;

            var type = typeof(T);
            string key = type.TableKey();
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    if (column != key) continue;

                    if (properties[i].PropertyType == typeof(int))
                    {
                        properties[i].SetValue(t, value.ToInt(), null);
                    }
                    if (properties[i].PropertyType == typeof(long))
                    {
                        properties[i].SetValue(t, value.ToLong(), null);
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        ///     设置主键值
        /// </summary>
        public static bool SetMark<T>(this DataRow row, object value)
        {
            if (value == null || value == DBNull.Value) return false;
            if (value.ToInt() == 0) return false;

            var type = typeof(T);
            string key = type.TableKey();
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    if (column != key) continue;

                    row[properties[i].Name] = value;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Replace
        /// <summary>
        ///     将指定类型转为Replace语句
        ///     Sqlite更新、插入方法，需将Key键设为唯一索引
        /// </summary>
        public static string Replace<T>(this T t, string getId, params string[] args)
        {
            var attr = AttrTable(typeof(T));

            string insert = null;
            string value = null;
            t.Insert(attr, typeof(T), true, ref insert, ref value, args);
            var sql = string.Format("replace into [{0}]({1}) values({2})", attr.Table, insert, value);
            sql = string.Format("{0};{1}", sql, getId);
            return sql;
        }
        /// <summary>
        ///     将指定类型转为Replace语句
        ///     Sqlite更新、插入方法，需将Key键设为唯一索引
        /// </summary>
        public static string Replace<T>(this DataRow row, string getId, params string[] args)
        {
            var attr = AttrTable(typeof(T));

            string insert = null;
            string value = null;
            row.Insert<T>(attr, typeof(T), true, ref insert, ref value, args);
            var sql = string.Format("replace into [{0}]({1}) values({2})", attr.Table, insert, value);
            sql = string.Format("{0};{1}", sql, getId);
            return sql;
        }

        /// <summary>
        ///     将指定类型转为UpdateOrInsert语句
        /// </summary>
        public static string UpdateOrInsert<T>(this T t, string getid, params string[] args)
        {
            var type = typeof(T);
            var attr = type.Table();

            var sql = "if exists(select 0 from [{1}] where [{0}]=@{0})";
            sql = string.Format(sql, attr.Keys, attr.Table);

            string update = null;
            string insert = null;
            string values = null;
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    if (column == attr.Key) continue;
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    if (t.IsNull(properties[i]))
                    {
                        update = string.Format("{0}[{1}]=NULL,", update, column);
                    }
                    else
                    {
                        update = string.Format("{0}[{1}]=@{1},", update, column);
                        insert = string.Format("{0}[{1}],", insert, column);
                        values = string.Format("{0}@{1},", values, column);
                    }
                }
            }
            update = update.TrimEnd(',');
            insert = insert.TrimEnd(',');
            values = values.TrimEnd(',');
            sql = string.Format("{0} update [{1}] set {2} where {3}=@{3} else insert into [{1}]({4}) values({5})",
                sql, attr.Table, update, attr.Keys, insert, values);

            sql = string.Format("{0};{1}", sql, getid);
            return sql;
        }
        /// <summary>
        ///     将指定类型转为UpdateOrInsert语句
        /// </summary>
        public static string UpdateOrInsert<T>(this DataRow row, string getid, params string[] args)
        {
            var type = typeof(T);
            var attr = type.Table();

            var sql = "if exists(select 0 from [{1}] where [{0}]=@{0})";
            sql = string.Format(sql, attr.Keys, attr.Table);

            string update = null;
            string insert = null;
            string values = null;
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    if (column == attr.Key) continue;
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    if (row.IsNull(properties[i]))
                    {
                        update = string.Format("{0}[{1}]=NULL,", update, column);
                    }
                    else
                    {
                        update = string.Format("{0}[{1}]=@{1},", update, column);
                        insert = string.Format("{0}[{1}],", insert, column);
                        values = string.Format("{0}@{1},", values, column);
                    }
                }
            }
            update = update.TrimEnd(',');
            insert = insert.TrimEnd(',');
            values = values.TrimEnd(',');
            sql = string.Format("{0} update [{1}] set {2} where {3}=@{3} else insert into [{1}]({4}) values({5})",
                sql, attr.Table, update, attr.Keys, insert, values);

            sql = string.Format("{0};{1}", sql, getid);
            return sql;
        }

        #endregion

        #region AddParameter
        /// <summary>
        ///     添加参数值到参数列表
        ///     主键
        /// </summary>
        public static DbParameter AddParameter<T>(Type ptype, object value)
        {
            var asmb = Assembly.GetAssembly(ptype);
            var param = asmb.CreateInstance(ptype.FullName) as DbParameter;
            param.ParameterName = string.Format("@{0}", typeof(T).TableKey());
            param.Value = value;
            return param;
        }
        /// <summary>
        ///     添加参数值到参数列表
        ///     通用型
        /// </summary>
        public static List<DbParameter> AddParameters<T>(this T t, Type ptype, params string[] args)
        {
            var type = typeof(T);
            var asmb = Assembly.GetAssembly(ptype);
            var pList = new List<DbParameter>();

            var key = type.TableKey();
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                object value = null;
                if (!t.IsValue(properties[i], ref value)) continue;

                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    //Key必须要
                    if (key != column && args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    var param = asmb.CreateInstance(ptype.FullName) as DbParameter;
                    param.ParameterName = string.Format("@{0}", column);
                    param.Value = value;
                    pList.Add(param);
                }
            }
            return pList;
        }
        /// <summary>
        ///     添加参数值到参数列表
        ///     通用型
        /// </summary>
        public static List<DbParameter> AddParameters<T>(this DataRow row, Type ptype, params string[] args)
        {
            var type = typeof(T);
            var asmb = Assembly.GetAssembly(ptype);
            var pList = new List<DbParameter>();

            var key = type.TableKey();
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                object value = null;
                if (!row.IsValue(properties[i], ref value)) continue;

                var column = properties[i].Name;
                if (properties[i].IsSelect(ref column))
                {
                    //Key必须要
                    if (key != column && args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    var param = asmb.CreateInstance(ptype.FullName) as DbParameter;
                    param.ParameterName = string.Format("@{0}", column);
                    param.Value = value;
                    pList.Add(param);
                }
            }
            return pList;
        }

        #endregion

        #region 特性
        private static bool IsNull<T>(this T t, PropertyInfo prop)
        {
            var value = prop.GetValue(t, null);
            if (value == null || value == DBNull.Value) return true;

            if (prop.PropertyType == typeof(DateTime) && value is DateTime)
            {
                var dt = value.ToDateTime();
                if (dt == DateTime.MinValue) return true;
            }
            return false;
        }
        private static bool IsNull(this DataRow row, PropertyInfo prop)
        {
            var value = row[prop.Name];
            if (value == null || value == DBNull.Value) return prop.PropertyType.IsValueType;

            if (prop.PropertyType == typeof(DateTime) && value is DateTime)
            {
                var dt = value.ToDateTime();
                if (dt == DateTime.MinValue) return true;
            }
            return false;
        }

        private static bool IsValue<T>(this T t, PropertyInfo prop, ref object value)
        {
            value = prop.GetValue(t, null);
            if (value == null || value == DBNull.Value) return false;

            if (prop.PropertyType == typeof(Image) && value is Image)
            {
                value = SctructHelper.GetByteFromObject(value);
            }
            else if (prop.PropertyType == typeof(DateTime) && value is DateTime)
            {
                var dt = value.ToDateTime();
                if (dt == DateTime.MinValue) return false;
                value = dt;
            }
            return true;
        }
        private static bool IsValue(this DataRow row, PropertyInfo prop, ref object value)
        {
            value = row[prop.Name];
            if (value == null || value == DBNull.Value)
            {
                bool result = prop.PropertyType.IsValueType;
                if (result) value = Activator.CreateInstance(prop.PropertyType);
                return result;
            }

            if (prop.PropertyType == typeof(Image) && value is Image)
            {
                value = SctructHelper.GetByteFromObject(value);
            }
            else if (prop.PropertyType == typeof(DateTime) && value is DateTime)
            {
                var dt = value.ToDateTime();
                if (dt == DateTime.MinValue) return false;
                value = dt;
            }
            return true;
        }

        private static bool IsSelect(this PropertyInfo pro, ref string column)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (list.Length == 1 && list[0].Column != null)
            {
                column = list[0].Column;
            }
            return list.Length == 0 || list[0].Select;
        }
        /// <summary>
        /// </summary>
        public static bool ITable(this PropertyInfo pro, ref string column)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (list.Length == 1 && list[0].Column != null)
            {
                column = list[0].Column;
            }
            return list.Length == 0 || list[0].Select || list[0].Excel;
        }
        /// <summary>
        /// </summary>
        public static bool IExcel(this PropertyInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            return list.Length == 0 || list[0].Excel;
        }
        /// <summary>
        /// </summary>
        public static bool IShow(this PropertyInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            return list.Length == 0 || list[0].Show;
        }
        /// <summary>
        /// </summary>
        public static bool ISort(this PropertyInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            return list.Length == 0 || list[0].Sort;
        }
        private static bool IsClone(this PropertyInfo pro)
        {
            var list = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            return list.Length == 0 || list[0].Clone;
        }

        #endregion

        /// <summary>
        ///     返回特性，检查表名
        /// </summary>
        private static PropertyAttribute AttrTable(Type type)
        {
            var attrList = type.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
            if (attrList.Length != 1) throw new ArgumentException(string.Format("类型 {0} 特性错误", type));
            if (attrList[0].Table == null) throw new ArgumentException("没有指定表名称");
            return attrList[0];
        }

        /// <summary>
        ///     返回特性，检查表名及主键或主列
        /// </summary>
        private static PropertyAttribute Table(this Type type)
        {
            var attr = AttrTable(type);
            if (attr.Keys == null) throw new ArgumentException("没有指定主键或主列名称");
            return attr;
        }
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

        #endregion

        #region Clone
        /// <summary>
        ///     返回泛型实参数类型
        /// </summary>
        public static Type GetListType(this IList list)
        {
            var type = list.GetType();
            var types = type.GetGenericArguments();
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
        public static T Clone<T>(this T t, bool child, T copy = default(T))
        {
            var type = typeof(T);
            var asmb = Assembly.GetAssembly(type);
            if (copy == null)
                copy = (T)asmb.CreateInstance(type.FullName);
            type.Clone(ref copy, t, child);

            return (T)copy;
        }

        /// <summary>
        ///     复制子级
        /// </summary>
        public static void Clone<T>(this Type parent, ref T copy, object t, bool child)
        {
            var properties = parent.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                if (!properties[i].IsClone()) continue;

                var value = properties[i].GetValue(t, null);
                if (properties[i].CanWrite)
                    properties[i].SetValue(copy, value, null);
                if (!child) continue;
                if (value is IList)
                {
                    var clist = properties[i].GetValue(copy, null) as IList;
                    var list = value as IList;
                    var type = list.GetListType();
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
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                if (aysc && !properties[i].IsClone()) continue;
                copy[properties[i].Name] = row[properties[i].Name];
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
            form.SizeChanged -= form_SizeChanged;
            form.SizeChanged += form_SizeChanged;
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

        private static void form_SizeChanged(object sender, EventArgs e)
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
        public static int TCompare(this PropertyInfo pro, object obj1, object obj2)
        {
            if ((obj1 == null || obj1 == DBNull.Value) && (obj2 == null || obj2 == DBNull.Value)) return 0;
            if (obj1 == null || obj1 == DBNull.Value) return -1;
            if (obj2 == null || obj2 == DBNull.Value) return 1;
            if (pro.PropertyType == typeof(Image))
            {
                return 0;
            }
            else if (pro.PropertyType == typeof(int))
            {
                return ((int)obj1).CompareTo((int)obj2);
            }
            else if (pro.PropertyType == typeof(long))
            {
                return ((long)obj1).CompareTo((long)obj2);
            }
            else if (pro.PropertyType == typeof(double))
            {
                return ((double)obj1).CompareTo((double)obj2);
            }
            else if (pro.PropertyType == typeof(float))
            {
                return ((float)obj1).CompareTo((float)obj2);
            }
            else if (pro.PropertyType == typeof(bool))
            {
                return ((bool)obj1).CompareTo((bool)obj2);
            }
            else if (pro.PropertyType == typeof(DateTime))
            {
                return (obj1.ToDateTime()).CompareTo(obj2.ToDateTime());
            }
            else if (pro.PropertyType == typeof(string))
            {
                return (obj1.ToString2()).CompareTo(obj2.ToString2());
            }
            else return 0;
        }

        #endregion
    }
}