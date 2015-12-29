using System.Drawing;
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
        public const string Name = "Tinn&a";

        /// <summary>
        /// </summary>
        public const string QQ = "9030140";

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

        private static Font _font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 1);

        /// <summary>
        ///     获取或设置控件显示的文字的字体
        /// </summary>
        public static Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

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
            control.BackColor = Color.Transparent;
            control.ForeColor = Color.Black;
            if (BackColor != null)
            {
                control.BackColor = BackColor ?? Color.Empty;
            }
            if (ForeColor != null)
            {
                control.ForeColor = ForeColor ?? Color.Empty;
            }
            if (Font.Name != "宋体" || Font.Size != 9f || Font.Style != FontStyle.Regular ||
                Font.Unit != GraphicsUnit.Point || Font.GdiCharSet != 1)
            {
                control.Font = Font;
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
    }
}