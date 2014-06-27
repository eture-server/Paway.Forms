using Paway.Forms;
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
        public Form1()
        {
            InitializeComponent();
            //toolClose.SelectedItemChanged += toolClose_SelectedItemChanged;
            //toolClose.EditClick += toolClose_EditClick;

        }
        void toolClose_SelectedItemChanged(object sender, EventArgs e)
        {
            //this.Text = "选中：" + toolClose.SelectedItem.Text + "=>" + DateTime.Now.Second;
        }

        void toolClose_EditClick(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //tControl1.Star();
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            string dt = DateTime.Now.Second.ToString(); ;
            //toolClose.Items.Add(new ToolItem(dt) { Desc = dt });
            //toolClose.Items[0].Text = "text2";
        }

        private void qqButton1_Click(object sender, EventArgs e)
        {
            //if (toolClose.SelectedItem != null)
            //{
            //    toolClose.Items.Remove(toolClose.SelectedItem);
            //}
        }

        private void btSearch_Click_1(object sender, EventArgs e)
        {
            toolBar1.TDirection = TDirection.Vertical;
        }

        private void qqButton1_Click_1(object sender, EventArgs e)
        {
            toolBar1.TDirection = TDirection.Level;
        }
    }
}
