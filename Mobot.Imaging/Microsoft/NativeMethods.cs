using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace Mobot.Imaging.Microsoft.Extension
{
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    internal static class NativeMethods
    {
        internal const int DefaultSendMessageTimeout = 0x3e8;
        private static readonly string DirectorySeparator = Path.DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture);
        internal const int ERROR_ACCESS_DENIED = 5;
        internal const int GCL_HICONSM = -34;
        internal const string IEControlClassName = "Internet Explorer_Server";
        private static bool? is64BitOperatingSystem;
        internal const int KeyStatePressed = 0x8000;
        private static readonly object lockObject = new object();
        private const int MaxPath = 0x100;
        internal const int MaxWindowTextLength = 0x400;
        internal const int OBJID_CLIENT = -4;
        internal const int OBJID_WINDOW = 0;
        internal const uint SWP_NOACTIVATE = 0x10;
        internal const uint SWP_NOMOVE = 2;
        internal const uint SWP_NOSIZE = 1;
        private const int TOKEN_QUERY = 8;
        private const int TokenElevationTypeClass = 0x12;
        private const int TokenElevationTypeFull = 2;
        internal const int ToolBarCloseBtnIndex = 5;
        internal const int ToolBarHelpBtnIndex = 4;
        internal const int ToolBarMaxBtnIndex = 3;
        internal const int ToolBarMinBtnIndex = 2;
        private const string WindowsHtmlMessage = "WM_HTML_GETOBJECT";
        private static readonly uint WindowsHtmlMessageId = RegisterWindowMessage("WM_HTML_GETOBJECT");
        internal const int WindowsLowLevelKeyboardHookId = 13;
        internal const int WindowsLowLevelMouseHookId = 14;
        internal const int WM_HOTKEY = 0x312;
        private const int WM_NULL = 0;
        internal const int WS_CAPTION = 0xc00000;
        internal const int WS_EX_APPWINDOW = 0x40000;
        internal const int WS_EX_LAYERED = 0x80000;
        internal const int WS_EX_LAYOUTRTL = 0x400000;
        internal const int WS_EX_RIGHT = 0x1000;
        internal const int WS_EX_RTLREADING = 0x2000;
        internal const int WS_EX_TOOLWINDOW = 0x80;
        internal const int WS_EX_TOPMOST = 8;
        internal const int WS_EX_TRANSPARENT = 0x20;
        internal const int WS_MINIMIZE = 0x20000000;

        [DllImport("user32.dll")]
        internal static extern IntPtr CallNextHookEx(LowLevelHookHandle idHook, int nCode, IntPtr wParam, IntPtr lParam);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr hObject);

        [DllImport("Gdi32.dll")]
        internal static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool DestroyIcon(IntPtr handle);
        [DllImport("user32.dll")]
        internal static extern IntPtr DispatchMessage(ref MSG msg);
        [DllImport("msdrm.dll")]
        public static extern int DRMIsWindowProtected(IntPtr windowHandle, [MarshalAs(UnmanagedType.Bool)] ref bool isProtected);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, ref IntPtr lParam);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool EnumWindows(EnumWindowsProc callBack, IntPtr param);
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable"), DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr FindResource(IntPtr hModule, uint lpName, uint lpType);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("user32.dll")]
        internal static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlag gaFlags);
        [DllImport("user32.dll")]
        internal static extern short GetAsyncKeyState(int vKey);
        internal static IntPtr GetClassLongPtr(IntPtr windowHandle, int nIndex)
        {
            if (IntPtr.Size == 8)
            {
                return GetClassLongPtr64(windowHandle, nIndex);
            }
            return new IntPtr(GetClassLongPtr32(windowHandle, nIndex));
        }

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        private static extern int GetClassLongPtr32(IntPtr hWnd, int nIndex);
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist"), DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        private static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        internal static Image GetCurrenProcessIcon()
        {
            Image currenProcessIconAsIs = GetCurrenProcessIconAsIs();
            if (((currenProcessIconAsIs != null) && (currenProcessIconAsIs.Size.Height != 0x10)) && (currenProcessIconAsIs.Size.Width != 0x10))
            {
                currenProcessIconAsIs = new Bitmap(currenProcessIconAsIs, new Size(0x10, 0x10));
            }
            return currenProcessIconAsIs;
        }

        internal static Image GetCurrenProcessIconAsIs()
        {
            Bitmap bitmap = null;
            using (Process process = Process.GetCurrentProcess())
            {
                IntPtr zero = IntPtr.Zero;
                Icon icon = null;
                try
                {
                    zero = GetClassLongPtr(process.MainWindowHandle, -34);
                    if (zero != IntPtr.Zero)
                    {
                        icon = Icon.FromHandle(zero);
                    }
                    if (icon == null)
                    {
                        icon = Icon.ExtractAssociatedIcon(process.MainModule.FileName);
                    }
                    if (icon != null)
                    {
                        bitmap = icon.ToBitmap();
                    }
                }
                finally
                {
                    if (icon != null)
                    {
                        icon.Dispose();
                    }
                    if (zero != IntPtr.Zero)
                    {
                        DestroyIcon(zero);
                    }
                }
            }
            return bitmap;
        }

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        internal static extern IntPtr GetDesktopWindow();
        [DllImport("gdi32.dll")]
        internal static extern int GetDeviceCaps(IntPtr hdc, int index);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetFocus();
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool GetGUIThreadInfo(uint idThread, out GUITHREADINFO lpgui);
        [DllImport("user32.dll")]
        internal static extern IntPtr GetKeyboardLayout(uint idThread);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool GetKeyboardState(byte[] lpKeyState);
        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        [DllImport("kernel32.dll")]
        internal static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("psapi.dll", CharSet = CharSet.Auto)]
        internal static extern uint GetProcessImageFileName(IntPtr hProcess, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpImageFileName, uint nSize);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool GetTitleBarInfo(IntPtr hwnd, ref TITLEBARINFO pti);
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool GetTokenInformation(IntPtr tokenHandle, uint tokenInformationClass, out uint tokenInformation, uint tokenInformationLength, out uint returnLength);
        [SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api", Justification = "Use of equivalent CultureInfo.InstallUICulture does not always provide an equivalent for GetUserDefaultUILanguage."), DllImport("kernel32.dll")]
        public static extern ushort GetUserDefaultUILanguage();
        [DllImport("kernel32.dll")]
        public static extern uint GetVersionEx(ref OSVERSIONINFO lpVersionInfo);
        public static int GetWindowLong(IntPtr hWnd, GWLParameter index)
        {
            try
            {
                if (IntPtr.Size == 8)
                {
                    return (int)GetWindowLongPtr64(hWnd, index);
                }
                return GetWindowLong32(hWnd, index);
            }
            catch (OverflowException)
            {
                return 0;
            }
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong32(IntPtr windowHandle, GWLParameter nIndex);
        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr windowHandle, GWLParameter nIndex);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool GetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT ptr);
        [SuppressMessage("Microsoft.Usage", "CA1806")]
        internal static uint GetWindowProcessId(IntPtr windowHandle)
        {
            uint num;
            GetWindowThreadProcessId(windowHandle, out num);
            return num;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        internal static string GetWindowText(IntPtr winHandle)
        {
            string str = null;
            StringBuilder windowText = new StringBuilder(0x400);
            if (GetWindowText(winHandle, windowText, windowText.Capacity) > 0)
            {
                str = windowText.ToString();
            }
            return str;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr windowHandle, StringBuilder windowText, int maxCharCount);
        [DllImport("user32.dll")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint dwProcessId);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern ushort GlobalAddAtom([MarshalAs(UnmanagedType.LPWStr)] string lpString);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern ushort GlobalDeleteAtom(ushort atom);
        [DllImport("ieframe.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr IEIsProtectedModeURL(string url);
        [DllImport("ieframe.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr IELaunchURL(string url, ref ProcessInformation pinfo, IntPtr ptr);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool IsIconic(IntPtr hWnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool IsWindow(IntPtr hWnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool IsWindowEnabled(IntPtr hWnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool IsWindowVisible(IntPtr hWnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll")]
        private static extern bool IsWow64Process(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] out bool lpSystemInfo);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool IsZoomed(IntPtr hWnd);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint code, uint mapType);
        internal static uint MapVirtualKey(Keys keys, VirtualKeyMapType mapType)
        {
            return MapVirtualKey((uint)keys, (uint)mapType);
        }

        [DllImport("user32.dll")]
        internal static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, [MarshalAs(UnmanagedType.Bool)] bool bRepaint);
        [return: MarshalAs(UnmanagedType.IDispatch)]
        [DllImport("oleacc.dll", PreserveSig = false)]
        private static extern object ObjectFromLresult(IntPtr msgcallResult, [MarshalAs(UnmanagedType.LPStruct)] Guid refGuid, IntPtr resultRef);
        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(ProcessAccessRights dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool PeekMessage(ref MSG msg, IntPtr hwnd, int nMsgFilterMin, int nMsgFilterMax, int wRemoveMsg);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern uint QueryDosDevice([MarshalAs(UnmanagedType.LPWStr)] string lpDeviceName, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpTargetPath, uint ucchMax);
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern uint RegCloseKey(UIntPtr hkey);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern uint RegisterWindowMessage(string messageString);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint RegOpenKeyEx(UIntPtr hKey, [MarshalAs(UnmanagedType.LPWStr)] string lpSubKey, uint ulOptions, int samDesired, out UIntPtr phkResult);
        [DllImport("user32.dll")]
        internal static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, StringBuilder builder);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags fuFlags, uint uTimeout, out IntPtr lpdwResult);
        [DllImport("user32.dll")]
        internal static extern IntPtr SetActiveWindow(IntPtr hWnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern IntPtr SetFocus(IntPtr hWnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);
        internal static int SetWindowLong(IntPtr windowHandle, GWLParameter nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return (int)SetWindowLongPtr64(windowHandle, nIndex, new IntPtr(dwNewLong));
            }
            return SetWindowLong32(windowHandle, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr windowHandle, GWLParameter nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);
        internal static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }
            return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist"), DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr windowHandle, GWLParameter nIndex, IntPtr dwNewLong);
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist"), DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("User32.dll")]
        internal static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, [MarshalAs(UnmanagedType.Bool)] bool redraw);
        [DllImport("user32.dll")]
        internal static extern LowLevelHookHandle SetWindowsHookEx(int idHook, LowLevelHookProc lpfn, IntPtr hInstance, uint threadId);
        [DllImport("user32.dll")]
        internal static extern IntPtr SetWinEventHook(AccessibleEvents eventMin, AccessibleEvents eventMax, IntPtr eventHookAssemblyHandle, WinEventProc eventHookHandle, uint processId, uint threadId, SetWinEventHookParameter parameterFlags);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);
        [DllImport("kernel32.dll")]
        internal static extern uint SizeofResource(IntPtr hModule, IntPtr hResource);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool UnhookWindowsHookEx(IntPtr idHook);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool UnhookWinEvent(IntPtr eventHookHandle);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable"), DllImport("user32.dll")]
        internal static extern IntPtr WindowFromPoint(POINT pt);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool Wow64DisableWow64FsRedirection(out IntPtr oldValue);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool Wow64RevertWow64FsRedirection(IntPtr oldValue);

        internal static bool Is64BitOperatingSystem
        {
            get
            {
                if (!is64BitOperatingSystem.HasValue)
                {
                    is64BitOperatingSystem = false;
                    SYSTEM_INFO lpSystemInfo = new SYSTEM_INFO();
                    GetNativeSystemInfo(ref lpSystemInfo);
                    ProcessorArchitecture wProcessorArchitecture = (ProcessorArchitecture)lpSystemInfo.wProcessorArchitecture;
                    if (wProcessorArchitecture == ProcessorArchitecture.PROCESSOR_ARCHITECTURE_INTEL)
                    {
                        is64BitOperatingSystem = false;
                    }
                    else if ((wProcessorArchitecture == ProcessorArchitecture.PROCESSOR_ARCHITECTURE_IA64) || (wProcessorArchitecture == ProcessorArchitecture.PROCESSOR_ARCHITECTURE_AMD64))
                    {
                        is64BitOperatingSystem = true;
                    }
                    else
                    {
                        is64BitOperatingSystem = new bool?(Environment.GetEnvironmentVariable("%ProgramFiles(x86)") != null);
                    }
                }
                return is64BitOperatingSystem.Value;
            }
        }

        [Flags]
        internal enum DIALOG_STYLES
        {
            DS_SETFONT = 0x40,
            DS_SHELLFONT = 0x48
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        internal struct DLGITEMTEMPLATE
        {
            public uint style;
            public uint exStyle;
            public short x;
            public short y;
            public short cx;
            public short cy;
            public ushort id;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        internal struct DLGITEMTEMPLATEEX
        {
            public uint helpID;
            public uint exStyle;
            public uint style;
            public short x;
            public short y;
            public short cx;
            public short cy;
            public uint id;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        internal struct DLGTEMPLATE
        {
            public uint dwStyle;
            public uint dwExStyle;
            public ushort cDlgItems;
            public short x;
            public short y;
            public short cx;
            public short cy;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        internal struct DLGTEMPLATEEX
        {
            public ushort wDlgVer;
            public ushort wSignature;
            public uint dwHelpID;
            public uint dwExStyle;
            public uint dwStyle;
            public ushort cDlgItems;
            public short x;
            public short y;
            public short cx;
            public short cy;
        }

        internal delegate bool EnumWindowsProc(IntPtr windowHandle, ref IntPtr lParam);

        internal enum ExtendedWindowStyles
        {
            WS_EX_LAYERED = 0x80000,
            WS_EX_TRANSPARENT = 0x20
        }

        internal enum GetAncestorFlag : uint
        {
            GA_PARENT = 1,
            GA_ROOT = 2,
            GA_ROOTOWNER = 3
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct GUITHREADINFO
        {
            public uint cbSize;
            public uint flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public NativeMethods.RECT rcCaret;
        }

        internal enum GWLParameter
        {
            GWL_EXSTYLE = -20,
            GWL_HINSTANCE = -6,
            GWL_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KeyboardHookStruct
        {
            internal int vkCode;
            internal int scanCode;
            internal int flags;
            internal int time;
            internal int dwExtraInfo;
        }

        internal class LowLevelHookHandle : SafeHandle
        {
            public LowLevelHookHandle()
                : base(IntPtr.Zero, true)
            {
            }

            protected override bool ReleaseHandle()
            {
                if (!this.IsInvalid)
                {
                    return NativeMethods.UnhookWindowsHookEx(base.handle);
                }
                return true;
            }

            public override bool IsInvalid
            {
                get
                {
                    return (base.handle == IntPtr.Zero);
                }
            }
        }

        internal delegate IntPtr LowLevelHookProc(int code, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        internal struct MouseLLHookStruct
        {
            internal NativeMethods.POINT pt;
            internal int mouseData;
            internal int flags;
            internal int time;
            internal IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MSG
        {
            internal IntPtr hwnd;
            internal uint message;
            internal IntPtr wParam;
            internal IntPtr lParam;
            internal uint time;
            internal int ptX;
            internal int ptY;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct OSVERSIONINFO
        {
            public uint dwOSVersionInfoSize;
            public uint dwMajorVersion;
            public uint dwMinorVersion;
            public uint dwBuildNumber;
            public uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            internal int x;
            internal int y;
            internal POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [Flags]
        internal enum ProcessAccessRights : uint
        {
            PROCESS_QUERY_INFORMATION = 0x400,
            PROCESS_QUERY_LIMITED_INFORMATION = 0x1000,
            PROCESS_VM_ALL = 0x38,
            PROCESS_VM_OPERATION = 8,
            PROCESS_VM_READ = 0x10,
            PROCESS_VM_WRITE = 0x20
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct ProcessInformation
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        internal enum ProcessorArchitecture
        {
            PROCESSOR_ARCHITECTURE_AMD64 = 9,
            PROCESSOR_ARCHITECTURE_IA64 = 6,
            PROCESSOR_ARCHITECTURE_INTEL = 0
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;
        }

        [Flags]
        internal enum SendMessageTimeoutFlags
        {
            SMTO_ABORTIFHUNG = 2,
            SMTO_BLOCK = 1,
            SMTO_NORMAL = 0,
            SMTO_NOTIMEOUTIFNOTHUNG = 8
        }

        [Flags]
        internal enum SetWinEventHookParameter
        {
            WINEVENT_INCONTEXT = 4,
            WINEVENT_OUTOFCONTEXT = 0,
            WINEVENT_SKIPOWNPROCESS = 2,
            WINEVENT_SKIPOWNTHREAD = 1
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEM_INFO
        {
            public ushort wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public UIntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
            public DateTime ToDateTime()
            {
                return new DateTime(this.wYear, this.wMonth, this.wDay, this.wHour, this.wMinute, this.wSecond, this.wMilliseconds);
            }

            public static NativeMethods.SYSTEMTIME FromDateTime(DateTime dateTime)
            {
                return new NativeMethods.SYSTEMTIME { wDay = (ushort)dateTime.Day, wDayOfWeek = (ushort)dateTime.DayOfWeek, wHour = (ushort)dateTime.Hour, wMilliseconds = (ushort)dateTime.Millisecond, wMinute = (ushort)dateTime.Minute, wMonth = (ushort)dateTime.Month, wSecond = (ushort)dateTime.Second, wYear = (ushort)dateTime.Year };
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class SYSTEMTIMEARRAY
        {
            public short wYearmin;
            public short wMonthmin;
            public short wDayOfWeekmin;
            public short wDaymin;
            public short wHourmin;
            public short wMinutemin;
            public short wSecondmin;
            public short wMillisecondsmin;
            public short wYearmax;
            public short wMonthmax;
            public short wDayOfWeekmax;
            public short wDaymax;
            public short wHourmax;
            public short wMinutemax;
            public short wSecondmax;
            public short wMillisecondsmax;
            public void ToDateTimeRange(out DateTime minDate, out DateTime maxDate)
            {
                minDate = DateTime.MinValue;
                maxDate = DateTime.MaxValue;
                try
                {
                    minDate = new DateTime(this.wYearmin, this.wMonthmin, this.wDaymin, this.wHourmin, this.wMinutemin, this.wSecondmin);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
                try
                {
                    maxDate = new DateTime(this.wYearmax, this.wMonthmax, this.wDaymax, this.wHourmax, this.wMinutemax, this.wSecondmax);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            public static NativeMethods.SYSTEMTIMEARRAY FromDateTimeRange(DateTime minDate, DateTime maxDate)
            {
                return new NativeMethods.SYSTEMTIMEARRAY { wDaymin = (short)minDate.Day, wDayOfWeekmin = (short)minDate.DayOfWeek, wHourmin = (short)minDate.Hour, wMillisecondsmin = (short)minDate.Millisecond, wMinutemin = (short)minDate.Minute, wMonthmin = (short)minDate.Month, wSecondmin = (short)minDate.Second, wYearmin = (short)minDate.Year, wDaymax = (short)maxDate.Day, wDayOfWeekmax = (short)maxDate.DayOfWeek, wHourmax = (short)maxDate.Hour, wMillisecondsmax = (short)maxDate.Millisecond, wMinutemax = (short)maxDate.Minute, wMonthmax = (short)maxDate.Month, wSecondmax = (short)maxDate.Second, wYearmax = (short)maxDate.Year };
            }
        }

        [Flags]
        internal enum tagOLECONTF : uint
        {
            OLECONTF_EMBEDDINGS = 1,
            OLECONTF_LINKS = 2,
            OLECONTF_ONLYIFRUNNING = 0x10,
            OLECONTF_ONLYUSER = 8,
            OLECONTF_OTHERS = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct TITLEBARINFO
        {
            public const int CCHILDREN_TITLEBAR = 5;
            public uint cbSize;
            public NativeMethods.RECT rcTitleBar;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public AccessibleStates[] rgstate;
        }

        internal enum VirtualKeyMapType : uint
        {
            ScanCodeToVirtualKey = 1,
            ScanCodeToVirtualKeyEx = 3,
            VirtualKeyToChar = 2,
            VirtualKeyToScanCode = 0,
            VirtualKeyToScanCodeEx = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPLACEMENT
        {
            public uint length;
            public uint flags;
            public uint showCmd;
            public NativeMethods.POINT ptMinPosition;
            public NativeMethods.POINT ptMaxPosition;
            public NativeMethods.RECT rcNormalPosition;
        }

        internal enum WindowShowStyle : uint
        {
            ForceMinimized = 11,
            Hide = 0,
            Maximize = 3,
            Minimize = 6,
            Restore = 9,
            Show = 5,
            ShowDefault = 10,
            ShowMaximized = 3,
            ShowMinimized = 2,
            ShowMinNoActivate = 7,
            ShowNoActivate = 8,
            ShowNormal = 1,
            ShowNormalNoActivate = 4
        }

        internal delegate void WinEventProc(IntPtr winEventHookHandle, AccessibleEvents accEvent, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds);
    }
}

