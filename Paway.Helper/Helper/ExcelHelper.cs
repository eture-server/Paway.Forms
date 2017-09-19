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
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'", fileName,
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
            var index = 0;
            var properties = type.GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var name = properties[i].Name;
                if (properties[i].ITable(ref name))
                {
                    dt.Columns[index++].ColumnName = name;
                }
            }
        }

        /// <summary>
        ///     将DataTable导出到Excel
        ///     HDR=yes 第一行写入列标题
        /// </summary>
        /// <param name="table">数据源</param>
        /// <param name="fileName">excel2003文件名</param>
        /// <param name="sheet">工作薄名称</param> 
        /// <param name="title">文件描述,在F1=0</param>
        public static void ExportExcel(DataTable table, string fileName, string sheet, string title = null)
        {
            var conString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=no'", fileName);
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
                string values = null;
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    insert = string.Format("{0}F{1},", insert, i + 1);
                    values = string.Format("{0}@F{1},", values, i + 1);
                }
                insert = insert.TrimEnd(',');
                values = values.TrimEnd(',');
                //写入标题
                if (!string.IsNullOrEmpty(title))
                {
                    string update = string.Format("update [{0}$] set F1 = '{1}' where F1 = (select top 1 F1 from [{0}$])", sheet, title);
                    cmd.CommandText = update;
                    cmd.ExecuteNonQuery();
                }
                //写入数据
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    var dr = table.Rows[i];
                    var pList = new OleDbParameter[table.Columns.Count];
                    for (var j = 0; j < table.Columns.Count; j++)
                    {
                        Type type = dr[j].GetType();
                        var param = new OleDbParameter();
                        param.ParameterName = string.Format("@F{0}", j + 1);
                        param.Value = dr[j];
                        pList[j] = param;
                    }
                    string sql = string.Format("insert into [{0}$]({1}) values({2})", sheet, insert, values);
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch
            {
                if (cmd != null && cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                throw;
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