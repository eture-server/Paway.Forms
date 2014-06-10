using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Paway.Helper;
using System.Reflection;
using System.Data.Common;

namespace Paway.Utils.Data
{
    /// <summary>
    /// SQL操作基类
    /// </summary>
    public abstract class SqlHelper : DataBase
    {
        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public SqlHelper()
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

        #endregion

        #region ExecuteNonQuery,ExecuteScalar,TransExecuteNonQuery
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected int ExecuteNonQuery(string sql)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ExecuteNonQuery.Error[{0}]\r\n{1}", sql, ex));
                throw;
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected object ExecuteScalar(string sql)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ExecuteScalar.Error[{0}]\r\n{1}", sql, ex));
                throw;
            }
        }

        /// <summary>
        /// 使用事务处理  Transact-SQL 语句列表
        /// </summary>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        protected bool TransExecuteNonQuery(List<string> sqlList)
        {
            bool error = true;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    con.Open();
                    SqlTransaction trans = con.BeginTransaction();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.Transaction = trans;
                        try
                        {
                            for (int i = 0; i < sqlList.Count; i++)
                            {
                                cmd.CommandText = sqlList[i];
                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            error = false;
                            log.Error(string.Format("TransExecuteNonQuery.Execute.Error[{0}]\r\n{1}", cmd.CommandText, ex));
                            throw;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (error) log.Error(string.Format("TransExecuteNonQuery.Other.Error\r\n{0}", ex));
                throw;
            }
        }

        #endregion

        #region 扩展.分步
        /// <summary>
        /// 打开一个连接
        /// </summary>
        /// <returns></returns>
        protected SqlCommand CommandStart()
        {
            return CommandStart(null);
        }
        /// <summary>
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected SqlCommand CommandStart(string sql)
        {
            return base.CommandStart(sql, typeof(SqlConnection), typeof(SqlCommand)) as SqlCommand;
        }

        /// <summary>
        /// 事务处理
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        /// <returns></returns>
        protected SqlCommand TransStart()
        {
            return base.TransStart(typeof(SqlConnection), typeof(SqlCommand)) as SqlCommand;
        }

        #endregion

        #region 扩展.语句
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个IList列表
        /// </summary>
        public IList<T> Find<T>()
        {
            return Find<T>(null);
        }
        /// <summary>
        /// 查找指定查询语句
        /// 填充 System.Data.DataSet 并返回一个IList列表
        /// </summary>
        /// <typeparam name="T">class.Type</typeparam>
        /// <param name="find">查询条件</param>
        /// <returns></returns>
        public IList<T> Find<T>(string find)
        {
            string sql = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    con.Open();
                    sql = default(T).Select(find);
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        IList<T> list = dt.ConvertTo<T>();
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ExecuteList.Error[{0}]\r\n{1}", sql, ex));
                throw;
            }
        }
        /// <summary>
        /// 插入列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Insert<T>(T t)
        {
            List<T> list = new List<T>() { t };
            return Insert<T>(list);
        }
        /// <summary>
        /// 插入列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Insert<T>(IList<T> list)
        {
            return base.Insert<T>(list, typeof(SqlConnection), typeof(SqlCommand), typeof(SqlParameter));
        }
        /// <summary>
        /// 更新列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Update<T>(T t)
        {
            List<T> list = new List<T>() { t };
            return Update<T>(list);
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Update<T>(IList<T> list)
        {
            return base.Update<T>(list, typeof(SqlConnection), typeof(SqlCommand), typeof(SqlParameter));
        }
        /// <summary>
        /// 删除列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Delete<T>(T t)
        {
            List<T> list = new List<T>() { t };
            return Delete<T>(list);
        }
        /// <summary>
        /// 删除列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Delete<T>(IList<T> list)
        {
            return base.Delete<T>(list, typeof(SqlConnection), typeof(SqlCommand), typeof(SqlParameter));
        }
        /// <summary>
        /// 更新或插入列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        public void UpdateOrInsert<T>(DataTable dt)
        {
            IList<T> list = dt.ConvertTo<T>();
            UpdateOrInsert<T>(list);
        }
        /// <summary>
        /// 更新或插入列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool UpdateOrInsert<T>(T t)
        {
            List<T> list = new List<T>() { t };
            return UpdateOrInsert<T>(list);
        }
        /// <summary>
        /// 更新或插入列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpdateOrInsert<T>(IList<T> list)
        {
            return base.UpdateOrInsert<T>(list, typeof(SqlConnection), typeof(SqlCommand), typeof(SqlParameter));
        }

        #endregion
    }
}
