using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 窗体示例
    /// </summary>
    public class TBaseForm : IBaseForm
    {
        /// <summary>
        /// </summary>
        protected Paway.Forms.TPanel panel1;

        /// <summary>
        /// 构造
        /// </summary>
        public TBaseForm()
        {
            InitializeComponent();
            this.TMouseMove(this.panel1);
            lbTitle.Paint -= LbTitle_Paint;
            lbTitle.TextChanged += delegate { this.OnSizeChanged(EventArgs.Empty); };
            if (TConfig.TBackColor != null) this.TBrush.ColorMove = TConfig.TBackColor ?? Color.Empty;
        }
        /// <summary>
        /// 激活第一个控件焦点
        /// </summary>
        protected override void ActivatedFirst(Control control)
        {
            base.ActivatedFirst(panel1);
        }
        /// <summary>
        /// 调整Title位置
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (lbTitle != null)
            {
                var size = TextRenderer.MeasureText(lbTitle.Text, lbTitle.Font);
                this.lbTitle.Location = new Point((this.Width - size.Width) / 2, (this.Padding.Top - size.Height * 2 / 3) / 2);
            }
        }

        /// <summary>
        /// 底边线颜色
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (panel1.Visible)
            {
                using (var pen = new Pen(panel1.BackColor))
                {
                    e.Graphics.DrawLine(pen, 2, Height - 2, Width - 3, Height - 2);
                }
            }
        }

        private void InitializeComponent()
        {
            this.panel1 = new Paway.Forms.TPanel();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.None;
            this.lbTitle.Location = new System.Drawing.Point(200, 32);
            this.lbTitle.Size = new System.Drawing.Size(51, 27);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(441, 217);
            this.panel1.TabIndex = 5;
            // 
            // TBaseForm
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.ClientSize = new System.Drawing.Size(443, 299);
            this.Controls.Add(this.panel1);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "TBaseForm";
            this.Padding = new System.Windows.Forms.Padding(1, 80, 1, 2);
            this.TBrush.ColorDown = System.Drawing.Color.Ivory;
            this.TBrush.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(84)))), ((int)(((byte)(74)))));
            this.Controls.SetChildIndex(this.lbTitle, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            if (panel1 != null)
            {
                panel1.Dispose();
                panel1 = null;
            }
            base.Dispose(disposing);
        }
    }
}
