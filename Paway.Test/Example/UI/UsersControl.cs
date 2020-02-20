using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;

namespace Paway.Test
{
    /// <summary>
    /// 用户
    /// </summary>
    public partial class UsersControl : UsersControlBase
    {
        public UsersControl() { }
    }
    public partial class UsersControlBase : TDataControl<UserInfo>
    {
        public UsersControlBase() { }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.AddItem();
            base.AddUpdate();
            base.AddDelete();
            if (DesignMode) return;
            base.InitData(Cache.UserList, DataService.Default);
        }

        #region 重载
        protected override UserInfo OnAdd()
        {
            UserForm add = new UserForm();
            if (add.ShowDialog(this) == DialogResult.OK)
            {
                return add.Info;
            }
            return base.OnAdd();
        }
        protected override Form OnUpdate(UserInfo info)
        {
            UserForm edit = new UserForm();
            edit.Info = info;
            return edit;
        }
        protected override Tuple<string, string> OnDelete(UserInfo info)
        {
            return new Tuple<string, string>(Config.Text, string.Format("确认删除用户：{0}", info.Name));
        }

        #endregion
    }
}
