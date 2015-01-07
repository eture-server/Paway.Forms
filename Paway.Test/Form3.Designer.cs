namespace Paway.Test
{
    partial class Form3
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
            this.tComboBox1 = new Paway.Forms.TComboBox();
            this.tComboBox2 = new Paway.Forms.TComboBox();
            this.tComboBoxBase1 = new Paway.Forms.TComboBoxBase();
            this.SuspendLayout();
            // 
            // tComboBox1
            // 
            // 
            // 
            // 
            this.tComboBox1.Edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.tComboBox1.Edit.DropDownHeight = 10;
            this.tComboBox1.Edit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.tComboBox1.Edit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tComboBox1.Edit.ForeColor = System.Drawing.Color.Black;
            this.tComboBox1.Edit.IntegralHeight = false;
            this.tComboBox1.Edit.Items.AddRange(new object[] {
            "11",
            "22",
            "33",
            "44",
            "55"});
            this.tComboBox1.Edit.Location = new System.Drawing.Point(1, 1);
            this.tComboBox1.Edit.Name = "tComboBox1";
            this.tComboBox1.Edit.Size = new System.Drawing.Size(121, 150);
            this.tComboBox1.Edit.TabIndex = 0;
            this.tComboBox1.Location = new System.Drawing.Point(87, 102);
            this.tComboBox1.Name = "tComboBox1";
            this.tComboBox1.Size = new System.Drawing.Size(123, 27);
            this.tComboBox1.TabIndex = 0;
            // 
            // tComboBox2
            // 
            // 
            // 
            // 
            this.tComboBox2.Edit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tComboBox2.Edit.ForeColor = System.Drawing.Color.Black;
            this.tComboBox2.Edit.IntegralHeight = false;
            this.tComboBox2.Edit.Location = new System.Drawing.Point(1, 1);
            this.tComboBox2.Edit.Name = "tComboBox1";
            this.tComboBox2.Edit.TabIndex = 0;
            this.tComboBox2.Location = new System.Drawing.Point(114, 152);
            this.tComboBox2.Name = "tComboBox2";
            this.tComboBox2.Size = new System.Drawing.Size(123, 25);
            this.tComboBox2.TabIndex = 1;
            // 
            // tComboBoxBase1
            // 
            this.tComboBoxBase1.IntegralHeight = false;
            this.tComboBoxBase1.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G"});
            this.tComboBoxBase1.Location = new System.Drawing.Point(114, 193);
            this.tComboBoxBase1.Name = "tComboBoxBase1";
            this.tComboBoxBase1.Size = new System.Drawing.Size(121, 23);
            this.tComboBoxBase1.TabIndex = 2;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 318);
            this.Controls.Add(this.tComboBoxBase1);
            this.Controls.Add(this.tComboBox2);
            this.Controls.Add(this.tComboBox1);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Form3";
            this.Text = "Form3";
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.TComboBox tComboBox1;
        private Forms.TComboBox tComboBox2;
        private Forms.TComboBoxBase tComboBoxBase1;
    }
}