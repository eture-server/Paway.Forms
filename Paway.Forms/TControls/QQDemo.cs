using Paway.Helper;
using Paway.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Win32;

namespace Paway.Forms
{
    /// <summary>
    /// QQ窗体示例
    /// </summary>
    public partial class QQDemo : QQForm
    {
        /// <summary>
        /// QQ窗体示例
        /// </summary>
        public QQDemo()
        {
            InitializeComponent();
            this.panel1.Paint += panel1_Paint;
            this.panel1.MouseDown += panel1_MouseDown;
        }
        void panel1_Paint(object sender, PaintEventArgs e)
        {
            base.DrawBelowBorder(panel1);
        }
        /// <summary>
        /// </summary>
        protected void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, 274, 61440 + 9, 0);
            }
        }
        /// <summary>
        /// 关闭时激发父窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (this.Owner != null)
            {
                Owner.Activate();
            }
        }
    }
}
