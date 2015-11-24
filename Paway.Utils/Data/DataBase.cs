using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Paway.Helper;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;

namespace Paway.Utils.Data
{
    /// <summary>
    /// 数据服务基类，不可创建实例
    /// </summary>
    public abstract class DataBase : IDisposable
    {
        /// <summary>
        /// 返回最新插入列主键Id
        /// </summary>
        protected string GetId { get; set; }

        /// <summary>
        /// </summary>
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string ConnString { get; set; }

        #region 构造.加载数据类型
        private Type connType;
        private Type cmdType;
        private Type paramType;
        /// <summary>
        /// 数据类型
        /// </summary>
        /// <param name="connType">连接类型</param>
        /// <param name="cmdType">执行</param>
        /// <param name="paramType">参数</param>
        protected DataBase(Type connType, Type cmdType, Type paramType)
        {
            if (!Licence.Checking()) return;

            this.connType = connType;
            this.cmdType = cmdType;
            this.paramType = paramType;
        }
        /// <summary>
        /// 对sql语句进行过滤
        /// </summary>
        protected virtual void OnCommandText(DbCommand cmd) { }

        #endregion

        #region 扩展.方法
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        public int ExecuteNonQuery(string sql, DbCommand cmd = null)
        {
            bool iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = sql;
                OnCommandText(cmd);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ExecuteNonQuery.Error[{0}]\r\n{1}", sql, ex));
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        public object ExecuteScalar(string sql, DbCommand cmd = null)
        {
            bool iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = sql;
                OnCommandText(cmd);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ExecuteScalar.Error[{0}]\r\n{1}", sql, ex));
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的DataTable
        /// </summary>
        public DataTable ExecuteDataTable(string sql, DbCommand cmd = null)
        {
            bool iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = sql;
                OnCommandText(cmd);
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    table.Load(dr);
                    return table;
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("ExecuteDataTable.Error[{0}]\r\n{1}", sql, ex));
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 使用事务处理  Transact-SQL 语句列表
        /// </summary>
        public bool TransExecuteNonQuery(List<string> sqlList, DbCommand cmd = null)
        {
            bool iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                for (int i = 0; i < sqlList.Count; i++)
                {
                    cmd.CommandText = sqlList[i];
                    OnCommandText(cmd);
                    cmd.ExecuteNonQuery();
                }
                if (iTrans) return TransCommit(cmd);
                else return true;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        #endregion

        #region 扩展.分步
        /// <summary>
        /// 打开一个连接
        /// </summary>
        /// <returns></returns>
        protected DbCommand CommandStart()
        {
            return CommandStart(null);
        }
        /// <summary>
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        protected DbCommand CommandStart(string sql)
        {
            try
            {
                DbConnection con = GetCon();
                DbCommand cmd = GetCmd();
                cmd.CommandText = sql;
                OnCommandText(cmd);
                cmd.Connection = con;
                return cmd;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("StartCommand.Error[{0}]\r\n{1}", sql, ex));
                throw new Exception(string.Empty, ex);
            }
        }
        /// <summary>
        /// 关闭DbCommand实例的连接，并释放
        /// </summary>
        /// <param name="cmd"></param>
        protected void CommandEnd(DbCommand cmd)
        {
            try
            {
                if (cmd != null)
                {
                    if (cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                    {
                        cmd.Connection.Close();
                        cmd.Connection.Dispose();
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("EndCommand.Error[{0}]\r\n{1}", cmd.CommandText, ex));
                throw new Exception(string.Empty, ex);
            }
        }

        /// <summary>
        /// 事务处理
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        /// <returns></returns>
        protected DbCommand TransStart()
        {
            try
            {
                DbConnection con = GetCon();
                DbTransaction trans = con.BeginTransaction();
                DbCommand cmd = GetCmd();
                cmd.Connection = con;
                cmd.Transaction = trans;

                return cmd;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("TransStartCommand.Error\r\n{0}", ex));
                throw new Exception(string.Empty, ex);
            }
        }
        /// <summary>
        /// 事务处理.提交事务
        /// </summary>
        /// <param name="cmd"></param>
        protected bool TransCommit(DbCommand cmd)
        {
            if (cmd == null || cmd.Connection == null || cmd.Transaction == null) return false;
            try
            {
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                TransError(cmd, ex);
                throw new Exception(string.Empty, ex);
            }
        }
        /// <summary>
        /// 事务处理异常回退
        /// 关闭DbCommand实例的连接，并释放
        /// </summary>
        protected void TransError(DbCommand cmd, Exception e)
        {
            if (cmd == null || cmd.Connection == null || cmd.Transaction == null) return;
            try
            {
                cmd.Transaction.Rollback();
                log.Error(string.Format("TransCommand.Execute.Error[{0}]\r\n{1}", cmd.CommandText, e));
            }
            catch (Exception ex)
            {
                log.Error(string.Format("TransError.Error{0}\r\n{1}", cmd.CommandText, ex));
                throw new Exception(string.Empty, ex);
            }
        }

        private DbConnection GetCon()
        {
            Assembly asmb = Assembly.GetAssembly(connType);
            DbConnection con = asmb.CreateInstance(connType.FullName) as DbConnection;
            con.ConnectionString = ConnString;
            con.Open();
            return con;
        }
        private DbCommand GetCmd()
        {
            Assembly asmb = Assembly.GetAssembly(cmdType);
            DbCommand cmd = asmb.CreateInstance(cmdType.FullName) as DbCommand;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        #endregion

        #region 扩展.语句
        #region Find
        /// <summary>
        /// 查找指定主列的数据
        /// </summary>
        public T Find<T>(long id, DbCommand cmd = null, params string[] args)
        {
            bool iTrans = cmd == null;
            string sql = null;
            try
            {
                if (iTrans) cmd = CommandStart();
                sql = default(T).Select(args);

                DbParameter parame = default(T).AddParameter<T>(paramType, id);
                cmd.CommandText = sql;
                OnCommandText(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(parame);

                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    IList<T> list = LoadDr<T>(dr, 1);
                    return list.Count == 1 ? list[0] : default(T);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Find.Error[{0}]\r\n{1}", sql, ex));
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个IList列表
        /// </summary>
        public IList<T> Find<T>(DbCommand cmd = null, params string[] args)
        {
            return FindTop<T>(null, 0, false, cmd, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个IList列表
        /// 查找指定查询语句
        /// </summary>
        public IList<T> Find<T>(string find, DbCommand cmd = null, params string[] args)
        {
            return FindTop<T>(find, 0, false, cmd, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个IList列表
        /// 查找指定查询语句
        /// 指定返回行数
        /// </summary>
        public virtual IList<T> FindTop<T>(string find, int count, DbCommand cmd = null, params string[] args)
        {
            return FindTop<T>(find, count, false, cmd, args);
        }
        /// <summary> 
        /// 填充 System.Data.DataSet 并返回一个IList列表
        /// 查找指定查询语句
        /// 指定返回行数
        /// 是否SQLite
        /// </summary>
        protected IList<T> FindTop<T>(string find, int count, bool isSQLite, DbCommand cmd = null, params string[] args)
        {
            bool iTrans = cmd == null;
            string sql = null;
            try
            {
                if (iTrans) cmd = CommandStart();

                if (isSQLite)
                {
                    sql = default(T).Select(find, args);
                    if (count > 0)
                    {
                        sql = string.Format("{0} limit {1}", sql, count);
                    }
                }
                else
                {
                    sql = default(T).Select(find, count, args);
                }
                cmd.CommandText = sql;
                OnCommandText(cmd);
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    return LoadDr<T>(dr);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Find.Error[{0}]\r\n{1}", sql, ex));
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }
        private IList<T> LoadDr<T>(DbDataReader dr, int count = int.MaxValue)
        {
            DataTable temp = dr.GetSchemaTable();
            DataTable dt = new DataTable();
            foreach (DataRow ilRow in temp.Rows)/*建表*/
            {
                /*获取这个字段的类型*/
                Type TilRowType = Type.GetType(ilRow["DataType"].ToString());
                dt.Columns.Add(ilRow["ColumnName"].ToString(), TilRowType);
            }
            DataRow row = dt.NewRow();
            IList<T> list = new List<T>();
            for (int i = 0; dr.Read() && i < count; i++)
            {
                T info = dr.CreateItem<T>(row);
                list.Add(info);
            }
            return list;
        }

        #endregion

        #region Insert
        /// <summary>
        /// 插入列
        /// </summary>
        public bool Insert<T>(T t, DbCommand cmd = null, bool Identity = false)
        {
            List<T> list = new List<T>() { t };
            return Insert<T>(list, cmd, Identity);
        }
        /// <summary>
        /// 插入列表
        /// </summary>
        public bool Insert<T>(DataTable dt, DbCommand cmd = null, bool Identity = false)
        {
            IList<T> list = dt.ToIList<T>();
            return Insert<T>(list, cmd, Identity);
        }
        /// <summary>
        /// 插入列表
        /// </summary>
        public bool Insert<T>(IList<T> list, DbCommand cmd = null, bool Identity = false)
        {
            bool iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                for (int i = 0; i < list.Count; i++)
                {
                    string sql = list[i].Insert<T>(GetId, Identity);
                    cmd.CommandText = sql;
                    OnCommandText(cmd);
                    DbParameter[] pList = list[i].AddParameters<T>(paramType).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            list[i].SetMark<T>(dr[0]);
                        }
                        else
                        {
                            throw new Exception("插入失败：无法读取Id");
                        }
                    }
                }
                if (iTrans) return TransCommit(cmd);
                else return true;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        #endregion

        #region Update
        /// <summary>
        /// 附加(更新)指定列
        /// </summary>
        public bool Append<T>(T t, T value, DbCommand cmd = null, params string[] args)
        {
            bool iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();

                string sql = t.Update<T>(true, args);
                cmd.CommandText = sql;
                OnCommandText(cmd);
                DbParameter[] pList = value.AddParameters<T>(paramType, args).ToArray();
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(pList);
                cmd.ExecuteNonQuery();

                if (iTrans) return TransCommit(cmd);
                else return true;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 更新列
        /// </summary>
        public bool Update<T>(T t, DbCommand cmd = null, params string[] args)
        {
            List<T> list = new List<T>() { t };
            return Update<T>(list, cmd, args);
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public bool Update<T>(DataTable dt, DbCommand cmd = null, params string[] args)
        {
            IList<T> list = dt.ToIList<T>();
            return Update<T>(list, cmd, args);
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public bool Update<T>(IList<T> list, DbCommand cmd = null, params string[] args)
        {
            bool iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                for (int i = 0; i < list.Count; i++)
                {
                    string sql = list[i].Update<T>(args);
                    cmd.CommandText = sql;
                    OnCommandText(cmd);
                    DbParameter[] pList = list[i].AddParameters<T>(paramType, args).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    cmd.ExecuteNonQuery();
                }
                if (iTrans) return TransCommit(cmd);
                else return true;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除所有行
        /// </summary>
        public bool Delete<T>(DbCommand cmd = null)
        {
            return this.Delete<T>("1=1", cmd);
        }
        /// <summary>
        /// 删除列
        /// </summary>
        public bool Delete<T>(long id, DbCommand cmd = null)
        {
            bool iTrans = cmd == null;
            string sql = null;
            try
            {
                if (iTrans) cmd = CommandStart();
                sql = default(T).Delete<T>();
                DbParameter parame = default(T).AddParameter<T>(paramType, id);
                cmd.CommandText = sql;
                OnCommandText(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(parame);
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Delete.Error[{0}]\r\n{1}", sql, ex));
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 删除指定条件下的数据
        /// </summary>
        public bool Delete<T>(string find, DbCommand cmd = null)
        {
            bool iTrans = cmd == null;
            string sql = null;
            try
            {
                if (iTrans) cmd = CommandStart();
                sql = default(T).Delete<T>(find);
                cmd.CommandText = sql;
                OnCommandText(cmd);
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Delete.Error[{0}]\r\n{1}", sql, ex));
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 删除列
        /// </summary>
        public bool Delete<T>(T t, DbCommand cmd = null)
        {
            List<T> list = new List<T>() { t };
            return Delete<T>(list, cmd);
        }
        /// <summary>
        /// 删除列表
        /// </summary>
        public bool Delete<T>(DataTable dt, DbCommand cmd = null)
        {
            IList<T> list = dt.ToIList<T>();
            return Delete<T>(list, cmd);
        }
        /// <summary>
        /// 删除列表
        /// </summary>
        public bool Delete<T>(IList<T> list, DbCommand cmd = null)
        {
            bool iTrans = cmd == null;
            string sql = default(T).Delete<T>();
            try
            {
                if (iTrans) cmd = TransStart();
                for (int i = 0; i < list.Count; i++)
                {
                    cmd.CommandText = sql;
                    OnCommandText(cmd);
                    DbParameter[] pList = list[i].AddParameters<T>(paramType).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    cmd.ExecuteNonQuery();
                }
                if (iTrans) return TransCommit(cmd);
                else return true;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        #endregion

        #region Replace
        /// <summary>
        /// 更新或插入列
        /// </summary>
        public bool Replace<T>(T t, DbCommand cmd = null)
        {
            List<T> list = new List<T>() { t };
            return Replace<T>(list, cmd);
        }
        /// <summary>
        /// 更新或插入列表
        /// </summary>
        public void Replace<T>(DataTable dt, DbCommand cmd = null)
        {
            IList<T> list = dt.ToIList<T>();
            Replace<T>(list, cmd);
        }
        /// <summary>
        /// 更新或插入列表
        /// </summary>
        public virtual bool Replace<T>(IList<T> list, DbCommand cmd = null)
        {
            return Replace<T>(list, false, cmd);
        }
        /// <summary>
        /// 更新或插入列表
        /// </summary>
        protected bool Replace<T>(IList<T> list, bool isSqlite, DbCommand cmd = null)
        {
            bool iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                for (int i = 0; i < list.Count; i++)
                {
                    string sql = null;
                    if (isSqlite)
                    {
                        sql = list[i].Replace<T>(GetId);
                    }
                    else
                    {
                        sql = list[i].UpdateOrInsert<T>(GetId);
                    }
                    cmd.CommandText = sql;
                    OnCommandText(cmd);
                    DbParameter[] pList = list[i].AddParameters<T>(paramType).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            list[i].SetMark<T>(dr[0]);
                        }
                        else
                        {
                            throw new Exception("Replace失败：无法读取Id");
                        }
                    }
                }
                if (iTrans) return TransCommit(cmd);
                else return true;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        #endregion

        #endregion

        #region Dispose
        private bool disposed = false;

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
            }
            disposed = true;
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~DataBase()
        {
            Dispose(false);
        }
        #endregion
    }
}
