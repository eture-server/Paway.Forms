using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Paway.Win32
{
    /// <summary>
    ///     Win32API 发送消息与查找窗体
    /// </summary>
    public abstract class Win32Helper
    {
        #region 静态变量
        /// <summary>
        /// </summary>
        public static IntPtr True { get { return (IntPtr)1; } }

        /// <summary>
        /// </summary>
        public static IntPtr False { get { return (IntPtr)0; } }

        #endregion

        #region 全屏置顶窗体

        /// <summary>
        ///     全屏置顶窗体
        /// </summary>
        /// <param name="hwnd">窗体句柄</param>
        public static void SetWinFullScreen(IntPtr hwnd)
        {
            var HWND_TOP = 0;
            uint SWP_SHOWWINDOW = 0x0040;

            var SM_CXSCREEN = 0;
            var SM_CYSCREEN = 1;
            var ScreenX = NativeMethods.GetSystemMetrics(SM_CXSCREEN);
            var ScreenY = NativeMethods.GetSystemMetrics(SM_CYSCREEN);
            NativeMethods.SetWindowPos(hwnd, HWND_TOP, 0, 0, ScreenX, ScreenY, SWP_SHOWWINDOW);
        }

        #endregion

        #region 任务栏

        /// <summary>
        ///     获取任务栏Rectangle
        /// </summary>
        /// <returns></returns>
        public static RECT TaskRect()
        {
            var hWnd = NativeMethods.FindWindow("Shell_TrayWnd", null);
            var rc = new RECT();
            NativeMethods.GetWindowRect(hWnd, ref rc);
            return rc;
        }

        /// <summary>
        ///     隐藏任务栏
        /// </summary>
        public static void HideTask()
        {
            var hWnd = NativeMethods.FindWindow("Shell_TrayWnd", null);
            NativeMethods.ShowWindow(hWnd, WindowShowStyle.Hide);
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        public static void ShowWindow(IntPtr handle)
        {
            NativeMethods.ShowWindow(handle, WindowShowStyle.ShowNormalNoActivate);
        }

        /// <summary>
        ///     显示任务栏
        /// </summary>
        public static void ShowTask()
        {
            var hWnd = NativeMethods.FindWindow("Shell_TrayWnd", null);
            NativeMethods.ShowWindow(hWnd, WindowShowStyle.Show);
        }

        #endregion

        #region SendMessage - 扩展消息

        /// <summary>
        ///     发送跨进程自定义消息
        /// </summary>
        public static IntPtr SendMessage<T>(string form, int type, T msg)
        {
            var hWnd = FindWindow(form);
            if (hWnd != IntPtr.Zero)
            {
                UserDataStruct cds;
                cds.vData = IntPtr.Zero;
                cds.lData = Marshal.SizeOf(typeof(T));
                cds.uData = StructureToByte(msg);
                NativeMethods.SendMessage(hWnd, (int)WindowsMessage.WM_COPYDATA, type, ref cds);
                return hWnd;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        ///     发送跨进程自定义消息 到所有指定结尾的窗体
        /// </summary>
        public static void SendMessageAll<T>(string form, int type, T msg)
        {
            var hList = FindWindowEnd(form);
            foreach (var hWnd in hList.Keys)
            {
                if (hWnd != IntPtr.Zero)
                {
                    UserDataStruct cds;
                    cds.vData = IntPtr.Zero;
                    cds.lData = Marshal.SizeOf(typeof(T));
                    cds.uData = StructureToByte(msg);
                    NativeMethods.SendMessage(hWnd, (int)WindowsMessage.WM_COPYDATA, type, ref cds);
                }
            }
        }

        /// <summary>
        ///     解析自定义消息 - 形参
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public static T GetUserStruct<T>(Message m)
        {
            var mystr = (UserDataStruct)m.GetLParam(typeof(UserDataStruct));
            var msg = ByteToStructure<T>(mystr.uData);
            return msg;
        }

        #endregion

        #region SendMessage - 自定义消息

        /// <summary>
        ///     发送跨进程自定义消息
        /// </summary>
        /// <param name="form">窗体名称</param>
        /// <param name="type">消息类型</param>
        /// <param name="msg">消息内容</param>
        /// <returns>成功返回窗体句柄</returns>
        public static IntPtr SendMessage(string form, int type, string msg)
        {
            var hWnd = FindWindow(form);
            if (hWnd != IntPtr.Zero)
            {
                SendMessage(hWnd, type, msg);
                return hWnd;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        ///     发送跨进程自定义消息 到所有指定结尾的窗体
        /// </summary>
        /// <param name="form">窗体名称</param>
        /// <param name="type">消息类型</param>
        /// <param name="msg">消息内容</param>
        public static void SendMessageAll(string form, int type, string msg)
        {
            var hList = FindWindowEnd(form);
            foreach (var hWnd in hList.Keys)
            {
                if (hWnd != IntPtr.Zero)
                {
                    SendMessage(hWnd, type, msg);
                }
            }
        }

        /// <summary>
        ///     发送消息到指定句柄的窗体
        /// </summary>
        public static void SendMessage(IntPtr hWnd, int type, string msg)
        {
            var cds = GetStruct(msg);
            NativeMethods.SendMessage(hWnd, (int)WindowsMessage.WM_COPYDATA, type, ref cds);
        }

        /// <summary>
        ///     解析自定义消息 - 字符
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string GetFromStruct(Message m)
        {
            var mystr = (CopyDataStruct)m.GetLParam(typeof(CopyDataStruct));
            return mystr.mData;
        }

        /// <summary>
        ///     返回自定义消息体
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static CopyDataStruct GetStruct(string msg)
        {
            if (msg == null) msg = string.Empty;
            var len = Encoding.Default.GetBytes(msg).Length;
            CopyDataStruct cds;
            cds.iData = (IntPtr)100;
            cds.mData = msg;
            cds.lData = len + 1;
            return cds;
        }

        #endregion

        #region ActiveForm

        /// <summary>
        ///     激活指定标题窗体
        /// </summary>
        public static IntPtr ActiveForm(string find)
        {
            var hWnd = FindWindow(find);
            if (IntPtr.Zero != hWnd)
            {
                ActiveForm(hWnd);
                return hWnd;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        ///     激活指定句柄窗体
        /// </summary>
        /// <param name="hWnd"></param>
        public static void ActiveForm(IntPtr hWnd)
        {
            NativeMethods.SwitchToThisWindow(hWnd, true);
        }

        #endregion

        #region FindWindowEnd(find)

        /// <summary>
        ///     查找所有指定以 find 结束的窗体
        /// </summary>
        /// <returns>返回窗体名称列表</returns>
        public static List<string> FindStringEnd(string find)
        {
            if (find == null) find = string.Empty;
            var result = find;
            NativeMethods.EnumWindows(EnumCallBack_StrEnd, ref result);
            var list = new List<string>();
            var data = result.Split(';');
            foreach (var item in data)
            {
                list.Add(item);
            }
            return list;
        }

        private static bool EnumCallBack_StrEnd(IntPtr hWnd, ref string lParam)
        {
            var sb = new StringBuilder(0x200);
            NativeMethods.GetWindowText(hWnd, sb, 0x200);
            var find = lParam.Split(';')[0];
            if (sb.ToString().EndsWith(find))
            {
                lParam = string.Format("{0};{1}", lParam, sb);
            }
            return true;
        }

        /// <summary>
        ///     查找所有指定以 find 结束的窗体
        /// </summary>
        /// <returns>返回窗体句柄列表</returns>
        public static List<IntPtr> FindHandleEnd(string find)
        {
            if (find == null) find = string.Empty;
            var result = find;
            NativeMethods.EnumWindows(EnumCallBack_HandleEnd, ref result);
            var list = new List<IntPtr>();
            var data = result.Split(';');
            foreach (var item in data)
            {
                list.Add((IntPtr)Convert.ToInt32(item));
            }
            return list;
        }

        private static bool EnumCallBack_HandleEnd(IntPtr hWnd, ref string lParam)
        {
            var sb = new StringBuilder(0x200);
            NativeMethods.GetWindowText(hWnd, sb, 0x200);
            var find = lParam.Split(';')[0];
            if (sb.ToString().EndsWith(find))
            {
                lParam = string.Format("{0};{1}", lParam, hWnd);
            }
            return true;
        }

        /// <summary>
        ///     查找所有指定以 find 结束的窗体
        /// </summary>
        /// <returns>返回窗体名称、句柄列表</returns>
        public static Dictionary<IntPtr, string> FindWindowEnd(string find)
        {
            if (find == null) find = string.Empty;
            var result = find;
            NativeMethods.EnumWindows(EnumCallBack_End, ref result);
            var list = new Dictionary<IntPtr, string>();
            var data = result.Split(';');
            for (var i = 1; i < data.Length; i += 2)
            {
                list.Add((IntPtr)Convert.ToInt32(data[i]), data[i + 1]);
            }
            return list;
        }

        private static bool EnumCallBack_End(IntPtr hWnd, ref string lParam)
        {
            var sb = new StringBuilder(0x200);
            NativeMethods.GetWindowText(hWnd, sb, 0x200);
            var find = lParam.Split(';')[0];
            if (sb.ToString().EndsWith(find))
            {
                lParam = string.Format("{0};{1};{2}", lParam, hWnd, sb);
            }
            return true;
        }

        #endregion

        #region FindWindow(find)

        /// <summary>
        ///     查找 符合指定标题:find 的第一个窗口。
        /// </summary>
        /// <returns>返回句柄</returns>
        public static IntPtr FindWindow(string find)
        {
            var zero = Marshal.StringToHGlobalAnsi(find);
            if (!NativeMethods.EnumWindows(EnumCallBack, ref zero))
            {
                return zero;
            }
            return IntPtr.Zero;
        }

        private static bool EnumCallBack(IntPtr hWnd, ref IntPtr lParam)
        {
            var sb = new StringBuilder(0x200);
            NativeMethods.GetWindowText(hWnd, sb, 0x200);
            var find = Marshal.PtrToStringAnsi(lParam);
            if (sb.ToString() == find)
            {
                lParam = hWnd;
                return false;
            }
            return true;
        }

        #endregion

        #region 隐藏显示光标

        /// <summary>
        ///     显示光标
        /// </summary>
        public static void CursorShow()
        {
            while (true)
            {
                var count = NativeMethods.ShowCursor(1);
                if (count > 0) break;
            }
        }

        /// <summary>
        ///     隐藏光标
        /// </summary>
        public static void CursorHide()
        {
            while (true)
            {
                var count = NativeMethods.ShowCursor(0);
                if (count < 0) break;
            }
        }

        #endregion

        #region 结构体-内存
        /// <summary>
        ///     由结构体分配到内存，返回句柄
        ///     需指定大小
        ///     [MarshalAs(UnmanagedType.LPStr, SizeConst = 1024)]
        /// </summary>
        public static IntPtr StructureToByte<T>(T structure)
        {
            var size = Marshal.SizeOf(typeof(T));
            var bufferIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, bufferIntPtr, true);
                //Marshal.Copy(bufferIntPtr, buffer, 0, size);
            }
            finally
            {
                //Marshal.FreeHGlobal(bufferIntPtr);
            }
            return bufferIntPtr;
        }

        /// <summary>
        ///     由句柄转换为结构体
        /// </summary>
        public static T ByteToStructure<T>(IntPtr allocIntPtr)
        {
            object structure;
            //IntPtr allocIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                //Marshal.Copy(dataBuffer, 0, allocIntPtr, size);
                structure = Marshal.PtrToStructure(allocIntPtr, typeof(T));
            }
            finally
            {
                //Marshal.FreeHGlobal(allocIntPtr);
            }
            return (T)structure;
        }

        #endregion
    }
}