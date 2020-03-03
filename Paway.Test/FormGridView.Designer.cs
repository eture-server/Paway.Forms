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
            this.button1 = new System.Windows.Forms.Button();
            this.tControl1 = new Paway.Forms.TControl();
            this.gridview3 = new Paway.Forms.TreeGridView();
            this.tControl3 = new Paway.Forms.TControl();
            this.gridview2 = new Paway.Forms.TDataGridViewPager();
            this.tControl2 = new Paway.Forms.TControl();
            this.gridview1 = new Paway.Forms.TDataGridView();
            this.tControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridview3)).BeginInit();
            this.tControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridview2.Edit)).BeginInit();
            this.tControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridview1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(743, 36);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tControl1
            // 
            this.tControl1.Controls.Add(this.gridview3);
            this.tControl1.Controls.Add(this.tControl3);
            this.tControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tControl1.Location = new System.Drawing.Point(0, 36);
            this.tControl1.Name = "tControl1";
            this.tControl1.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.tControl1.Size = new System.Drawing.Size(743, 369);
            this.tControl1.TabIndex = 8;
            // 
            // gridview3
            // 
            this.gridview3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridview3.GridColor = System.Drawing.SystemColors.ControlDark;
            this.gridview3.ImageList = null;
            this.gridview3.Location = new System.Drawing.Point(0, 0);
            this.gridview3.Name = "gridview3";
            this.gridview3.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.gridview3.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.gridview3.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridview3.RowTemplate.Height = 32;
            this.gridview3.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridview3.Size = new System.Drawing.Size(738, 150);
            this.gridview3.TabIndex = 9;
            this.gridview3.TextColumn = null;
            // 
            // tControl3
            // 
            this.tControl3.Controls.Add(this.gridview2);
            this.tControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tControl3.Location = new System.Drawing.Point(0, 150);
            this.tControl3.Name = "tControl3";
            this.tControl3.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.tControl3.Size = new System.Drawing.Size(738, 219);
            this.tControl3.TabIndex = 8;
            // 
            // gridview2
            // 
            this.gridview2.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.gridview2.Edit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridview2.Edit.Location = new System.Drawing.Point(0, 0);
            this.gridview2.Edit.Name = "gridview1";
            this.gridview2.Edit.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.gridview2.Edit.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.gridview2.Edit.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridview2.Edit.RowTemplate.Height = 32;
            this.gridview2.Edit.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridview2.Edit.Size = new System.Drawing.Size(738, 184);
            this.gridview2.Edit.TabIndex = 12;
            this.gridview2.Location = new System.Drawing.Point(0, 5);
            this.gridview2.Name = "gridview2";
            this.gridview2.Size = new System.Drawing.Size(738, 214);
            this.gridview2.TabIndex = 0;
            // 
            // 
            // 
            this.gridview2.TPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.gridview2.TPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridview2.TPager.Location = new System.Drawing.Point(0, 184);
            this.gridview2.TPager.Name = "pager1";
            this.gridview2.TPager.Size = new System.Drawing.Size(738, 30);
            this.gridview2.TPager.TabIndex = 11;
            // 
            // tControl2
            // 
            this.tControl2.Controls.Add(this.gridview1);
            this.tControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tControl2.Location = new System.Drawing.Point(0, 405);
            this.tControl2.Name = "tControl2";
            this.tControl2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.tControl2.Size = new System.Drawing.Size(743, 167);
            this.tControl2.TabIndex = 7;
            // 
            // gridview1
            // 
            this.gridview1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridview1.GridColor = System.Drawing.SystemColors.ControlDark;
            this.gridview1.Location = new System.Drawing.Point(0, 5);
            this.gridview1.Name = "gridview1";
            this.gridview1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.gridview1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.gridview1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridview1.RowTemplate.Height = 32;
            this.gridview1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridview1.Size = new System.Drawing.Size(743, 162);
            this.gridview1.TabIndex = 0;
            // 
            // FormGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(743, 572);
            this.Controls.Add(this.tControl1);
            this.Controls.Add(this.tControl2);
            this.Controls.Add(this.button1);
            this.Name = "FormGridView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormTree";
            this.tControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridview3)).EndInit();
            this.tControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridview2.Edit)).EndInit();
            this.tControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridview1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private Forms.TControl tControl2;
        private Forms.TControl tControl1;
        private Forms.TreeGridView gridview3;
        private Forms.TControl tControl3;
        private Forms.TDataGridViewPager gridview2;
        private Forms.TDataGridView gridview1;
    }
}