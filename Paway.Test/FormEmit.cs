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
    public partial class FormEmit : QQForm
    {
        public FormEmit()
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

            ITestInfo info = server.Find<TestInfo>(1);
            info.FindInfo = new FindInfo() { Id = 100 };
            info.List.Add(new FindInfo());
            info.List2 = new List<FindInfo>();
            server.Update(info);

            var obj3 = info.Clone(true);
            obj3.FindInfo.Id = 102;
            obj3.List.Clear();
            obj3.List2.Add(new FindInfo());
            ITestInfo obj4 = new TestInfo();
            info.Clone(obj4, true);
            obj4.FindInfo.Id = 103;
            obj4.List.Clear();
            obj4.List2.Add(new FindInfo());


            Stopwatch sw = new Stopwatch();
            sw.Restart();
            var list = server.Find<TestInfo>("1=1 limit 20");
            var dt = list.ToDataTable();
            Debug.WriteLine("FindList=>" + sw.ElapsedMilliseconds);

            var list3 = list.Clone(true);
            list.Clone(list3);

            var dt2 = server.FindTable<ITestInfo>("1=1 limit 20");
            var list2 = dt2.ToList<TestInfo>();

            sw.Restart();
            list.Sort(nameof(TestInfo.UserType));
            Debug.WriteLine("OrderBy2=>" + sw.ElapsedMilliseconds);
            sw.Restart();
            var temp = list.AsParallel().OrderBy(c => c.Name);
            Debug.WriteLine("OrderBy1=>" + sw.ElapsedMilliseconds);
            sw.Restart();
            var temp2 = list.AsParallel().OrderBy(c => c.Name).ToList();
            Debug.WriteLine("ToList.Time=>" + sw.ElapsedMilliseconds);
            sw.Restart();
            foreach (var item in temp)
            { }
            Debug.WriteLine("ToList.Time=>" + sw.ElapsedMilliseconds);
            //gridview1.DataSource = list;
        }
    }
}
