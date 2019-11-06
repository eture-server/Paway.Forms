using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;

namespace Paway.Utils
{
    /// <summary>
    /// Oracle操作基类
    /// </summary>
    public abstract class OracleHelper : DataBase
    {
        #region 初始化
        /// <summary>
        /// 构造
        /// </summary>
        public OracleHelper() : base(typeof(OracleConnection), typeof(OracleCommand), typeof(OracleParameter)) { }

        #endregion

        #region 重载
        /// <summary>
        /// 对sql语句进行过滤
        /// </summary>
        protected override string OnCommandText(string sql)
        {
            if (sql == null) throw new ArgumentNullException();
            return sql.Replace("[", "").Replace("]", "");
        }

        #endregion
    }
}