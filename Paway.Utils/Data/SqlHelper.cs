using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Paway.Utils.Data
{
    /// <summary>
    /// SQL操作基类
    /// </summary>
    public abstract class SqlHelper : DataBase
    {
        /// <summary>
        /// 连接字符模板
        /// Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};
        /// </summary>
        protected const string DbConnect = @"Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};";

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public SqlHelper()
            : base(typeof(SqlConnection), typeof(SqlCommand), typeof(SqlParameter))
        {
            base.GetId = "select @@IDENTITY Id";
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

        #region 扩展.分步
        /// <summary>
        /// 打开一个连接
        /// </summary>
        /// <returns></returns>
        protected new SqlCommand CommandStart()
        {
            return base.CommandStart() as SqlCommand;
        }
        /// <summary>
        /// 打开一个连接
        /// </summary>
        /// <returns></returns>
        protected new SqlCommand CommandStart(string sql)
        {
            return base.CommandStart(sql) as SqlCommand;
        }
        /// <summary>
        /// 事务处理
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        /// <returns></returns>
        protected new SqlCommand TransStart()
        {
            return base.TransStart() as SqlCommand;
        }

        #endregion
    }
}
