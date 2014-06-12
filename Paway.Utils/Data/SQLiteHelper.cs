using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace Paway.Utils.Data
{
    /// <summary>
    /// SqLite操作基类
    /// </summary>
    public abstract class SQLiteHelper : DataBase
    {
        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public SQLiteHelper()
        {
            base.GetId = "select LAST_INSERT_ROWID() Id";
            this.InitType(typeof(SQLiteConnection), typeof(SQLiteCommand), typeof(SQLiteParameter));
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
    }
}
