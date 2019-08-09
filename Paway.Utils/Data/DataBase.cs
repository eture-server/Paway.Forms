using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using log4net;
using Paway.Helper;
using System.ComponentModel;
using System.Linq;

namespace Paway.Utils
{
    /// <summary>
    /// 数据服务基类，不可创建实例
    /// </summary>
    public abstract class DataBase : IDisposable, IDataService
    {
        /// <summary>
        /// 日志
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region 字段与属性
        private readonly Type connType;
        private readonly Type cmdType;
        private readonly Type paramType;

        /// <summary>
        /// 长连接开关
        /// </summary>
        protected bool ILongConnect;
        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string ConnString { get; set; }

        /// <summary>
        /// 长连接对象
        /// </summary>
        internal DbConnection Connection;

        /// <summary>
        /// 返回最新插入列主键Id
        /// </summary>
        internal string GetId { get; set; }

        #endregion

        #region 事件
        /// <summary>
        /// 抛出事件
        /// </summary>
        protected virtual void OnUpdate<T>(DbCommand cmd, List<T> list, OperType type)
        {
            UpdateEvent?.Invoke(typeof(T), type, list);
        }
        /// <summary>
        /// 更新事件
        /// </summary>
        public event Action<Type, OperType, object> UpdateEvent;

        #endregion

        #region 构造
        /// <summary>
        /// 数据类型
        /// </summary>
        /// <param name="connType">连接类型</param>
        /// <param name="cmdType">执行</param>
        /// <param name="paramType">参数</param>
        protected DataBase(Type connType, Type cmdType, Type paramType)
        {
            Licence.Checking();
            this.connType = connType;
            this.cmdType = cmdType;
            this.paramType = paramType;
        }

        /// <summary>
        /// 对sql语句进行过滤
        /// </summary>
        protected virtual string OnCommandText(string sql) { return sql; }

        #endregion

        #region public 执行外部Sql
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        public int ExecuteNonQuery(string sql, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = OnCommandText(sql);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ExecuteNonQuery.Error[{0}]\r\n{1}", cmd.CommandText, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
                else cmd.CommandText = string.Empty;
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        public object ExecuteScalar(string sql, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = OnCommandText(sql);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ExecuteScalar.Error[{0}]\r\n{1}", cmd.CommandText, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
                else cmd.CommandText = string.Empty;
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的DataTable
        /// </summary>
        public DataTable ExecuteDataTable(string sql, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = OnCommandText(sql);
                using (var dr = cmd.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(dr);
                    return table;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ExecuteDataTable.Error[{0}]\r\n{1}", cmd.CommandText, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
                else cmd.CommandText = string.Empty;
            }
        }

        /// <summary>
        /// 使用事务处理  Transact-SQL 语句列表
        /// </summary>
        public bool TransExecuteNonQuery(List<string> sqlList, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                foreach (var sql in sqlList)
                {
                    cmd.CommandText = OnCommandText(sql);
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
                else cmd.CommandText = string.Empty;
            }
        }

        #endregion

        #region public Find
        /// <summary>
        /// 查找指定主列的数据
        /// 使用long兼容int
        /// </summary>
        public T Find<T>(long id, params string[] args) where T : new()
        {
            return Find<T>(id, null, args);
        }
        /// <summary>
        /// 查找指定主列的数据
        /// </summary>
        public T Find<T>(long id, DbCommand cmd = null, params string[] args) where T : new()
        {
            var attr = typeof(T).Table();
            var sql = string.Format("[{0}] = {1}", attr.Keys, id);
            var list = Find<T>(sql, cmd, args);
            return list.Count == 1 ? list[0] : default;
        }

        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// </summary>
        public List<T> Find<T>(DbCommand cmd = null, params string[] args) where T : new()
        {
            return Find<T>(null, 0, cmd, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个DataTable
        /// </summary>
        public DataTable FindTable<T>(DbCommand cmd = null, params string[] args)
        {
            return FindTable<T>(null, 0, false, cmd, args);
        }

        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// </summary>
        public List<T> Find<T>(string find, params string[] args) where T : new()
        {
            return Find<T>(find, null, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// </summary>
        public List<T> Find<T>(string find, DbCommand cmd = null, params string[] args) where T : new()
        {
            return Find<T>(find, 0, cmd, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个DataTable
        /// 查找指定查询语句
        /// </summary>
        public DataTable FindTable<T>(string find, DbCommand cmd = null, params string[] args)
        {
            return FindTable<T>(find, 0, false, cmd, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// 指定返回行数
        /// </summary>
        public List<T> Find<T>(string find, int count, DbCommand cmd = null, params string[] args) where T : new()
        {
            var table = FindTable<T>(find, count, false, cmd, args);
            return table.ToList<T>();
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// 指定返回行数
        /// 标记是否使用Limit查找指定数量
        /// </summary>
        protected virtual DataTable FindTable<T>(string find, int count, bool iLimit, DbCommand cmd = null, params string[] args)
        {
            Type type = typeof(T);
            var iTrans = cmd == null;
            string sql;
            try
            {
                if (iTrans) cmd = CommandStart();

                if (iLimit)
                {
                    sql = type.Select(find, 0, args);
                    if (count > 0)
                    {
                        sql = string.Format("{0} limit {1}", sql, count);
                    }
                }
                else
                {
                    sql = type.Select(find, count, args);
                }
                cmd.CommandText = OnCommandText(sql);
                using (var dr = cmd.ExecuteReader())
                {
                    var table = type.CreateTable(true);
                    table.Load(dr);
                    //未知处理
                    //table.PrimaryKey = new DataColumn[] { table.Columns[type.TableKey()] };
                    //table.PrimaryKey = null;
                    return table;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Find.Error[{0}]\r\n{1}", cmd.CommandText, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
                else cmd.CommandText = string.Empty;
            }
        }

        #endregion

        #region public Insert
        /// <summary>
        /// 插入行
        /// </summary>
        public bool Insert<T>(T t, DbCommand cmd = null, bool Identity = false)
        {
            return Insert(new List<T> { t }, cmd, Identity);
        }
        /// <summary>
        /// 插入列表
        /// </summary>
        public bool Insert<T>(List<T> list, DbCommand cmd = null, bool Identity = false)
        {
            if (list.Count == 0) return false;
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                var sql = typeof(T).Insert(GetId, Identity);
                cmd.CommandText = OnCommandText(sql);
                var builder = SQLBuilder.CreateBuilder(list[0].GetType(), paramType);
                for (var i = 0; i < list.Count; i++)
                {
                    var pList = builder.Build(list[i]).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            list[i].SetValue(nameof(IId.Id), dr[0].ToInt());
                        }
                        else
                        {
                            throw new PawayException("插入失败：无法读取Id");
                        }
                    }
                }
                OnUpdate(cmd, list, OperType.Insert);
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
                else cmd.CommandText = string.Empty;
            }
        }

        #endregion

        #region public Update
        /// <summary>
        /// 更新行
        /// </summary>
        public bool Update<T>(T t, params string[] args)
        {
            return Update(t, null, args);
        }
        /// <summary>
        /// 更新行
        /// </summary>
        public bool Update<T>(T t, DbCommand cmd = null, params string[] args)
        {
            return Update(new List<T> { t }, cmd, args);
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public bool Update<T>(List<T> list, params string[] args)
        {
            return Update(list, null, args);
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public bool Update<T>(List<T> list, DbCommand cmd = null, params string[] args)
        {
            if (list.Count == 0) return false;
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                var sql = typeof(T).Update(args);
                cmd.CommandText = OnCommandText(sql);
                var builder = SQLBuilder.CreateBuilder(list[0].GetType(), paramType, args);
                foreach (var item in list)
                {
                    var pList = builder.Build(item).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    cmd.ExecuteNonQuery();
                }
                OnUpdate(cmd, list, OperType.Update);
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
                else cmd.CommandText = string.Empty;
            }
        }

        #endregion

        #region public Delete
        /// <summary>
        /// 删除所有行(不监听更新事件)
        /// </summary>
        public int Delete<T>(DbCommand cmd = null)
        {
            return Delete<T>("1=1", cmd);
        }
        /// <summary>
        /// 删除指定条件下的数据(不监听更新事件)
        /// </summary>
        public int Delete<T>(string find, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            string sql = null;
            try
            {
                if (iTrans) cmd = CommandStart();
                sql = DataBaseHelper.Delete<T>(find);
                cmd.CommandText = OnCommandText(sql);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Delete.Error[{0}]\r\n{1}", cmd.CommandText, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
                else cmd.CommandText = string.Empty;
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        public bool Delete<T>(T t, DbCommand cmd = null)
        {
            var list = new List<T> { t };
            return Delete<T>(list, cmd);
        }

        /// <summary>
        /// 删除列表
        /// </summary>
        public bool Delete<T>(List<T> list, DbCommand cmd = null)
        {
            if (list.Count == 0) return false;
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                var sql = DataBaseHelper.Delete<T>();
                cmd.CommandText = OnCommandText(sql);
                var builder = SQLBuilder.CreateBuilder(list[0].GetType(), paramType);
                foreach (var item in list)
                {
                    var pList = builder.Build(item).ToArray();
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
                else cmd.CommandText = string.Empty;
            }
        }

        #endregion

        #region public Replace
        /// <summary>
        /// 替换,由insert/Update替代
        /// </summary>
        public void Replace<T>(List<T> list, DbCommand cmd = null) where T : IId
        {
            bool iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();

                List<T> iList = new List<T>();
                List<T> uList = new List<T>();
                foreach (var item in list)
                {
                    if (item.Id == 0) iList.Add(item);
                    else uList.Add(item);
                }
                if (iList.Count > 0) Insert(iList, cmd);
                if (uList.Count > 0) Update(uList, cmd);

                if (iTrans) TransCommit(cmd);
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw;
            }
            finally
            {
                if (iTrans) CommandEnd(cmd);
                else cmd.CommandText = string.Empty;
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

        #region protected 执行步骤
        /// <summary>
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        protected DbCommand CommandStart()
        {
            var con = GetCon();
            var cmd = GetCmd();
            cmd.Connection = con;
            return cmd;
        }

        /// <summary>
        /// 关闭DbCommand实例的连接，并释放
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
                log.ErrorFormat("CommandEnd.Error[{0}]\r\n{1}", cmd.CommandText, ex);
                throw;
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
            var con = GetCon();
            var trans = con.BeginTransaction();
            var cmd = GetCmd();
            cmd.Connection = con;
            cmd.Transaction = trans;

            return cmd;
        }

        /// <summary>
        /// 事务处理.提交事务
        /// </summary>
        /// <param name="cmd"></param>
        protected bool TransCommit(DbCommand cmd)
        {
            if (cmd == null || cmd.Connection == null || cmd.Transaction == null) return false;
            {
                cmd.Transaction.Commit();
                return true;
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
                log.ErrorFormat("TransError[{0}]\r\n{1}", cmd.CommandText, e);
                cmd.Transaction.Rollback();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("TransError.Error{0}\r\n{1}", cmd.CommandText, ex);
                throw;
            }
        }

        #endregion

        #region private 创建对象
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
            var con = (DbConnection)Activator.CreateInstance(connType);
            con.ConnectionString = ConnString;
            con.Open();
            return con;
        }
        private DbCommand GetCmd()
        {
            var cmd = (DbCommand)Activator.CreateInstance(cmdType);
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// 标识此对象已释放
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// 参数为true表示释放所有资源，只能由使用者调用
        /// 参数为false表示释放非托管资源，只能由垃圾回收器自动调用
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                if (disposing)
                {
                    // TODO: 释放托管资源(托管的对象)。
                }
                // TODO: 释放未托管资源(未托管的对象)
                // 关闭连接
                if (this.Connection != null)
                {
                    if (this.Connection.State == ConnectionState.Open || this.Connection.State == ConnectionState.Broken)
                    {
                        this.Connection.Close();
                    }
                    this.Connection.Dispose();
                    this.Connection = null;
                }
            }
        }
        /// <summary>
        /// 析构，释放非托管资源
        /// </summary>
        ~DataBase()
        {
            Dispose(false);
        }
        /// <summary>
        /// 释放资源
        /// 由类的使用者，在外部显示调用，释放类资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
    internal static class DataBaseHelper
    {
        #region SQL.Select
        /// <summary>
        /// 将指定类型转为Select语句
        /// 指定查询条件
        /// 返回指定行数
        /// </summary>
        public static string Select(this Type type, string find, int count, params string[] args)
        {
            var attr = type.Table();
            var sql = type.Select(count, args);
            sql = string.Format("{0} from [{1}]", sql, attr.Table);
            if (find != null)
            {
                sql = string.Format("{0} where {1}", sql, find);
            }
            return sql;
        }
        private static string Select(this Type type, int count, params string[] args)
        {
            var sql = "select";
            if (count != 0)
            {
                sql = string.Format("{0} Top {1}", sql, count);
            }
            foreach (var property in type.PropertiesValue())
            {
                if (property.ISelect(out string column))
                {
                    if (args.Length > 0 &&
                        args.FirstOrDefault(c => c == column) == null &&
                        args.FirstOrDefault(c => c == property.Name) == null) continue;
                    sql = string.Format("{0} [{1}],", sql, column);
                }
            }
            sql = sql.TrimEnd(',');
            return sql;
        }

        #endregion

        #region SQL.Delete
        /// <summary>
        /// 将指定类型转为Delete语句
        /// 指定删除条件为主列
        /// </summary>
        public static string Delete<T>()
        {
            var attr = typeof(T).Table();
            var sql = string.Format("delete from [{0}] where [{1}]=@{1}", attr.Table, attr.Keys);
            return sql;
        }
        /// <summary>
        /// 将指定类型转为Delete语句
        /// 指定删除条件
        /// </summary>
        public static string Delete<T>(string find)
        {
            var attr = typeof(T).Table();
            return string.Format("delete from [{0}] where {1}", attr.Table, find);
        }

        #endregion

        #region SQL.Update
        /// <summary>
        /// 将指定类型转为Update语句
        /// </summary>
        public static string Update(this Type type, params string[] args)
        {
            return type.Update(false, args);
        }
        /// <summary>
        /// 将指定类型转为Update语句
        /// append=true时为附加,对应Sql语句中的+
        /// </summary>
        public static string Update(this Type type, bool append = false, params string[] args)
        {
            var attr = type.Table();
            var sql = "update [{0}] set";
            sql = string.Format(sql, attr.Table);
            foreach (var property in type.PropertiesValue())
            {
                if (property.ISelect(out string column))
                {
                    if (column == attr.Key) continue;
                    if (args.Length > 0 &&
                        args.FirstOrDefault(c => c == column) == null &&
                        args.FirstOrDefault(c => c == property.Name) == null) continue;
                    if (append)
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

        #region SQL.Insert
        /// <summary>
        /// 将指定类型转为Insert语句
        /// </summary>
        public static string Insert(this Type type, string getId, bool Identity)
        {
            var attr = type.Table();

            type.Insert(attr.Key, out string insert, out string value);
            var sql = string.Format("insert into [{0}]({1}) values({2})", attr.Table, insert, value);
            sql = string.Format("{0};{1}", sql, getId);
            if (Identity)
            {
                sql = string.Format("SET IDENTITY_INSERT [{0}] ON;{1}", attr.Table, sql);
            }
            return sql;
        }
        private static void Insert(this Type type, string key, out string insert, out string value, params string[] args)
        {
            insert = string.Empty;
            value = string.Empty;
            foreach (var property in type.PropertiesValue())
            {
                if (property.ISelect(out string column))
                {
                    if (column == key) continue;
                    if (args.Length > 0 &&
                        args.FirstOrDefault(c => c == column) == null &&
                        args.FirstOrDefault(c => c == property.Name) == null) continue;
                    insert = string.Format("{0}[{1}],", insert, column);
                    value = string.Format("{0}@{1},", value, column);
                }
            }
            insert = insert.TrimEnd(',');
            value = value.TrimEnd(',');
        }

        #endregion
    }
}