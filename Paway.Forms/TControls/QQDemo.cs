﻿using Paway.Helper;
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
        /// 构造
        /// </summary>
        public QQDemo()
        {
            InitializeComponent();
            this.TDrawBelowBorder(panel1);
            this.TMouseMove(panel1);
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
