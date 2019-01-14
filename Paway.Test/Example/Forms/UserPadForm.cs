using Paway.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Helper;

namespace Paway.Test
{
    public partial class UserPadForm : BaseForm
    {
        public UserInfo Info { get; set; }

        public UserPadForm()
        {
            InitializeComponent();
            base.IEnterClick = true;
            this.toolOk.ItemClick += ToolOk_ItemClick;
            this.toolCancel.ItemClick += ToolCancel_ItemClick;
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.tbPad.Focus();
        }

        protected override bool OnCommit()
        {
            if (tbPad.IError) return false;
            if (tbPad1.IError) return false;
            if (tbPad2.IError) return false;
            DataService.Default.UpdatePad(tbPad.Text, tbPad1.Text, tbPad2.Text);
            return base.OnCommit();
        }
    }
}
