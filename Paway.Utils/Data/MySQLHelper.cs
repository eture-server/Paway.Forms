using MySQLDriverCS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Paway.Helper;
using System.Data.Common;

namespace Paway.Utils.Data
{
    /// <summary>
    /// MySql操作基类
    /// </summary>
    public class MySQLHelper : DataBase
    {
        #region 初始化
        /// <summary>
        /// 初始化
        /// 没有
        /// </summary>
        public MySQLHelper()
            : base(typeof(MySQLConnection), typeof(MySQLCommand), typeof(MySQLParameter))
        {
            base.GetId = "SELECT LAST_INSERT_ID() Id";
        }
        /// <summary>
        /// 传入连接字符
        /// </summary>
        /// <param name="connect"></param>
        protected void InitConnect(string connect)
        {
            ConnString = connect;
        }
        /// <summary>
        /// 传入连接字符
        /// </summary>
        protected void InitConnect(string server, string database, string root, string password)
        {
            ConnString = new MySQLConnectionString(server, database, root, password).AsString;
        }
        /// <summary>
        /// 传入连接字符
        /// </summary>
        protected void InitConnect(string server, string database, string root, string password, int port)
        {
            ConnString = new MySQLConnectionString(server, database, root, password, port).AsString;
        }

        #endregion

        #region 重载
        /// <summary>
        /// 对sql语句进行过滤
        /// </summary>
        protected override void OnCommandText(DbCommand cmd)
        {
            if (cmd.CommandText != null)
                cmd.CommandText = cmd.CommandText.Replace("[", "").Replace("]", "");
        }
        protected override DataTable CreateTable(DataTable temp)
        {
            DataTable dt = new DataTable();
            foreach (DataColumn dc in temp.Columns)
            {
                String s = dc.ToString();
                DataColumn dcNew = new DataColumn(s);
                dt.Columns.Add(dcNew);
            }
            return dt;
        }

        #endregion

        #region 扩展.分步
        /// <summary>
        /// 打开一个连接
        /// </summary>
        protected new MySQLCommand CommandStart()
        {
            return base.CommandStart() as MySQLCommand;
        }
        /// <summary>
        /// 打开一个连接
        /// </summary>
        /// <returns></returns>
        protected new MySQLCommand CommandStart(string sql)
        {
            return base.CommandStart(sql) as MySQLCommand;
        }
        /// <summary>
        /// 事务处理
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        /// <returns></returns>
        protected new MySQLCommand TransStart()
        {
            return base.TransStart() as MySQLCommand;
        }

        #endregion
    }
}
