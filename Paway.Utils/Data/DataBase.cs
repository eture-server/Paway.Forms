using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using log4net;
using Paway.Helper;
using System.Drawing;
using System.ComponentModel;
using System.Linq;

namespace Paway.Utils
{
    /// <summary>
    ///     数据服务基类，不可创建实例
    /// </summary>
    public abstract class DataBase : IDisposable, IDataService
    {
        /// <summary>
        /// </summary>
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region 事件
        /// <summary>
        /// 更新事件
        /// </summary>
        public event Action<Type, OperType, object> UpdateEvent;
        /// <summary>
        /// 抛出事件
        /// </summary>
        protected virtual void OnUpdate<T>(DbCommand cmd, List<T> list, OperType type)
        {
            UpdateEvent?.Invoke(typeof(T), type, list);
        }
        #endregion

        /// <summary>
        ///     返回最新插入列主键Id
        /// </summary>
        protected string GetId { get; set; }

        /// <summary>
        ///     连接字符串
        /// </summary>
        protected string ConnString { get; set; }

        /// <summary>
        /// 长连接对象
        /// </summary>
        protected DbConnection Connection;
        /// <summary>
        /// 长连接开关
        /// </summary>
        protected bool ILongConnect;

        #region 构造.加载数据类型

        private readonly Type connType;
        private readonly Type cmdType;
        private readonly Type paramType;

        /// <summary>
        ///     数据类型
        /// </summary>
        /// <param name="connType">连接类型</param>
        /// <param name="cmdType">执行</param>
        /// <param name="paramType">参数</param>
        protected DataBase(Type connType, Type cmdType, Type paramType)
        {
            Licence.Checking(MethodBase.GetCurrentMethod().DeclaringType);
            this.connType = connType;
            this.cmdType = cmdType;
            this.paramType = paramType;
        }

        /// <summary>
        ///     对sql语句进行过滤
        /// </summary>
        protected virtual void OnCommandText(DbCommand cmd)
        {
        }

        #endregion

        #region 扩展.方法

        /// <summary>
        ///     对连接执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        public int ExecuteNonQuery(string sql, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = sql;
                OnCommandText(cmd);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ExecuteNonQuery.Error[{0}]\r\n{1}", sql, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        public object ExecuteScalar(string sql, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = sql;
                OnCommandText(cmd);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ExecuteScalar.Error[{0}]\r\n{1}", sql, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        /// <summary>
        ///     执行查询，并返回查询所返回的DataTable
        /// </summary>
        public DataTable ExecuteDataTable(string sql, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = sql;
                OnCommandText(cmd);
                using (var dr = cmd.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(dr);
                    return table;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ExecuteDataTable.Error[{0}]\r\n{1}", sql, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        /// <summary>
        ///     使用事务处理  Transact-SQL 语句列表
        /// </summary>
        public bool TransExecuteNonQuery(List<string> sqlList, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                for (var i = 0; i < sqlList.Count; i++)
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
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        #endregion

        #region 扩展.分步

        /// <summary>
        ///     打开一个连接
        /// </summary>
        /// <returns></returns>
        protected DbCommand CommandStart()
        {
            return CommandStart(null);
        }

        /// <summary>
        ///     打开一个连接
        ///     返回SqlCommand实例
        /// </summary>
        protected DbCommand CommandStart(string sql)
        {
            try
            {
                var con = GetCon();
                var cmd = GetCmd();
                cmd.CommandText = sql;
                OnCommandText(cmd);
                cmd.Connection = con;
                return cmd;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("StartCommand.Error[{0}]\r\n{1}", sql, ex);
                throw;
            }
        }

        /// <summary>
        ///     关闭DbCommand实例的连接，并释放
        /// </summary>
        /// <param name="cmd"></param>
        protected virtual void CommandEnd(DbCommand cmd)
        {
            try
            {
                if (cmd != null)
                {
                    if (!ILongConnect && cmd.Connection != null)
                    {
                        if (cmd.Connection.State == ConnectionState.Open || cmd.Connection.State == ConnectionState.Broken)
                        {
                            cmd.Connection.Close();
                        }
                        cmd.Connection.Dispose();
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("EndCommand.Error[{0}]\r\n{1}", cmd.CommandText, ex);
                throw;
            }
        }

        /// <summary>
        ///     事务处理
        ///     打开一个连接
        ///     返回SqlCommand实例
        /// </summary>
        /// <returns></returns>
        protected DbCommand TransStart()
        {
            try
            {
                var con = GetCon();
                var trans = con.BeginTransaction();
                var cmd = GetCmd();
                cmd.Connection = con;
                cmd.Transaction = trans;

                return cmd;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("TransStartCommand.Error\r\n{0}", ex);
                throw;
            }
        }

        /// <summary>
        ///     事务处理.提交事务
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
        ///     事务处理异常回退
        ///     关闭DbCommand实例的连接，并释放
        /// </summary>
        protected void TransError(DbCommand cmd, Exception e)
        {
            if (cmd == null || cmd.Connection == null || cmd.Transaction == null) return;
            try
            {
                cmd.Transaction.Rollback();
                log.ErrorFormat("TransCommand.Execute.Error[{0}]", cmd.CommandText);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("TransError.Error{0}\r\n{1}", cmd.CommandText, ex);
                throw;
            }
        }

        private DbConnection GetCon()
        {
            if (ILongConnect)
            {
                if (this.Connection == null)
                {
                    this.Connection = InitCon();
                }
                if (this.Connection.State == ConnectionState.Closed || this.Connection.State == ConnectionState.Broken)
                {
                    this.Connection.Close();
                    this.Connection.Open();
                }
                return this.Connection;
            }
            return InitCon();
        }
        private DbConnection InitCon()
        {
            var asmb = Assembly.GetAssembly(connType);
            var con = asmb.CreateInstance(connType.FullName) as DbConnection;
            con.ConnectionString = ConnString;
            con.Open();
            return con;
        }

        private DbCommand GetCmd()
        {
            var asmb = Assembly.GetAssembly(cmdType);
            var cmd = asmb.CreateInstance(cmdType.FullName) as DbCommand;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        #endregion

        #region 扩展.语句

        #region Find
        /// <summary>
        ///     查找指定主列的数据
        /// </summary>
        public T Find<T>(long id, params string[] args)
        {
            return Find<T>(id, null, args);
        }
        /// <summary>
        ///     查找指定主列的数据
        /// </summary>
        public T Find<T>(long id, DbCommand cmd = null, params string[] args)
        {
            var iTrans = cmd == null;
            string sql = null;
            try
            {
                if (iTrans) cmd = CommandStart();
                sql = DataBaseHelper.Select<T>(args);

                var parame = paramType.AddParameter<T>(id);
                cmd.CommandText = sql;
                OnCommandText(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(parame);
                using (var dr = cmd.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    table.Load(dr);
                    var list = table.ToList<T>(1);
                    return list.Count == 1 ? list[0] : default(T);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Find.Error[{0}]\r\n{1}", sql, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        /// <summary>
        ///     填充 System.Data.DataSet 并返回一个List列表
        /// </summary>
        public List<T> Find<T>(params string[] args)
        {
            return Find<T>(null, null, args);
        }
        /// <summary>
        ///     填充 System.Data.DataSet 并返回一个List列表
        /// </summary>
        public List<T> Find<T>(DbCommand cmd = null, params string[] args)
        {
            return Find<T>(null, 0, false, cmd, args);
        }
        /// <summary>
        ///     填充 System.Data.DataSet 并返回一个DataTable
        /// </summary>
        public DataTable FindTable<T>(DbCommand cmd = null, params string[] args)
        {
            return FindTable<T>(null, 0, false, cmd, args);
        }

        /// <summary>
        ///     填充 System.Data.DataSet 并返回一个List列表
        ///     查找指定查询语句
        /// </summary>
        public List<T> Find<T>(string find, params string[] args)
        {
            return Find<T>(find, null, args);
        }
        /// <summary>
        ///     填充 System.Data.DataSet 并返回一个List列表
        ///     查找指定查询语句
        /// </summary>
        public List<T> Find<T>(string find, DbCommand cmd = null, params string[] args)
        {
            return Find<T>(find, 0, false, cmd, args);
        }
        /// <summary>
        ///     填充 System.Data.DataSet 并返回一个DataTable
        ///     查找指定查询语句
        /// </summary>
        public DataTable FindTable<T>(string find, DbCommand cmd = null, params string[] args)
        {
            return FindTable<T>(find, 0, false, cmd, args);
        }

        /// <summary>
        ///     填充 System.Data.DataSet 并返回一个List列表
        ///     查找指定查询语句
        ///     指定返回行数
        /// </summary>
        public virtual List<T> Find<T>(string find, int count, DbCommand cmd = null, params string[] args)
        {
            return Find<T>(find, count, false, cmd, args);
        }
        /// <summary>
        ///     填充 System.Data.DataSet 并返回一个List列表
        ///     查找指定查询语句
        ///     指定返回行数
        /// </summary>
        public virtual DataTable FindTable<T>(string find, int count, DbCommand cmd = null, params string[] args)
        {
            return FindTable<T>(find, count, false, cmd, args);
        }

        /// <summary>
        ///     填充 System.Data.DataSet 并返回一个List列表
        ///     查找指定查询语句
        ///     指定返回行数
        ///     是否SQLite
        /// </summary>
        protected List<T> Find<T>(string find, int count, bool isSQLite, DbCommand cmd = null, params string[] args)
        {
            var iTrans = cmd == null;
            string sql = null;
            try
            {
                if (iTrans) cmd = CommandStart();

                if (isSQLite)
                {
                    sql = DataBaseHelper.Select<T>(find, args);
                    if (count > 0)
                    {
                        sql = string.Format("{0} limit {1}", sql, count);
                    }
                }
                else
                {
                    sql = DataBaseHelper.Select<T>(find, count, args);
                }
                cmd.CommandText = sql;
                OnCommandText(cmd);
                using (var dr = cmd.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    table.Load(dr);
                    return table.ToList<T>();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Find.Error[{0}]\r\n{1}", sql, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }
        /// <summary>
        ///     填充 System.Data.DataSet 并返回一个List列表
        ///     查找指定查询语句
        ///     指定返回行数
        ///     是否SQLite
        /// </summary>
        protected DataTable FindTable<T>(string find, int count, bool isSQLite, DbCommand cmd = null, params string[] args)
        {
            Type type = typeof(T);
            var iTrans = cmd == null;
            string sql = null;
            try
            {
                if (iTrans) cmd = CommandStart();

                if (isSQLite)
                {
                    sql = DataBaseHelper.Select<T>(find, args);
                    if (count > 0)
                    {
                        sql = string.Format("{0} limit {1}", sql, count);
                    }
                }
                else
                {
                    sql = DataBaseHelper.Select<T>(find, count, args);
                }
                cmd.CommandText = sql;
                OnCommandText(cmd);
                using (var dr = cmd.ExecuteReader())
                {
                    var table = type.CreateTable();
                    table.Load(dr);
                    table.PrimaryKey = new DataColumn[] { table.Columns[type.TableKey()] };
                    table.PrimaryKey = null;
                    return table;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Find.Error[{0}]\r\n{1}", sql, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        #endregion

        #region Insert
        /// <summary>
        ///     插入行
        /// </summary>
        public bool Insert<T>(T t, DbCommand cmd = null, bool Identity = false)
        {
            var list = new List<T> { t };
            return Insert<T>(list, cmd, Identity);
        }

        /// <summary>
        ///     插入列表
        /// </summary>
        public bool Insert<T>(List<T> list, DbCommand cmd = null, bool Identity = false)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                for (var i = 0; i < list.Count; i++)
                {
                    var sql = list[i].Insert(GetId, Identity);
                    cmd.CommandText = sql;
                    OnCommandText(cmd);
                    var pList = list[i].AddParameters(paramType).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            list[i].SetMark(dr[0]);
                        }
                        else
                        {
                            throw new Exception("插入失败：无法读取Id");
                        }
                    }
                }
                OnUpdate<T>(cmd, list, OperType.Insert);
                if (iTrans) return TransCommit(cmd);
                else return true;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        #endregion

        #region Update
        /// <summary>
        ///     更新行
        /// </summary>
        public bool Update<T>(T t, params string[] args)
        {
            return Update<T>(t, null, args);
        }
        /// <summary>
        ///     更新行
        /// </summary>
        public bool Update<T>(T t, DbCommand cmd = null, params string[] args)
        {
            var list = new List<T> { t };
            return Update(list, cmd, args);
        }

        /// <summary>
        ///     更新列表
        /// </summary>
        public bool Update<T>(List<T> list, params string[] args)
        {
            return Update(list, null, args);
        }
        /// <summary>
        ///     更新列表
        /// </summary>
        public bool Update<T>(List<T> list, DbCommand cmd = null, params string[] args)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                for (var i = 0; i < list.Count; i++)
                {
                    var sql = list[i].Update(args);
                    cmd.CommandText = sql;
                    OnCommandText(cmd);
                    var pList = list[i].AddParameters(paramType, args).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    cmd.ExecuteNonQuery();
                }
                OnUpdate<T>(cmd, list, OperType.Update);
                if (iTrans) return TransCommit(cmd);
                else return true;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        #endregion

        #region Delete
        /// <summary>
        ///     删除所有行(不监听更新事件)
        /// </summary>
        public bool Delete<T>(DbCommand cmd = null)
        {
            return Delete<T>("1=1", cmd);
        }
        /// <summary>
        ///     删除指定条件下的数据(不监听更新事件)
        /// </summary>
        public bool Delete<T>(string find, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            string sql = null;
            try
            {
                if (iTrans) cmd = CommandStart();
                sql = DataBaseHelper.Delete<T>(find);
                cmd.CommandText = sql;
                OnCommandText(cmd);
                bool result = false;
                result = cmd.ExecuteNonQuery() == 1;
                return result;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Delete.Error[{0}]\r\n{1}", sql, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        /// <summary>
        ///     删除行
        /// </summary>
        public bool Delete<T>(T t, DbCommand cmd = null)
        {
            var list = new List<T> { t };
            return Delete<T>(list, cmd);
        }

        /// <summary>
        ///     删除列表
        /// </summary>
        public bool Delete<T>(List<T> list, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            var sql = DataBaseHelper.Delete<T>();
            try
            {
                if (iTrans) cmd = TransStart();
                for (var i = 0; i < list.Count; i++)
                {
                    cmd.CommandText = sql;
                    OnCommandText(cmd);
                    var pList = list[i].AddParameters(paramType).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    cmd.ExecuteNonQuery();
                }
                OnUpdate<T>(cmd, list, OperType.Delete);
                if (iTrans) return TransCommit(cmd);
                else return true;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
            }
        }

        #endregion

        #region Replace
        /// <summary>
        /// 替换,由insert/Update替代
        /// </summary>
        public void Replace<T>(List<T> list, DbCommand cmd = null) where T : IId
        {
            bool iAlone = cmd == null;
            try
            {
                if (iAlone) cmd = TransStart();

                List<T> iList = new List<T>();
                List<T> uList = new List<T>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Id == 0) iList.Add(list[i]);
                    else iList.Add(list[i]);
                }
                if (iList.Count > 0) Insert(iList, cmd);
                if (uList.Count > 0) Update(uList, cmd);

                if (iAlone) TransCommit(cmd);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (iAlone) TransError(cmd, ex);
                throw;
            }
            finally
            {
                if (iAlone) CommandEnd(cmd);
            }
        }
        /// <summary>
        /// 替换,由insert/Update替代
        /// </summary>
        public void Replace<T>(T t, DbCommand cmd = null) where T : IId
        {
            if (t.Id == 0) Insert(t, cmd);
            else Update(t, cmd);
        }

        #endregion

        #endregion

        #region Dispose
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (this.Connection != null)
            {
                if (this.Connection.State == ConnectionState.Open || this.Connection.State == ConnectionState.Broken)
                {
                    this.Connection.Close();
                }
                this.Connection.Dispose();
            }
        }

        private bool disposed;

        /// <summary>
        ///     释放
        ///     不能在这里关闭连接
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
        ///     析构
        /// </summary>
        ~DataBase()
        {
            Dispose(false);
        }

        #endregion
    }
    internal static class DataBaseHelper
    {
        #region Select
        /// <summary>
        ///     将指定类型转为Select语句
        ///     指定查询条件为主列
        /// </summary>
        public static string Select<T>(params string[] args)
        {
            var attr = typeof(T).Table();
            var sql = Select<T>(0, args);
            sql = string.Format("{0} from [{1}]", sql, attr.Table);
            sql = string.Format("{0} where [{1}]=@{1}", sql, attr.Keys);
            return sql;
        }

        /// <summary>
        ///     将指定类型转为Select语句
        ///     指定查询条件
        /// </summary>
        public static string Select<T>(string find, params string[] args)
        {
            return Select<T>(find, 0, args);
        }

        /// <summary>
        ///     将指定类型转为Select语句
        ///     指定查询条件
        ///     返回指定行数
        /// </summary>
        public static string Select<T>(string find, int count, params string[] args)
        {
            var attr = ConverHelper.AttrTable(typeof(T));
            var sql = Select<T>(count, args);
            sql = string.Format("{0} from [{1}]", sql, attr.Table);
            if (find != null)
            {
                sql = string.Format("{0} where {1}", sql, find);
            }
            return sql;
        }

        private static string Select<T>(int count, params string[] args)
        {
            var type = typeof(T);
            var sql = "select";
            if (count != 0)
            {
                sql = string.Format("{0} Top {1}", sql, count);
            }
            var properties = type.Properties();
            foreach (var property in properties)
            {
                if (property.ISelect(out string column))
                {
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    sql = string.Format("{0} [{1}],", sql, column);
                }
            }
            sql = sql.TrimEnd(',');
            return sql;
        }

        #endregion

        #region Delete
        /// <summary>
        ///     将指定类型转为Delete语句
        ///     指定删除条件为主列
        /// </summary>
        public static string Delete<T>()
        {
            var attr = typeof(T).Table();
            var sql = string.Format("delete from [{0}] where [{1}]=@{1}", attr.Table, attr.Keys);
            return sql;
        }

        /// <summary>
        ///     将指定类型转为Delete语句
        ///     指定删除条件
        /// </summary>
        public static string Delete<T>(string find)
        {
            var attr = ConverHelper.AttrTable(typeof(T));
            return string.Format("delete from [{0}] where {1}", attr.Table, find);
        }

        #endregion

        #region Update
        /// <summary>
        ///     将指定类型转为Update语句
        /// </summary>
        public static string Update<T>(this T t, params string[] args)
        {
            return t.Update(false, args);
        }
        /// <summary>
        ///     将指定类型转为Update语句
        /// </summary>
        public static string Update<T>(this DataRow row, params string[] args)
        {
            return row.Update<T>(false, args);
        }

        /// <summary>
        ///     将指定类型转为Update语句
        ///     append=true时为附加,对应Sql语句中的+
        /// </summary>
        public static string Update<T>(this T t, bool append = false, params string[] args)
        {
            var type = typeof(T);
            var attr = type.Table();
            var sql = "update [{0}] set";
            sql = string.Format(sql, attr.Table);
            var properties = type.Properties();
            var descriptors = type.Descriptors();
            foreach (var property in properties)
            {
                if (property.ISelect(out string column))
                {
                    if (column == attr.Key) continue;
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    var descriptor = descriptors.Find(c => c.Name == property.Name);
                    if (t.IsNull(descriptor))
                    {
                        sql = string.Format("{0} [{1}]=NULL,", sql, column);
                    }
                    else if (append)
                    {
                        sql = string.Format("{0} [{1}]=[{1}]+@{1},", sql, column);
                    }
                    else
                    {
                        sql = string.Format("{0} [{1}]=@{1},", sql, column);
                    }
                }
            }
            sql = sql.TrimEnd(',');
            sql = string.Format("{0} where [{1}]=@{1}", sql, attr.Keys);
            return sql;
        }
        /// <summary>
        ///     将指定类型转为Update语句
        ///     append=true时为附加,对应Sql语句中的+
        /// </summary>
        public static string Update<T>(this DataRow row, bool append = false, params string[] args)
        {
            var type = typeof(T);
            var attr = type.Table();
            var sql = "update [{0}] set";
            sql = string.Format(sql, attr.Table);
            var properties = type.Properties();
            foreach (var property in properties)
            {
                if (property.ISelect(out string column))
                {
                    if (column == attr.Key) continue;
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    if (row.IsNull(property))
                    {
                        sql = string.Format("{0} [{1}]=NULL,", sql, column);
                    }
                    else if (append)
                    {
                        sql = string.Format("{0} [{1}]=[{1}]+@{1},", sql, column);
                    }
                    else
                    {
                        sql = string.Format("{0} [{1}]=@{1},", sql, column);
                    }
                }
            }
            sql = sql.TrimEnd(',');
            sql = string.Format("{0} where [{1}]=@{1}", sql, attr.Keys);
            return sql;
        }

        #endregion

        #region Insert
        /// <summary>
        ///     将指定类型转为Insert语句
        /// </summary>
        public static string Insert<T>(this T t, string getId, bool Identity)
        {
            var attr = ConverHelper.AttrTable(typeof(T));

            t.Insert(attr.Key, typeof(T), out string insert, out string value);
            var sql = string.Format("insert into [{0}]({1}) values({2})", attr.Table, insert, value);
            sql = string.Format("{0};{1}", sql, getId);
            if (Identity)
            {
                sql = string.Format("SET IDENTITY_INSERT [{0}] ON;{1}", attr.Table, sql);
            }
            return sql;
        }
        /// <summary>
        ///     将指定类型转为Insert语句
        /// </summary>
        public static string Insert<T>(this DataRow row, string getId, bool Identity)
        {
            var attr = ConverHelper.AttrTable(typeof(T));

            row.Insert<T>(attr.Key, typeof(T), out string insert, out string value);
            var sql = string.Format("insert into [{0}]({1}) values({2})", attr.Table, insert, value);
            sql = string.Format("{0};{1}", sql, getId);
            if (Identity)
            {
                sql = string.Format("SET IDENTITY_INSERT [{0}] ON;{1}", attr.Table, sql);
            }
            return sql;
        }

        private static void Insert<T>(this T t, string key, Type type, out string insert, out string value, params string[] args)
        {
            insert = null;
            value = null;
            var properties = type.Properties();
            var descriptors = type.Descriptors();
            foreach (var descriptor in descriptors)
            {
                if (t.IsNull(descriptor)) continue;

                var propertie = properties.Find(c => c.Name == descriptor.Name);
                if (propertie.ISelect(out string column))
                {
                    if (column == key) continue;
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    insert = string.Format("{0}[{1}],", insert, column);
                    value = string.Format("{0}@{1},", value, column);
                }
            }
            insert = insert.TrimEnd(',');
            value = value.TrimEnd(',');
        }
        private static void Insert<T>(this DataRow row, string key, Type type, out string insert, out string value, params string[] args)
        {
            insert = null;
            value = null;
            var properties = type.Properties();
            foreach (var property in properties)
            {
                if (row.IsNull(property)) continue;

                if (property.ISelect(out string column))
                {
                    if (column == key) continue;
                    if (args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    insert = string.Format("{0}[{1}],", insert, column);
                    value = string.Format("{0}@{1},", value, column);
                }
            }
            insert = insert.TrimEnd(',');
            value = value.TrimEnd(',');
        }

        /// <summary>
        ///     设置主键值
        /// </summary>
        public static bool SetMark<T>(this T t, object value)
        {
            if (value == null || value == DBNull.Value) return false;
            if (value.ToInt() == 0) return false;

            var type = typeof(T);
            string key = type.TableKey();
            var properties = type.Properties();
            var descriptors = type.Descriptors();
            foreach (var property in properties)
            {
                if (property.ISelect(out string column))
                {
                    if (column != key) continue;

                    var descriptor = descriptors.Find(c => c.Name == property.Name);
                    if (descriptor.PropertyType == typeof(int))
                    {
                        descriptor.SetValue(t, value.ToInt());
                    }
                    if (descriptor.PropertyType == typeof(long))
                    {
                        descriptor.SetValue(t, value.ToLong());
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        ///     设置主键值
        /// </summary>
        public static bool SetMark<T>(this DataRow row, object value)
        {
            if (value == null || value == DBNull.Value) return false;
            if (value.ToInt() == 0) return false;

            var type = typeof(T);
            string key = type.TableKey();
            var properties = type.Properties();
            foreach (var property in properties)
            {
                if (property.ISelect(out string column))
                {
                    if (column != key) continue;

                    row[property.Name] = value;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region AddParameter
        /// <summary>
        ///     添加参数值到参数列表
        ///     主键
        /// </summary>
        public static DbParameter AddParameter<T>(this Type ptype, object value)
        {
            var asmb = Assembly.GetAssembly(ptype);
            return AddParameter(asmb, ptype, typeof(T).TableKey(), value);
        }
        private static DbParameter AddParameter(Assembly asmb, Type ptype, string column, object value)
        {
            var param = asmb.CreateInstance(ptype.FullName) as DbParameter;
            param.ParameterName = string.Format("@{0}", column);
            if (value is DateTime time)
            {
                if (TConfig.IUtcTime && time.Kind != DateTimeKind.Utc)
                {
                    time = TimeZoneInfo.ConvertTimeToUtc(time, TimeZoneInfo.Local);
                }
                param.Value = time;
            }
            else
            {
                param.Value = value;
            }
            return param;
        }
        /// <summary>
        ///     添加参数值到参数列表
        ///     通用型
        /// </summary>
        public static List<DbParameter> AddParameters<T>(this T t, Type ptype, params string[] args)
        {
            var type = typeof(T);
            var asmb = Assembly.GetAssembly(ptype);
            var pList = new List<DbParameter>();

            var key = type.TableKey();
            var properties = type.Properties();
            var descriptors = type.Descriptors();
            foreach (var descriptor in descriptors)
            {
                if (!t.IsValue(descriptor, out object value)) continue;

                var property = properties.Find(c => c.Name == descriptor.Name);
                if (property.ISelect(out string column))
                {
                    //Key必须要
                    if (key != column && args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    var param = AddParameter(asmb, ptype, column, value);
                    pList.Add(param);
                }
            }
            return pList;
        }
        /// <summary>
        ///     添加参数值到参数列表
        ///     通用型
        /// </summary>
        public static List<DbParameter> AddParameters<T>(this DataRow row, Type ptype, params string[] args)
        {
            var type = typeof(T);
            var asmb = Assembly.GetAssembly(ptype);
            var pList = new List<DbParameter>();

            var key = type.TableKey();
            var properties = type.Properties();
            foreach (var property in properties)
            {
                if (!row.IsValue(property, out object value)) continue;

                if (property.ISelect(out string column))
                {
                    //Key必须要
                    if (key != column && args.Length > 0 && args.FirstOrDefault(c => c == column) != column) continue;

                    var param = AddParameter(asmb, ptype, column, value);
                    pList.Add(param);
                }
            }
            return pList;
        }

        #endregion

        #region 特性
        private static bool IsNull<T>(this T t, PropertyDescriptor prop)
        {
            var value = prop.GetValue(t);
            if (value == null || value == DBNull.Value) return true;

            if (prop.PropertyType == typeof(DateTime) && value is DateTime)
            {
                var dt = value.ToDateTime();
                if (dt == DateTime.MinValue) return true;
            }
            return false;
        }
        private static bool IsNull(this DataRow row, PropertyInfo prop)
        {
            var value = row[prop.Name];
            if (value == null || value == DBNull.Value) return prop.PropertyType.IsValueType;

            if (prop.PropertyType == typeof(DateTime) && value is DateTime)
            {
                var dt = value.ToDateTime();
                if (dt == DateTime.MinValue) return true;
            }
            return false;
        }
        private static bool IsValue<T>(this T t, PropertyDescriptor prop, out object value)
        {
            value = prop.GetValue(t);
            if (value == null || value == DBNull.Value) return false;

            if (prop.PropertyType == typeof(Image) && value is Image)
            {
                value = SctructHelper.ImageToBytes((Image)value);
                if (value == null) return false;
            }
            else if (prop.PropertyType == typeof(DateTime) && value is DateTime)
            {
                var dt = value.ToDateTime();
                if (dt == DateTime.MinValue) return false;
                value = dt;
            }
            return true;
        }
        private static bool IsValue(this DataRow row, PropertyInfo prop, out object value)
        {
            value = null;
            value = row[prop.Name];
            if (value == null || value == DBNull.Value)
            {
                bool result = prop.PropertyType.IsValueType;
                if (result) value = Activator.CreateInstance(prop.PropertyType);
                return result;
            }

            if (prop.PropertyType == typeof(Image) && value is Image)
            {
                value = SctructHelper.ImageToBytes((Image)value);
                if (value == null) return false;
            }
            else if (prop.PropertyType == typeof(DateTime) && value is DateTime)
            {
                var dt = value.ToDateTime();
                if (dt == DateTime.MinValue) return false;
                value = dt;
            }
            return true;
        }

        #endregion
    }
}