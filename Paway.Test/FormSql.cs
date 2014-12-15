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
using Paway.Utils.Data;

namespace Paway.Test
{
    public partial class FormSql : QQDemo
    {
        public FormSql()
        {
            InitializeComponent();
        }

        DataService service = new DataService();

        List<TestData> list = new List<TestData>();
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
                MessageBox.Show(this, "SQL错误", string.Format("{0}", ex), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                service.Update<TestData>(list[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "SQL错误", string.Format("{0}", ex), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btUpOrIn_Click(object sender, EventArgs e)
        {
            try
            {
                service.Replace<TestData>(list[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "SQL错误", string.Format("{0}", ex), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            try
            {
                object result = service.ExecuteScalar("select count(0) from Hello");
                service.Delete<TestData>(5);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "SQL错误", string.Format("{0}", ex), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            try
            {
                service.Find();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "SQL错误", string.Format("{0}", ex), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    public class DataService : SqlHelper
    {
        public DataService()
        {
            base.InitConnect("ConnectionString");
        }
        public void Find()
        {
            List<TestData> list = Find<TestData>() as List<TestData>;
        }
    }

    [Serializable, Property(Table = "Hello", Mark = "Tid")]
    public class TestData : BaseData
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

        public TestData() : this(1) { }
        public TestData(int a)
        { }
    }
    [Serializable, Property(Table = "HelloBase", Mark = "Tid")]
    public class BaseData
    {
        [Property(Column = "Data")]
        public int Data { get; set; }
    }
}
