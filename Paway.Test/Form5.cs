using Paway.Forms;
using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class Form5 : QQForm
    {
        public Form5()
        {
            InitializeComponent();
            toolBar1.ItemClick += ToolBar1_ItemClick;
        }
        private void ToolBar1_ItemClick(ToolItem arg1, EventArgs arg2)
        {
            var server = new SQLiteService();
            ITestInfo obj = new TestInfo();
            //obj.Id = 1;
            //obj.Image = BitmapHelper.GetBitmapFormFile(@"D:\Tinn\DotNet\House\bin\Debug\Code\110031622_45259-02360-00.png");
            //server.Update(obj);

            var dt = server.FindTable<ITestInfo>("1=1 limit 1");
            ITestInfo info = server.Find<TestInfo>(1);
            info.FindInfo = new FindInfo();
            info.List.Add(new FindInfo());
            info.Clone(obj, false);
            var obj2 = info.Clone(true);
            var list2 = server.Find<TestInfo>("1=1 limit 100");
            var dt2 = list2.ToDataTable();

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            var list = server.Find<TestInfo>("1=1 limit 20");
            Debug.WriteLine("FindList=>" + sw.ElapsedMilliseconds);
            var descriptors = typeof(ITestInfo).Descriptors();
            var descriptor = descriptors.Find(c => c.Name == nameof(TestInfo.Name));

            sw.Restart();
            list.Sort(nameof(TestInfo.UserType));
            Debug.WriteLine("OrderBy3=>" + sw.ElapsedMilliseconds);
            sw.Restart();
            var temp = list.AsParallel().OrderBy(c => c.Name).ToList();
            Debug.WriteLine("OrderBy1=>" + sw.ElapsedMilliseconds);
            gridview1.DataSource = list;
        }
    }
}
