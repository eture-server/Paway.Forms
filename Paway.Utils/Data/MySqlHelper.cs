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
    public class MySqlHelper : DataBase
    {
        private MySQLConnection conn;
        /// <summary>
        /// 初始化
        /// 没有
        /// </summary>
        public MySqlHelper()
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
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="root"></param>
        /// <param name="password"></param>
        protected void InitConnect(string server, string database, string root, string password)
        {
            ConnString = new MySQLConnectionString(server, database, root, password).AsString;

        }
        /// <summary>
        /// 传入连接字符
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="root"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        protected void InitConnect(string server, string database, string root, string password, int port)
        {
            ConnString = new MySQLConnectionString(server, database, root, password, port).AsString;
        }
        /// <summary>
        /// 连接实例
        /// </summary>
        /// <returns></returns>
        protected MySQLConnection GetCon()
        {
            if (conn == null)
            {
                conn = new MySQLConnection(ConnString);
            }
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

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
                using (MySQLConnection con = new MySQLConnection(ConnString))
                {
                    con.Open();
                    using (MySQLCommand cmd = new MySQLCommand(sql, con))
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
                using (MySQLConnection con = new MySQLConnection(ConnString))
                {
                    con.Open();
                    using (MySQLCommand cmd = new MySQLCommand(sql, con))
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
                using (MySQLConnection con = new MySQLConnection(ConnString))
                {
                    con.Open();
                    DbTransaction trans = con.BeginTransaction();
                    using (MySQLCommand cmd = new MySQLCommand())
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
        protected MySQLCommand CommandStart()
        {
            return CommandStart(null);
        }
        /// <summary>
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected MySQLCommand CommandStart(string sql)
        {
            return base.CommandStart(sql, typeof(MySQLConnection), typeof(MySQLCommand)) as MySQLCommand;
        }

        /// <summary>
        /// 事务处理
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        /// <returns></returns>
        protected MySQLCommand TransStart()
        {
            return base.TransStart(typeof(MySQLConnection), typeof(MySQLCommand)) as MySQLCommand;
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
                using (MySQLConnection con = new MySQLConnection(ConnString))
                {
                    con.Open();
                    sql = default(T).Select(find);
                    using (MySQLDataAdapter da = new MySQLDataAdapter(sql, con))
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
            return base.Insert<T>(list, typeof(MySQLConnection), typeof(MySQLCommand), typeof(MySQLParameter));
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
            return base.Update<T>(list, typeof(MySQLConnection), typeof(MySQLCommand), typeof(MySQLParameter));
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
            return base.Delete<T>(list, typeof(MySQLConnection), typeof(MySQLCommand), typeof(MySQLParameter));
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
            return base.UpdateOrInsert<T>(list, typeof(MySQLConnection), typeof(MySQLCommand), typeof(MySQLParameter));
        }

        #endregion
    }
}
