using Paway.Helper;
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
    }
    /// <summary>
    /// 图片位置
    /// </summary>
    public enum TILocation
    {
        /// <summary>
        /// 上面
        /// </summary>
        Up,
        /// <summary>
        /// 左面
        /// </summary>
        Left,
    }
    /// <summary>
    /// 方向
    /// </summary>
    public enum TDirection
    {
        /// <summary>
        /// 水平
        /// </summary>
        [EnumTextValue("水平")]
        Level,
        /// <summary>
        /// 垂直
        /// </summary>
        [EnumTextValue("垂直")]
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
