using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 初始化时的全局变量配置
    /// 与IControl接口属性对应
    /// Global全局
    /// </summary>
    public abstract class TConfig
    {
        #region 一些常量
        /// <summary>
        /// 名称
        /// </summary>
        public const string Name = "Tinn";
        /// <summary>
        /// 后辍
        /// </summary>
        public const string Suffix = "MobotaA*";
        /// <summary>
        /// Loading...
        /// </summary>
        public const string Loading = "Loading...";
        /// <summary>
        /// None
        /// </summary>
        public const string None = "None";
        /// <summary>
        /// All
        /// </summary>
        public const string All = "All";
        /// <summary>
        /// 默认属性搜索条件
        /// </summary>
        public static BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        #endregion

        #region 全局配置
        /// <summary>
        /// ToolBar滚动条自动隐藏
        /// </summary>
        public static bool IAutoHideScroll { get; set; }

        #endregion

        #region 其它一些静态设置
        private static string dot;
        /// <summary>
        /// 不同语言区域下的小数点
        /// </summary>
        public static string Dot
        {
            get
            {
                if (dot == null) dot = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
                return dot;
            }
        }
        /// <summary>
        /// 使用Utc时间
        /// </summary>
        public static bool IUtcTime { get; set; }

        #endregion
    }
}