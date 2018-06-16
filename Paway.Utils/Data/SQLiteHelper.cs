using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace Paway.Utils.Data
{
    /// <summary>
    ///     SqLite操作基类
    /// </summary>
    public abstract class SQLiteHelper : DataBase
    {
        #region 初始化

        /// <summary>
        ///     文件名
        /// </summary>
        protected string FileName { get; private set; }

        /// <summary>
        ///     初始化
        /// </summary>
        public SQLiteHelper()
            : base(typeof(SQLiteConnection), typeof(SQLiteCommand), typeof(SQLiteParameter))
        {
            GetId = "select LAST_INSERT_ROWID() Id";
        }

        /// <summary>
        ///     初始化连接字符串
        /// </summary>
        /// <param name="fileName"></param>
        protected void InitConnect(string fileName)
        {
            FileName = fileName;
            var path = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            ConnString = GetConnString(fileName);
        }

        /// <summary>
        ///     根据指定资源sql语句创建数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected bool InitCreate(string sql)
        {
            if (File.Exists(FileName)) return false;

            var list = new List<string>();
            var sqlList = sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < sqlList.Length; i++)
            {
                var cmdLine = sqlList[i].Trim();
                if (string.IsNullOrEmpty(cmdLine)) continue;
                list.Add(cmdLine);
            }
            return TransExecuteNonQuery(list);
        }

        /// <summary>
        ///     获取连接字符串
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected string GetConnString(string fileName)
        {
            var sb = new SQLiteConnectionStringBuilder()
            {
                DataSource = fileName,
                SyncMode = SynchronizationModes.Off,
                PageSize = 4096,
                CacheSize = 70 * 1024
            };
            return sb.ConnectionString;
        }

        #endregion

        #region 扩展.分步

        /// <summary>
        ///     打开一个连接
        /// </summary>
        /// <returns></returns>
        protected new SQLiteCommand CommandStart()
        {
            return base.CommandStart() as SQLiteCommand;
        }

        /// <summary>
        ///     打开一个连接
        /// </summary>
        /// <returns></returns>
        protected new SQLiteCommand CommandStart(string sql)
        {
            return base.CommandStart(sql) as SQLiteCommand;
        }

        /// <summary>
        ///     事务处理
        ///     打开一个连接
        ///     返回SqlCommand实例
        /// </summary>
        /// <returns></returns>
        protected new SQLiteCommand TransStart()
        {
            return base.TransStart() as SQLiteCommand;
        }

        #endregion

        #region 扩展重载

        /// <summary>
        ///     查找指定查询语句
        ///     填充 System.Data.DataSet 并返回一个List列表
        /// </summary>
        public override List<T> Find<T>(string find, int count, DbCommand cmd = null, params string[] args)
        {
            return base.Find<T>(find, count, true, cmd, args);
        }
        /// <summary>
        ///     查找指定查询语句
        ///     填充 System.Data.DataSet 并返回一个DataTable
        /// </summary>
        public override DataTable FindTable<T>(string find, int count, DbCommand cmd = null, params string[] args)
        {
            return base.FindTable<T>(find, count, true, cmd, args);
        }

        /// <summary>
        ///     更新或插入列表
        ///     需要标记唯一键为唯一索引
        /// </summary>
        public override bool Replace<T>(List<T> list, DbCommand cmd = null, params string[] args)
        {
            return base.Replace(list, true, cmd, args);
        }
        /// <summary>
        ///     更新或插入列表
        ///     需要标记唯一键为唯一索引
        /// </summary>
        public override bool Replace<T>(DataTable table, DbCommand cmd = null, params string[] args)
        {
            return base.Replace<T>(table, true, cmd, args);
        }

        #endregion
    }
}