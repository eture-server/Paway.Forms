using Mobot.Imaging;
using Paway.Forms;
using Paway.Forms.Metro;
using Paway.Helper;
using Paway.Utils.Tcp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
            //Service server = new Service();
            //server.Listener(HardWareHandler.GetIpAddress(), 9998);
            //DateTime dt = WebHelper.GetBeijingTime();
            //log4net.Config.XmlConfigurator.Configure();
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alog.xml")));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            log.Error("log.Error:" + DateTime.Now);
            InitConfig.IMouseMove = true;
            //InitConfig.BackColor = Color.Green;
            //InitConfig.ForeColor = Color.Red;
            //InitConfig.Font = new Font("微软雅黑", 16f);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                //被识别的主图
                //Bitmap on = Paway.Helper.BitmapHelper.GetBitmapFormFile(@"d:\23.jpg");
                //需要识别的小图
                //Bitmap fo = Paway.Helper.BitmapHelper.GetBitmapFormFile(@"d:\4.png");
                //var a = ImageRecognitionHelper.SearchBitmap_Test(fo, on);

                //Color color = Color.FromArgb(30, 210, 120);
                //int[] list = Paway.Helper.BitmapHelper.RGBToHSL(color);
                //Color temp = Paway.Helper.BitmapHelper.HSLToRGB(list[0], list[1], list[2] + 20);
                //temp = Paway.Helper.BitmapHelper.HSLToRGB(list[0], list[1], list[2] - 20);

                Application.Run(new FormSql());
            }
            catch (Exception ex)
            {
                log.ErrorFormat("软件出现未处理的异常，即将退出0。\r\n{0}", ex);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Error("软件出现未处理的异常，即將退出。" + e.ExceptionObject);
        }
    }
}
