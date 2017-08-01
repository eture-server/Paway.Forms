﻿using Paway.Helper;
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
using Paway.Utils.Data;
using System.IO;
using Paway.Test.Properties;

namespace Paway.Test
{
    public partial class FormSql : QQDemo
    {
        SqlDataService service = new SqlDataService();
        SerDataService service2 = new SerDataService();
        List<TestData> list = new List<TestData>();

        public FormSql()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            for (int i = 0; i < 10; i++)
            {
                TestData data = new TestData
                {
                    T2 = i,
                    N2 = "Name" + i,
                    Tag = "Tag" + i,
                    I2 = pictureBox1.Image,
                };
                list.Add(data);
            }
        }
        private void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                service.Insert<TestData>(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("{0}", ex), "SQL错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
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
                MessageBox.Show(this, string.Format("{0}", ex), "SQL错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btUpOrIn_Click(object sender, EventArgs e)
        {
            try
            {
                //暂不支持MySql
                if (list.Count == 0) return;
                list[0].N2 = DateTime.Now.Minute.ToString();
                list[0].V2 = DateTime.Now.Second.ToString();
                service.Replace(list[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("{0}", ex), "SQL错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (list.Count == 0) return;
                object result = service.ExecuteScalar("select count(0) from Hello");
                service.Delete<TestData>(list[0].T2);
                list.RemoveAt(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("{0}", ex), "SQL错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            try
            {
                if (list.Count == 0) return;
                service.Insert(list[0]);
                list[0].N2 = "V3";
                list[0].T2 = 17;
                service.Replace(list[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("{0}", ex), "SQL错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            try
            {
                list = service.Find<TestData>() as List<TestData>;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("{0}", ex), "SQL错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    public class SqlDataService : SqlHelper //MySqlHelper//SqlHelper//SQLiteHelper
    {
        public const string dbName = "paway.db";
        public SqlDataService()
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory;
            //string file = Path.Combine(path, dbName);
            //base.InitConnect(file);
            //if (base.InitCreate(Resources.script))
            //{
            //    UserInfo info = new UserInfo();
            //    info.Name = "admin0";
            //    Insert(info);
            //    info.Id = 19;
            //    info.Name = "admin001";
            //    Replace(info);
            //}

            //base.InitConnect("127.0.0.1", "Test", "root", "mobot");//MySqlHelper
            base.InitConnect("(local)", "Test", "mobot", "mobot");//SqlHelper
            //base.InitConnect("ConnectionString");//SqlHelper
        }
    }
    public class SerDataService : SqlHelper
    {
        private object syncLock = new object();
        public SerDataService()
        {
            base.InitConnect(@"(local)\SQLEXPRESS", "DiningLC", "mobot", "mobot");
        }
    }

    [Serializable]
    [Property(Table = "Users", Key = "Id")]
    public class UserInfo
    {
        [Property(Show = false)]
        public long Id { get; set; }

        [Property(Text = "用户名")]
        public string Name { get; set; }

        [Property(Show = false)]
        public DateTime CreateDate { get; set; }

        public UserInfo()
        {
            this.CreateDate = DateTime.Now;
        }
    }
    [Serializable, Property(Table = "Hello", Key = "Tid")]
    public class TestData
    {
        //public int Id { get; set; }
        [Property(Column = "Tid")]
        public long T2 { get; set; }
        [Property(Column = "Name")]
        public string N2 { get; set; }
        [Property(Select = false)]
        public string Tag { get; set; }
        [Property(Column = "Value")]
        public string V2 { get; set; }
        [Property(Column = "Image")]
        public Image I2 { get; set; }

        public DateTime Date { get; set; }

        public TestData() : this(1) { }
        public TestData(int a)
        {
            this.Date = DateTime.Now;
        }
    }
}
