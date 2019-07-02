﻿using System;
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
            base.AddEdit();
            base.AddDelete();
            base.HaveQuery();
            if (DesignMode) return;
            var server = new SQLiteService();
            var list = server.Find<TestInfo>("1=1 limit 100000");
            base.InitData(server, list);
        }
    }
}