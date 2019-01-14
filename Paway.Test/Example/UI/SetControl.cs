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
    /// 导航
    /// </summary>
    public partial class SetControl : BaseControl
    {
        public SetControl()
        {
            InitializeComponent();
            toolBar1.ItemClick += ToolBar1_ItemClick;
        }
        protected override void OnItemClick(Paway.Forms.ToolItem item)
        {
            switch (item.Text)
            {
                case "报表":
                    break;
                case "创建":
                    break;
                case "记录":
                    break;
                case "注册":
                    break;
                case "修改密码":
                    new UserPadForm().ShowDialog(this);
                    break;
                case "关于我们":
                    new AboutForm().ShowDialog(this);
                    break;
                case "退出系统":
                    OnChanged(this, new ExitEventArgs());
                    break;
            }
        }
    }
}
