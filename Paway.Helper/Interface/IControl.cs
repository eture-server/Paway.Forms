using System.Drawing;

namespace Paway.Helper
{
    /// <summary>
    ///     控件接口
    /// </summary>
    public interface IControl
    {
        /// <summary>
        ///     控件透明度
        /// </summary>
        int Trans { get; set; }

        /// <summary>
        ///     移动控件(父)窗体
        /// </summary>
        bool IMouseMove { get; set; }

        /// <summary>
        ///     坐标点是否包含在项中
        /// </summary>
        bool Contain(Point p);
    }
}