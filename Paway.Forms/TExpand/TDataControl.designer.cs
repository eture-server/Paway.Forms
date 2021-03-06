﻿namespace Paway.Forms
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
            if (disposing)
            {
                UnLoadEvent();
                if (components != null) components.Dispose();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.panel3.Font = new System.Drawing.Font("微软雅黑", 11F);
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
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.panel1.Size = new System.Drawing.Size(693, 34);
            this.panel1.TabIndex = 67;
            // 
            // toolBar1
            // 
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolBar1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.toolBar1.IAutoWidth = true;
            this.toolBar1.IClickEvent = true;
            this.toolBar1.IImageShow = true;
            toolItem1.Image = global::Paway.Forms.Properties.Resources.refresh;
            toolItem1.Keys = System.Windows.Forms.Keys.F5;
            toolItem1.Tag = "刷新";
            toolItem1.Text = "刷新";
            toolItem2.Image = global::Paway.Forms.Properties.Resources.add;
            toolItem2.Keys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            toolItem2.Tag = "添加";
            toolItem2.Text = "添加";
            toolItem3.Image = global::Paway.Forms.Properties.Resources.edit;
            toolItem3.Keys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            toolItem3.Tag = "编辑";
            toolItem3.Text = "编辑";
            toolItem4.Image = global::Paway.Forms.Properties.Resources.close;
            toolItem4.Keys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            toolItem4.Tag = "删除";
            toolItem4.Text = "删除";
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.Items.Add(toolItem2);
            this.toolBar1.Items.Add(toolItem3);
            this.toolBar1.Items.Add(toolItem4);
            this.toolBar1.ItemSize = new System.Drawing.Size(0, 32);
            this.toolBar1.Location = new System.Drawing.Point(0, 2);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(515, 32);
            this.toolBar1.TabIndex = 69;
            this.toolBar1.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.toolBar1.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.toolBar1.TextFirst.ColorMove = System.Drawing.Color.Blue;
            this.toolBar1.TScrollHeight = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.tbName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.panel2.Location = new System.Drawing.Point(515, 2);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 3, 5, 0);
            this.panel2.Size = new System.Drawing.Size(178, 32);
            this.panel2.TabIndex = 68;
            this.panel2.Visible = false;
            // 
            // tbName
            // 
            this.tbName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbName.Icon = global::Paway.Forms.Properties.Resources.search;
            this.tbName.IconIsButton = true;
            this.tbName.ITrans = true;
            this.tbName.Lines = new string[0];
            this.tbName.Location = new System.Drawing.Point(0, 3);
            this.tbName.MaxLength = 16;
            this.tbName.Name = "tbName";
            this.tbName.RegexType = Paway.Helper.RegexType.Normal;
            this.tbName.Size = new System.Drawing.Size(173, 29);
            this.tbName.TabIndex = 60;
            this.tbName.WaterText = "Ctrl+F搜索";
            // 
            // gridview1
            // 
            this.gridview1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 11F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridview1.Edit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 11F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridview1.Edit.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridview1.Edit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridview1.Edit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridview1.Edit.Location = new System.Drawing.Point(0, 0);
            this.gridview1.Edit.Name = "tDataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridview1.Edit.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridview1.Edit.RowTemplate.Height = 32;
            this.gridview1.Edit.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridview1.Edit.Size = new System.Drawing.Size(683, 172);
            this.gridview1.Edit.TabIndex = 12;
            this.gridview1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.gridview1.Location = new System.Drawing.Point(0, 34);
            this.gridview1.Name = "gridview1";
            this.gridview1.Size = new System.Drawing.Size(683, 203);
            this.gridview1.TabIndex = 69;
            // 
            // 
            // 
            this.gridview1.TPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.gridview1.TPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridview1.TPager.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.gridview1.TPager.Location = new System.Drawing.Point(0, 172);
            this.gridview1.TPager.Name = "pager1";
            this.gridview1.TPager.Size = new System.Drawing.Size(683, 30);
            this.gridview1.TPager.TabIndex = 11;
            // 
            // TDataControl
            // 
            this.Controls.Add(this.gridview1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.MInterval = 6;
            this.Name = "TDataControl";
            this.Size = new System.Drawing.Size(693, 237);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridview1.Edit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// </summary>
        protected Paway.Forms.TControl panel1;
        /// <summary>
        /// </summary>
        protected Paway.Forms.TDataGridViewPager gridview1;
        /// <summary>
        /// </summary>
        protected Paway.Forms.TControl panel3;
        /// <summary>
        /// </summary>
        protected ToolBar toolBar1;
        /// <summary>
        /// </summary>
        protected Paway.Forms.TControl panel2;
        /// <summary>
        /// </summary>
        protected QQTextBox tbName;
    }
}
