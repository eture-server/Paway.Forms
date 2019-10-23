namespace Paway.Forms
{
    partial class TMultipleEnum
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
            this.gridview1 = new Paway.Forms.TDataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridview1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridview1
            // 
            this.gridview1.AllowUserToResizeColumns = false;
            this.gridview1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridview1.ColumnHeadersVisible = false;
            this.gridview1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id});
            this.gridview1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridview1.GridColor = System.Drawing.SystemColors.ControlDark;
            this.gridview1.Location = new System.Drawing.Point(0, 0);
            this.gridview1.Name = "gridview1";
            this.gridview1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.gridview1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.gridview1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridview1.RowTemplate.Height = 30;
            this.gridview1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridview1.Size = new System.Drawing.Size(207, 152);
            this.gridview1.TabIndex = 34;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // TQuery
            // 
            this.Controls.Add(this.gridview1);
            this.Name = "TQuery";
            this.Size = new System.Drawing.Size(207, 152);
            ((System.ComponentModel.ISupportInitialize)(this.gridview1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Paway.Forms.TDataGridView gridview1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;

    }
}
