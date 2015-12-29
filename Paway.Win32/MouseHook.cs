using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Paway.Win32
{
    /// <summary>
    ///     鼠标钩子
    /// </summary>
    public class MouseHook
    {
        #region 变量

        /// <summary>
        ///     处理鼠标钩子的过程
        /// </summary>
        private int MouseHookHandle;

        private HookProc MouseDelegate;

        /// <summary>
        ///     记录鼠标所按下的键
        /// </summary>
        private MouseButtons MouseButton;

        /// <summary>
        ///     双击鼠标的间隔计时器
        /// </summary>
        private Timer DoubleClickTimer;

        private int Old_X;
        private int Old_Y;

        #endregion

        #region 事件

        /// <summary>
        /// </summary>
        private event MouseEventHandler EventMouseMove;

        /// <summary>
        /// </summary>
        private event MouseEventHandler EventMouseDown;

        /// <summary>
        /// </summary>
        private event MouseEventHandler EventMouseUp;

        /// <summary>
        /// </summary>
        private event MouseEventHandler EventMouseWheel;

        /// <summary>
        /// </summary>
        private event MouseEventHandler EventMouseClick;

        /// <summary>
        /// </summary>
        private event MouseEventHandler EventMouseDoubleClick;

        /// <summary>
        /// </summary>
        private event EventHandler<MouseEventExtArgs> EventMouseMoveExt;

        /// <summary>
        /// </summary>
        private event EventHandler<MouseEventExtArgs> EventMouseClickExt;

        /// <summary>
        /// </summary>
        public event MouseEventHandler MouseMove
        {
            add
            {
                Install_Hook();
                EventMouseMove += value;
            }
            remove
            {
                EventMouseMove -= value;
                Uninstall_Hook();
            }
        }

        /// <summary>
        /// </summary>
        public event MouseEventHandler MouseDown
        {
            add
            {
                Install_Hook();
                EventMouseDown += value;
            }
            remove
            {
                EventMouseDown -= value;
                Uninstall_Hook();
            }
        }

        /// <summary>
        /// </summary>
        public event MouseEventHandler MouseUp
        {
            add
            {
                Install_Hook();
                EventMouseUp += value;
            }
            remove
            {
                EventMouseUp -= value;
                Uninstall_Hook();
            }
        }

        /// <summary>
        /// </summary>
        public event MouseEventHandler MouseWheel
        {
            add
            {
                Install_Hook();
                EventMouseWheel += value;
            }
            remove
            {
                EventMouseWheel -= value;
                Uninstall_Hook();
            }
        }

        /// <summary>
        /// </summary>
        public event MouseEventHandler MouseClick
        {
            add
            {
                Install_Hook();
                EventMouseClick += value;
            }
            remove
            {
                EventMouseClick -= value;
                Uninstall_Hook();
            }
        }

        /// <summary>
        /// </summary>
        public event MouseEventHandler MouseDoubleClick
        {
            add
            {
                Install_Hook();
                if (EventMouseDoubleClick == null)
                {
                    DoubleClickTimer = new Timer
                    {
                        Interval = NativeMethods.GetDoubleClickTime(),
                        Enabled = false
                    };
                    DoubleClickTimer.Tick += DoubleClickTimer_Tick;
                    MouseUp += OnMouseUp;
                }
                EventMouseDoubleClick += value;
            }
            remove
            {
                if (EventMouseDoubleClick != null)
                {
                    EventMouseDoubleClick -= value;
                    if (EventMouseDoubleClick == null)
                    {
                        MouseUp -= OnMouseUp;
                        // 释放计时器
                        DoubleClickTimer.Tick -= DoubleClickTimer_Tick;
                        DoubleClickTimer.Dispose();
                        DoubleClickTimer = null;
                    }
                }
                Uninstall_Hook();
            }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     安装钩子
        /// </summary>
        public void Install_Hook()
        {
            if (MouseHookHandle == 0)
            {
                MouseDelegate = MouseHookProc;
                MouseHookHandle = NativeMethods.SetWindowsHookEx(
                    HookType.WH_MOUSE_LL,
                    MouseDelegate,
                    NativeMethods.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName),
                    0);
                if (MouseHookHandle == 0)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception("MouseHook.EnsureGlobalMouseEvents()->" +
                                             NativeMethods.GetLastErrorString(errorCode));
                }
            }
        }

        /// <summary>
        ///     卸载钩子
        /// </summary>
        public void Uninstall_Hook()
        {
            if (EventMouseClick == null &&
                EventMouseDown == null &&
                EventMouseMove == null &&
                EventMouseUp == null &&
                EventMouseWheel == null &&
                EventMouseClickExt == null &&
                EventMouseMoveExt == null)
            {
                if (MouseHookHandle != 0)
                {
                    // 卸载钩子
                    var result = NativeMethods.UnhookWindowsHookEx(MouseHookHandle);
                    MouseHookHandle = 0;
                    MouseDelegate = null;
                    if (result == 0)
                    {
                        var errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception("MouseHook.Uninstall_Hook()->" +
                                                 NativeMethods.GetLastErrorString(errorCode));
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
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

                switch (wParam)
                {
                    case (int)WindowsMessage.WM_LBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case (int)WindowsMessage.WM_LBUTTONUP:
                        mouseUp = true;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case (int)WindowsMessage.WM_LBUTTONDBLCLK:
                        button = MouseButtons.Left;
                        clickCount = 2;
                        break;
                    case (int)WindowsMessage.WM_RBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case (int)WindowsMessage.WM_RBUTTONUP:
                        mouseUp = true;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case (int)WindowsMessage.WM_RBUTTONDBLCLK:
                        button = MouseButtons.Right;
                        clickCount = 2;
                        break;
                    case (int)WindowsMessage.WM_MOUSEWHEEL:
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
                        break;
                }

                var e = new MouseEventExtArgs(
                    button,
                    clickCount,
                    mouseHookStruct.Point.X,
                    mouseHookStruct.Point.Y,
                    mouseDelta);

                if (EventMouseUp != null && mouseUp)
                    EventMouseUp.Invoke(null, e);
                if (EventMouseDown != null && mouseDown)
                    EventMouseDown.Invoke(null, e);
                if (EventMouseClick != null && clickCount > 0)
                    EventMouseClick.Invoke(null, e);
                if (EventMouseClickExt != null && clickCount > 0)
                    EventMouseClickExt.Invoke(null, e);
                if (EventMouseDoubleClick != null && clickCount == 2)
                    EventMouseDoubleClick.Invoke(null, e);
                if (EventMouseWheel != null && mouseDelta != 0)
                    EventMouseWheel.Invoke(null, e);
                if ((EventMouseMove != null || EventMouseMoveExt != null) &&
                    (Old_X != mouseHookStruct.Point.X || Old_Y != mouseHookStruct.Point.Y))
                {
                    Old_X = mouseHookStruct.Point.X;
                    Old_Y = mouseHookStruct.Point.Y;
                    if (EventMouseMove != null)
                        EventMouseMove.Invoke(null, e);
                    if (EventMouseMoveExt != null)
                        EventMouseMoveExt.Invoke(null, e);
                }
                if (e.Handled)
                    return -1;
            }
            return NativeMethods.CallNextHookEx(MouseHookHandle, nCode, wParam, lParam);
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Clicks < 1)
                return;
            if (e.Button.Equals(MouseButton))
            {
                if (EventMouseDoubleClick != null)
                    EventMouseDoubleClick.Invoke(null, e);
                // 停止计时器
                DoubleClickTimer.Enabled = false;
                MouseButton = MouseButtons.None;
            }
            else
            {
                DoubleClickTimer.Enabled = true;
                MouseButton = e.Button;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleClickTimer_Tick(object sender, EventArgs e)
        {
            DoubleClickTimer.Enabled = false;
            MouseButton = MouseButtons.None;
        }

        #endregion
    }
}