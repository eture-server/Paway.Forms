namespace Paway.Test
{
    partial class UsersControl
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
            this.toolBar1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridview1.Edit)).BeginInit();
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.TextFirst.ColorMove = System.Drawing.Color.Blue;
            this.toolBar1.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolBar1.Controls.SetChildIndex(this.tbName, 0);
            // 
            // tbName
            // 
            this.tbName.Lines = new string[0];
            // 
            // gridview1
            // 
            // 
            // 
            // 
            this.gridview1.Edit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridview1.Edit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridview1.Edit.Location = new System.Drawing.Point(0, 0);
            this.gridview1.Edit.Name = "tDataGridView1";
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridview1.Edit.RowTemplate.Height = 32;
            this.gridview1.Edit.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridview1.Edit.Size = new System.Drawing.Size(679, 174);
            this.gridview1.Edit.TabIndex = 12;
            // 
            // 
            // 
            this.gridview1.TPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.gridview1.TPager.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gridview1.TPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridview1.TPager.Location = new System.Drawing.Point(0, 174);
            this.gridview1.TPager.Name = "pager1";
            this.gridview1.TPager.Size = new System.Drawing.Size(679, 30);
            this.gridview1.TPager.TabIndex = 11;
            // 
            // UsersControl
            // 
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Name = "UsersControl";
            this.toolBar1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridview1.Edit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
