using Paway.Forms;
using Paway.Helper;
using Paway.Utils;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Paway.Test
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                var _mutex = new Mutex(true, "OneInstanceTestOnly", out bool createdNew);
                if (!createdNew)
                {//已经有实例在运行，则激活该实例的主窗体。
                    IntPtr hWnd = Win32Helper.ActiveForm(Config.Text);
                    Win32Helper.SendMessage(hWnd, 0, null);
                    return;
                }
                //TConfig.IMouseMove = true;
                //TConfig.BackColor = Color.Red;
                //TConfig.ForeColor = Color.Red;
                //TConfig.Trans = 100;

                ////被识别的主图
                //Bitmap on = Paway.Helper.BitmapHelper.GetBitmapFormFile(@"d:\0.png");
                ////需要识别的小图
                //Bitmap fo = Paway.Helper.BitmapHelper.GetBitmapFormFile(@"d:\2.png");
                //SearchResult[] result = ImageRecognitionHelper.SearchBitmap_Test(fo, on);

                //Color color = Color.FromArgb(30, 210, 120);
                //int[] list = Paway.Helper.BitmapHelper.RGBToHSL(color);
                //Color temp = Paway.Helper.BitmapHelper.HSLToRGB(list[0], list[1], list[2] + 20);
                //temp = Paway.Helper.BitmapHelper.HSLToRGB(list[0], list[1], list[2] - 20);

                //log4net.Config.XmlConfigurator.Configure();
                log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.xml")));
                Assembly assembly = Assembly.GetExecutingAssembly();
                AssemblyTitleAttribute attrTitle = Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
                string.Format("{0} v{1} ({2})", attrTitle.Title, assembly.GetName().Version, Environment.MachineName).Log();

                Progress.Initialize();
                TConfig.IAutoHideScroll = true;
                {
                }
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.Run(new Form3());
            }
            catch (Exception e)
            {
                e.Show("软件出现未处理的异常，即将退出。", false);
            }
            finally
            {
                Progress.Abort();
                Application.Exit();
            }
        }
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            e.Exception.Show("软件出现未被捕获的线程异常。", false);
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.ExceptionObject.Show("软件出现未被捕获的异常，即将退出。", false);
        }
    }
}
