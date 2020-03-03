using Paway.Forms;
using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class FormGridView : Form
    {
        public FormGridView()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            var server = new SQLiteService();
            var list = server.Find<TestInfo>("1=1 limit 100");

            gridview3.DataSource = list;
            gridview3.ExpandAll();

            gridview2.DataSource = list;

            gridview1.DataSource = list;
        }
    }
}
