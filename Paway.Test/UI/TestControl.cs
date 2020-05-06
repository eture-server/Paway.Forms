using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;
using Paway.Helper;

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
        protected override void ReLoad(bool first)
        {
            base.ReLoad(first);
        }
        protected override void OnButtonClicked(TestInfo info, string name, object value)
        {
            base.OnButtonClicked(info, name, value);
            switch (name)
            {
                case nameof(TestInfo.Name):
                    value.Show();
                    break;
                case nameof(TestInfo.Images):
                    info.Name = null;
                    gridview1.Edit.UpdateRow(info);
                    info.Id.ToString().Show();
                    break;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.AddItem();
            base.AddUpdate();
            base.AddDelete();
            base.HaveQuery();
            gridview1.TPadding = new Padding(0);
            gridview1.PageSize = 5;
            gridview1.Edit.AddSpanHeader(typeof(TestInfo), nameof(TestInfo.Name), 2, "操作", true, StringAlignment.Near);
            gridview1.Edit.ColumnHeadersHeight = 77;
            if (DesignMode) return;
            //gridview1.IGroup = false;
            //this.UserTree(true);
            var server = new SQLiteService();
            base.find = "Id<=917551";
            var list = server.Find<TestInfo>("1=1 limit 1000");
            base.InitData(list, server);
        }
    }
}
