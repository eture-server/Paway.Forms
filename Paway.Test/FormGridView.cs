using Paway.Helper;
using Paway.Forms;
using Paway.Test.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class FormGridView : TBaseForm
    {
        public FormGridView()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            WaitDrawDataGridView();
            CheckBoxDataGridView();
            CheckBox();
            this.tDataGridView1.TProgressIndex = -1;
            this.tDataGridView1.CurrentCellChanged += comBoxGridView1_CurrentCellChanged;
        }

        int index = -1;
        Bitmap bitmap;
        Bitmap last;
        private void comBoxGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.tDataGridView1.CurrentCell == null) return;
            if (this.tDataGridView1.CurrentCell.RowIndex == index) return;
            if (index != -1) this.tDataGridView1.Rows[index].Cells[0].Value = false;
            int old = index;

            index = this.tDataGridView1.CurrentCell.RowIndex;
            this.tDataGridView1.TProgressIndex = index;
            bitmap = this.tDataGridView1.Rows[index].Cells["Image"].Value as Bitmap;
            this.tDataGridView1.Rows[index].Cells[0].Value = true;
            this.tDataGridView1.Rows[index].Cells[2].Value = string.Format("点击123456&第1步123456");
            if (last != null)
            {
                this.tDataGridView1.Rows[old].Cells[2].Value = "点击";
                this.tDataGridView1.Rows[old].Cells["Image"].Value = last;
            }
            if (bitmap != null)
            {
                last = bitmap.Clone() as Bitmap;
            }
        }

        protected void WaitDrawDataGridView()
        {
            BindingList<WaitDrawDataGridViewData> list = new BindingList<WaitDrawDataGridViewData>();
            tDataGridViewPager1.PagerInfo.PageSize = 50;
            //tDataGridViewPager1.PageChanged += tDataGridViewPager1_PageChanged;
            tDataGridViewPager1.DataSource = new Data2() { A = "正在加载" };
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image");
            for (int i = 0; i < 113; i++)
            {
                bool a = false, b = false, c = false;
                if (i % 10 == 1) a = true;
                if (i % 10 == 3) b = true;
                if (i % 10 == 5) c = true;
                WaitDrawDataGridViewData dti = new WaitDrawDataGridViewData
                {
                    StatuImage = BitmapHelper.GetBitmapFormFile(string.Format("{0}\\{1}.png", path, i % 3)),
                    Product = "Product" + (!b ? 1 : 2),
                    AppName = "AppName" + (!a ? 1 : (!b ? 2 : (!c ? 3 : 4))),
                    Index = i,
                    Progress = i,
                };
                switch (i % 3)
                {
                    case 0: dti.Device = DeviceType.Carte; break;
                    case 1: dti.Device = DeviceType.Delete; break;
                    case 2: dti.Device = DeviceType.Checkout; break;
                }
                list.Add(dti);
            }
            tDataGridViewPager1.DataSource = list;
        }
        void tDataGridViewPager1_PageChanged(object sender, EventArgs e)
        {
        }
        protected void CheckBoxDataGridView()
        {
            List<CheckBoxDataGridViewData> list = new List<CheckBoxDataGridViewData>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image");
            for (int i = 0; i < 3; i++)
            {
                list.Add(new CheckBoxDataGridViewData
                {
                    Id = (i + 3).ToString(),
                    ISelect = false,
                    CommandType = "点击",
                    ActionTime = DateTime.Now.ToString(),
                    Image = imageList1.Images[i],
                    //Image = BitmapHelper.GetBitmapFormFile(string.Format("{0}\\{1}.png", path, i)),
                    NameStr = "图标" + i,
                    ComponentId = Guid.NewGuid(),
                });
            }
            tDataGridView1.DataSource = list;

            this.tDataGridView1.AddSpanHeader(2, 2, "Hello");
        }
        protected void CheckBox()
        {
            List<CheckBoxData> list = new List<CheckBoxData>();
            for (int i = 0; i < 4; i++)
            {
                list.Add(new CheckBoxData
                {
                    ProColor = "红色ABC123456789CBFWEFJKGRNFKEFNLWDF",
                    ProSize = 35 + (i < 2 ? 0 : 1),
                    Price = i,
                });
            }
            for (int i = 0; i < 4; i++)
            {
                list.Add(new CheckBoxData
                {
                    ProColor = "黑色",
                    ProSize = 35 + (i < 2 ? 0 : 1),
                    Price = i,
                });
            }
            tDataGridView2.DataSource = list;
            tDataGridView2.AddSpanColumns(0, 1);

            BindingList<Data2> list2 = new BindingList<Data2>();
            for (int i = 0; i < 6; i++)
            {
                list2.Add(new Data2
                {
                    A = "红色" + i,
                    B = i.ToString(),
                });
            }

        }
    }
    public class Data2
    {
        public string A { get; set; }

        [Property(IShow = false, ISelect = false)]
        public string B { get; set; }

    }
    class WaitDrawDataGridViewData : Data2
    {
        public int Statu { get; set; }

        public Image StatuImage { get; set; }

        public DeviceType Device { get; set; }

        public string Product { get; set; }

        public string AppName { get; set; }

        public int? Index { get; set; }

        public int Progress { get; set; }
    }
    public class CheckBoxDataGridViewData
    {
        public bool ISelect { get; set; }

        public string Id { get; set; }

        public string NameStr { get; set; }

        public string CommandType { get; set; }

        public Image Image { get; set; }

        [Property(IShow = false)]
        public string ActionTime { get; set; }

        [Property(IShow = false)]
        public Guid ComponentId { get; set; }
    }
    public class CheckBoxData
    {
        public string ProColor { get; set; }

        public long ProSize { get; set; }

        public long Price { get; set; }
    }
    enum DeviceType
    {
        None,
        Carte,
        Delete,
        Checkout,
        Query,
    }
}
