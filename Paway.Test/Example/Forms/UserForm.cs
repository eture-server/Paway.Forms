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
    public partial class UserForm : BaseForm
    {
        public UserInfo Info { get; set; }
        public UserForm()
        {
            InitializeComponent();
            base.IEnterClick = true;
            this.toolOk.ItemClick += ToolOk_ItemClick;
            this.toolCancel.ItemClick += ToolCancel_ItemClick;
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.Info == null) this.Info = new UserInfo();
            UI();
        }

        protected override bool OnCommit()
        {
            if (tbName.IError) return false;
            if (tbPad.IError) return false;

            UserInfo info = Cache.Find(tbName.Text);
            if (info != null && this.Info.Id != info.Id)
            {
                throw new WarningException(string.Format("用户：{0} 已存在", tbName.Text));
            }
            UpdateInfo();
            return true;
        }
        private void UpdateInfo()
        {
            this.Info.Name = tbName.Text;
            if (!this.tbPad.Text.IsNullOrEmpty())
            {
                this.Info.Pad = EncryptHelper.EncryptMD5(this.tbPad.Text + Config.Suffix);
            }
        }

        #region 界面快捷
        private void UI()
        {
            if (Info.Id > 0)
            {
                this.lbTitle.Text = string.Format("用户 - {0}", Info.Name);
                tbName.Text = Info.Name;
            }
        }

        #endregion
    }
}
