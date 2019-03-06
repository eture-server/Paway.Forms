using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;
using System.Reflection;
using Paway.Helper;

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
            this.tsmAbout.Click += TsmAbout_Click;
        }

        private void TsmAbout_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog(this);
        }

        private void ToolBar1_SelectedItemChanged(ToolItem item, EventArgs e)
        {
            MControl.ReLoad(panel1, typeof(Control1), EventArgs.Empty, TMDirection.None, new Action<object, EventArgs>(Method));
        }
        public void Method(object sender, EventArgs e) { }
    }
}
