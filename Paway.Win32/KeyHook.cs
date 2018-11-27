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
        public event Action<char> KeyDownEvent;

        /// <summary>
        ///     当抬起键盘按键时发生
        /// </summary>
        public event Action<char> KeyUpEvent;

        #endregion

        #region 激发事件的参数

        /// <summary>
        ///     激发KeyDownEvent事件
        /// </summary>
        public void OnKeyDownEvent(char chr)
        {
            KeyDownEvent?.Invoke(chr);
        }

        /// <summary>
        ///     激发KeyUpEvent事件
        /// </summary>
        public void OnKeyUpEvent(char chr)
        {
            KeyUpEvent?.Invoke(chr);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     安装键盘钩子
        /// </summary>
        public void Start()
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
                if (hHook == 0) Stop();
            }
        }

        /// <summary>
        ///     卸载键盘钩子
        /// </summary>
        public void Stop()
        {
            if (hHook != 0)
            {
                var result = NativeMethods.UnhookWindowsHookEx(hHook);
                hHook = 0;
                if (result == 0)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(string.Format("MouseHook.Uninstall_Hook()->{0:D5}", errorCode));
                }
            }
        }

        private int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                var vkCode = kbh.vkCode & 0xff;//虚拟吗
                var scanCode = kbh.scanCode & 0xff;//扫描码
                byte[] kbArray = new byte[256];
                NativeMethods.GetKeyboardState(kbArray);
                uint uKey = 0;
                char chr = (char)0;
                if (NativeMethods.ToAscii(vkCode, scanCode, kbArray, ref uKey, 0))
                {
                    chr = Convert.ToChar(uKey);
                }
                var key = (Keys)Enum.Parse(typeof(Keys), kbh.vkCode.ToString());
                if (kbh.flags == 0)
                {
                    //这里写按下后做什么
                    OnKeyDownEvent(chr);
                }
                else if (kbh.flags == 128)
                {
                    //放开后做什么
                    OnKeyUpEvent(chr);
                }
            }
            return NativeMethods.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        #endregion
    }
}