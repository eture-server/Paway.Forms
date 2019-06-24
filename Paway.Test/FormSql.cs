using Paway.Helper;
using Paway.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Utils;
using System.Data.SqlClient;
using System.IO;
using Paway.Test.Properties;
using Paway.Win32;
using System.Diagnostics;
using System.Data.Common;
using System.Threading;
using System.Reflection;
using log4net;
using System.Data.SQLite;

namespace Paway.Test
{
    public partial class FormSql : TBaseForm
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        SqlService service = new SqlService();
        List<TestData> list = new List<TestData>();

        public FormSql()
        {
            InitializeComponent();
            btnStart.Click += BtnStart_Click;
            btnTest.Click += BtnTest_Click;
        }
        private void BtnStart_Click(object sender, EventArgs e)
        {
            int maxCount = tbStart.Text.ToInt();
            for (int i = 0; i < maxCount; i++)
            {
                SqlService server = new SqlService();
                server.Connect();
                log.Debug(string.Format("成功创建连接对象{0}", i + 1));
            }
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            var time = Environment.TickCount;
            var b = Environment.TickCount - time;
            for (int i = 0; i < 10; i++)
            {
                TestData data = new TestData
                {
                    Id = i,
                    N2 = "Name" + i,
                    Tag = "Tag" + i,
                    //Image = pictureBox1.Image,
                };
                list.Add(data);
            }
            Win32Helper.ActiveForm(this.Handle);
        }
        private void BtInsert_Click(object sender, EventArgs e)
        {
            try
            {
                ITestData data = new TestData
                {
                    Image = pictureBox1.Image,
                    V2 = "11"
                };
                data.FindInfo = new FindInfo();

                var d2 = data.Clone();
                ITestData data2 = new TestData();
                data.Clone(data2);

                var list2 = new List<ITestData>();
                //var d2 = data.Clone(true);
                list2.Add(data);
                list2.Sort("Id");
                var dt = list2.ToDataTable();
                list2.Sort("V2");

                var list3 = dt.ToList<TestData>();

                data.SetValue("Id", 33L);
                var id = data.GetValue("Id");

                var builder = SQLBuilder.CreateBuilder(typeof(TestData));
                builder.Build(data, 12);

                service.Insert(list2);
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        private void BtUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (list.Count == 0) return;
                list[0].N2 = DateTime.Now.Minute.ToString();
                list[0].V2 = DateTime.Now.Second.ToString();
                service.Update<TestData>(list[0]);
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        private void BtUpOrIn_Click(object sender, EventArgs e)
        {
            try
            {
                //暂不支持MySql
                if (list.Count == 0) return;
                list[0].N2 = DateTime.Now.Minute.ToString();
                list[0].V2 = DateTime.Now.Second.ToString();
                service.Update(list[0]);
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        private void BtDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (list.Count == 0) return;
                object result = service.ExecuteScalar("select count(0) from Hello");
                service.Delete(list[0]);
                list.RemoveAt(0);
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        private void BtnReplace_Click(object sender, EventArgs e)
        {
            try
            {
                if (list.Count == 0) return;
                service.Insert(list[0]);
                list[0].N2 = "V3";
                list[0].Id = 17;
                service.Update(list[0]);
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        private void BtSelect_Click(object sender, EventArgs e)
        {
            try
            {
                new SqlService().Connect();
                list = service.Find<TestData>("id=" + 12948) as List<TestData>;
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            try
            {
                //new SQLiteService().Insert(new UserInfo());
                var list = new SQLiteService().Find<UserInfo>();
                //new SqlService().Test();
            }
            catch (Exception ex)
            {
                ExceptionHelper.Show(ex);
            }
        }

    }
    public class MySqlService : MySqlHelper
    {
        public MySqlService()
        {
            base.InitConnect("127.0.0.1", "DiningRe", "root", "mobotaA*");
        }
        public void Connect()
        {
            var sql = "select Id from Cashs";
            var list = ExecuteDataTable(sql);
        }
    }
    public class SQLiteService : SQLiteHelper
    {
        public const string dbName = "paway.db";
        public SQLiteService()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string file = Path.Combine(path, dbName);
            base.InitConnect(file);
            if (base.InitCreate(Resources.script))
            {
                UserInfo info = new UserInfo()
                {
                    Name = "admin0"
                };
                Insert(info);
                info.Id = 19;
                info.Name = "admin001";
                Insert(info);
            }
        }
    }
    public class SqlService : SqlHelper
    {
        public SqlService()
        {
            base.InitConnect(@"(local)", "DiningLC", "mobot", "mobot");
        }
        public void Connect()
        {
            base.CommandStart();
        }
        public void Test()
        {
            new Action(Test1).BeginInvoke(null, null);
            new Action(Test1).BeginInvoke(null, null);
            new Action(Test1).BeginInvoke(null, null);
            new Action(Test1).BeginInvoke(null, null);
        }
        private void Test1()
        {
            Thread.Sleep(100);
            log.Debug("ThreadId:" + Thread.CurrentThread.ManagedThreadId);
            DbCommand cmd = null;
            try
            {
                cmd = TransStart();

                var sql = "update Users set Money =1 where Id= 3";
                ExecuteNonQuery(sql, cmd);
                var dt = ExecuteDataTable("select * from Users", cmd);
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(100);
                }
                sql = "update Users set Money =0 where Id= 3";
                ExecuteNonQuery(sql, cmd);

                TransCommit(cmd);
                log.Debug("ThreadId完成:" + Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                TransError(cmd);
                log.Debug("ThreadId异常:" + Thread.CurrentThread.ManagedThreadId);
                throw;
            }
            finally
            {
                CommandEnd(cmd);
            }
        }
    }
    public interface ITestData2
    {
        long Id { get; set; }
    }
    [Table(Table = "Cashs", Key = "Id")]
    public interface ITestData : ITestData2
    {
        [Property(ISelect = false, Column = "Value")]
        string V2 { get; set; }
        [Property(ISelect = false)]
        FindInfo FindInfo { get; set; }
        Image Image { get; set; }
    }
    [Serializable, Table(Table = "Cashs", Key = "Id")]
    public class TestData : ITestData
    {
        //public int Id { get; set; }
        public long Id { get; set; }

        [Property(ISelect = false, Column = "Name")]
        public string N2 { get; set; }

        [Property(ISelect = false)]
        public string Tag { get; set; }

        [Property(ISelect = false, Column = "Value")]
        public string V2 { get; set; }

        [Property(Column = "Image")]
        public Image Image { get; set; }

        [Property(ISelect = false)]
        public DateTime Date { get; set; }

        [Property(ISelect = false)]
        public FindInfo FindInfo { get; set; }

        public TestData()
        {
            this.Date = DateTime.Now;
        }
    }
}
