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

namespace Paway.Test
{
    public partial class Form360 : _360Form
    {
        public Form360()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.panel1.Paint += panel1_Paint;
            this.tsmAbout.Click += tsmAbout_Click;
            this.toolBar1.ItemClick += toolBar1_ItemClickChanged;
        }

        void panel1_Paint(object sender, PaintEventArgs e)
        {
            base.DrawBelowBorder(panel1);
        }

        void toolBar1_ItemClickChanged(object sender, EventArgs e)
        {
        }

        void tsmAbout_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ReSet("hello");
            about.ShowDialog(this);
        }

        private void toolBar1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("下标改变了，当前下标为：" + this.toolBar1.SelectedIndex);
        }

        private void toolBar1_SelectedItemChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("项发生改变了，当前项为：" + this.toolBar1.SelectedItem);
        }
    }
}
