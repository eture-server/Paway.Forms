using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    ///     一些公共方法
    /// </summary>
    public abstract class TMethod
    {
        #region 关于四舍五入
        /// <summary>
        /// 中国式四舍五入,取两位
        /// </summary>
        public static double Round(double value)
        {
            return (double)Math.Round((decimal)value.ClearError(), 2, MidpointRounding.AwayFromZero);
        }

        #endregion

        #region 关于随机数
        private static Random random;
        /// <summary>
        /// 获取一个随机整数
        /// </summary>
        public static int Random()
        {
            return Random(int.MaxValue / 100);
        }
        /// <summary>
        /// 获取一个随机整数(指定最大值)
        /// </summary>
        public static int Random(int max)
        {
            return Random(0, max);
        }
        /// <summary>
        /// 获取一个随机整数(指定最大、最小值)
        /// </summary>
        public static int Random(int min, int max)
        {
            if (max <= min) return min;
            if (random == null)
            {
                random = new Random();
            }
            int index = random.Next(0, max * 100);
            index %= max - min;
            index += min;
            return index;
        }

        #endregion

        #region 最大公约数
        /// <summary>
        /// 两个数的最大公约数
        /// </summary>
        public static int MaxDivisor(int x, int y)
        {
            int z;
            while (y != 0)
            {
                z = x % y;
                x = y;
                y = z;
            }
            return (x);
        }

        #endregion
    }
}