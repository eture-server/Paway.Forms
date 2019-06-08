namespace Paway.Test
{
    partial class Control1
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            this.panel1 = new Paway.Forms.TControl();
            this.toolbar = new Paway.Forms.ToolBar();
            this.tDataGridViewPager1 = new Paway.Forms.TDataGridViewPager();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridViewPager1.Edit)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolbar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(533, 49);
            this.panel1.TabIndex = 1;
            // 
            // toolbar
            // 
            this.toolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbar.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolbar.IClickEvent = true;
            toolItem1.Tag = "登陆";
            toolItem1.Text = "登陆";
            toolItem2.Tag = "更新密码";
            toolItem2.TColor.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            toolItem2.TColor.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            toolItem2.TColor.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(239)))));
            toolItem2.Text = "更新密码";
            toolItem3.Tag = "语言";
            toolItem3.TColor.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(110)))), ((int)(((byte)(18)))));
            toolItem3.TColor.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(187)))), ((int)(((byte)(95)))));
            toolItem3.TColor.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(26)))));
            toolItem3.Text = "Deutsch";
            this.toolbar.Items.Add(toolItem1);
            this.toolbar.Items.Add(toolItem2);
            this.toolbar.Items.Add(toolItem3);
            this.toolbar.ItemSize = new System.Drawing.Size(94, 46);
            this.toolbar.ItemSpace = 15;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.MDirection = Paway.Helper.TMDirection.Up;
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(533, 49);
            this.toolbar.TabIndex = 88;
            this.toolbar.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(50)))), ((int)(((byte)(27)))));
            this.toolbar.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(127)))), ((int)(((byte)(104)))));
            this.toolbar.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(72)))), ((int)(((byte)(38)))));
            this.toolbar.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolbar.TextFirst.ColorMove = System.Drawing.Color.Black;
            this.toolbar.TextFirst.ColorNormal = System.Drawing.Color.White;
            this.toolbar.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolbar.Trans = 150;
            // 
            // tDataGridViewPager1
            // 
            this.tDataGridViewPager1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.tDataGridViewPager1.Edit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tDataGridViewPager1.Edit.Location = new System.Drawing.Point(0, 0);
            this.tDataGridViewPager1.Edit.Name = "tDataGridView1";
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.tDataGridViewPager1.Edit.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tDataGridViewPager1.Edit.Size = new System.Drawing.Size(533, 138);
            this.tDataGridViewPager1.Edit.TabIndex = 12;
            this.tDataGridViewPager1.Location = new System.Drawing.Point(0, 49);
            this.tDataGridViewPager1.Name = "tDataGridViewPager1";
            this.tDataGridViewPager1.Size = new System.Drawing.Size(533, 168);
            this.tDataGridViewPager1.TabIndex = 4;
            // 
            // 
            // 
            this.tDataGridViewPager1.TPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.tDataGridViewPager1.TPager.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tDataGridViewPager1.TPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tDataGridViewPager1.TPager.Location = new System.Drawing.Point(0, 138);
            this.tDataGridViewPager1.TPager.Name = "pager1";
            this.tDataGridViewPager1.TPager.Size = new System.Drawing.Size(533, 30);
            this.tDataGridViewPager1.TPager.TabIndex = 11;
            // 
            // Control1
            // 
            this.Controls.Add(this.tDataGridViewPager1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.Name = "Control1";
            this.Size = new System.Drawing.Size(533, 217);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridViewPager1.Edit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.TControl panel1;
        private Forms.ToolBar toolbar;
        private Forms.TDataGridViewPager tDataGridViewPager1;

    }
}
