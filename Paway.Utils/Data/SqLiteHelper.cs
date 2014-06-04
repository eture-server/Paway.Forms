using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using Paway.Helper;

namespace Paway.Utils.Data
{
    /// <summary>
    /// SqLite操作基类
    /// </summary>
    public abstract class SqLiteHelper : DataBase
    {
        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public SqLiteHelper()
        {
            base.GetId = "select LAST_INSERT_ROWID() Id";
        }
        /// <summary>
        /// 文件名
        /// </summary>
        protected string FileName { get; private set; }

        /// <summary>
        /// 初始化连接字符串
        /// </summary>
        /// <param name="fileName"></param>
        protected void InitConnect(string fileName)
        {
            this.FileName = fileName;
            string path = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            ConnString = GetConnString(fileName);
        }

        /// <summary>
        /// 根据指定资源sql语句创建数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected bool InitCreate(string sql)
        {
            if (File.Exists(this.FileName)) return false;

            List<string> list = new List<string>();
            string[] sqlList = sql.Split(new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < sqlList.Length; i++)
            {
                string cmdLine = sqlList[i].Trim();
                if (string.IsNullOrEmpty(cmdLine)) continue;
                list.Add(cmdLine);
            }
            return TransExecuteNonQuery(list);
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected string GetConnString(string fileName)
        {
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = fileName;
            sb.SyncMode = SynchronizationModes.Off;
            sb.PageSize = 4096;
            sb.CacheSize = 70 * 1024;
            return sb.ConnectionString;
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
                using (SQLiteConnection conn = new SQLiteConnection(ConnString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
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
                using (SQLiteConnection conn = new SQLiteConnection(ConnString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
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
                using (SQLiteConnection con = new SQLiteConnection(ConnString))
                {
                    con.Open();
                    SQLiteTransaction trans = con.BeginTransaction();
                    using (SQLiteCommand cmd = new SQLiteCommand())
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
        protected SQLiteCommand CommandStart()
        {
            return CommandStart(null);
        }
        /// <summary>
        /// 打开一个连接
        /// 返回SQLiteCommand实例
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected SQLiteCommand CommandStart(string sql)
        {
            return base.CommandStart(sql, typeof(SQLiteConnection), typeof(SQLiteCommand)) as SQLiteCommand;
        }

        /// <summary>
        /// 事务处理
        /// 打开一个连接
        /// 返回SQLiteCommand实例
        /// </summary>
        /// <returns></returns>
        protected SQLiteCommand TransStart()
        {
            return base.TransStart(typeof(SQLiteConnection), typeof(SQLiteCommand)) as SQLiteCommand;
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
                using (SQLiteConnection con = new SQLiteConnection(ConnString))
                {
                    con.Open();
                    sql = default(T).Select(find);
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con))
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
            return base.Insert<T>(list, typeof(SQLiteConnection), typeof(SQLiteCommand), typeof(SQLiteParameter));
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
            return base.Update<T>(list, typeof(SQLiteConnection), typeof(SQLiteCommand), typeof(SQLiteParameter));
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
            return base.Delete<T>(list, typeof(SQLiteConnection), typeof(SQLiteCommand), typeof(SQLiteParameter));
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
            return base.UpdateOrInsert<T>(list, typeof(SQLiteConnection), typeof(SQLiteCommand), typeof(SQLiteParameter));
        }

        #endregion
    }
}
