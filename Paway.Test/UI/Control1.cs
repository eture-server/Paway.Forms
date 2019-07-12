using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;
using System.IO;
using Paway.Helper;

namespace Paway.Test
{
    public partial class Control1 : MControl
    {
        public Control1()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            OnChanged(this, e);
            WaitDrawDataGridView();
        }
        public override void ReLoad()
        {
            base.ReLoad();
            toolbar.MStart();
        }
        public override bool UnLoad()
        {
            return base.UnLoad();
        }
        protected void WaitDrawDataGridView()
        {
            var server = new SQLiteService();
            var list = server.Find<TestInfo>("1=1 limit 100");
            tDataGridViewPager1.DataSource = list;
        }
    }
}
