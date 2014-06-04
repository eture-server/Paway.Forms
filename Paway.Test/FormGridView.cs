﻿using Paway.Helper;
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
    public partial class FormGridView : QQDemo
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
            this.comBoxGridView1.ProgressIndex = -1;
            this.comBoxGridView1.CurrentCellChanged += comBoxGridView1_CurrentCellChanged;
        }

        int index = -1;
        Bitmap bitmap;
        Bitmap last;
        void comBoxGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.comBoxGridView1.CurrentCell.RowIndex == index) return;
            if (index != -1) this.comBoxGridView1.Rows[index].Cells[0].Value = false;
            int old = index;

            index = this.comBoxGridView1.CurrentCell.RowIndex;
            this.comBoxGridView1.ProgressIndex = index;
            bitmap = this.comBoxGridView1.Rows[index].Cells["Image"].Value as Bitmap;
            this.comBoxGridView1.Rows[index].Cells[0].Value = true;
            this.comBoxGridView1.Rows[index].Cells[2].Value = string.Format("点击&第1步");
            if (last != null)
            {
                this.comBoxGridView1.Rows[old].Cells[2].Value = "点击";
                this.comBoxGridView1.Rows[old].Cells["Image"].Value = last;
            }
            last = bitmap.Clone() as Bitmap;
        }

        protected void WaitDrawDataGridView()
        {
            BindingList<WaitDrawDataGridViewData> list = new BindingList<WaitDrawDataGridViewData>();
            gridView1.DataSource = list;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image");
            for (int i = 0; i < 10; i++)
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
            //int height = this.gridView1.ColumnHeadersHeight;
            //if (height < Resources.online.Height)
            //{
            //    height = Resources.online.Height;
            //}
            //this.Height = (gridView1.Rows.Count + 1) * (height + 1) + 26;
            //this.Height = gridView1.Rows.Count * (Resources.online.Height + 1) + this.gridView1.ColumnHeadersHeight;
        }
        protected void CheckBoxDataGridView()
        {
            List<CheckBoxDataGridViewData> list = new List<CheckBoxDataGridViewData>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image");
            for (int i = 0; i < 4; i++)
            {
                list.Add(new CheckBoxDataGridViewData
                {
                    Id = (i + 3).ToString(),
                    IsSelect = false,
                    CommandType = "点击",
                    ActionTime = DateTime.Now.ToString(),
                    Image = imageList1.Images[i],
                    //Image = BitmapHelper.GetBitmapFormFile(string.Format("{0}\\{1}.png", path, i)),
                    NameStr = "图标" + i,
                    ComponentId = Guid.NewGuid(),
                });
            }
            comBoxGridView1.DataSource = list;
        }
        protected void CheckBox()
        {
            List<CheckBoxData> list = new List<CheckBoxData>();
            for (int i = 0; i < 4; i++)
            {
                list.Add(new CheckBoxData
                {
                    ProColor = "红色",
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
            comBoxGridView2.DataSource = list;

            BindingList<Data2> list2 = new BindingList<Data2>();
            for (int i = 0; i < 6; i++)
            {
                list2.Add(new Data2
                {
                    A = "红色" + i,
                    B = i.ToString(),
                });
            }
            dataGridView1.DataSource = list2.ToDataTable();
        }
    }
    public class Data2
    {
        public string A { get; set; }

        public string B { get; set; }

    }
    public class WaitDrawDataGridViewData
    {
        public int Statu { get; set; }

        public Image StatuImage { get; set; }

        public string Device { get; set; }

        public string Product { get; set; }

        public string AppName { get; set; }

        public int Index { get; set; }

        public int Progress { get; set; }
    }
    public class CheckBoxDataGridViewData
    {
        public bool IsSelect { get; set; }

        public string Id { get; set; }

        public string CommandType { get; set; }

        public string ActionTime { get; set; }

        public Image Image { get; set; }

        public string NameStr { get; set; }

        public Guid ComponentId { get; set; }
    }
    public class CheckBoxData
    {
        public string ProColor { get; set; }

        public long ProSize { get; set; }

        public long Price { get; set; }
    }
}
