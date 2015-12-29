﻿using System;
using System.Reflection;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    ///     关于窗体
    /// </summary>
    public partial class AboutForm : QQForm
    {
        private bool m_showing = true;
        private Timer timer;

        /// <summary>
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();
            //输出软件名称和版本号
            var assembly = Assembly.GetExecutingAssembly();
            lbDesc.Text = "宁波欢迎您";
        }

        /// <summary>
        ///     重置显示描述
        /// </summary>
        public void Reset(string desc)
        {
            lbDesc.Text = desc;
        }

        /// <summary>
        ///     输出软件名称和版本号
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Assembly assembly = null;
            if (Owner == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }
            else
            {
                assembly = Assembly.GetAssembly(Owner.GetType());
            }
            var attrTitle =
                Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
            var attrCopyright =
                Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute)) as
                    AssemblyCopyrightAttribute;
            var plat = assembly.GetName().ProcessorArchitecture;
            lbVersion.Text = string.Format("{0}\r\nv{1}({2})<{3}>", attrTitle.Title, assembly.GetName().Version,
                plat == ProcessorArchitecture.MSIL ? "Any" : plat.ToString(), Environment.MachineName);
            lbCopyright.Text = string.Format("{0}", attrCopyright.Copyright.Replace("\u00A9", "(C)"));
        }

        /// <summary>
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            timer = new Timer();
            timer.Tick += timer_Tick;
            Opacity = 0.0;
            Activate();
            Refresh();
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (m_showing)
            {
                var d = 1000.0 / timer.Interval / 100.0;
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
                var d = 1000.0 / timer.Interval / 100.0;
                if (Opacity - d <= 0.0)
                {
                    Opacity = 0.0;
                    timer.Stop();
                    Close();
                }
                else
                {
                    Opacity = Opacity - d;
                }
            }
        }

        /// <summary>
        ///     关闭时激发父窗体
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (m_showing)
            {
                e.Cancel = true;
                m_showing = false;
                timer.Start();
            }
            else if (Owner != null)
            {
                Owner.Activate();
            }
        }
    }
}