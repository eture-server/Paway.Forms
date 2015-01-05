namespace Paway.Test
{
    partial class FormGridView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridView));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tDataGridView1 = new Paway.Forms.TDataGridView();
            this.IsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CommandType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.NameStr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComponentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tDataGridView2 = new Paway.Forms.TDataGridView();
            this.ProColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.tDataGridViewPager1 = new Paway.Forms.TDataGridViewPager();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridViewPager1.Edit)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.tDataGridView1);
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 2);
            this.panel1.Size = new System.Drawing.Size(457, 546);
            // 
            // tDataGridView1
            // 
            this.tDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tDataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.tDataGridView1.CheckBoxName = "IsSelect";
            this.tDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.tDataGridView1.ColumnHeadersHeight = 30;
            this.tDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.tDataGridView1.ColumnImage = "Image";
            this.tDataGridView1.ColumnImageText = "NameStr";
            this.tDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsSelect,
            this.CommandType,
            this.Image,
            this.NameStr,
            this.ActionTime,
            this.ComponentId,
            this.Id});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tDataGridView1.DefaultCellStyle = dataGridViewCellStyle10;
            this.tDataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tDataGridView1.GridColor = System.Drawing.Color.LightBlue;
            this.tDataGridView1.IsDrawCheckBox = true;
            this.tDataGridView1.IsMultiText = true;
            this.tDataGridView1.Location = new System.Drawing.Point(10, 10);
            this.tDataGridView1.MultiSelect = false;
            this.tDataGridView1.Name = "tDataGridView1";
            this.tDataGridView1.ProgressImage = global::Paway.Test.Properties.Resources.process;
            this.tDataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.tDataGridView1.RowHeadersWidth = 21;
            this.tDataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.tDataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.tDataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.tDataGridView1.RowTemplate.Height = 32;
            this.tDataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tDataGridView1.Size = new System.Drawing.Size(437, 149);
            this.tDataGridView1.TabIndex = 8;
            // 
            // IsSelect
            // 
            this.IsSelect.DataPropertyName = "IsSelect";
            this.IsSelect.FillWeight = 83.18353F;
            this.IsSelect.HeaderText = "";
            this.IsSelect.Name = "IsSelect";
            // 
            // CommandType
            // 
            this.CommandType.DataPropertyName = "CommandType";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.CommandType.DefaultCellStyle = dataGridViewCellStyle8;
            this.CommandType.FillWeight = 74.60026F;
            this.CommandType.HeaderText = "类型";
            this.CommandType.Name = "CommandType";
            this.CommandType.ReadOnly = true;
            this.CommandType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Image
            // 
            this.Image.DataPropertyName = "Image";
            this.Image.FillWeight = 128.9345F;
            this.Image.HeaderText = "图片";
            this.Image.Name = "Image";
            // 
            // NameStr
            // 
            this.NameStr.DataPropertyName = "NameStr";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.NameStr.DefaultCellStyle = dataGridViewCellStyle9;
            this.NameStr.FillWeight = 80.68803F;
            this.NameStr.HeaderText = "名称";
            this.NameStr.Name = "NameStr";
            this.NameStr.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ActionTime
            // 
            this.ActionTime.DataPropertyName = "ActionTime";
            this.ActionTime.HeaderText = "ActionTime";
            this.ActionTime.Name = "ActionTime";
            this.ActionTime.Visible = false;
            // 
            // ComponentId
            // 
            this.ComponentId.DataPropertyName = "ComponentId";
            this.ComponentId.HeaderText = "ComponentId";
            this.ComponentId.Name = "ComponentId";
            this.ComponentId.Visible = false;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.Visible = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.panel3.Controls.Add(this.tDataGridView2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 316);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.panel3.Size = new System.Drawing.Size(437, 220);
            this.panel3.TabIndex = 10;
            // 
            // tDataGridView2
            // 
            this.tDataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tDataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.tDataGridView2.CheckBoxName = "IsSelect";
            this.tDataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tDataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tDataGridView2.ColumnHeadersHeight = 30;
            this.tDataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.tDataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProColor,
            this.ProSize,
            this.Price});
            this.tDataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tDataGridView2.GridColor = System.Drawing.Color.YellowGreen;
            this.tDataGridView2.IsDrawMerger = true;
            this.tDataGridView2.IsDrawMove = false;
            this.tDataGridView2.Location = new System.Drawing.Point(1, 0);
            this.tDataGridView2.Name = "tDataGridView2";
            this.tDataGridView2.RowHeadersVisible = false;
            this.tDataGridView2.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.tDataGridView2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Peru;
            this.tDataGridView2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.tDataGridView2.RowTemplate.Height = 23;
            this.tDataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tDataGridView2.Size = new System.Drawing.Size(435, 219);
            this.tDataGridView2.TabIndex = 9;
            // 
            // ProColor
            // 
            this.ProColor.DataPropertyName = "ProColor";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ProColor.DefaultCellStyle = dataGridViewCellStyle2;
            this.ProColor.FillWeight = 35F;
            this.ProColor.HeaderText = "颜色";
            this.ProColor.MinimumWidth = 35;
            this.ProColor.Name = "ProColor";
            this.ProColor.ReadOnly = true;
            this.ProColor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ProSize
            // 
            this.ProSize.DataPropertyName = "ProSize";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ProSize.DefaultCellStyle = dataGridViewCellStyle3;
            this.ProSize.FillWeight = 35F;
            this.ProSize.HeaderText = "尺码";
            this.ProSize.MinimumWidth = 35;
            this.ProSize.Name = "ProSize";
            this.ProSize.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.FillWeight = 35F;
            this.Price.HeaderText = "价格";
            this.Price.MinimumWidth = 35;
            this.Price.Name = "Price";
            this.Price.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "up.png");
            this.imageList1.Images.SetKeyName(1, "warn.png");
            this.imageList1.Images.SetKeyName(2, "error.png");
            this.imageList1.Images.SetKeyName(3, "finished.png");
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.Controls.Add(this.tDataGridViewPager1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 159);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.panel2.Size = new System.Drawing.Size(437, 157);
            this.panel2.TabIndex = 9;
            // 
            // tDataGridViewPager1
            // 
            this.tDataGridViewPager1.CurrenetPageIndex = 1;
            this.tDataGridViewPager1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.tDataGridViewPager1.Edit.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tDataGridViewPager1.Edit.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.tDataGridViewPager1.Edit.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tDataGridViewPager1.Edit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.tDataGridViewPager1.Edit.ColumnHeadersHeight = 30;
            this.tDataGridViewPager1.Edit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tDataGridViewPager1.Edit.DefaultCellStyle = dataGridViewCellStyle5;
            this.tDataGridViewPager1.Edit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tDataGridViewPager1.Edit.GridColor = System.Drawing.Color.LightBlue;
            this.tDataGridViewPager1.Edit.Location = new System.Drawing.Point(0, 0);
            this.tDataGridViewPager1.Edit.MultiSelect = false;
            this.tDataGridViewPager1.Edit.Name = "tDataGridView1";
            this.tDataGridViewPager1.Edit.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tDataGridViewPager1.Edit.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.tDataGridViewPager1.Edit.RowHeadersVisible = false;
            this.tDataGridViewPager1.Edit.RowHeadersWidth = 21;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.tDataGridViewPager1.Edit.RowTemplate.Height = 32;
            this.tDataGridViewPager1.Edit.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tDataGridViewPager1.Edit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tDataGridViewPager1.Edit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tDataGridViewPager1.Edit.Size = new System.Drawing.Size(437, 126);
            this.tDataGridViewPager1.Edit.TabIndex = 12;
            this.tDataGridViewPager1.Location = new System.Drawing.Point(0, 1);
            this.tDataGridViewPager1.Name = "tDataGridViewPager1";
            this.tDataGridViewPager1.Size = new System.Drawing.Size(437, 156);
            this.tDataGridViewPager1.TabIndex = 0;
            // 
            // 
            // 
            this.tDataGridViewPager1.TPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.tDataGridViewPager1.TPager.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tDataGridViewPager1.TPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tDataGridViewPager1.TPager.Location = new System.Drawing.Point(0, 126);
            this.tDataGridViewPager1.TPager.Name = "pager1";
            this.tDataGridViewPager1.TPager.Size = new System.Drawing.Size(437, 30);
            this.tDataGridViewPager1.TPager.TabIndex = 11;
            // 
            // FormGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(459, 574);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormGridView";
            this.Text = "DemoGridView";
            this.TextShow = "hello,world";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView2)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridViewPager1.Edit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.TDataGridView tDataGridView1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn CommandType;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameStr;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActionTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComponentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.ImageList imageList1;
        private Forms.TDataGridView tDataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.Panel panel2;
        private Forms.TDataGridViewPager tDataGridViewPager1;

    }
}