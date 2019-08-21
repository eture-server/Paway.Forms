namespace Paway.Test
{
    partial class BaseForm
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
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.ClientSize = new System.Drawing.Size(443, 299);
            this.Name = "BaseForm";
            this.TBrush.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(84)))), ((int)(((byte)(74)))));
            this.TBrush.ColorDown = System.Drawing.Color.Ivory;
            this.Text = "BaseForm1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}