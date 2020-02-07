using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Paway.Win32
{
    /// <summary>
    /// 鼠标钩子
    /// </summary>
    public class MouseHook
    {
        #region 变量
        /// <summary>
        /// 挂钩句柄
        /// </summary>
        private int idHook;
        private NativeMethods.HookProc mouseDelegate;

        /// <summary>
        /// 记录鼠标所按下的键
        /// </summary>
        private MouseButtons mouseButton;
        /// <summary>
        /// 双击鼠标的间隔计时器
        /// </summary>
        private Timer doubleClickTimer;

        private int oldX;
        private int oldY;

        #endregion

        #region 事件
        /// <summary>
        /// 鼠标操作事件
        /// </summary>
        public event EventHandler<MouseEventExtArgs> MouseEvent;
        /// <summary>
        /// 鼠标松开事件
        /// </summary>
        public event EventHandler<MouseEventExtArgs> MouseUpEvent;
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        public event EventHandler<MouseEventExtArgs> MouseDownEvent;
        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        public event EventHandler<MouseEventExtArgs> MouseClickEvent;
        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        public event EventHandler<MouseEventExtArgs> MouseDoubleClickEvent;
        /// <summary>
        /// 鼠标滚轮滚动事件
        /// </summary>
        public event EventHandler<MouseEventExtArgs> MouseWheelEvent;
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        public event EventHandler<MouseEventExtArgs> MouseMoveEvent;

        #endregion

        #region 方法
        /// <summary>
        /// 安装鼠标钩子
        /// </summary>
        public void Start()
        {
            if (idHook == 0)
            {
                mouseDelegate = MouseHookProc;
                idHook = NativeMethods.SetWindowsHookEx(
                    HookType.WH_MOUSE_LL,
                    mouseDelegate,
                    NativeMethods.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName),
                    0);
                if (idHook == 0) HookError();
                doubleClickTimer = new Timer
                {
                    Interval = NativeMethods.GetDoubleClickTime(),
                    Enabled = false
                };
                doubleClickTimer.Tick += DoubleClickTimer_Tick;
                MouseUpEvent += OnMouseUp;
            }
        }
        /// <summary>
        /// 卸载鼠标钩子
        /// </summary>
        public void Stop()
        {
            if (idHook != 0)
            {
                var result = NativeMethods.UnhookWindowsHookEx(idHook);
                idHook = 0;
                mouseDelegate = null;
                if (result == 0) HookError();
                MouseUpEvent -= OnMouseUp;
                // 释放计时器
                doubleClickTimer.Tick -= DoubleClickTimer_Tick;
                doubleClickTimer.Dispose();
                doubleClickTimer = null;
            }
        }
        /// <summary>
        /// 钩子安装/卸载失败
        /// </summary>
        private void HookError()
        {
            var errorCode = Marshal.GetLastWin32Error();
            throw new Win32Exception(string.Format("MouseHook.Uninstall_Hook()->{0:D5}.", errorCode));
        }
        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var mouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                var button = MouseButtons.None;
                short mouseDelta = 0;
                var clickCount = 0;
                var mouseDown = false;
                var mouseUp = false;
                switch ((WindowsMessage)wParam)
                {
                    case WindowsMessage.WM_LBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WindowsMessage.WM_LBUTTONUP:
                        mouseUp = true;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WindowsMessage.WM_LBUTTONDBLCLK:
                        button = MouseButtons.Left;
                        clickCount = 2;
                        break;
                    case WindowsMessage.WM_RBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WindowsMessage.WM_RBUTTONUP:
                        mouseUp = true;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WindowsMessage.WM_RBUTTONDBLCLK:
                        button = MouseButtons.Right;
                        clickCount = 2;
                        break;
                    case WindowsMessage.WM_MOUSEWHEEL:
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
                        break;
                }
                var e = new MouseEventExtArgs(
                    button,
                    clickCount,
                    mouseHookStruct.Point.X,
                    mouseHookStruct.Point.Y,
                    mouseDelta);
                if (MouseEvent != null)
                    MouseEvent.Invoke(null, e);
                if (MouseUpEvent != null && mouseUp)
                    MouseUpEvent.Invoke(null, e);
                if (MouseDownEvent != null && mouseDown)
                    MouseDownEvent.Invoke(null, e);
                if (MouseClickEvent != null && clickCount > 0)
                    MouseClickEvent.Invoke(null, e);
                if (MouseDoubleClickEvent != null && clickCount == 2)//这里不会触发
                    MouseDoubleClickEvent.Invoke(null, e);
                if (MouseWheelEvent != null && mouseDelta != 0)
                    MouseWheelEvent.Invoke(null, e);
                if (MouseMoveEvent != null && (oldX != mouseHookStruct.Point.X || oldY != mouseHookStruct.Point.Y))
                {
                    oldX = mouseHookStruct.Point.X;
                    oldY = mouseHookStruct.Point.Y;
                    if (MouseMoveEvent != null)
                        MouseMoveEvent.Invoke(null, e);
                }
                if (e.Handled) return -1;
            }
            return NativeMethods.CallNextHookEx(idHook, nCode, wParam, lParam);
        }

        #endregion

        #region 双击处理
        /// <summary>
        /// </summary>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Clicks < 1)
                return;
            if (e.Button.Equals(mouseButton))
            {
                if (MouseDoubleClickEvent != null)
                    MouseDoubleClickEvent.Invoke(null, new MouseEventExtArgs(e));
                // 停止计时器
                doubleClickTimer.Enabled = false;
                mouseButton = MouseButtons.None;
            }
            else
            {
                doubleClickTimer.Enabled = true;
                mouseButton = e.Button;
            }
        }
        /// <summary>
        /// </summary>
        private void DoubleClickTimer_Tick(object sender, EventArgs e)
        {
            doubleClickTimer.Enabled = false;
            mouseButton = MouseButtons.None;
        }

        #endregion
    }
}