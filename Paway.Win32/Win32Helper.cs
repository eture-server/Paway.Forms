using Paway.Helper;
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
    /// Win32API 发送消息与查找窗体
    /// </summary>
    public class Win32Helper
    {
        /// <summary>
        /// </summary>
        public static IntPtr True { get { return (IntPtr)1; } }
        /// <summary>
        /// </summary>
        public static IntPtr False { get { return (IntPtr)0; } }

        #region 任务栏Rectangle
        /// <summary>
        /// 获取任务栏Rectangle
        /// </summary>
        /// <returns></returns>
        public static RECT GetTaskRect()
        {
            IntPtr hWnd = NativeMethods.FindWindow("Shell_TrayWnd", null);
            RECT rc = new RECT();
            NativeMethods.GetWindowRect(hWnd, ref rc);
            return rc;
        }

        #endregion

        #region SendMessage - 扩展消息
        /// <summary>
        /// 发送跨进程自定义消息
        /// </summary>
        public static IntPtr SendMessage<T>(string form, int type, T msg)
        {
            IntPtr hWnd = FindWindow(form);
            if (hWnd != IntPtr.Zero)
            {
                int size = Marshal.SizeOf(typeof(T));
                UserDataStruct cds;
                cds.vData = IntPtr.Zero;
                cds.lData = Marshal.SizeOf(typeof(T));
                cds.uData = SctructHelper.StructureToByte(msg);
                NativeMethods.SendMessage(hWnd, (int)WindowsMessage.WM_COPYDATA, type, ref cds);
                return hWnd;
            }
            return IntPtr.Zero;
        }
        /// <summary>
        /// 发送跨进程自定义消息 到所有指定结尾的窗体
        /// </summary>
        public static void SendMessageAll<T>(string form, int type, T msg)
        {
            Dictionary<IntPtr, string> hList = FindWindowEnd(form);
            for (int i = 0; i < hList.Count; i++)
            {
                IntPtr hWnd = hList.Keys.ElementAt(i);
                if (hWnd != IntPtr.Zero)
                {
                    int size = Marshal.SizeOf(typeof(T));
                    UserDataStruct cds;
                    cds.vData = IntPtr.Zero;
                    cds.lData = Marshal.SizeOf(typeof(T));
                    cds.uData = SctructHelper.StructureToByte(msg);
                    NativeMethods.SendMessage(hWnd, (int)WindowsMessage.WM_COPYDATA, type, ref cds);
                }
            }
        }
        /// <summary>
        /// 解析自定义消息 - 形参
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public static T GetUserStruct<T>(Message m)
        {
            UserDataStruct mystr = (UserDataStruct)m.GetLParam(typeof(UserDataStruct));
            T msg = SctructHelper.ByteToStructure<T>(mystr.uData);
            return msg;
        }

        #endregion

        #region SendMessage - 自定义消息
        /// <summary>
        /// 发送跨进程自定义消息
        /// </summary>
        /// <param name="form">窗体名称</param>
        /// <param name="type">消息类型</param>
        /// <param name="msg">消息内容</param>
        /// <returns>成功返回窗体句柄</returns>
        public static IntPtr SendMessage(string form, int type, string msg)
        {
            IntPtr hWnd = FindWindow(form);
            if (hWnd != IntPtr.Zero)
            {
                SendMessage(hWnd, type, msg);
                return hWnd;
            }
            return IntPtr.Zero;
        }
        /// <summary>
        /// 发送跨进程自定义消息 到所有指定结尾的窗体
        /// </summary>
        /// <param name="form">窗体名称</param>
        /// <param name="type">消息类型</param>
        /// <param name="msg">消息内容</param>
        public static void SendMessageAll(string form, int type, string msg)
        {
            Dictionary<IntPtr, string> hList = FindWindowEnd(form);
            for (int i = 0; i < hList.Count; i++)
            {
                IntPtr hWnd = hList.Keys.ElementAt(i);
                if (hWnd != IntPtr.Zero)
                {
                    SendMessage(hWnd, type, msg);
                }
            }
        }
        /// <summary>
        /// 发送消息到指定句柄的窗体
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, int type, string msg)
        {
            CopyDataStruct cds = GetStruct(msg);
            NativeMethods.SendMessage(hWnd, (int)WindowsMessage.WM_COPYDATA, type, ref cds);
            return hWnd;
        }
        /// <summary>
        /// 解析自定义消息 - 字符
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string GetFromStruct(Message m)
        {
            CopyDataStruct mystr = (CopyDataStruct)m.GetLParam(typeof(CopyDataStruct));
            return mystr.mData;
        }
        /// <summary>
        /// 返回自定义消息体
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static CopyDataStruct GetStruct(string msg)
        {
            if (msg == null) msg = string.Empty;
            int len = Encoding.Default.GetBytes(msg).Length;
            CopyDataStruct cds;
            cds.iData = (IntPtr)100;
            cds.mData = msg;
            cds.lData = len + 1;
            return cds;
        }

        #endregion

        #region WindowsShell
        /// <summary>
        /// 关闭进程
        /// Execute("taskkill.exe", " /F /FI \"IMAGENAME eq adb.exe\" /T");
        /// </summary>
        /// <param name="exe"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool Execute(string exe, string args)
        {
            Process process = null;
            bool success = true;
            try
            {
                process = new Process
                {
                    StartInfo =
                    {
                        FileName = exe,
                        Arguments = args,
                        RedirectStandardOutput = false,
                        RedirectStandardInput = false,
                        RedirectStandardError = false,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                    }
                };
                success = process.Start();
            }
            catch
            {
                success = false;
                throw;
            }
            finally
            {
                if (process != null)
                {
                    process.Dispose();
                }
            }
            return success;
        }

        #endregion

        #region ActiveForm
        /// <summary>
        /// 激活指定标题窗体
        /// </summary>
        /// <param name="find"></param>
        public static void ActiveForm(string find)
        {
            IntPtr hWnd = FindWindow(find);
            if (IntPtr.Zero != hWnd)
            {
                NativeMethods.SwitchToThisWindow(hWnd, true);
            }
        }
        /// <summary>
        /// 激活指定句柄窗体
        /// </summary>
        /// <param name="hWnd"></param>
        public static void ActiveForm(IntPtr hWnd)
        {
            NativeMethods.SwitchToThisWindow(hWnd, true);
        }

        #endregion

        #region FindWindowEnd(find)
        /// <summary>
        /// 查找所有指定以 find 结束的窗体
        /// </summary>
        /// <returns>返回窗体名称列表</returns>
        public static List<string> FindStringEnd(string find)
        {
            string result = find;
            NativeMethods.EnumWindows(new NativeMethods.EnumWindowsStr(EnumCallBack_StrEnd), ref result);
            List<string> list = new List<string>();
            string[] data = result.Split(';');
            for (int i = 1; i < data.Length; i++)
            {
                list.Add(data[i]);
            }
            return list;
        }
        private static bool EnumCallBack_StrEnd(IntPtr hWnd, ref string lParam)
        {
            StringBuilder sb = new StringBuilder(0x200);
            NativeMethods.GetWindowText(hWnd, sb, 0x200);
            string find = lParam.Split(';')[0];
            if (sb.ToString().EndsWith(find))
            {
                lParam = string.Format("{0};{1}", lParam, sb);
            }
            return true;
        }

        /// <summary>
        /// 查找所有指定以 find 结束的窗体
        /// </summary>
        /// <returns>返回窗体句柄列表</returns>
        public static List<IntPtr> FindHandleEnd(string find)
        {
            string result = find;
            NativeMethods.EnumWindows(new NativeMethods.EnumWindowsStr(EnumCallBack_HandleEnd), ref result);
            List<IntPtr> list = new List<IntPtr>();
            string[] data = result.Split(';');
            for (int i = 1; i < data.Length; i++)
            {
                list.Add((IntPtr)Convert.ToInt32(data[i]));
            }
            return list;
        }
        private static bool EnumCallBack_HandleEnd(IntPtr hWnd, ref string lParam)
        {
            StringBuilder sb = new StringBuilder(0x200);
            NativeMethods.GetWindowText(hWnd, sb, 0x200);
            string find = lParam.Split(';')[0];
            if (sb.ToString().EndsWith(find))
            {
                lParam = string.Format("{0};{1}", lParam, hWnd);
            }
            return true;
        }

        /// <summary>
        /// 查找所有指定以 find 结束的窗体
        /// </summary>
        /// <returns>返回窗体名称、句柄列表</returns>
        public static Dictionary<IntPtr, string> FindWindowEnd(string find)
        {
            string result = find;
            NativeMethods.EnumWindows(new NativeMethods.EnumWindowsStr(EnumCallBack_End), ref result);
            Dictionary<IntPtr, string> list = new Dictionary<IntPtr, string>();
            string[] data = result.Split(';');
            for (int i = 1; i < data.Length; i += 2)
            {
                list.Add((IntPtr)Convert.ToInt32(data[i]), data[i + 1]);
            }
            return list;
        }
        private static bool EnumCallBack_End(IntPtr hWnd, ref string lParam)
        {
            StringBuilder sb = new StringBuilder(0x200);
            NativeMethods.GetWindowText(hWnd, sb, 0x200);
            string find = lParam.Split(';')[0];
            if (sb.ToString().EndsWith(find))
            {
                lParam = string.Format("{0};{1};{2}", lParam, hWnd, sb);
            }
            return true;
        }

        #endregion

        #region FindWindow(find)
        /// <summary>
        /// 查找 符合指定标题:find 的第一个窗口。
        /// </summary>
        /// <returns>返回句柄</returns>
        public static IntPtr FindWindow(string find)
        {
            IntPtr zero = Marshal.StringToHGlobalAnsi(find);
            if (!NativeMethods.EnumWindows(new NativeMethods.EnumWindowsProc(EnumCallBack), ref zero))
            {
                return zero;
            }
            return IntPtr.Zero;
        }
        private static bool EnumCallBack(IntPtr hWnd, ref IntPtr lParam)
        {
            StringBuilder sb = new StringBuilder(0x200);
            NativeMethods.GetWindowText(hWnd, sb, 0x200);
            string find = Marshal.PtrToStringAnsi(lParam);
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
        /// 显示光标
        /// </summary>
        public static void CursorShow()
        {
            while (true)
            {
                int count = NativeMethods.ShowCursor(1);
                if (count > 0) break;
            }
        }
        /// <summary>
        /// 隐藏光标
        /// </summary>
        public static void CursorHide()
        {
            while (true)
            {
                int count = NativeMethods.ShowCursor(0);
                if (count < 0) break;
            }
        }

        #endregion
    }
}
