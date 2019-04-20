namespace Paway.Forms
{
    partial class TDataControl<T>
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
            Paway.Forms.ToolItem toolItem4 = new Paway.Forms.ToolItem();
            this.panel3 = new Paway.Forms.TControl();
            this.panel1 = new Paway.Forms.TControl();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.panel2 = new Paway.Forms.TControl();
            this.tbName = new Paway.Forms.QQTextBox();
            this.gridview1 = new Paway.Forms.TDataGridViewPager();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridview1.Edit)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(683, 34);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 203);
            this.panel3.TabIndex = 66;
            this.panel3.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.toolBar1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.panel1.Size = new System.Drawing.Size(693, 34);
            this.panel1.TabIndex = 67;
            // 
            // toolBar1
            // 
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolBar1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolBar1.IAutoWidth = true;
            this.toolBar1.IClickEvent = true;
            this.toolBar1.IImageShow = true;
            toolItem1.Image = global::Paway.Forms.Properties.Resources.refresh;
            toolItem1.Tag = "刷新";
            toolItem1.Text = "刷新(F5)";
            toolItem2.Image = global::Paway.Forms.Properties.Resources.add;
            toolItem2.Tag = "添加";
            toolItem2.Text = "添加(A)";
            toolItem3.Image = global::Paway.Forms.Properties.Resources.edit;
            toolItem3.Tag = "编辑";
            toolItem3.Text = "编辑(E)";
            toolItem4.Image = global::Paway.Forms.Properties.Resources.close;
            toolItem4.Tag = "删除";
            toolItem4.Text = "删除(D)";
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.Items.Add(toolItem2);
            this.toolBar1.Items.Add(toolItem3);
            this.toolBar1.Items.Add(toolItem4);
            this.toolBar1.ItemSize = new System.Drawing.Size(0, 32);
            this.toolBar1.Location = new System.Drawing.Point(0, 2);
            this.toolBar1.MDirection = Paway.Helper.TMDirection.Up;
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(535, 32);
            this.toolBar1.TabIndex = 69;
            this.toolBar1.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.toolBar1.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.toolBar1.TextFirst.ColorMove = System.Drawing.Color.Blue;
            this.toolBar1.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.tbName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(535, 2);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 3, 5, 0);
            this.panel2.Size = new System.Drawing.Size(158, 32);
            this.panel2.TabIndex = 68;
            this.panel2.Visible = false;
            // 
            // tbName
            // 
            this.tbName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbName.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.tbName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbName.Icon = global::Paway.Forms.Properties.Resources.search;
            this.tbName.IconIsButton = true;
            this.tbName.ITrans = true;
            this.tbName.Lines = new string[0];
            this.tbName.Location = new System.Drawing.Point(0, 3);
            this.tbName.MaxLength = 16;
            this.tbName.Name = "tbName";
            this.tbName.Regex = "";
            this.tbName.RegexType = Paway.Helper.RegexType.Normal;
            this.tbName.Size = new System.Drawing.Size(153, 29);
            this.tbName.TabIndex = 60;
            this.tbName.WaterText = "Ctrl+F搜索";
            // 
            // gridview1
            // 
            this.gridview1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.gridview1.Edit.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridview1.Edit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridview1.Edit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridview1.Edit.Location = new System.Drawing.Point(0, 0);
            this.gridview1.Edit.Name = "tDataGridView1";
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridview1.Edit.RowTemplate.Height = 32;
            this.gridview1.Edit.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridview1.Edit.Size = new System.Drawing.Size(683, 173);
            this.gridview1.Edit.TabIndex = 12;
            this.gridview1.Location = new System.Drawing.Point(0, 34);
            this.gridview1.Name = "gridview1";
            this.gridview1.Size = new System.Drawing.Size(683, 203);
            this.gridview1.TabIndex = 69;
            // 
            // 
            // 
            this.gridview1.TPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.gridview1.TPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridview1.TPager.Location = new System.Drawing.Point(0, 173);
            this.gridview1.TPager.Name = "pager1";
            this.gridview1.TPager.Size = new System.Drawing.Size(683, 30);
            this.gridview1.TPager.TabIndex = 11;
            // 
            // TDataControl
            // 
            this.Controls.Add(this.gridview1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.MInterval = 6;
            this.Name = "TDataControl";
            this.Size = new System.Drawing.Size(693, 237);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridview1.Edit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected Paway.Forms.TControl panel1;
        protected Paway.Forms.TDataGridViewPager gridview1;
        protected Paway.Forms.TControl panel3;
        protected ToolBar toolBar1;
        protected TControl panel2;
        protected QQTextBox tbName;
    }
}
