using Paway.Forms;
using Paway.Helper;
using System;
using System.Collections;
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
            this.TMouseMove(tControl1);
        }
        private void ToolBar1_ItemClick(ToolItem arg1, EventArgs arg2)
        {
            var server = new SQLiteService();
            ITestInfo obj = new TestInfo();
            //obj.Id = 1;
            //obj.Image = BitmapHelper.GetBitmapFormFile(@"D:\Tinn\DotNet\House\bin\Debug\Code\110031622_45259-02360-00.png");
            //server.Insert(obj);

            ITestInfo info = server.Find<TestInfo>(7);
            info.FindInfo = new FindInfo() { Id = 100 };
            info.List.Add(new FindInfo());
            info.List2 = new List<FindInfo>();
            server.Update(info);

            TestBase b = new TestBase();
            var b1 = (TestBase)info;
            b1.Clone(b);
            b.Id = 3;
            b.Clone(b1);

            var id = info.GetValue(nameof(TestInfo.FindInfo));

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
            var list = server.Find<TestInfo>("1=1 limit 10000");
            Debug.WriteLine("FindList=>" + sw.ElapsedMilliseconds);
            sw.Restart();
            //var dt3 = list.ToDataTable();
            Debug.WriteLine("ToDataTable=>" + sw.ElapsedMilliseconds);

            var type = typeof(TestInfo);
            sw.Restart();
            for (int i = 0; i < 10; i++)
            {
                var dt4 = list.ToDataTable();
            }
            Debug.WriteLine("ToDataTable=>" + sw.ElapsedMilliseconds);
            sw.Restart();
            for (int i = 0; i < 10; i++)
            {
                //var list4 = dt3.ToList<TestInfo>();
            }
            Debug.WriteLine("ToDataTable2=>" + sw.ElapsedMilliseconds);


            sw.Restart();
            for (int i = 0; i < 10 * 10; i++)
            {
                var ix = list.Find(c => c.Id == 1014509);
            }
            Debug.WriteLine("Find=>" + sw.ElapsedMilliseconds);

            var dict = list.Cast<TestInfo>().ToDictionary(o => o.Id, o => o);
            sw.Restart();
            for (int i = 0; i < 10 * 10; i++)
            {
                if (dict.ContainsKey(1014509))
                {
                    var ix = dict[1014509];
                }
            }
            Debug.WriteLine("Key=>" + sw.ElapsedMilliseconds);
            var map = new Hashtable();
            map.Add(1, list[0]);
            map.Add("1", list[1]);
            var b2 = map[1];


            var list3 = list.Clone(true);
            sw.Restart();
            for (int i = 0; i < 100; i++)
            {
                list.Clone(list3);
            }
            Debug.WriteLine("Clone=>" + sw.ElapsedMilliseconds);
            sw.Restart();
            for (int i = 0; i < 100; i++)
            {
                //var list3 = list.Clone2(true);
                list.Clone(list3);
            }
            Debug.WriteLine("Clone2=>" + sw.ElapsedMilliseconds);

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
