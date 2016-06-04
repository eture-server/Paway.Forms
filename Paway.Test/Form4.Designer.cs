namespace Paway.Test
{
    partial class Form4
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
            this.tProgressBar1 = new Paway.Forms.TControls.TProgressBar();
            this.btnReset = new Paway.Forms.QQButton();
            this.SuspendLayout();
            // 
            // tProgressBar1
            // 
            this.tProgressBar1.ForeColor = System.Drawing.Color.LightGray;
            this.tProgressBar1.Location = new System.Drawing.Point(69, 79);
            this.tProgressBar1.Name = "tProgressBar1";
            this.tProgressBar1.Size = new System.Drawing.Size(169, 169);
            this.tProgressBar1.TabIndex = 0;
            this.tProgressBar1.TFont = new System.Drawing.Font("微软雅黑", 32F);
            this.tProgressBar1.Trans = 200;
            this.tProgressBar1.TWidth = 3;
            this.tProgressBar1.Value = 10;
            // 
            // btnReset
            // 
            this.btnReset.Image = null;
            this.btnReset.Location = new System.Drawing.Point(186, 29);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(73, 28);
            this.btnReset.TabIndex = 46;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.tProgressBar1);
            this.IShadow = false;
            this.Name = "Form4";
            this.Text = "Form4";
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.TControls.TProgressBar tProgressBar1;
        private Forms.QQButton btnReset;
    }
}