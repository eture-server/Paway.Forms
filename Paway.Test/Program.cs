using Paway.Forms;
using Paway.Forms.Metro;
using System;
using System.Collections.Generic;
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
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alog.xml")));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            log.Error("log.Error:" + DateTime.Now);
            Application.Run(new Form360());
        }
    }
}
