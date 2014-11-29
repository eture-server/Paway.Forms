using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;
using Paway.Forms.Metro;
using System.Reflection;

namespace Paway.Test
{
    public partial class Form360 : _360Form
    {
        public Form360()
        {
            InitializeComponent();
            this.TMouseMove(toolBar1);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.TDrawBelowBorder(panel1);
            this.tsmAbout.Click += tsmAbout_Click;
            this.toolBar1.ItemClick += toolBar1_ItemClickChanged;
        }

        void toolBar1_ItemClickChanged(object sender, EventArgs e)
        {
        }

        void tsmAbout_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog(this);
        }

        private void toolBar1_SelectedItemChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("项发生改变了，当前项为：" + this.toolBar1.SelectedItem);
        }
    }
}
