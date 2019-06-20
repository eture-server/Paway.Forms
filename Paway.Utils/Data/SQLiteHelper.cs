using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace Paway.Utils
{
    /// <summary>
    ///     SqLite操作基类
    /// </summary>
    public class SQLiteHelper : DataBase
    {
        #region 初始化
        /// <summary>
        ///     文件名
        /// </summary>
        protected string FileName { get; private set; }

        /// <summary>
        ///     初始化
        /// </summary>
        public SQLiteHelper() : base(typeof(SQLiteConnection), typeof(SQLiteCommand), typeof(SQLiteParameter))
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

        #region 扩展重载
        /// <summary>
        ///     查找指定查询语句
        ///     填充 System.Data.DataSet 并返回一个DataTable
        ///     标记是否使用Limit查找指定数量
        /// </summary>
        protected override DataTable FindTable<T>(string find, int count, bool iLimit, DbCommand cmd = null, params string[] args)
        {
            return base.FindTable<T>(find, count, true, cmd, args);
        }

        #endregion
    }
}