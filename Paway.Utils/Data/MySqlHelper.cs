﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Paway.Helper;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Paway.Utils.Data
{
    /// <summary>
    /// MySql操作基类
    /// </summary>
    public class MySqlHelper : DataBase
    {
        /// <summary>
        /// 连接字符模板
        /// Data Source={0};Persist Security Info=True;Database={1};User ID={2};Password={3};
        /// </summary>
        protected const string DbConnect = @"Data Source={0};Persist Security Info=True;Database={1};User ID={2};Password={3};";

        #region 初始化
        /// <summary>
        /// 初始化
        /// 没有
        /// </summary>
        public MySqlHelper()
            : base(typeof(MySqlConnection), typeof(MySqlCommand), typeof(MySqlParameter))
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
        protected void InitConnect(string server, string database, string user, string pad)
        {
            ConnString = string.Format(DbConnect, server, database, user, pad);
        }

        #endregion

        #region 重载
        /// <summary>
        /// 对sql语句进行过滤
        /// </summary>
        protected override void OnCommandText(DbCommand cmd)
        {
            if (cmd.CommandText != null)
                cmd.CommandText = cmd.CommandText.Replace("[", "").Replace("]", "");
        }

        #endregion

        #region 扩展.分步
        /// <summary>
        /// 打开一个连接
        /// </summary>
        protected new MySqlCommand CommandStart()
        {
            return base.CommandStart() as MySqlCommand;
        }
        /// <summary>
        /// 打开一个连接
        /// </summary>
        /// <returns></returns>
        protected new MySqlCommand CommandStart(string sql)
        {
            return base.CommandStart(sql) as MySqlCommand;
        }
        /// <summary>
        /// 事务处理
        /// 打开一个连接
        /// 返回SqlCommand实例
        /// </summary>
        /// <returns></returns>
        protected new MySqlCommand TransStart()
        {
            return base.TransStart() as MySqlCommand;
        }

        #endregion

        #region 扩展重载
        /// <summary>
        /// 查找指定查询语句
        /// 填充 System.Data.DataSet 并返回一个IList列表
        /// </summary>
        public override IList<T> FindTop<T>(string find, int count, DbCommand cmd = null, params string[] args)
        {
            return base.FindTop<T>(find, count, true, cmd, args);
        }
        /// <summary>
        /// 更新或插入列表
        /// 需要标记唯一键为唯一索引
        /// </summary>
        public override bool Replace<T>(IList<T> list, DbCommand cmd = null)
        {
            return base.Replace<T>(list, true, cmd);
        }

        #endregion
    }
}