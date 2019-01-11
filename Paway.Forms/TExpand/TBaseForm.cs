using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    ///     窗体示例
    /// </summary>
    public class TBaseForm : IBaseForm
    {
        /// <summary>
        /// </summary>
        protected TControl panel1;

        /// <summary>
        /// 构造
        /// </summary>
        public TBaseForm()
        {
            InitializeComponent();
            this.TMouseMove(this.panel1);
            lbTitle.Paint -= LbTitle_Paint;
            lbTitle.TextChanged += delegate { this.OnSizeChanged(EventArgs.Empty); };
            lbTitle.Text = "aaaaaaaa";
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

        private void InitializeComponent()
        {
            this.panel1 = new Paway.Forms.TControl();
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
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(441, 218);
            this.panel1.TabIndex = 5;
            // 
            // TBaseForm
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.ClientSize = new System.Drawing.Size(443, 299);
            this.Controls.Add(this.panel1);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "TBaseForm";
            this.Padding = new System.Windows.Forms.Padding(1, 80, 1, 1);
            this.TBrush.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(57)))), ((int)(((byte)(50)))));
            this.TBrush.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.TBrush.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(84)))), ((int)(((byte)(74)))));
            this.TBrush.ColorSpace = System.Drawing.Color.Ivory;
            this.Controls.SetChildIndex(this.lbTitle, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
