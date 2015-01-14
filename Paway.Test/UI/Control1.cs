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

namespace Paway.Test.UI
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
        protected void WaitDrawDataGridView()
        {
            BindingList<WaitDrawDataGridViewData> list = new BindingList<WaitDrawDataGridViewData>();
            tDataGridViewPager1.DataSource = new WaitDrawDataGridViewData() { Device = "正在加载" };
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image");
            for (int i = 0; i < 9; i++)
            {
                bool a = false, b = false, c = false;
                if (i > 1) a = true;
                if (i > 3) b = true;
                if (i > 5) c = true;
                WaitDrawDataGridViewData dti = new WaitDrawDataGridViewData
                {
                    StatuImage = BitmapHelper.GetBitmapFormFile(string.Format("{0}\\{1}.png", path, i)),
                    Device = "Device" + i,
                    Product = "Product" + (!b ? 1 : 2),
                    AppName = "AppName" + (!a ? 1 : (!b ? 2 : (!c ? 3 : 4))),
                    Index = i,
                    Progress = i,
                };
                list.Add(dti);
            }
            tDataGridViewPager1.DataSource = list;
        }
    }
}
