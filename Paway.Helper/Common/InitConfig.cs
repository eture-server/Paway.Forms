using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 初始化时的全局变量
    /// 与IControl接口属性对应
    /// </summary>
    public abstract class InitConfig
    {
        /// <summary>
        /// 获取或设置控件的背景色
        /// </summary>
        public static Color? BackColor { get; set; }

        private static Font _font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)1);
        /// <summary>
        /// 获取或设置控件显示的文字的字体
        /// </summary>
        public static Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// 颜色透明度且颜色不透明时应用此值
        /// </summary>
        public static int? Trans { get; set; }

        /// <summary>
        /// 移动控件(父)窗体
        /// </summary>
        public static bool? IMouseMove { get; set; }
    }
}
