using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Paway.Win32
{
    /// <summary>
    /// </summary>
    /// <param name="nCode"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

    /// <summary>
    ///     键盘钩子
    /// </summary>
    public class KeyHook
    {
        #region 变量

        private static int hHook;
        private HookProc KeyBoardHookProcedure;

        #endregion

        #region 事件

        /// <summary>
        ///     当按下键盘按键时发生
        /// </summary>
        public event KeyEventHandler KeyDownEvent;

        /// <summary>
        ///     当抬起键盘按键时发生
        /// </summary>
        public event KeyEventHandler keyUpEvent;

        #endregion

        #region 激发事件的参数

        /// <summary>
        ///     激发KeyDownEvent事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">包含事件数据的 System.Windows.Forms.KeyEventArgs</param>
        public void OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (KeyDownEvent != null)
            {
                KeyDownEvent(sender, e);
            }
        }

        /// <summary>
        ///     激发KeyUpEvent事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">包含事件数据的 System.Windows.Forms.KeyEventArgs</param>
        public void OnKeyUpEvent(object sender, KeyEventArgs e)
        {
            if (keyUpEvent != null)
            {
                keyUpEvent(sender, e);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     安装键盘钩子
        /// </summary>
        public void Install_Hook()
        {
            if (hHook == 0)
            {
                KeyBoardHookProcedure = KeyBoardHookProc;
                hHook = NativeMethods.SetWindowsHookEx(
                    HookType.WH_KEYBORARD_LL,
                    KeyBoardHookProcedure,
                    NativeMethods.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName),
                    0);

                //如果设置钩子失败
                if (hHook == 0)
                    Uninstall_Hook();
            }
        }

        /// <summary>
        ///     卸载键盘钩子
        /// </summary>
        public void Uninstall_Hook()
        {
            if (hHook != 0)
            {
                var result = NativeMethods.UnhookWindowsHookEx(hHook);
                hHook = 0;
                if (result == 0)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception("KeyHook.Uninstall_Hook()->" + NativeMethods.GetLastErrorString(errorCode));
                }
            }
        }

        private int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                var key = (Keys)Enum.Parse(typeof(Keys), kbh.vkCode.ToString());
                if (kbh.flags == 0)
                {
                    //这里写按下后做什么
                    var e = new KeyEventArgs(key);
                    OnKeyDownEvent(this, e);
                }
                else if (kbh.flags == 128)
                {
                    //放开后做什么
                    var e = new KeyEventArgs(key);
                    OnKeyUpEvent(this, e);
                }
                return 1;
            }
            return NativeMethods.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        #endregion
    }
}