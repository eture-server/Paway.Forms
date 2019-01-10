using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 数据库接口
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// </summary>
        List<T> Find<T>(string find, params string[] args);
        /// <summary>
        /// </summary>
        bool Insert<T>(T t, DbCommand cmd = null, bool Identity = false);
        /// <summary>
        /// </summary>
        bool Update<T>(T t, params string[] args);
        /// <summary>
        /// </summary>
        bool Delete<T>(T t, DbCommand cmd = null);
    }
}