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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            toolClose.SelectedItemChanged += toolClose_SelectedItemChanged;
            toolClose.EditClick += toolClose_EditClick;
            this.AutoValidate = AutoValidate.Disable;
        }
        void toolClose_SelectedItemChanged(object sender, EventArgs e)
        {
            this.Text = "选中：" + toolClose.SelectedItem.Text + "=>" + DateTime.Now.Second;
        }

        void toolClose_EditClick(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //tControl1.Star();
        }
    }
}
