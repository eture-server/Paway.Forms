using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Paway.Win32
{
    /// <summary>
    /// 键盘钩子
    /// GetKeyboardState获取的并不是实时的，需要先用GetKeyState刷新状态。 
    /// </summary>
    public class KeyHook
    {
        #region 变量
        /// <summary>
        /// 挂钩句柄
        /// </summary>
        private static int idHook;
        private NativeMethods.HookProc keyDelegate;

        #endregion

        #region 事件
        /// <summary>
        /// 当按下键盘按键时发生
        /// </summary>
        public event Action<char> KeyDownEvent;
        /// <summary>
        /// 当抬起键盘按键时发生
        /// </summary>
        public event Action<char> KeyUpEvent;
        /// <summary>
        /// 激发KeyDownEvent事件
        /// </summary>
        private void OnKeyDownEvent(char chr)
        {
            KeyDownEvent?.Invoke(chr);
        }
        /// <summary>
        /// 激发KeyUpEvent事件
        /// </summary>
        private void OnKeyUpEvent(char chr)
        {
            KeyUpEvent?.Invoke(chr);
        }

        #endregion

        #region 方法
        /// <summary>
        /// 安装键盘钩子
        /// </summary>
        public void Start()
        {
            if (idHook == 0)
            {
                keyDelegate = KeyHookProc;
                idHook = NativeMethods.SetWindowsHookEx(
                    HookType.WH_KEYBORARD_LL,
                    keyDelegate,
                    NativeMethods.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName),
                    0);
                if (idHook == 0) HookError();
            }
        }
        /// <summary>
        /// 卸载键盘钩子
        /// </summary>
        public void Stop()
        {
            if (idHook != 0)
            {
                var result = NativeMethods.UnhookWindowsHookEx(idHook);
                idHook = 0;
                keyDelegate = null;
                if (result == 0) HookError();
            }
        }
        /// <summary>
        /// 钩子安装/卸载失败
        /// </summary>
        private void HookError()
        {
            var errorCode = Marshal.GetLastWin32Error();
            throw new Win32Exception(string.Format("KeyHook.Uninstall_Hook()->{0:D5}.", errorCode));
        }
        private int KeyHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                var vkCode = kbh.vkCode & 0xff;//虚拟吗
                var scanCode = kbh.scanCode & 0xff;//扫描码
                byte[] kbArray = new byte[256];
                NativeMethods.GetKeyState(0);
                NativeMethods.GetKeyboardState(kbArray);
                uint uKey = 0;
                if (NativeMethods.ToAscii(vkCode, scanCode, kbArray, ref uKey, 0))
                {
                    char chr = Convert.ToChar(uKey);
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
            }
            return NativeMethods.CallNextHookEx(idHook, nCode, wParam, lParam);
        }

        #endregion
    }
}