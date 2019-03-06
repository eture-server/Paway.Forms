using Paway.Forms;
using Paway.Helper;
using Paway.Test.Properties;
using Paway.Utils;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            toolBar.ItemClick += ToolClose_ItemClick;
            toolBar.EditClick += ToolClose_EditClick;
            btName.Click += BtName_Click;
            tip = new ToolTip();
            this.Opacity = 0.8;
        }

        private ToolTip tip;
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            toolBar.TChangeStart();
            //toolBar.Items[1].Text = string.Format("{0}&{1}", "你好", null);
        }

        private void ToolClose_EditClick(ToolItem item, EventArgs e)
        {
            //new QQDemo().ShowDialog(this);
        }

        private void BtName_Click(object sender, EventArgs e)
        {
            //toolBar.Items.Add(new ToolItem(DateTime.Now.Second.ToString()) { IHeard = true });
            toolBar.Items.Add(new ToolItem(DateTime.Now.Second.ToString() + "A"));
            //toolBar.Items.RemoveAt(toolBar.Items.Count - 1);
            //toolBar.Items[toolBar.Items.Count - 1].Text = DateTime.Now.Second.ToString();
            //toolBar.Items[toolBar.Items.Count - 1].TColor.ColorNormal = Color.Red;
            return;
            if (tbRsa2.IError) return;
            toolBar.TStart();
            string ip = HardWareHandler.GetIpAddress();
            try
            {
                for (int i = 0; i < 1000; i++)
                {
                    Client client = new Client(ip, 9998);
                    client.Connect();
                }
            }
            catch { }
            //TControl control = new TControl();
            //control.Dock = DockStyle.Fill;
            //eventRead.WaitOne(3000);
            //eventRead.Set();
            //eventRead.Reset();
            //this.Controls.Add(control);

            //this.pictureBox1.Image = Resources.process;
            //this.BackgroundImage = pictureBox1.Image;
            //this.timer.Tick += timer_Tick;
            //timer.Enabled = true;
        }

        private void ToolClose_ItemClick(ToolItem item, EventArgs e)
        {
            item.IChange = false;
            //this.TextShow = DateTime.Now.ToString();
        }
    }
}
