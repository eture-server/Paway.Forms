using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Paway.Win32
{
    /// <summary>
    /// NativeMethods - Win32 API 方法
    /// </summary>
    public abstract class NativeMethods
    {
        #region user32.dll
        #region 系统菜单
        /// <summary>
        /// 获得有关指定窗口的信息，函数也获得在额外窗口内存中指定偏移位地址的32位度整型值。
        /// </summary>
        [DllImport("user32.DLL", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        /// <summary>
        /// 改变指定窗口的属性．函数也将指定的一个32位值设置在窗口的额外存储空间的指定偏移位置。
        /// </summary>
        [DllImport("user32.DLL", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// 允许应用程序为复制或修改而访问
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// 删除指定的菜单条目
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool DeleteMenu(IntPtr hMenu, int uPosition, int uFlags);
        /// <summary>
        /// 追加菜单条目
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int AppendMenu(IntPtr hMenu, int Flagsw, IntPtr IDNewItem, string lpNewItem);
        /// <summary>
        /// 插入菜单条目
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int InsertMenu(IntPtr hMenu, int uFlags, int Flagsw, IntPtr IDNewItem, string lpNewItem);

        /// <summary>
        /// 该函数在指定位置显示快捷菜单，并跟踪菜单项的选择
        /// </summary>
        [DllImport("user32.dll")]
        public static extern int TrackPopupMenu(IntPtr hMenu, int uFlags, int x, int y, int nReserved, IntPtr hWnd, ref RECT rect);

        #endregion

        #region 窗体阴影
        /// <summary>
        /// 该函数能在显示与隐藏窗口时能产生特殊的效果。有两种类型的动画效果：滚动动画和滑动动画。
        /// </summary>
        [DllImport("user32.DLL")]
        public static extern bool AnimateWindow(IntPtr whnd, int dwtime, int dwflag);

        #endregion

        /// <summary>
        /// 锁定指定窗口，禁止它更新。
        /// Windows系统下同时只能有一个窗口处于锁定状态。
        /// </summary>
        /// <param name="hWnd">将被锁定的窗口句柄。如果此句柄为NULL，则是解锁该窗口。</param>
        /// <returns>非零表示成功，零表示失败（比如另外已有一个窗口锁定）</returns>
        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWnd);

        /// <summary>
        /// 创建一个圆角矩形区域
        /// </summary>
        /// <param name="nLeftRect">x坐标左上角</param>
        /// <param name="nTopRect">y坐标左上角</param>
        /// <param name="nRightRect">x坐标右上角</param>
        /// <param name="nBottomRect">y坐标右上角</param>
        /// <param name="nWidthEllipse">椭圆的宽度</param>
        /// <param name="nHeightEllipse">椭圆的高度</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        #region FindWindow

        /// <summary>
        /// <para>返回与指定字符串相匹配的窗口类名或窗口名的最顶层窗口的窗口句柄</para>
        /// <para>如果函数执行成功，则返回值是拥有指定窗口类名或窗口名的窗口的句柄。</para>
        /// <para>如果函数执行失败，则返回值为 NULL 。</para>
        /// <para>可以通过调用GetLastError函数获得更加详细的错误信息。</para>
        /// </summary>
        /// <param name="lpClassName">指向包含了窗口类名的空中止(C语言)字串的指针;或设为零,表示接收任何类</param>
        /// <param name="lpWindowName">
        /// 指向包含了窗口文本(或标签)的空中止(C语言)字串的指针;或设
        /// 为零,表示接收任何窗口标题
        /// </param>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 该函数设置指定窗口的显示状态。
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        /// <summary>
        /// 获取一个前台窗口的句柄
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 该函数枚举所有屏幕上的顶层窗口，并将窗口句柄传送给应用程序定义的回调函数。
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref IntPtr lParam);

        /// <summary>
        /// EnumWindows回调函数
        /// 传递句柄
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate bool EnumWindowsProc(IntPtr hWnd, ref IntPtr lParam);

        /// <summary>
        /// 该函数枚举所有屏幕上的顶层窗口，并将窗口句柄传送给应用程序定义的回调函数。
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool EnumWindows(EnumWindowsStr lpEnumFunc, ref string lParam);

        /// <summary>
        /// EnumWindows回调函数
        /// 传递字符
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        internal delegate bool EnumWindowsStr(IntPtr hWnd, ref string lParam);

        /// <summary>
        /// 该函数将指定窗口的标题条文本（如果存在）拷贝到一个缓存区内。如果指定的窗口是一个控件，则拷贝控件的文本。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 激活指定窗口(无论是否最小化)
        /// </summary>
        /// <param name="hWnd">要激活的窗口句柄</param>
        /// <param name="fAltTab">是否使最小化的窗口还原</param>
        [DllImport("user32.dll")]
        internal static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        #endregion

        /// <summary>
        /// 该函数显示或隐藏光标
        /// 该函数设置了一个内部显示计数器以确定光标是否显示，仅当显示计数器的值大于或等于0时，光标才显示，
        /// 如果安装了鼠标，则显示计数的初始值为0。如果没有安装鼠标，显示计数是C1。
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int ShowCursor(int status);

        /// <summary>
        /// 该函数检索一指定窗口的客户区域或整个屏幕的显示设备上下文环境的句柄，
        /// 以后可以在GDI函数中使用该句柄来在设备上下文环境中绘图。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// 该函数返回指定窗口的边框矩形的尺寸。该尺寸以相对于屏幕坐标左上角的屏幕坐标给出。
        /// </summary>
        /// <param name="hWnd">想获得范围矩形的那个窗口的句柄</param>
        /// <param name="lpRect">屏幕坐标中随同窗口装载的矩形</param>
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        /// <summary>
        /// 释放设备上下文环境（DC）供其他应用程序使用。函数的效果与设备上下文环境类型有关。它只释放公用的和设备上下文环境，对于类或私有的则无效。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hDC"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// <para>该函数从当前线程中的窗口释放鼠标捕获，并恢复通常的鼠标输入处理。捕获鼠标的窗口接收所有</para>
        /// <para>的鼠标输入（无论光标的位置在哪里），除非点击鼠标键时，光标热点在另一个线程的窗口中。</para>
        /// </summary>
        [DllImport("user32.dll")]
        public static extern int ReleaseCapture();

        #region SendMessage

        /// <summary>
        /// <para>该函数将指定的消息发送到一个或多个窗口。</para>
        /// <para>此函数为指定的窗口调用窗口程序直到窗口程序处理完消息再返回。</para>
        /// <para>而函数PostMessage不同，将一个消息寄送到一个线程的消息队列后立即返回。</para>
        /// return 返回值 : 指定消息处理的结果，依赖于所发送的消息。
        /// </summary>
        /// <param name="hWnd">要接收消息的那个窗口的句柄</param>
        /// <param name="Msg">消息的标识符</param>
        /// <param name="wParam">具体取决于消息</param>
        /// <param name="lParam">具体取决于消息</param>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// 将指定的消息发送到一个或多个窗口
        /// </summary>
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref CopyDataStruct lParam);

        /// <summary>
        /// 将指定的消息发送到一个或多个窗口
        /// </summary>
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref UserDataStruct lParam);

        #endregion

        /// <summary>
        /// 用于得到被定义的系统数据或者系统配置信息.
        /// </summary>
        /// <param name="which"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(int which);

        /// <summary>
        /// 设置窗口在屏幕中的位置
        /// </summary>
        /// <param name="hWnd">指定窗口句柄</param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy,
            uint uFlags);

        /// <summary>
        /// 设置窗口的区域的窗口。窗口区域决定在窗户上的地区——该系统允许绘画。
        /// 该系统不显示任何部分是一个窗口,窗户外面地区
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hRgn">处理区域</param>
        /// <param name="bRedraw">重绘窗体选项</param>
        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hwnd, IntPtr hRgn, bool bRedraw);

        /// <summary>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hdcDst"></param>
        /// <param name="pptDst"></param>
        /// <param name="psize"></param>
        /// <param name="hdcSrc"></param>
        /// <param name="ppSrc"></param>
        /// <param name="crKey"></param>
        /// <param name="pblend"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT ppSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        #endregion

        #region gdi32.dll

        /// <summary>
        /// 创建一个与指定设备兼容的内存设备上下文环境（DC）。通过GetDc()获取的HDC直接与相关设备沟通，而本函数创建的DC，则是与内存中的一个表面相关联。
        /// </summary>
        /// <param name="hDC"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        /// <summary>
        /// 删除指定的设备上下文环境（Dc）
        /// </summary>
        /// <param name="hdc"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// 删除一个逻辑笔、画笔、字体、位图、区域或者调色板，释放所有与该对象有关的系统资源，在对象被删除之后，指定的句柄也就失效了。
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 选择一对象到指定的设备上下文环境中，该新对象替换先前的相同类型的对象。
        /// </summary>
        /// <param name="hDC"></param>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        #endregion

        #region Other Methods

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int LOWORD(int value)
        {
            return value & 0xFFFF;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int HIWORD(int value)
        {
            return value >> 16;
        }

        /// <summary>
        /// 获取线程执行的周期个数。
        /// </summary>
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        /// <summary>
        /// 获取当前线程的一个伪句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThread();

        #endregion
    }
}