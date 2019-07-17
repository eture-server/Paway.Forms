using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Helper
{
    /// <summary>
    /// 图片转换类型
    /// </summary>
    public enum TConvertType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,

        /// <summary>
        /// 光暗
        /// </summary>
        Brightness,

        /// <summary>
        /// 反色
        /// </summary>
        Anti,

        /// <summary>
        /// 浮雕
        /// </summary>
        Relief,

        /// <summary>
        /// 滤色
        /// </summary>
        Color,

        /// <summary>
        /// 左右
        /// </summary>
        LeftRight,

        /// <summary>
        /// 上下
        /// </summary>
        UpDown,

        /// <summary>
        /// 黑白
        /// </summary>
        BlackWhite,

        /// <summary>
        /// 灰度
        /// </summary>
        Grayscale,

        /// <summary>
        /// 透明
        /// </summary>
        Trans,

        /// <summary>
        /// 替换
        /// </summary>
        Replace,

        /// <summary>
        /// HSL亮度，以每个像分别计算，对于高分辨率图片速度很慢
        /// </summary>
        HSL
    }

    /// <summary>
    /// 梯形方向(3D旋转)类型
    /// </summary>
    public enum T3Direction
    {
        /// <summary>
        /// 空
        /// </summary>
        None,

        /// <summary>
        /// 左侧翻转
        /// </summary>
        Left,

        /// <summary>
        /// 右侧翻转
        /// </summary>
        Right,

        /// <summary>
        /// 上侧翻转
        /// </summary>
        Up,

        /// <summary>
        /// 下侧翻转
        /// </summary>
        Down
    }
}
