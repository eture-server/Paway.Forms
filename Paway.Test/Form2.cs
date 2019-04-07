using Paway.Forms;
using Paway.Helper;
using Paway.Test.Properties;
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
            toolBar1.ItemClick += ToolBar1_ItemClick;
            toolBar2.ItemClick += ToolBar2_ItemClick;
            toolUp.ItemClick += toolUp_ItemClick;
            //toolOk.MDirection = TMDirection.Transparent;
        }

        private void toolUp_ItemClick(ToolItem item, EventArgs e)
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
                ToolItem item = new ToolItem
                {
                    Text = i.ToString(),
                    Image = Resources.online,
                    EndDesc = "x1",
                    HeadDesc = "16.00",
                    Desc = "+"
                };
                toolBar2.Items.Add(item);
            }
            toolBar2.TChangeStart();
            toolBar2.TStart();
            //toolBar1.TClickFirst();
        }
        private void ToolBar2_ItemClick(ToolItem item, EventArgs e)
        {
            item.Text = DateTime.Now.Second.ToString();
        }

        private void ToolBar1_ItemClick(ToolItem item, EventArgs e)
        {
            //MControl.ReSet(panel3);
            switch (item.Text)
            {
                case "上":
                  MControl.ReLoad(panel3, typeof(Control2), TMDirection.Up);
                    break;
                case "下":
                     MControl.ReLoad(panel3, typeof(Control2), TMDirection.Down);
                    break;
                case "左":
                     MControl.ReLoad(panel3, typeof(Control1), TMDirection.Left);
                    break;
                case "右":
                     MControl.ReLoad(panel3, typeof(Control1), TMDirection.Right);
                    break;
                case "中":
                     MControl.ReLoad(panel3, typeof(Control1), TMDirection.Center);
                    break;
                case "无":
                     MControl.ReLoad(panel3, typeof(Control1));
                    break;
                case "色1":
                     MControl.ReLoad(panel3, typeof(Control1), TMDirection.Transparent);
                    break;
                case "色2":
                     MControl.ReLoad(panel3, typeof(Control2), TMDirection.None);
                    break;
                case "左转":
                     MControl.ReLoad(panel3, typeof(Control1), TMDirection.T3DLeft);
                    break;
                case "左右转":
                     MControl.ReLoad(panel3, typeof(Control1), TMDirection.T3DLeftToRight);
                    break;
                case "右转":
                     MControl.ReLoad(panel3, typeof(Control2), TMDirection.T3DRight);
                    break;
                case "右左转":
                     MControl.ReLoad(panel3, typeof(Control2), TMDirection.T3DRightToLeft);
                    break;
                case "上转":
                     MControl.ReLoad(panel3, typeof(Control1), TMDirection.T3DUp);
                    break;
                case "上下转":
                     MControl.ReLoad(panel3, typeof(Control1), TMDirection.T3DUpToDown);
                    break;
                case "下转":
                     MControl.ReLoad(panel3, typeof(Control2), TMDirection.T3DDown);
                    break;
                case "下上转":
                     MControl.ReLoad(panel3, typeof(Control2), TMDirection.T3DDownToUp);
                    break;
            }
        }
    }
}
