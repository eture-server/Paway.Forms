using Paway.Forms;
using Paway.Test.Properties;
using Paway.Test.UI;
using Paway.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class Form2 : Demo360
    {
        public Form2()
        {
            InitializeComponent();
            toolBar1.ItemClick += toolBar1_ItemClick;
            toolBar2.ItemClick += toolBar2_ItemClick;
            toolUp.ItemClick += toolUp_ItemClick;
            //toolOk.MDirection = TMDirection.Transparent;
        }

        void toolUp_ItemClick(object sender, EventArgs e)
        {
            //toolBar2.Items.Add(new ToolItem(DateTime.Now.Second.ToString()));
            toolBar2.Items.RemoveAt(1);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            for (int i = 0; i < 3; i++)
            {
                toolBar2.Items.RemoveAt(i);
            }
            for (int i = 0; i < 3; i++)
            {
                toolBar2.Items.Insert(0, new ToolItem(i.ToString()));
            }
            for (int i = 0; i < 900; i++)
            {
                ToolItem item = new ToolItem();
                item.Text = i.ToString();
                item.Image = Resources.online;
                item.EndDesc = "x1";
                item.HeadDesc = "16.00";
                item.Desc = "+";
                item.ContextMenuStrip = contextMenuStrip1;
                toolBar2.Items.Add(item);
            }
            toolBar2.TChangeStart();
            toolBar2.TStart();
            //toolBar1.TClickFirst();
        }
        void toolBar2_ItemClick(object sender, EventArgs e)
        {
            ToolItem item = sender as ToolItem;
            item.Text = DateTime.Now.Second.ToString();
            toolBar2.TRefresh(item);
        }

        void toolBar1_ItemClick(object sender, EventArgs e)
        {
            ToolItem item = sender as ToolItem;
            MControl control = null;
            //MControl.ReSet(panel3);
            switch (item.Text)
            {
                case "上":
                    control = MControl.ReLoad(panel3, typeof(Control2), TMDirection.Up);
                    break;
                case "下":
                    control = MControl.ReLoad(panel3, typeof(Control2), TMDirection.Down);
                    break;
                case "左":
                    control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.Left);
                    break;
                case "右":
                    control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.Right);
                    break;
                case "中":
                    control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.Center);
                    break;
                case "无":
                    control = MControl.ReLoad(panel3, typeof(Control1));
                    break;
                case "色1":
                    control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.Transparent);
                    break;
                case "色2":
                    control = MControl.ReLoad(panel3, typeof(Control2), TMDirection.None);
                    break;
                case "左转":
                    control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.T3DLeft);
                    break;
                case "左右转":
                    control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.T3DLeftToRight);
                    break;
                case "右转":
                    control = MControl.ReLoad(panel3, typeof(Control2), TMDirection.T3DRight);
                    break;
                case "右左转":
                    control = MControl.ReLoad(panel3, typeof(Control2), TMDirection.T3DRightToLeft);
                    break;
                case "上转":
                    control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.T3DUp);
                    break;
                case "上下转":
                    control = MControl.ReLoad(panel3, typeof(Control1), TMDirection.T3DUpToDown);
                    break;
                case "下转":
                    control = MControl.ReLoad(panel3, typeof(Control2), TMDirection.T3DDown);
                    break;
                case "下上转":
                    control = MControl.ReLoad(panel3, typeof(Control2), TMDirection.T3DDownToUp);
                    break;
            }
        }
    }
}
