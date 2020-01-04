using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public class Config
    {
        #region 常量
        public const string Text = "Test - 测试版";
        /// <summary>
        /// 后辍
        /// </summary>
        public const string Suffix = "Test";
        public const string Loading = "Loading...";

        #endregion

        #region Public
        public static UserInfo User { get; set; }
        public static AdminInfo Admin { get; set; }
        public static Form MainForm { get; set; }
        private static LocationInfo _location;
        /// <summary>
        /// 本地数据
        /// </summary>
        public static LocationInfo Location
        {
            get
            {
                if (_location == null)
                {
                    _location = TMethod.Load<LocationInfo>();
                    _location.Save();
                }
                return _location;
            }
        }

        #endregion

        #region 注册
        /// <summary>
        /// 本机唯一编码
        /// </summary>
        public static string MacId { get; set; }
        public static bool IListener { get; set; }

        #endregion
    }
}
