using Paway.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class TreeForm : Form
    {
        public TreeForm()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var server = new SQLiteService();
            var list = server.Find<TestInfo>("1=1 limit 100");


            treeGridView1.Nodes.Clear();
            treeGridView1.Columns.Clear();

            var column = new TreeGridColumn();
            column.Name = "Name";
            column.HeaderText = "名称";
            treeGridView1.Columns.Add(column);
            treeGridView1.Columns.Add("Id", "ID");
            treeGridView1.Columns.Add("Value", "Value");

            treeGridView1.SuspendLayout();

            var tempList = list.FindAll(c => c.ParentId == 0);
            foreach (var temp in tempList)
            {
                var projectNode = treeGridView1.Nodes.Add(temp.Name, temp.Id, temp.Value);

                var childList = list.FindAll(c => c.ParentId == temp.Id);
                foreach (var child in childList)
                {
                    projectNode.Nodes.Add(child.Name, child.Id, child.Value);
                }
            }

            treeGridView1.ResumeLayout(false);

            tDataGridView1.DataSource = list;
            tDataGridViewPager1.DataSource = list;
        }
    }
}
