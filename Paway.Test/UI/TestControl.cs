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
    /// 测试
    /// </summary>
    public partial class TestControls : TestControl
    {
        public TestControls() { }
    }
    public partial class TestControl : TDataControl<TestInfo>
    {
        public TestControl() { }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.AddItem();
            base.AddUpdate();
            base.AddDelete();
            base.HaveQuery();
            if (DesignMode) return;
            gridview1.IGroup = false;
            this.UserTree(true);
            var server = new SQLiteService();
            base.find = "Id<=927551";
            var list = server.Find<TestInfo>("1=1 limit 10000");
            base.InitData(server, list);
        }
    }
}
