﻿using Paway.Forms;
using Paway.Helper;
using Paway.Test.Properties;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class Form1 : QQForm
    {
        private Timer timer = new Timer();
        readonly System.Threading.AutoResetEvent eventRead = new System.Threading.AutoResetEvent(true);
        public Form1()
        {
            InitializeComponent();
            toolBar.ItemClick += toolClose_ItemClick;
            toolBar.EditClick += toolClose_EditClick;
            btName.Click += btName_Click;
            btName_Click(this, EventArgs.Empty);
            this.MouseMove += Demo1_MouseMove;
            toolBar.MouseLeave += toolBar_MouseLeave;
            tip = new ToolTip();
            this.MouseMove += Form1_MouseMove;
        }

        void toolBar_MouseLeave(object sender, EventArgs e)
        {
            if (toolBar.MStatus) return;
            Console.WriteLine(toolBar.MStatus);
            Console.WriteLine("leave");
            toolBar.Tag = false;
            toolBar.MStart();
        }

        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (toolBar.MStatus) return;
            toolBar.Tag = e.Y <= (63 + toolBar.Width) ? true : false;
            //Console.WriteLine(toolBar.Tag);
            toolBar.MStart();
        }

        private ToolTip tip;
        void Demo1_MouseMove(object sender, MouseEventArgs e)
        {
            //tip.SetToolTip(this, "hello");
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            toolBar.Items[1].Text = string.Format("{0}&{1}", "你好", null);
        }

        void toolClose_EditClick(object sender, EventArgs e)
        {
            new QQDemo().ShowDialog(this);
        }

        void btName_Click(object sender, EventArgs e)
        {
            TControl control = new TControl();
            control.Dock = DockStyle.Fill;
            //eventRead.WaitOne(3000);
            //eventRead.Set();
            //eventRead.Reset();
            this.Controls.Add(control);

            //this.pictureBox1.Image = Resources.process;
            //this.BackgroundImage = pictureBox1.Image;
            //this.timer.Tick += timer_Tick;
            //timer.Enabled = true;
        }

        void toolClose_ItemClick(object sender, EventArgs e)
        {
            this.TextShow = DateTime.Now.ToString();
        }
    }
}
