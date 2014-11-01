namespace Paway.Test.UI
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
            this.tDataGridViewPager1 = new Paway.Forms.TDataGridViewPager();
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridViewPager1.Edit)).BeginInit();
            this.SuspendLayout();
            // 
            // tDataGridViewPager1
            // 
            this.tDataGridViewPager1.CurrenetPageIndex = 1;
            this.tDataGridViewPager1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.tDataGridViewPager1.Edit.AllowUserToAddRows = false;
            this.tDataGridViewPager1.Edit.AllowUserToDeleteRows = false;
            this.tDataGridViewPager1.Edit.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tDataGridViewPager1.Edit.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.tDataGridViewPager1.Edit.CheckBoxName = "";
            this.tDataGridViewPager1.Edit.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.tDataGridViewPager1.Edit.ColumnHeadersHeight = 30;
            this.tDataGridViewPager1.Edit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.tDataGridViewPager1.Edit.ColumnImage = "";
            this.tDataGridViewPager1.Edit.ColumnImageText = "";
            this.tDataGridViewPager1.Edit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tDataGridViewPager1.Edit.GridColor = System.Drawing.Color.LightBlue;
            this.tDataGridViewPager1.Edit.Location = new System.Drawing.Point(0, 0);
            this.tDataGridViewPager1.Edit.MultiSelect = false;
            this.tDataGridViewPager1.Edit.Name = "tDataGridView1";
            this.tDataGridViewPager1.Edit.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.tDataGridViewPager1.Edit.RowHeadersVisible = false;
            this.tDataGridViewPager1.Edit.RowHeadersWidth = 21;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.tDataGridViewPager1.Edit.RowTemplate.Height = 32;
            this.tDataGridViewPager1.Edit.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tDataGridViewPager1.Edit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tDataGridViewPager1.Edit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tDataGridViewPager1.Edit.Size = new System.Drawing.Size(533, 187);
            this.tDataGridViewPager1.Edit.TabIndex = 12;
            this.tDataGridViewPager1.Location = new System.Drawing.Point(0, 0);
            this.tDataGridViewPager1.Name = "tDataGridViewPager1";
            this.tDataGridViewPager1.Size = new System.Drawing.Size(533, 217);
            this.tDataGridViewPager1.TabIndex = 0;
            // 
            // 
            // 
            this.tDataGridViewPager1.TPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.tDataGridViewPager1.TPager.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tDataGridViewPager1.TPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tDataGridViewPager1.TPager.Location = new System.Drawing.Point(0, 187);
            this.tDataGridViewPager1.TPager.Name = "pager1";
            this.tDataGridViewPager1.TPager.PageSize = 50;
            this.tDataGridViewPager1.TPager.RecordCount = 0;
            this.tDataGridViewPager1.TPager.Size = new System.Drawing.Size(533, 30);
            this.tDataGridViewPager1.TPager.TabIndex = 11;
            // 
            // Control1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tDataGridViewPager1);
            this.Name = "Control1";
            this.Size = new System.Drawing.Size(533, 217);
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridViewPager1.Edit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.TDataGridViewPager tDataGridViewPager1;

    }
}
