namespace Paway.Test
{
    partial class FormEmit
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
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            this.tControl1 = new Paway.Forms.TControl();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.gridview1 = new Paway.Forms.TDataGridViewPager();
            this.tControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridview1.Edit)).BeginInit();
            this.SuspendLayout();
            // 
            // tControl1
            // 
            this.tControl1.Controls.Add(this.toolBar1);
            this.tControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tControl1.Location = new System.Drawing.Point(1, 26);
            this.tControl1.Name = "tControl1";
            this.tControl1.Size = new System.Drawing.Size(675, 32);
            this.tControl1.TabIndex = 67;
            // 
            // toolBar1
            // 
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolBar1.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolBar1.IClickEvent = true;
            this.toolBar1.ImageSize = new System.Drawing.Size(0, 0);
            toolItem2.Text = "hello";
            this.toolBar1.Items.Add(toolItem2);
            this.toolBar1.ItemSize = new System.Drawing.Size(64, 32);
            this.toolBar1.ItemSpace = 5;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(64, 32);
            this.toolBar1.TabIndex = 70;
            this.toolBar1.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolBar1.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolBar1.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(239)))));
            this.toolBar1.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolBar1.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolBar1.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolBar1.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolBar1.TLineColor.ColorDown = System.Drawing.Color.Transparent;
            this.toolBar1.TLineColor.ColorMove = System.Drawing.Color.Transparent;
            this.toolBar1.TLineColor.ColorNormal = System.Drawing.Color.Transparent;
            this.toolBar1.Trans = 150;
            // 
            // gridview1
            // 
            this.gridview1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.gridview1.Edit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridview1.Edit.Location = new System.Drawing.Point(0, 0);
            this.gridview1.Edit.Name = "gridview1";
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.gridview1.Edit.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridview1.Edit.RowTemplate.Height = 32;
            this.gridview1.Edit.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridview1.Edit.Size = new System.Drawing.Size(675, 278);
            this.gridview1.Edit.TabIndex = 12;
            this.gridview1.Location = new System.Drawing.Point(1, 58);
            this.gridview1.Name = "gridview1";
            this.gridview1.Size = new System.Drawing.Size(675, 308);
            this.gridview1.TabIndex = 68;
            // 
            // 
            // 
            this.gridview1.TPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.gridview1.TPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridview1.TPager.Location = new System.Drawing.Point(0, 278);
            this.gridview1.TPager.Name = "pager1";
            this.gridview1.TPager.Size = new System.Drawing.Size(675, 30);
            this.gridview1.TPager.TabIndex = 11;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 367);
            this.Controls.Add(this.gridview1);
            this.Controls.Add(this.tControl1);
            this.Name = "Form5";
            this.Text = "Form5";
            this.tControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridview1.Edit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.TControl tControl1;
        private Forms.ToolBar toolBar1;
        private Forms.TDataGridViewPager gridview1;
    }
}