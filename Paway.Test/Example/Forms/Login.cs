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
    public partial class Login : BaseForm
    {
        public Login()
        {
            InitializeComponent();
            this.Text = Config.Text;
            UI();
            this.toolOk.ItemClick += ToolOk_ItemClick;
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.tbName.Focus();
#if DEBUG
            tbName.Text = "admin";
            tbPad.Text = "admin";
#endif
        }

        protected override bool OnCommit()
        {
            if (tbName.IError) return false;
            if (tbPad.IError) return false;
            toolOk.Items[0].Text = Config.Loading;
            toolOk.TRefresh();
            Application.DoEvents();
            Config.User = DataService.Default.EncryptLogin(tbName.Text, tbPad.Text);
            toolOk.Items[0].Text = "登陆";
            return true;
        }
        protected override void OnFailed()
        {
            base.OnFailed();
            toolOk.Items[0].Text = "登陆";
            toolOk.TRefresh(0);
        }

        #region 界面快捷
        private void UI()
        {
            tbName.KeyDown += tbName_KeyDown;
            tbPad.KeyDown += tbEnName_KeyDown;
        }
        void tbEnName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                toolOk.TClickFirst();
            }
        }
        void tbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbPad.Focus();
            }
        }

        #endregion
    }
}
