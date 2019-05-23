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
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            this.tProgressBar1 = new Paway.Forms.TProgressBar();
            this.btnReset = new Paway.Forms.ToolBar();
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
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 15F);
            this.btnReset.IClickEvent = true;
            this.btnReset.TLineColor.ColorDown = System.Drawing.Color.Transparent;
            this.btnReset.TLineColor.ColorMove = System.Drawing.Color.Transparent;
            this.btnReset.TLineColor.ColorNormal = System.Drawing.Color.Transparent;
            this.btnReset.ImageSize = new System.Drawing.Size(0, 0);
            toolItem1.Text = "Reset";
            this.btnReset.Items.Add(toolItem1);
            this.btnReset.ItemSize = new System.Drawing.Size(75, 32);
            this.btnReset.ItemSpace = 5;
            this.btnReset.Location = new System.Drawing.Point(191, 41);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 32);
            this.btnReset.TabIndex = 67;
            this.btnReset.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.btnReset.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.btnReset.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(239)))));
            this.btnReset.TextFirst.ColorDown = System.Drawing.Color.White;
            this.btnReset.TextFirst.ColorMove = System.Drawing.Color.White;
            this.btnReset.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.btnReset.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.btnReset.Trans = 150;
            // 
            // Form4
            // 
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.tProgressBar1);
            this.Name = "Form4";
            this.Text = "Form4";
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.TProgressBar tProgressBar1;
        private Forms.ToolBar btnReset;
    }
}