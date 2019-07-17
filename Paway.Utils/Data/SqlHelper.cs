using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Paway.Utils
{
    /// <summary>
    /// SQL操作基类
    /// </summary>
    public class SqlHelper : DataBase
    {
        /// <summary>
        /// 连接字符模板
        /// Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Pooling=true;Max Pool Size=3000;Min Pool Size=0;
        /// </summary>
        protected const string DbConnect =
            @"Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Pooling=true;Max Pool Size=3000;Min Pool Size=0;";

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        public SqlHelper()
            : base(typeof(SqlConnection), typeof(SqlCommand), typeof(SqlParameter))
        {
            GetId = "select @@IDENTITY Id";
        }

        /// <summary>
        /// 传入连接字符
        /// </summary>
        /// <param name="connectName"></param>
        protected void InitConnect(string connectName)
        {
            ConnString = ConfigurationManager.ConnectionStrings[connectName].ConnectionString;
        }

        /// <summary>
        /// 传入连接参数
        /// Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};
        /// </summary>
        protected void InitConnect(string host, string source, string user, string pad)
        {
            ConnString = string.Format(DbConnect, host, source, user, pad);
        }

        #endregion

        #region 扩展重载
        /// <summary>
        /// 查找指定查询语句
        /// 填充 System.Data.DataSet 并返回一个DataTable
        /// 标记是否使用Limit查找指定数量
        /// </summary>
        protected override DataTable FindTable<T>(string find, int count, bool iLimit, DbCommand cmd = null, params string[] args)
        {
            return base.FindTable<T>(find, count, false, cmd, args);
        }

        #endregion
    }
}