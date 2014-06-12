using System;
using System.Text;

namespace Paway.Forms
{
    /// <summary>
    /// 位置
    /// </summary>
    public enum TLocation
    {
        /// <summary>
        /// 向上
        /// </summary>
        Up,
        /// <summary>
        /// 向下
        /// </summary>
        Down,
        /// <summary>
        /// 向左
        /// </summary>
        Left,
        /// <summary>
        /// 向右
        /// </summary>
        Right,
        /// <summary>
        /// 居中
        /// </summary>
        Center,
    }
    /// <summary>
    /// 方向
    /// </summary>
    public enum TDirection
    {
        /// <summary>
        /// 水平
        /// </summary>
        Level,
        /// <summary>
        /// 垂直
        /// </summary>
        Vertical,
    }
    /// <summary>
    /// 事件触发
    /// </summary>
    public enum TEvent
    {
        /// <summary>
        /// 按下时
        /// </summary>
        Down,
        /// <summary>
        /// 抬起时
        /// </summary>
        Up,
    }
}
