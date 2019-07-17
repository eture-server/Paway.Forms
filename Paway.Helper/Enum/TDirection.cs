using Paway.Helper;
using System.ComponentModel;

namespace Paway.Helper
{
    /// <summary>
    /// 图片位置
    /// </summary>
    public enum TLocation
    {
        /// <summary>
        /// 上面
        /// </summary>
        Up,

        /// <summary>
        /// 左面
        /// </summary>
        Left
    }

    /// <summary>
    /// 方向
    /// </summary>
    public enum TDirection
    {
        /// <summary>
        /// 水平
        /// </summary>
        [Description("水平")]
        Level,

        /// <summary>
        /// 垂直
        /// </summary>
        [Description("垂直")]
        Vertical
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
        Up
    }

    /// <summary>
    /// 移动方向
    /// </summary>
    public enum TMDirection
    {
        /// <summary>
        /// 不移动
        /// </summary>
        None,

        /// <summary>
        /// 不移动只触发事件
        /// </summary>
        Normal,

        /// <summary>
        /// 从上到下
        /// </summary>
        Up,

        /// <summary>
        /// 从下到上
        /// </summary>
        Down,

        /// <summary>
        /// 从左到右
        /// </summary>
        Left,

        /// <summary>
        /// 从右到左
        /// </summary>
        Right,

        /// <summary>
        /// 从中间开始
        /// </summary>
        Center,

        /// <summary>
        /// 透明度渐进
        /// </summary>
        Transparent,

        /// <summary>
        /// 3D左边旋转
        /// </summary>
        T3DLeft,

        /// <summary>
        /// 3D旋转从左到右
        /// </summary>
        T3DLeftToRight,

        /// <summary>
        /// 3D右边旋转
        /// </summary>
        T3DRight,

        /// <summary>
        /// 3D旋转从右到左
        /// </summary>
        T3DRightToLeft,

        /// <summary>
        /// 3D上边旋转
        /// </summary>
        T3DUp,

        /// <summary>
        /// 3D旋转从上到下
        /// </summary>
        T3DUpToDown,

        /// <summary>
        /// 3D下边旋转
        /// </summary>
        T3DDown,

        /// <summary>
        /// 3D旋转从下到上
        /// </summary>
        T3DDownToUp
    }
}