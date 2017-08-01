using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;
using Paway.Test.Properties;
using System.IO;
using Paway.Helper;

namespace Paway.Test
{
    public partial class FormQQ : QQForm
    {
        public FormQQ()
        {
            InitializeComponent();
            this.contextMenuStrip1.Renderer = new QQToolStripRenderer();
        }

        private void AnimalShow()
        {
            string caption = "消息提示";
            string text = @"“末日”前晒出流逝的岁月
                            上传一组证明您岁月痕迹的新老对比照片
                            即可获得抽奖资格和微博积分";
            text = string.Format("{0}\r\n{1}", text, DateTime.Now.ToLongTimeString());
            NotifyForm form = new NotifyForm();
            form.AnimalShow(caption, text, int.MaxValue);
        }

        private void qqButton1_Click(object sender, EventArgs e)
        {
            AnimalShow();
            TreeView();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.contextMenuStrip1.ContextMenuStripChanged += contextMenuStrip1_ContextMenuStripChanged;
            this.contextMenuStrip1.Paint += contextMenuStrip1_Paint;
            TreeView();
        }

        private void TreeView()
        {
            List<TreeData> list = new List<TreeData>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image");
            for (int i = 1; i < 3; i++)
            {
                TreeData tda = new TreeData
                {
                    Id = i,
                    Name = "Name" + i,
                    Custom = "Custom" + i,
                    Value = "Value" + i,
                    Statu = BitmapHelper.GetBitmapFormFile(string.Format("{0}\\{1}.png", path, i)),
                };
                list.Add(tda);
                for (int j = 10; j < 14; j++)
                {
                    list.Add(new TreeData
                    {
                        Id = i * 10 + j,
                        ParentId = tda.Id,
                        Name = "C_N" + j,
                        Custom = "C_C" + i,
                        Statu = BitmapHelper.GetBitmapFormFile(string.Format("{0}\\{1}.png", path, j - 10)),
                        Value = "C_V" + j,
                    });
                }
            }
            this.treeView1.DataSource = list;
        }

        void contextMenuStrip1_Paint(object sender, PaintEventArgs e)
        {
        }

        void contextMenuStrip1_HandleCreated(object sender, EventArgs e)
        {
        }

        void contextMenuStrip1_ContextMenuStripChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ItemNode node = treeView1.SelectedNode as ItemNode;
            if (node == null) return;

            int id = node["Id"].ToInt();
            List<TreeData> list = this.treeView1.DataSource as List<TreeData>;
            TreeData info = list.Find(c => c.Id == id);

            info.Name = DateTime.Now.Second.ToString();
            treeView1.UpdateNode(info.Id);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ItemNode node = treeView1.SelectedNode as ItemNode;
            List<TreeData> list = this.treeView1.DataSource as List<TreeData>;

            TreeData info = new TreeData() { Id = list[list.Count - 1].Id + 1 };
            info.Name = DateTime.Now.Second.ToString();
            info.ParentId = node == null ? 0 : node["Id"].ToInt();
            list.Add(info);
            treeView1.UpdateNode(info.Id);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ItemNode node = treeView1.SelectedNode as ItemNode;
            int id = node["Id"].ToInt();
            List<TreeData> list = this.treeView1.DataSource as List<TreeData>;
            TreeData info = list.Find(c => c.Id == id);

            list.Remove(info);
            treeView1.UpdateNode(id);
        }
    }
    public class TreeData
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public string Custom { get; set; }

        public Image Statu { get; set; }

        public string Value { get; set; }
    }
}
