using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using log4net;
using Paway.Helper;
using System.ComponentModel;
using System.Linq;
using System.Collections;
using System.Diagnostics;

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
        private readonly Stopwatch sw = new Stopwatch();

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
        /// 更新事件(直接执行的SQL语句不引发事件)
        /// </summary>
        public event Action<Type, OperType, object> UpdateEvent;
        /// <summary>
        /// SQL执行记录事件
        /// </summary>
        public event Action<long, string, DbParameterCollection> ExecuteEvent;
        /// <summary>
        /// 引发更新事件
        /// </summary>
        protected virtual void OnUpdate(DbCommand cmd, IList list, OperType type)
        {
            UpdateEvent?.Invoke(list.GenericType(), type, list);
        }
        /// <summary>
        /// 引发SQL执行记录事件
        /// </summary>
        protected virtual void OnExecute(DbCommand cmd)
        {
            ExecuteEvent?.Invoke(sw.ElapsedMilliseconds, cmd.CommandText, cmd.Parameters); ;
        }

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
        /// 对连接对象执行 SQL 语句
        /// </summary>
        /// <returns>受影响的行数</returns>
        public int Execute(string sql, DbCommand cmd = null)
        {
            return Execute(sql, null, cmd);
        }
        /// <summary>
        /// 对连接对象执行 SQL 语句
        /// <para>不引发UpdateEvent</para>
        /// </summary>
        /// <returns>受影响的行数</returns>
        public int Execute(string sql, dynamic param, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = OnCommandText(sql);
                AddParameters(cmd, param);
                var result = cmd.ExecuteNonQuery();
                OnExecute(cmd);
                return result;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ExecuteNonQuery.Error[{0}]\r\n{1}", cmd?.CommandText, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd, iTrans);
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <returns>结果集中第一行的第一列。</returns>
        public object ExecuteScalar(string sql, DbCommand cmd = null)
        {
            return ExecuteScalar(sql, null, cmd);
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <returns>结果集中第一行的第一列。</returns>
        public object ExecuteScalar(string sql, dynamic param, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = OnCommandText(sql);
                AddParameters(cmd, param);
                var obj = cmd.ExecuteScalar();
                OnExecute(cmd);
                return obj;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ExecuteScalar.Error[{0}]\r\n{1}", cmd?.CommandText, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd, iTrans);
            }
        }

        /// <summary>
        /// 执行查询，并返回泛型列表
        /// </summary>
        public List<T> ExecuteList<T>(string sql = null, DbCommand cmd = null) where T : new()
        {
            return ExecuteList<T>(sql, null, cmd);
        }
        /// <summary>
        /// 执行查询，并返回泛型列表
        /// </summary>
        public List<T> ExecuteList<T>(string sql, dynamic param, DbCommand cmd = null) where T : new()
        {
            DataTable table = ExecuteDataTable(sql, param, cmd, typeof(T));
            return table.ToList<T>();
        }
        /// <summary>
        /// 执行查询，并返回泛型列表
        /// </summary>
        public IList ExecuteList(Type type, string sql = null, DbCommand cmd = null)
        {
            return ExecuteList(type, sql, null, cmd);
        }
        /// <summary>
        /// 执行查询，并返回泛型列表
        /// </summary>
        public IList ExecuteList(Type type, string sql, dynamic param = null, DbCommand cmd = null)
        {
            DataTable table = ExecuteDataTable(sql, param, cmd, type);
            return table.ToList(type);
        }
        /// <summary>
        /// 执行查询，并返回DataTable对象
        /// </summary>
        public DataTable ExecuteDataTable(string sql, DbCommand cmd = null, Type type = null)
        {
            return ExecuteDataTable(sql, null, cmd, type);
        }
        /// <summary>
        /// 执行查询，并返回DataTable对象
        /// </summary>
        public DataTable ExecuteDataTable(string sql, dynamic param, DbCommand cmd = null, Type type = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                cmd.CommandText = OnCommandText(sql);
                AddParameters(cmd, param);
                using (var dr = cmd.ExecuteReader())
                {
                    OnExecute(cmd);
                    var table = type == null ? new DataTable() : type.CreateTable(true);
                    table.Load(dr);
                    return table;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ExecuteDataTable.Error[{0}]\r\n{1}", cmd?.CommandText, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd, iTrans);
            }
        }

        /// <summary>
        /// 使用事务处理  Transact-SQL 语句列表
        /// <para>不引发UpdateEvent</para>
        /// </summary>
        public int Execute(List<string> sqlList, DbCommand cmd = null)
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                int result = 0;
                foreach (var sql in sqlList)
                {
                    cmd.CommandText = OnCommandText(sql);
                    result += cmd.ExecuteNonQuery();
                    OnExecute(cmd);
                }
                if (iTrans) TransCommit(cmd);
                return result;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd, iTrans);
            }
        }

        #endregion

        #region public Find
        /// <summary>
        /// 查找指定主列的数据
        /// </summary>
        public T FindById<T>(dynamic id, params string[] args) where T : new()
        {
            return FindById<T>(id, null, args);
        }
        /// <summary>
        /// 查找指定主列的数据
        /// </summary>
        public T FindById<T>(dynamic id, DbCommand cmd, params string[] args) where T : new()
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();

                var keys = typeof(T).TableKeys();
                cmd.Parameters.Clear();
                var param = AddParameters(keys, id);
                cmd.Parameters.Add(param);
                var sql = string.Format("[{0}] = {1}", keys, param.ParameterName);
                List<T> list = Find<T>(sql, null, cmd, 0, args);
                T t = list.Count == 1 ? list[0] : default;
                return t;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ExecuteNonQuery.Error[{0}]\r\n{1}", cmd?.CommandText, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd, iTrans);
            }
        }

        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// </summary>
        public List<T> Find<T>(DbCommand cmd, params string[] args) where T : new()
        {
            return Find<T>(null, null, cmd, 0, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// </summary>
        public List<T> Find<T>(string find, params string[] args) where T : new()
        {
            return Find<T>(find, new { }, null, 0, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// </summary>
        public List<T> Find<T>(string find, dynamic param, params string[] args) where T : new()
        {
            return Find<T>(find, param, null, 0, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// </summary>
        public List<T> Find<T>(string find, DbCommand cmd, params string[] args) where T : new()
        {
            return Find<T>(find, null, null, 0, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// </summary>
        public List<T> Find<T>(string find, dynamic param, DbCommand cmd, params string[] args) where T : new()
        {
            return Find<T>(find, param, cmd, 0, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// 指定返回行数
        /// </summary>
        public List<T> Find<T>(string find = null, dynamic param = null, DbCommand cmd = null, int count = 0, params string[] args) where T : new()
        {
            DataTable table = FindTable(typeof(T), find, param, cmd, count, false, args);
            return table.ToList<T>();
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// 指定返回行数
        /// </summary>
        public IList Find(Type type, string find = null, dynamic param = null, DbCommand cmd = null, int count = 0, params string[] args)
        {
            DataTable table = FindTable(type, find, param, cmd, count, false, args);
            return table.ToList(type);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个DataTable
        /// </summary>
        public DataTable FindTable<T>(DbCommand cmd, params string[] args)
        {
            return FindTable(typeof(T), null, null, cmd, 0, false, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个DataTable
        /// 查找指定查询语句
        /// </summary>
        public DataTable FindTable<T>(string find = null, dynamic param = null, DbCommand cmd = null, int count = 0, params string[] args)
        {
            return FindTable(typeof(T), find, param, cmd, count, false, args);
        }
        /// <summary>
        /// 填充 System.Data.DataSet 并返回一个List列表
        /// 查找指定查询语句
        /// 指定返回行数
        /// 标记是否使用Limit查找指定数量
        /// </summary>
        protected virtual DataTable FindTable(Type type, string find = null, dynamic param = null, DbCommand cmd = null, int count = 0, bool iLimit = false, params string[] args)
        {
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
                AddParameters(cmd, param);
                using (var dr = cmd.ExecuteReader())
                {
                    OnExecute(cmd);
                    var table = type.CreateTable(true);
                    table.Load(dr);
                    return table;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Find.Error[{0}]\r\n{1}", cmd?.CommandText, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd, iTrans);
            }
        }

        #endregion

        #region public Insert
        /// <summary>
        /// 插入行
        /// </summary>
        public int Insert<T>(T t, DbCommand cmd = null)
        {
            IList list = new List<T> { t };
            return InsertList(list, cmd);
        }
        /// <summary>
        /// 插入行
        /// </summary>
        public int Insert(object t, DbCommand cmd = null)
        {
            var list = t.GetType().GenericList();
            list.Add(t);
            return InsertList(list, cmd);
        }
        /// <summary>
        /// 插入列表
        /// </summary>
        public int Insert<T>(List<T> list, DbCommand cmd = null)
        {
            return InsertList(list, cmd);
        }
        /// <summary>
        /// 插入列表
        /// </summary>
        public int Insert(IList list, DbCommand cmd = null)
        {
            return InsertList(list, cmd);
        }
        /// <summary>
        /// 插入列表
        /// </summary>
        private int InsertList(IList list, DbCommand cmd = null)
        {
            if (list.Count == 0) return 0;
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                var type = list.GenericType();
                var sql = type.Insert(GetId);
                cmd.CommandText = OnCommandText(sql);
                var builder = SQLBuilder.CreateBuilder(list[0].GetType(), paramType);
                var tableKey = type.TableKey();
                var property = type.Property(tableKey);
                int result = 0;
                for (var i = 0; i < list.Count; i++)
                {
                    var pList = builder.Build(list[i]).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    using (var dr = cmd.ExecuteReader())
                    {
                        result++;
                        OnExecute(cmd);
                        if (property == null) continue;
                        if (dr.Read())
                        {
                            if (property.PropertyType == typeof(long))
                            {
                                list[i].SetValue(property.Name, dr[nameof(IId.Id)].ToLong());
                            }
                            else
                            {
                                list[i].SetValue(property.Name, dr[nameof(IId.Id)].ToInt());
                            }
                        }
                        else
                        {
                            throw new PawayException("插入失败：无法读取Id");
                        }
                    }
                }
                OnUpdate(cmd, list, OperType.Insert);
                if (iTrans) TransCommit(cmd);
                return result;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd, iTrans);
            }
        }

        #endregion

        #region public Update
        /// <summary>
        /// 更新行
        /// </summary>
        public int Update<T>(T t, params string[] args)
        {
            return Update(t, null, args);
        }
        /// <summary>
        /// 更新行
        /// </summary>
        public int Update<T>(T t, DbCommand cmd = null, params string[] args)
        {
            IList list = new List<T> { t };
            return UpdateList(list, cmd, args);
        }
        /// <summary>
        /// 更新行
        /// </summary>
        public int Update<T>(object t, params string[] args)
        {
            var list = t.GetType().GenericList();
            list.Add(t);
            return UpdateList(list, null, args);
        }
        /// <summary>
        /// 更新行
        /// </summary>
        public int Update(object t, DbCommand cmd = null, params string[] args)
        {
            var list = t.GetType().GenericList();
            list.Add(t);
            return UpdateList(list, cmd, args);
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public int Update<T>(List<T> list, params string[] args)
        {
            return UpdateList(list, null, args);
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public int Update(IList list, params string[] args)
        {
            return UpdateList(list, null, args);
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public int Update<T>(List<T> list, DbCommand cmd = null, params string[] args)
        {
            return UpdateList(list, cmd, args);
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public int Update(IList list, DbCommand cmd = null, params string[] args)
        {
            return UpdateList(list, cmd, args);
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        private int UpdateList(IList list, DbCommand cmd = null, params string[] args)
        {
            if (list.Count == 0) return 0;
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                var type = list.GenericType();
                var sql = type.Update(args);
                cmd.CommandText = OnCommandText(sql);
                var builder = SQLBuilder.CreateBuilder(list[0].GetType(), paramType, args);
                int result = 0;
                foreach (var item in list)
                {
                    var pList = builder.Build(item).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    result += cmd.ExecuteNonQuery();
                    OnExecute(cmd);
                }
                OnUpdate(cmd, list, OperType.Update);
                if (iTrans) TransCommit(cmd);
                return result;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd, iTrans);
            }
        }

        #endregion

        #region public Delete
        /// <summary>
        /// 删除所有行
        /// </summary>
        public int Delete<T>(DbCommand cmd = null) where T : new()
        {
            return Delete<T>(string.Empty, null, cmd);
        }
        /// <summary>
        /// 删除指定条件下的数据
        /// </summary>
        public int Delete<T>(string find, DbCommand cmd = null) where T : new()
        {
            return Delete<T>(find, null, cmd);
        }
        /// <summary>
        /// 删除指定条件下的数据
        /// </summary>
        public int Delete<T>(string find, dynamic param, DbCommand cmd = null) where T : new()
        {
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = CommandStart();
                List<T> list = null;
                if (UpdateEvent != null)
                {
                    list = Find<T>(find, param, cmd, 0);
                }
                var sql = typeof(T).Delete(find);
                cmd.CommandText = OnCommandText(sql);
                AddParameters(cmd, param);
                var result = cmd.ExecuteNonQuery();
                OnExecute(cmd);
                if (list != null) OnUpdate(cmd, list, OperType.Delete);
                return result;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Delete.Error[{0}]\r\n{1}", cmd?.CommandText, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd, iTrans);
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        public int Delete<T>(T t, DbCommand cmd = null)
        {
            IList list = new List<T> { t };
            return DeleteList(list, cmd);
        }
        /// <summary>
        /// 删除行
        /// </summary>
        public int Delete(object t, DbCommand cmd = null)
        {
            var list = t.GetType().GenericList();
            list.Add(t);
            return DeleteList(list, cmd);
        }
        /// <summary>
        /// 删除列表
        /// </summary>
        public int Delete<T>(List<T> list, DbCommand cmd = null)
        {
            return DeleteList(list, cmd);
        }
        /// <summary>
        /// 删除列表
        /// </summary>
        public int Delete(IList list, DbCommand cmd = null)
        {
            return DeleteList(list, cmd);
        }
        /// <summary>
        /// 删除列表
        /// </summary>
        private int DeleteList(IList list, DbCommand cmd = null)
        {
            if (list.Count == 0) return 0;
            var iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();
                var type = list.GenericType();
                var sql = type.Delete();
                cmd.CommandText = OnCommandText(sql);
                var builder = SQLBuilder.CreateBuilder(list[0].GetType(), paramType);
                int result = 0;
                foreach (var item in list)
                {
                    var pList = builder.Build(item).ToArray();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    result += cmd.ExecuteNonQuery();
                    OnExecute(cmd);
                }
                OnUpdate(cmd, list, OperType.Delete);
                if (iTrans) TransCommit(cmd);
                return result;
            }
            catch (Exception ex)
            {
                if (iTrans) TransError(cmd, ex);
                throw;
            }
            finally
            {
                CommandEnd(cmd, iTrans);
            }
        }

        #endregion

        #region public Replace
        /// <summary>
        /// 替换,由insert/Update替代
        /// </summary>
        public void Replace<T>(List<T> list, DbCommand cmd = null)
        {
            bool iTrans = cmd == null;
            try
            {
                if (iTrans) cmd = TransStart();

                var type = typeof(T);
                var key = type.TableKeys();
                var property = type.Property(key);

                List<T> iList = new List<T>();
                List<T> uList = new List<T>();
                foreach (var item in list)
                {
                    if (item.GetValue(key).Equals(Activator.CreateInstance(property.PropertyType))) iList.Add(item);
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
                CommandEnd(cmd, iTrans);
            }
        }
        /// <summary>
        /// 替换,由insert/Update替代
        /// </summary>
        public void Replace<T>(T t, DbCommand cmd = null)
        {
            var type = typeof(T);
            var key = type.TableKeys();
            var property = type.Property(key);

            if (t.GetValue(key).Equals(Activator.CreateInstance(property.PropertyType))) Insert(t, cmd);
            else Update(t, cmd);
        }

        #endregion

        #region protected 执行步骤
        private DbParameter AddParameters(string name, object value)
        {
            var param = (DbParameter)Activator.CreateInstance(paramType);
            param.ParameterName = string.Format("@{0}", name);
            param.Value = value;
            if (param.Value == null) param.Value = DBNull.Value;
            return param;
        }

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
        protected virtual void CommandEnd(DbCommand cmd, bool iTrans = false)
        {
            try
            {
                if (cmd == null) return;
                if (!iTrans)
                {
                    cmd.CommandText = string.Empty;
                    return;
                }
                if (!ILongConnect && cmd.Connection != null)
                {
                    if (cmd.Connection.State == ConnectionState.Open || cmd.Connection.State == ConnectionState.Broken)
                    {
                        cmd.Connection.Close();
                    }
                    cmd.Connection.Dispose();
                }
                cmd.Dispose();
                sw.Stop();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("CommandEnd.Error[{0}]\r\n{1}", cmd?.CommandText, ex);
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
            sw.Restart();
            return cmd;
        }
        private void AddParameters(DbCommand cmd, object param)
        {
            if (param == null) return;
            cmd.Parameters.Clear();
            var type = param.GetType();
            foreach (var item in type.PropertiesCache())
            {
                var value = item.GetValue(param, null);
                var paramItem = AddParameters(item.Name, value);
                cmd.Parameters.Add(paramItem);
            }
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
    /// <summary>
    /// 生成SQL语句
    /// </summary>
    internal static class DataBaseHelper
    {
        #region SQL.Select
        /// <summary>
        /// 将指定类型转为Select语句
        /// 指定查询条件
        /// 返回指定行数
        /// </summary>
        public static string Select(this Type type, string find = null, int count = 0, params string[] args)
        {
            var tableName = type.TableName();
            var sql = type.Select(count, args);
            sql = string.Format("{0} from [{1}]", sql, tableName);
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
                if (property.ISelect())
                {
                    var column = property.ColumnName();
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
        public static string Delete(this Type type)
        {
            var sql = string.Format("delete from [{0}] where [{1}]=@{1}", type.TableName(), type.TableKeys());
            return sql;
        }
        /// <summary>
        /// 将指定类型转为Delete语句
        /// 指定删除条件
        /// </summary>
        public static string Delete(this Type type, string find)
        {
            var sql = string.Format("delete from [{0}]", type.TableName());
            if (!find.IsNullOrEmpty())
            {
                sql = string.Format("{0} where {1}", sql, find);
            }
            return sql;
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
        private static string Update(this Type type, bool append = false, params string[] args)
        {
            var tableKey = type.TableKeys();
            var sql = "update [{0}] set";
            sql = string.Format(sql, type.TableName());
            foreach (var property in type.PropertiesValue())
            {
                if (property.ISelect())
                {
                    var column = property.ColumnName();
                    if (column == tableKey) continue;
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
            sql = string.Format("{0} where [{1}]=@{1}", sql, tableKey);
            return sql;
        }

        #endregion

        #region SQL.Insert
        /// <summary>
        /// 将指定类型转为Insert语句
        /// </summary>
        public static string Insert(this Type type, string getId)
        {
            var tableName = type.TableName();
            type.Insert(type.TableKey(), out string insert, out string value);
            var sql = string.Format("insert into [{0}]({1}) values({2})", tableName, insert, value);
            sql = string.Format("{0};{1}", sql, getId);
            return sql;
        }
        private static void Insert(this Type type, string key, out string insert, out string value, params string[] args)
        {
            insert = string.Empty;
            value = string.Empty;
            foreach (var property in type.PropertiesValue())
            {
                if (property.ISelect())
                {
                    var column = property.ColumnName();
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