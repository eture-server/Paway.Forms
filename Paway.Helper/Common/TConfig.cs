using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    ///     初始化时的全局变量配置
    ///     与IControl接口属性对应
    ///     Global全局
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

        #endregion

        #region 全局配置
        /// <summary>
        ///     统一风格颜色（窗体）
        /// </summary>
        public static Color? TBackColor { get; set; } = Color.CornflowerBlue;

        /// <summary>
        ///     颜色透明度且颜色不透明时应用此值
        /// </summary>
        public static int? Trans { get; set; }

        /// <summary>
        ///     移动控件(父)窗体
        /// </summary>
        public static bool? IMouseMove { get; set; } = true;

        #endregion

        #region 加载全局属性
        /// <summary>
        ///     加载IControl全局属性
        /// </summary>
        public static void Init(IControl control)
        {
            if (Trans != null && control.Trans == 255)
            {
                control.Trans = Trans ?? 255;
            }
            if (IMouseMove != null)
            {
                control.IMouseMove = IMouseMove ?? false;
            }
        }

        #endregion

        #region 其它一些常量
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