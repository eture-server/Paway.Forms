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
            Form1 form = null;
            try
            {
                form = new Form1();
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                if (form!=null)
                MessageBox.Show(form, ex.Message, "Tinn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Error("软件出现未处理的异常，即將退出。" + e.ExceptionObject);
        }
    }
}
