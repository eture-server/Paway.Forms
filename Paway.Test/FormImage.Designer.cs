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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btChange = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btSave = new System.Windows.Forms.Button();
            this.btClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btColor = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btAdd = new System.Windows.Forms.Button();
            this.btLess = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btLess);
            this.panel1.Controls.Add(this.btAdd);
            this.panel1.Controls.Add(this.btChange);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Controls.Add(this.btClear);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btColor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(253, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(114, 296);
            this.panel1.TabIndex = 49;
            // 
            // btChange
            // 
            this.btChange.Location = new System.Drawing.Point(3, 199);
            this.btChange.Name = "btChange";
            this.btChange.Size = new System.Drawing.Size(60, 23);
            this.btChange.TabIndex = 55;
            this.btChange.Text = "换色";
            this.btChange.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 54;
            this.label2.Text = "透明度";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(49, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(59, 21);
            this.textBox1.TabIndex = 53;
            this.textBox1.Text = "230";
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(4, 241);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(60, 23);
            this.btSave.TabIndex = 52;
            this.btSave.Text = "保存";
            this.btSave.UseVisualStyleBackColor = true;
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(4, 270);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(60, 23);
            this.btClear.TabIndex = 51;
            this.btClear.Text = "清除";
            this.btClear.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 50;
            this.label1.Text = "label1";
            // 
            // btColor
            // 
            this.btColor.Location = new System.Drawing.Point(4, 104);
            this.btColor.Name = "btColor";
            this.btColor.Size = new System.Drawing.Size(60, 23);
            this.btColor.TabIndex = 49;
            this.btColor.Text = "取色";
            this.btColor.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(253, 296);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 50;
            this.pictureBox1.TabStop = false;
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(4, 157);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(39, 23);
            this.btAdd.TabIndex = 56;
            this.btAdd.Text = "+";
            this.btAdd.UseVisualStyleBackColor = true;
            // 
            // btLess
            // 
            this.btLess.Location = new System.Drawing.Point(49, 157);
            this.btLess.Name = "btLess";
            this.btLess.Size = new System.Drawing.Size(39, 23);
            this.btLess.TabIndex = 57;
            this.btLess.Text = "-";
            this.btLess.UseVisualStyleBackColor = true;
            // 
            // FormImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 296);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Name = "FormImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormImage";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btColor;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btChange;
        private System.Windows.Forms.Button btLess;
        private System.Windows.Forms.Button btAdd;
    }
}