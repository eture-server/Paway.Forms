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
        /// </summary>
        public const string Name = "Tinn";

        #endregion

        #region 全局配置

        /// <summary>
        ///     获取或设置控件的背景色
        /// </summary>
        public static Color? BackColor { get; set; }

        /// <summary>
        ///     获取或设置控件的前景色
        /// </summary>
        public static Color? ForeColor { get; set; }

        /// <summary>
        ///     颜色透明度且颜色不透明时应用此值
        /// </summary>
        public static int? Trans { get; set; }

        /// <summary>
        ///     移动控件(父)窗体
        /// </summary>
        public static bool? IMouseMove { get; set; }

        #endregion

        #region 加载全局属性

        /// <summary>
        ///     加载全局属性
        /// </summary>
        public static void Init(Control control)
        {
            if (BackColor != null)
            {
                control.BackColor = BackColor ?? Color.Empty;
            }
            if (ForeColor != null)
            {
                control.ForeColor = ForeColor ?? Color.Empty;
            }
            if (control is IControl)
            {
                Init(control as IControl);
            }
        }

        /// <summary>
        ///     加载IControl全局属性
        /// </summary>
        private static void Init(IControl control)
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