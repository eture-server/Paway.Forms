using System.Windows.Forms;

namespace Paway.Test
{
    partial class FormSql
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
            this.btInsert = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btUpdate = new System.Windows.Forms.Button();
            this.btUpOrIn = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.btSelect = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbStart = new Paway.Forms.TNumTestBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbStart);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Controls.Add(this.btSelect);
            this.panel1.Controls.Add(this.btnReplace);
            this.panel1.Controls.Add(this.btDelete);
            this.panel1.Controls.Add(this.btUpOrIn);
            this.panel1.Controls.Add(this.btUpdate);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btInsert);
            this.panel1.Size = new System.Drawing.Size(405, 290);
            // 
            // lbTitle
            // 
            this.lbTitle.Location = new System.Drawing.Point(178, 31);
            // 
            // btInsert
            // 
            this.btInsert.Location = new System.Drawing.Point(25, 104);
            this.btInsert.Name = "btInsert";
            this.btInsert.Size = new System.Drawing.Size(75, 28);
            this.btInsert.TabIndex = 12;
            this.btInsert.Text = "Insert";
            this.btInsert.UseVisualStyleBackColor = false;
            this.btInsert.Click += new System.EventHandler(this.BtInsert_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Paway.Test.Properties.Resources.offline;
            this.pictureBox1.Location = new System.Drawing.Point(25, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 56);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // btUpdate
            // 
            this.btUpdate.Location = new System.Drawing.Point(102, 104);
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.Size = new System.Drawing.Size(75, 28);
            this.btUpdate.TabIndex = 14;
            this.btUpdate.Text = "Update";
            this.btUpdate.UseVisualStyleBackColor = false;
            this.btUpdate.Click += new System.EventHandler(this.BtUpdate_Click);
            // 
            // btUpOrIn
            // 
            this.btUpOrIn.Location = new System.Drawing.Point(183, 104);
            this.btUpOrIn.Name = "btUpOrIn";
            this.btUpOrIn.Size = new System.Drawing.Size(75, 28);
            this.btUpOrIn.TabIndex = 15;
            this.btUpOrIn.Text = "UpOrIn";
            this.btUpOrIn.UseVisualStyleBackColor = false;
            this.btUpOrIn.Click += new System.EventHandler(this.BtUpOrIn_Click);
            // 
            // btDelete
            // 
            this.btDelete.Location = new System.Drawing.Point(265, 104);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(75, 28);
            this.btDelete.TabIndex = 16;
            this.btDelete.Text = "Delete";
            this.btDelete.UseVisualStyleBackColor = false;
            this.btDelete.Click += new System.EventHandler(this.BtDelete_Click);
            // 
            // btSelect
            // 
            this.btSelect.Location = new System.Drawing.Point(25, 162);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(75, 28);
            this.btSelect.TabIndex = 17;
            this.btSelect.Text = "Select";
            this.btSelect.UseVisualStyleBackColor = false;
            this.btSelect.Click += new System.EventHandler(this.BtSelect_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(102, 162);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(75, 28);
            this.btnReplace.TabIndex = 16;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = false;
            this.btnReplace.Click += new System.EventHandler(this.BtnReplace_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(102, 206);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 28);
            this.btnStart.TabIndex = 17;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtSelect_Click);
            // 
            // tbStart
            // 
            this.tbStart.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbStart.Lines = new string[] {
        "1000"};
            this.tbStart.Location = new System.Drawing.Point(25, 208);
            this.tbStart.MaxLength = 10;
            this.tbStart.Name = "tbStart";
            this.tbStart.Size = new System.Drawing.Size(75, 24);
            this.tbStart.TabIndex = 38;
            this.tbStart.Text = "1000";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(183, 162);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 28);
            this.btnTest.TabIndex = 17;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = false;
            // 
            // FormSql
            // 
            this.ClientSize = new System.Drawing.Size(407, 372);
            this.Name = "FormSql";
            this.ShowInTaskbar = true;
            this.Text = "Sql测试";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btUpdate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Button btInsert;
        private Button btUpOrIn;
        private Button btDelete;
        private Button btSelect;
        private Button btnReplace;
        private Button btnStart;
        private Forms.TNumTestBox tbStart;
        private Button btnTest;
    }
}