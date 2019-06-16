using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Data;

namespace Paway.Utils
{
    /// <summary>
    ///     MySql操作基类
    /// </summary>
    public class MySqlHelper : DataBase
    {
        /// <summary>
        ///     连接字符模板
        ///     Data Source={0};Database={1};User Id={2};Password={3};Persist Security Info=True;pooling=false;CharSet=utf8;port={4}
        /// </summary>
        protected const string DbConnect =
            @"Data Source={0};Database={1};User Id={2};Password={3};Persist Security Info=True;pooling=false;CharSet=utf8;port={4}";

        #region 重载
        /// <summary>
        ///     对sql语句进行过滤
        /// </summary>
        protected override void OnCommandText(DbCommand cmd)
        {
            if (cmd.CommandText != null)
                cmd.CommandText = cmd.CommandText.Replace("[", "`").Replace("]", "`");
        }

        #endregion

        #region 初始化
        /// <summary>
        ///     初始化
        ///     没有
        /// </summary>
        public MySqlHelper()
            : base(typeof(MySqlConnection), typeof(MySqlCommand), typeof(MySqlParameter))
        {
            base.GetId = "SELECT LAST_INSERT_ID() Id";
            base.ILongConnect = true;
        }

        /// <summary>
        ///     传入连接字符
        /// </summary>
        /// <param name="connect"></param>
        protected void InitConnect(string connect)
        {
            ConnString = connect;
        }

        /// <summary>
        ///     传入连接字符
        /// </summary>
        protected void InitConnect(string host, string database, string user, string pad, int port = 3306)
        {
            ConnString = string.Format(DbConnect, host, database, user, pad, port);
        }

        #endregion

        #region 扩展重载
        /// <summary>
        ///     查找指定查询语句
        ///     填充 System.Data.DataSet 并返回一个DataTable
        ///     标记是否使用Limit查找指定数量
        /// </summary>
        protected override DataTable FindTable<T>(string find, int count, bool iLimit, DbCommand cmd = null, params string[] args)
        {
            return base.FindTable<T>(find, count, true, cmd, args);
        }

        #endregion
    }
}