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
        /// 连接字符模板
        /// </summary>
        protected const string DbConnect = @"Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};";
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

        #region 扩展.分步
        /// <summary>
        /// 关闭DbCommand实例的连接，并释放
        /// </summary>
        /// <param name="cmd"></param>
        protected void CommandEnd(DbCommand cmd)
        {
            if (cmd == null || cmd.Connection == null) return;
            try
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("EndCommand.Error[{0}]\r\n{1}", cmd.CommandText, ex));
                throw;
            }
        }
        /// <summary>
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        protected DbCommand CommandStart(string sql, Type connType, Type cmdType)
        {
            try
            {
                DbConnection con = GetCon(connType);
                DbCommand cmd = GetCmd(cmdType);
                cmd.CommandText = sql;
                cmd.Connection = con;
                return cmd;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("StartCommand.Error[{0}]\r\n{1}", sql, ex));
                throw;
            }
        }

        /// <summary>
        /// 事务处理
        /// 关闭DbCommand实例的连接，并释放
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
                throw;
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
                throw;
            }
        }
        /// <summary>
        /// 事务处理
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        /// <returns></returns>
        protected DbCommand TransStart(Type connType, Type cmdType)
        {
            try
            {
                DbConnection con = GetCon(connType);
                DbTransaction trans = con.BeginTransaction();
                DbCommand cmd = GetCmd(cmdType);
                cmd.Connection = con;
                cmd.Transaction = trans;

                return cmd;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("TransStartCommand.Error\r\n{0}", ex));
                throw;
            }
        }

        private DbConnection GetCon(Type type)
        {
            Assembly asmb = Assembly.GetAssembly(type);
            DbConnection con = asmb.CreateInstance(type.FullName) as DbConnection;
            con.ConnectionString = ConnString;
            con.Open();
            return con;
        }
        private DbCommand GetCmd(Type type)
        {
            Assembly asmb = Assembly.GetAssembly(type);
            DbCommand cmd = asmb.CreateInstance(type.FullName) as DbCommand;
            return cmd;
        }
        private DbDataAdapter GetDa(Type type)
        {
            Assembly asmb = Assembly.GetAssembly(type);
            DbDataAdapter da = asmb.CreateInstance(type.FullName) as DbDataAdapter;
            return da;
        }
        private DbDataReader GetDr(Type type)
        {
            Assembly asmb = Assembly.GetAssembly(type);
            DbDataReader dr = asmb.CreateInstance(type.FullName) as DbDataReader;
            return dr;
        }

        #endregion

        #region 扩展.语句
        /// <summary>
        /// 插入列表
        /// </summary>
        protected bool Insert<T>(IList<T> list, Type connType, Type cmdType, Type paramType)
        {
            DbCommand cmd = null;
            try
            {
                cmd = TransStart(connType, cmdType);
                for (int i = 0; i < list.Count; i++)
                {
                    string sql = list[i].Insert<T>(GetId);
                    cmd.CommandText = sql;
                    DbParameter[] pList = list[i].AddParameters<T>(paramType).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    DbDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        list[i].SetMark<T>(dr[0]);
                    }
                    else
                    {
                        throw new Exception("更新失败");
                    }
                    dr.Close();
                }
                return TransCommit(cmd);
            }
            catch (Exception ex)
            {
                TransError(cmd, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        protected bool Update<T>(IList<T> list, Type connType, Type cmdType, Type paramType)
        {
            DbCommand cmd = null;
            try
            {
                cmd = TransStart(connType, cmdType);
                for (int i = 0; i < list.Count; i++)
                {
                    string sql = list[i].Update<T>();
                    cmd.CommandText = sql;
                    DbParameter[] pList = list[i].AddParameters<T>(paramType).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    cmd.ExecuteNonQuery();
                }
                return TransCommit(cmd);
            }
            catch (Exception ex)
            {
                TransError(cmd, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 删除列表
        /// </summary>
        protected bool Delete<T>(IList<T> list, Type connType, Type cmdType, Type paramType)
        {
            string sql = default(T).Delete<T>();
            DbCommand cmd = null;
            try
            {
                cmd = TransStart(connType, cmdType);
                for (int i = 0; i < list.Count; i++)
                {
                    cmd.CommandText = sql;
                    DbParameter[] pList = list[i].AddParameters<T>(paramType).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    cmd.ExecuteNonQuery();
                }
                return TransCommit(cmd);
            }
            catch (Exception ex)
            {
                TransError(cmd, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 更新或插入列表
        /// </summary>
        protected bool UpdateOrInsert<T>(IList<T> list, Type connType, Type cmdType, Type paramType)
        {
            DbCommand cmd = null;
            try
            {
                cmd = TransStart(connType, cmdType);
                for (int i = 0; i < list.Count; i++)
                {
                    string sql = list[i].UpdateOrInsert<T>(GetId);
                    cmd.CommandText = sql;
                    DbParameter[] pList = list[i].AddParameters<T>(paramType).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    DbDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        list[i].SetMark<T>(dr[0]);
                    }
                    else
                    {
                        throw new Exception("更新失败");
                    }
                    dr.Close();
                }
                return TransCommit(cmd);
            }
            catch (Exception ex)
            {
                TransError(cmd, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd);
            }
        }

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
