using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;

namespace Paway.Helper
{
    /// <summary>
    ///     Excel文件导入导出DataTable
    /// </summary>
    public abstract class ExcelHelper
    {
        /// <summary>
        ///     从Excel导入DataTable
        /// </summary>
        public static DataTable ImportExcel(string fileName, string sheet)
        {
            return ImportExcel(fileName, sheet, true);
        }

        /// <summary>
        ///     从Excel导入DataTable
        ///     HDR=yes 第一行是列名而不是数据
        /// </summary>
        public static DataTable ImportExcel(string fileName, string sheet, bool hdd)
        {
            var conString =
                string.Format(
                    "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'", fileName,
                    hdd ? "yes" : "no");
            var sql = string.Format("select * from [{0}$]", sheet);
            using (var con = new OleDbConnection(conString))
            {
                con.Open();
                using (var cmd = new OleDbDataAdapter(sql, con))
                {
                    var ds = new DataSet();
                    cmd.Fill(ds);
                    var dt = ds.Tables[0];
                    return dt;
                }
            }
        }

        /// <summary>
        ///     更新表列名
        ///     实体类中列名与表名一一对应，无则Excel=false
        /// </summary>
        public static void UpdateColumn<T>(DataTable dt)
        {
            var type = typeof(T);
            var properties = TypeDescriptor.GetProperties(type);
            var index = 0;
            for (var i = 0; i < properties.Count; i++)
            {
                var pro = type.GetProperty(properties[i].Name, properties[i].PropertyType);
                var itemList = pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                if (itemList == null || itemList.Length == 0 || itemList[0].Excel)
                {
                    dt.Columns[index++].ColumnName = properties[i].Name;
                }
            }
        }

        /// <summary>
        ///     将DataTable导出到Excel
        /// </summary>
        public static void ExportExcel<T>(DataTable dt, string fileName, string sheet)
        {
            ExportExcel<T>(dt, fileName, sheet, false);
        }

        /// <summary>
        ///     将DataTable导出到Excel
        ///     HDR=yes 第一行写入列标题
        /// </summary>
        public static void ExportExcel<T>(DataTable dt, string fileName, string sheet, bool hdd)
        {
            var conString =
                string.Format(
                    "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=no'", fileName);
            OleDbCommand cmd = null;
            try
            {
                var con = new OleDbConnection(conString);
                con.Open();
                var trans = con.BeginTransaction();
                cmd = new OleDbCommand();
                cmd.Connection = con;
                cmd.Transaction = trans;

                string insert = null;
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    insert = string.Format("{0}F{1},", insert, i + 1);
                }
                insert = insert.TrimEnd(',');
                string sql = null;
                //写入列标题
                if (hdd)
                {
                    var type = typeof(T);
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        var pro = type.GetProperty(dt.Columns[i].ColumnName);
                        var name = dt.Columns[i].ColumnName;
                        if (pro != null)
                        {
                            var itemList =
                                pro.GetCustomAttributes(typeof(PropertyAttribute), false) as PropertyAttribute[];
                            if (itemList != null && itemList[0].Text != null)
                            {
                                name = itemList[0].Text;
                            }
                        }
                        sql = string.Format("{0}'{1}',", sql, name.Replace("'", "''"));
                    }
                    sql = sql.TrimEnd(',');
                    sql = string.Format("insert into [{0}$]({1}) values({2})", sheet, insert, sql);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                //写入数据
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    sql = null;
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        sql = string.Format("{0}'{1}',", sql, dr[j].ToString().Replace("'", "''"));
                    }
                    sql = sql.TrimEnd(',');
                    sql = string.Format("insert into [{0}$]({1}) values({2})", sheet, insert, sql);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (cmd != null && cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (cmd != null)
                {
                    if (cmd.Connection != null)
                    {
                        cmd.Connection.Close();
                        cmd.Connection.Dispose();
                    }
                    cmd.Dispose();
                }
            }
        }
    }
}