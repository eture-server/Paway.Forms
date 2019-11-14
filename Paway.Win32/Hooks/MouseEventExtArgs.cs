using System.Windows.Forms;

namespace Paway.Win32
{
    /// <summary>
    /// 鼠标钩子扩展事件
    /// </summary>
    public class MouseEventExtArgs : MouseEventArgs
    {
        #region 属性
        /// <summary>
        /// 如果为 true 防止进一步的处理其他应用程序事件。
        /// </summary>
        public bool Handled { get; set; }

        #endregion

        #region 构造函数
        /// <summary>
        /// 鼠标事件数据
        /// </summary>
        public MouseEventExtArgs(MouseButtons buttons, int clicks, int x, int y, int delta) : base(buttons, clicks, x, y, delta) { }
        /// <summary>
        /// 鼠标事件数据
        /// </summary>
        public MouseEventExtArgs(MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta) { }

        #endregion
    }
}