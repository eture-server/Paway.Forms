using System;

namespace Paway.Win32
{
    /// <summary>
    ///     Win32 API 常量
    /// </summary>
    public abstract class Consts
    {
        #region Result
        /// <summary>
        ///     1 真
        /// </summary>
        public static readonly IntPtr TRUE = new IntPtr(1);

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