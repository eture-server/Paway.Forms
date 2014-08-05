using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// Excel文件导入导出DataTable
    /// </summary>
    public abstract class ExcelHelper
    {
        /// <summary>
        /// 从Excel导入DataTable
        /// </summary>
        public static DataTable ImportExcel(string fileName, string sheet)
        {
            string conString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;'", fileName);
            string sql = string.Format("select * from [{0}$]", sheet);
            using (OleDbConnection con = new OleDbConnection(conString))
            {
                con.Open();
                using (OleDbDataAdapter cmd = new OleDbDataAdapter(sql, con))
                {
                    DataSet ds = new DataSet();
                    cmd.Fill(ds);
                    con.Close();
                    DataTable dt = ds.Tables[0];
                    return dt;
                }
            }
        }
        /// <summary>
        /// 将DataTable导出到Excel
        /// </summary>
        public static void ExportExcel(DataTable dt, string fileName, string sheet)
        {
            String conString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", fileName);
            OleDbCommand cmd = null;
            try
            {
                OleDbConnection con = new OleDbConnection(conString);
                con.Open();
                OleDbTransaction trans = con.BeginTransaction();
                cmd = new OleDbCommand();
                cmd.Connection = con;
                cmd.Transaction = trans;

                string insert = null;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    insert = string.Format("{0}{1},", insert, dt.Columns[i].ColumnName);
                }
                insert = insert.TrimEnd(',');
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string sql = null;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sql = string.Format("{0}'{1}',", sql, dr[j]);
                    }
                    sql = sql.TrimEnd(',');
                    sql = string.Format("insert into [{0}$]({1}) values({2})", sheet, insert, sql);
                    cmd.CommandText = sql;
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
