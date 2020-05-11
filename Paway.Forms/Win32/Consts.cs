using System;

namespace Paway.Forms
{
    /// <summary>
    /// Win32 API 常量
    /// </summary>
    public abstract class Consts
    {
        #region Result
        /// <summary>
        /// 1 真
        /// </summary>
        public static readonly IntPtr True = new IntPtr(1);
        /// <summary>
        /// 0 假
        /// </summary>
        public static readonly IntPtr False = new IntPtr(0);

        #endregion

        #region AC_SRC
        /// <summary>
        /// </summary>
        public const byte AC_SRC_OVER = 0x00;

        /// <summary>
        /// </summary>
        public const byte AC_SRC_ALPHA = 0x01;

        /// <summary>
        /// </summary>
        public const int ULW_ALPHA = 0x02;

        #endregion
    }
}