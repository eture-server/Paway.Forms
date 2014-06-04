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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSql));
            this.btInsert = new Paway.Forms.QQButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btUpdate = new Paway.Forms.QQButton();
            this.btUpOrIn = new Paway.Forms.QQButton();
            this.btDelete = new Paway.Forms.QQButton();
            this.btSelect = new Paway.Forms.QQButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btSelect);
            this.panel1.Controls.Add(this.btDelete);
            this.panel1.Controls.Add(this.btUpOrIn);
            this.panel1.Controls.Add(this.btUpdate);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btInsert);
            this.panel1.Size = new System.Drawing.Size(407, 298);
            // 
            // btInsert
            // 
            this.btInsert.DownImage = ((System.Drawing.Image)(resources.GetObject("btInsert.DownImage")));
            this.btInsert.ForeColor = System.Drawing.Color.Black;
            this.btInsert.Image = null;
            this.btInsert.Location = new System.Drawing.Point(25, 143);
            this.btInsert.MoveImage = ((System.Drawing.Image)(resources.GetObject("btInsert.MoveImage")));
            this.btInsert.Name = "btInsert";
            this.btInsert.NormalImage = ((System.Drawing.Image)(resources.GetObject("btInsert.NormalImage")));
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
            this.btUpdate.DownImage = ((System.Drawing.Image)(resources.GetObject("btUpdate.DownImage")));
            this.btUpdate.ForeColor = System.Drawing.Color.Black;
            this.btUpdate.Image = null;
            this.btUpdate.Location = new System.Drawing.Point(102, 143);
            this.btUpdate.MoveImage = ((System.Drawing.Image)(resources.GetObject("btUpdate.MoveImage")));
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.NormalImage = ((System.Drawing.Image)(resources.GetObject("btUpdate.NormalImage")));
            this.btUpdate.Size = new System.Drawing.Size(59, 28);
            this.btUpdate.TabIndex = 14;
            this.btUpdate.Text = "Update";
            this.btUpdate.UseVisualStyleBackColor = false;
            this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
            // 
            // btUpOrIn
            // 
            this.btUpOrIn.DownImage = ((System.Drawing.Image)(resources.GetObject("btUpOrIn.DownImage")));
            this.btUpOrIn.ForeColor = System.Drawing.Color.Black;
            this.btUpOrIn.Image = null;
            this.btUpOrIn.Location = new System.Drawing.Point(183, 143);
            this.btUpOrIn.MoveImage = ((System.Drawing.Image)(resources.GetObject("btUpOrIn.MoveImage")));
            this.btUpOrIn.Name = "btUpOrIn";
            this.btUpOrIn.NormalImage = ((System.Drawing.Image)(resources.GetObject("btUpOrIn.NormalImage")));
            this.btUpOrIn.Size = new System.Drawing.Size(59, 28);
            this.btUpOrIn.TabIndex = 15;
            this.btUpOrIn.Text = "UpOrIn";
            this.btUpOrIn.UseVisualStyleBackColor = false;
            this.btUpOrIn.Click += new System.EventHandler(this.btUpOrIn_Click);
            // 
            // btDelete
            // 
            this.btDelete.DownImage = ((System.Drawing.Image)(resources.GetObject("btDelete.DownImage")));
            this.btDelete.ForeColor = System.Drawing.Color.Black;
            this.btDelete.Image = null;
            this.btDelete.Location = new System.Drawing.Point(265, 143);
            this.btDelete.MoveImage = ((System.Drawing.Image)(resources.GetObject("btDelete.MoveImage")));
            this.btDelete.Name = "btDelete";
            this.btDelete.NormalImage = ((System.Drawing.Image)(resources.GetObject("btDelete.NormalImage")));
            this.btDelete.Size = new System.Drawing.Size(59, 28);
            this.btDelete.TabIndex = 16;
            this.btDelete.Text = "Delete";
            this.btDelete.UseVisualStyleBackColor = false;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btSelect
            // 
            this.btSelect.DownImage = ((System.Drawing.Image)(resources.GetObject("btSelect.DownImage")));
            this.btSelect.ForeColor = System.Drawing.Color.Black;
            this.btSelect.Image = null;
            this.btSelect.Location = new System.Drawing.Point(25, 201);
            this.btSelect.MoveImage = ((System.Drawing.Image)(resources.GetObject("btSelect.MoveImage")));
            this.btSelect.Name = "btSelect";
            this.btSelect.NormalImage = ((System.Drawing.Image)(resources.GetObject("btSelect.NormalImage")));
            this.btSelect.Size = new System.Drawing.Size(59, 28);
            this.btSelect.TabIndex = 17;
            this.btSelect.Text = "Select";
            this.btSelect.UseVisualStyleBackColor = false;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // FormSql
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 324);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormSql";
            this.Text = "Sql测试";
            this.TextShow = "Form1";
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




    }
}