namespace Paway.Test
{
    partial class FormImage
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tPictureBox1 = new Paway.Forms.TPictureBox();
            this.btLess = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.btChange = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btSave = new System.Windows.Forms.Button();
            this.btnBrush = new System.Windows.Forms.Button();
            this.btClear = new System.Windows.Forms.Button();
            this.lbColor = new System.Windows.Forms.Label();
            this.btColor = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolAuto = new System.Windows.Forms.ToolStripMenuItem();
            this.toolReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tPictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tPictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.btLess);
            this.splitContainer1.Panel2.Controls.Add(this.btAdd);
            this.splitContainer1.Panel2.Controls.Add(this.btChange);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Panel2.Controls.Add(this.btSave);
            this.splitContainer1.Panel2.Controls.Add(this.btnBrush);
            this.splitContainer1.Panel2.Controls.Add(this.btClear);
            this.splitContainer1.Panel2.Controls.Add(this.lbColor);
            this.splitContainer1.Panel2.Controls.Add(this.btColor);
            this.splitContainer1.Size = new System.Drawing.Size(554, 371);
            this.splitContainer1.SplitterDistance = 411;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 50;
            // 
            // tPictureBox1
            // 
            this.tPictureBox1.BackColor = System.Drawing.Color.Black;
            this.tPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tPictureBox1.Image = null;
            this.tPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.tPictureBox1.Name = "tPictureBox1";
            this.tPictureBox1.Size = new System.Drawing.Size(411, 371);
            this.tPictureBox1.TabIndex = 55;
            this.tPictureBox1.TabStop = false;
            // 
            // btLess
            // 
            this.btLess.Location = new System.Drawing.Point(60, 144);
            this.btLess.Name = "btLess";
            this.btLess.Size = new System.Drawing.Size(39, 23);
            this.btLess.TabIndex = 66;
            this.btLess.Text = "-";
            this.btLess.UseVisualStyleBackColor = true;
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(15, 144);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(39, 23);
            this.btAdd.TabIndex = 65;
            this.btAdd.Text = "+";
            this.btAdd.UseVisualStyleBackColor = true;
            // 
            // btChange
            // 
            this.btChange.Location = new System.Drawing.Point(14, 186);
            this.btChange.Name = "btChange";
            this.btChange.Size = new System.Drawing.Size(60, 23);
            this.btChange.TabIndex = 64;
            this.btChange.Text = "换色";
            this.btChange.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 63;
            this.label2.Text = "透明度";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(60, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(59, 21);
            this.textBox1.TabIndex = 62;
            this.textBox1.Text = "0";
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(15, 228);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(60, 23);
            this.btSave.TabIndex = 61;
            this.btSave.Text = "保存";
            this.btSave.UseVisualStyleBackColor = true;
            // 
            // btnBrush
            // 
            this.btnBrush.Location = new System.Drawing.Point(15, 286);
            this.btnBrush.Name = "btnBrush";
            this.btnBrush.Size = new System.Drawing.Size(73, 23);
            this.btnBrush.TabIndex = 60;
            this.btnBrush.Text = "渐变透明";
            this.btnBrush.UseVisualStyleBackColor = true;
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(15, 257);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(60, 23);
            this.btClear.TabIndex = 60;
            this.btClear.Text = "清除";
            this.btClear.UseVisualStyleBackColor = true;
            // 
            // lbColor
            // 
            this.lbColor.Location = new System.Drawing.Point(78, 91);
            this.lbColor.Name = "lbColor";
            this.lbColor.Size = new System.Drawing.Size(21, 23);
            this.lbColor.TabIndex = 59;
            // 
            // btColor
            // 
            this.btColor.Location = new System.Drawing.Point(15, 91);
            this.btColor.Name = "btColor";
            this.btColor.Size = new System.Drawing.Size(60, 23);
            this.btColor.TabIndex = 58;
            this.btColor.Text = "取色";
            this.btColor.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAuto,
            this.toolReset,
            this.toolStripSeparator1,
            this.toolSave});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 98);
            // 
            // toolAuto
            // 
            this.toolAuto.Name = "toolAuto";
            this.toolAuto.Size = new System.Drawing.Size(152, 22);
            this.toolAuto.Text = "自动";
            // 
            // toolReset
            // 
            this.toolReset.Name = "toolReset";
            this.toolReset.Size = new System.Drawing.Size(152, 22);
            this.toolReset.Text = "重置";
            // 
            // toolSave
            // 
            this.toolSave.Name = "toolSave";
            this.toolSave.Size = new System.Drawing.Size(152, 22);
            this.toolSave.Text = "保存";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // FormImage
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(188)))));
            this.ClientSize = new System.Drawing.Size(554, 371);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormImage";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tPictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Forms.TPictureBox tPictureBox1;
        private System.Windows.Forms.Button btLess;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btChange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.Label lbColor;
        private System.Windows.Forms.Button btColor;
        private System.Windows.Forms.Button btnBrush;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolAuto;
        private System.Windows.Forms.ToolStripMenuItem toolReset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolSave;
    }
}