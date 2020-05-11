using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// Win32API 发送消息与查找窗体
    /// </summary>
    internal abstract class Win32Helper
    {
        /// <summary>
        /// 获取线程执行的周期个数。
        /// </summary>
        [DllImport("kernel32.dll")]
        internal static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);
        /// <summary>
        /// 获取当前线程的一个伪句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentThread();

        /// <summary>
        /// 获取一个前台窗口的句柄
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 激活指定窗口(无论是否最小化)
        /// </summary>
        /// <param name="hWnd">要激活的窗口句柄</param>
        /// <param name="fAltTab">是否使最小化的窗口还原</param>
        [DllImport("user32.dll")]
        internal static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
    }
}