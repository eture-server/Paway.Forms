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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridView));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel3 = new Paway.Forms.TControl();
            this.tDataGridView2 = new Paway.Forms.TDataGridView();
            this.ProColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new Paway.Forms.TControl();
            this.tDataGridViewPager1 = new Paway.Forms.TDataGridViewPager();
            this.tDataGridView1 = new Paway.Forms.TDataGridView();
            this.IsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CommandType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.NameStr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComponentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridViewPager1.Edit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.tDataGridView1);
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 2);
            this.panel1.Size = new System.Drawing.Size(465, 546);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.panel3.Controls.Add(this.tDataGridView2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(10, 355);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(3);
            this.panel3.Size = new System.Drawing.Size(445, 189);
            this.panel3.TabIndex = 10;
            // 
            // tDataGridView2
            // 
            this.tDataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.tDataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.tDataGridView2.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tDataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tDataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProColor,
            this.ProSize,
            this.Price});
            this.tDataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tDataGridView2.GridColor = System.Drawing.Color.YellowGreen;
            this.tDataGridView2.IMerger = true;
            this.tDataGridView2.IMove = false;
            this.tDataGridView2.IMoveColor = System.Drawing.Color.Empty;
            this.tDataGridView2.Location = new System.Drawing.Point(3, 3);
            this.tDataGridView2.MultiSelect = true;
            this.tDataGridView2.Name = "tDataGridView2";
            this.tDataGridView2.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.tDataGridView2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Peru;
            this.tDataGridView2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.tDataGridView2.RowTemplate.Height = 23;
            this.tDataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tDataGridView2.Size = new System.Drawing.Size(439, 183);
            this.tDataGridView2.TabIndex = 12;
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
            this.ProColor.Width = 146;
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
            this.ProSize.Width = 146;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.FillWeight = 35F;
            this.Price.HeaderText = "价格";
            this.Price.MinimumWidth = 35;
            this.Price.Name = "Price";
            this.Price.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Price.Width = 146;
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
            this.panel2.Controls.Add(this.tDataGridViewPager1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 181);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(445, 174);
            this.panel2.TabIndex = 9;
            // 
            // tDataGridViewPager1
            // 
            this.tDataGridViewPager1.CurrenetPageIndex = 1;
            this.tDataGridViewPager1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tDataGridViewPager1.Edit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
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
            this.tDataGridViewPager1.Edit.Name = "tDataGridView1";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tDataGridViewPager1.Edit.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.tDataGridViewPager1.Edit.RowHeadersVisible = true;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.tDataGridViewPager1.Edit.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.tDataGridViewPager1.Edit.RowTemplate.Height = 32;
            this.tDataGridViewPager1.Edit.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tDataGridViewPager1.Edit.Size = new System.Drawing.Size(439, 138);
            this.tDataGridViewPager1.Edit.TabIndex = 12;
            this.tDataGridViewPager1.Location = new System.Drawing.Point(3, 3);
            this.tDataGridViewPager1.Name = "tDataGridViewPager1";
            this.tDataGridViewPager1.Size = new System.Drawing.Size(439, 168);
            this.tDataGridViewPager1.TabIndex = 1;
            // 
            // 
            // 
            this.tDataGridViewPager1.TPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.tDataGridViewPager1.TPager.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tDataGridViewPager1.TPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tDataGridViewPager1.TPager.Location = new System.Drawing.Point(0, 138);
            this.tDataGridViewPager1.TPager.Name = "pager1";
            this.tDataGridViewPager1.TPager.Size = new System.Drawing.Size(439, 30);
            this.tDataGridViewPager1.TPager.TabIndex = 11;
            // 
            // tDataGridView1
            // 
            this.tDataGridView1.AllowUserToAddRows = true;
            this.tDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.tDataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.tDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.tDataGridView1.ColumnHeadersHeight = 23;
            this.tDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsSelect,
            this.CommandType,
            this.Image,
            this.NameStr,
            this.ActionTime,
            this.ComponentId,
            this.Id});
            this.tDataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(186)))), ((int)(((byte)(132)))));
            this.tDataGridView1.ICheckBoxName = "IsSelect";
            this.tDataGridView1.IMove = false;
            this.tDataGridView1.IMoveColor = System.Drawing.Color.Empty;
            this.tDataGridView1.IMultiText = true;
            this.tDataGridView1.Location = new System.Drawing.Point(10, 10);
            this.tDataGridView1.MultiSelect = true;
            this.tDataGridView1.Name = "tDataGridView1";
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.tDataGridView1.RowHeadersVisible = true;
            this.tDataGridView1.RowHeadersWidth = 41;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            this.tDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.tDataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            this.tDataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Thistle;
            this.tDataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Lime;
            this.tDataGridView1.RowTemplate.Height = 32;
            this.tDataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tDataGridView1.Size = new System.Drawing.Size(445, 171);
            this.tDataGridView1.TabIndex = 8;
            this.tDataGridView1.TColumnImage = "Image";
            this.tDataGridView1.TColumnText = "NameStr";
            this.tDataGridView1.TProgressImage = global::Paway.Test.Properties.Resources.process;
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
            // FormGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(467, 574);
            this.Name = "FormGridView";
            this.Text = "DemoGridView";
            this.TextShow = "hello,world";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView2)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridViewPager1.Edit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Paway.Forms.TControl panel3;
        private System.Windows.Forms.ImageList imageList1;
        private Paway.Forms.TControl panel2;
        private Forms.TDataGridViewPager tDataGridViewPager1;
        private Forms.TDataGridView tDataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn CommandType;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameStr;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActionTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComponentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private Forms.TDataGridView tDataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;

    }
}