using Paway.Helper;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 关于窗体
    /// </summary>
    public partial class AboutForm : QQForm
    {
        private Timer timer = null;
        private bool m_showing = true;

        /// <summary>
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();
            //输出软件名称和版本号
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version(assembly);
            lbDesc.Text = null;
        }

        /// <summary>
        /// 重置显示描述
        /// </summary>
        /// <param name="desc"></param>
        public void ReDescription(string desc)
        {
            lbDesc.Text = desc;
        }
        /// <summary>
        /// 显示引用程序集版本
        /// </summary>
        /// <param name="type"></param>
        public void ReVersion(Type type)
        {
            Assembly assembly = Assembly.GetAssembly(type);
            Version(assembly);
        }

        private void Version(Assembly assembly = null)
        {
            AssemblyTitleAttribute attrTitle = Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
            AssemblyCopyrightAttribute attrCopyright = Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
            ProcessorArchitecture plat = assembly.GetName().ProcessorArchitecture;
            if (lbPlatform.Text == "<Platform>")
            {
                lbCopyright.Text = string.Format("{0}", attrCopyright.Copyright.Replace("\u00A9", "(C)"));
            }
            lbPlatform.Text = string.Format("Platform:{0}", plat == ProcessorArchitecture.MSIL ? "Any" : plat.ToString());
            lbVersion.Text = string.Format("{0} v{1} ({2})", attrTitle.Title, assembly.GetName().Version, Environment.MachineName);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.TMouseMove(panel1);
            this.TMouseMove(panel2);
            timer = new Timer();
            timer.Tick += timer_Tick;
            this.Opacity = 0.0;
            this.Activate();
            this.Refresh();
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (m_showing)
            {
                double d = 1000.0 / timer.Interval / 100.0;
                if (Opacity + d >= 1.0)
                {
                    Opacity = 1.0;
                    timer.Stop();
                }
                else
                {
                    Opacity = Opacity + d;
                }
            }
            else
            {
                double d = 1000.0 / timer.Interval / 100.0;
                if (Opacity - d <= 0.0)
                {
                    Opacity = 0.0;
                    timer.Stop();
                    this.Close();
                }
                else
                {
                    Opacity = Opacity - d;
                }
            }
        }

        /// <summary>
        /// 关闭时激发父窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (m_showing)
            {
                e.Cancel = true;
                m_showing = false;
                timer.Start();
            }
            else if (this.Owner != null)
            {
                Owner.Activate();
            }
        }
    }
}
