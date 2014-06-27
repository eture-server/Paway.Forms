using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 控件接口
    /// </summary>
    public interface IControl
    {
        /// <summary>
        /// 坐标点是否包含在项中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        bool Contain(Point p);

        /// <summary>
        /// 控件透明度
        /// </summary>
        int Trans { get; set; }

        /// <summary>
        /// 表示控件背景色
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// 获取或设置控件显示的文字的字体
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// 移动控件(父)窗体
        /// </summary>
        bool IMouseMove { get; set; }
    }
}
