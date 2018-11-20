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
            this.btInsert = new Paway.Forms.QQButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btUpdate = new Paway.Forms.QQButton();
            this.btUpOrIn = new Paway.Forms.QQButton();
            this.btDelete = new Paway.Forms.QQButton();
            this.btSelect = new Paway.Forms.QQButton();
            this.btnReplace = new Paway.Forms.QQButton();
            this.btnStart = new Paway.Forms.QQButton();
            this.tbStart = new Paway.Forms.TNumTestBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbStart);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.btSelect);
            this.panel1.Controls.Add(this.btnReplace);
            this.panel1.Controls.Add(this.btDelete);
            this.panel1.Controls.Add(this.btUpOrIn);
            this.panel1.Controls.Add(this.btUpdate);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btInsert);
            this.panel1.Size = new System.Drawing.Size(405, 296);
            // 
            // btInsert
            // 
            this.btInsert.Location = new System.Drawing.Point(25, 143);
            this.btInsert.Name = "btInsert";
            this.btInsert.Size = new System.Drawing.Size(59, 28);
            this.btInsert.TabIndex = 12;
            this.btInsert.Text = "Insert";
            this.btInsert.UseVisualStyleBackColor = false;
            this.btInsert.Click += new System.EventHandler(this.btInsert_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Paway.Test.Properties.Resources.offline;
            this.pictureBox1.Location = new System.Drawing.Point(25, 47);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 56);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // btUpdate
            // 
            this.btUpdate.Location = new System.Drawing.Point(102, 143);
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.Size = new System.Drawing.Size(59, 28);
            this.btUpdate.TabIndex = 14;
            this.btUpdate.Text = "Update";
            this.btUpdate.UseVisualStyleBackColor = false;
            this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
            // 
            // btUpOrIn
            // 
            this.btUpOrIn.Location = new System.Drawing.Point(183, 143);
            this.btUpOrIn.Name = "btUpOrIn";
            this.btUpOrIn.Size = new System.Drawing.Size(59, 28);
            this.btUpOrIn.TabIndex = 15;
            this.btUpOrIn.Text = "UpOrIn";
            this.btUpOrIn.UseVisualStyleBackColor = false;
            this.btUpOrIn.Click += new System.EventHandler(this.btUpOrIn_Click);
            // 
            // btDelete
            // 
            this.btDelete.Location = new System.Drawing.Point(265, 143);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(59, 28);
            this.btDelete.TabIndex = 16;
            this.btDelete.Text = "Delete";
            this.btDelete.UseVisualStyleBackColor = false;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btSelect
            // 
            this.btSelect.Location = new System.Drawing.Point(25, 201);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(59, 28);
            this.btSelect.TabIndex = 17;
            this.btSelect.Text = "Select";
            this.btSelect.UseVisualStyleBackColor = false;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(102, 201);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(59, 28);
            this.btnReplace.TabIndex = 16;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = false;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(102, 245);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(59, 28);
            this.btnStart.TabIndex = 17;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // tbStart
            // 
            this.tbStart.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbStart.ForeColor = System.Drawing.Color.Black;
            this.tbStart.Lines = new string[] {
        "1000"};
            this.tbStart.Location = new System.Drawing.Point(25, 247);
            this.tbStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbStart.MaxLength = 10;
            this.tbStart.Name = "tbStart";
            this.tbStart.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.tbStart.Size = new System.Drawing.Size(71, 24);
            this.tbStart.TabIndex = 38;
            // 
            // FormSql
            // 
            this.ClientSize = new System.Drawing.Size(407, 324);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormSql";
            this.Text = "Sql测试";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.QQButton btUpdate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Forms.QQButton btInsert;
        private Forms.QQButton btUpOrIn;
        private Forms.QQButton btDelete;
        private Forms.QQButton btSelect;
        private Forms.QQButton btnReplace;
        private Forms.QQButton btnStart;
        private Forms.TNumTestBox tbStart;
    }
}