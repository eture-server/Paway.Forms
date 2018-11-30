using Paway.Forms;
using Paway.Helper;
using Paway.Utils.Tcp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Paway.Test
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //log4net.Config.XmlConfigurator.Configure();
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alog.xml")));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TConfig.IMouseMove = true;
            //TConfig.BackColor = Color.Green;
            //TConfig.ForeColor = Color.Red;
            //TConfig.Font = new Font("微软雅黑", 16f);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                AssemblyTitleAttribute attrTitle = Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
                log.InfoFormat("{0} v{1} ({2})", attrTitle.Title, assembly.GetName().Version, Environment.MachineName);

                ////被识别的主图
                //Bitmap on = Paway.Helper.BitmapHelper.GetBitmapFormFile(@"d:\0.png");
                ////需要识别的小图
                //Bitmap fo = Paway.Helper.BitmapHelper.GetBitmapFormFile(@"d:\2.png");
                //SearchResult[] result = ImageRecognitionHelper.SearchBitmap_Test(fo, on);

                //Color color = Color.FromArgb(30, 210, 120);
                //int[] list = Paway.Helper.BitmapHelper.RGBToHSL(color);
                //Color temp = Paway.Helper.BitmapHelper.HSLToRGB(list[0], list[1], list[2] + 20);
                //temp = Paway.Helper.BitmapHelper.HSLToRGB(list[0], list[1], list[2] - 20);

                Progress.Initialize();
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                log.ErrorFormat("软件出现未处理的异常，即将退出。\r\n{0}", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Progress.Abort();
            }
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            log.ErrorFormat("软件出现未处理的线程异常。\r\n{0}", e.Exception);
            MessageBox.Show(e.Exception.Message);
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.ErrorFormat("软件出现未处理的异常。\r\n{0}", e.ExceptionObject);
            MessageBox.Show(((Exception)e.ExceptionObject).Message);
        }
    }
}
