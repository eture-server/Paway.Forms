using System.ComponentModel;
namespace Paway.Forms
{
    partial class TBaseForm
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
            this.panel1 = new Paway.Forms.TControl();
            this.lbTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(441, 217);
            this.panel1.TabIndex = 1;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbTitle.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.lbTitle.Location = new System.Drawing.Point(197, 32);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(51, 27);
            this.lbTitle.TabIndex = 4;
            this.lbTitle.Text = "Title";
            // 
            // TBaseForm
            // 
            this.ClientSize = new System.Drawing.Size(443, 299);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbTitle);
            this.ForeColor = System.Drawing.Color.White;
            this.IResize = false;
            this.Name = "TBaseForm";
            this.Padding = new System.Windows.Forms.Padding(1, 80, 1, 2);
            this.ShowIcon = false;
            this.SysButton = Paway.Helper.TSysButton.Close;
            this.TBrush.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(57)))), ((int)(((byte)(50)))));
            this.TBrush.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.TBrush.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(84)))), ((int)(((byte)(74)))));
            this.TBrush.ColorSpace = System.Drawing.Color.Ivory;
            this.Text = "Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// </summary>
        protected TControl panel1;
        /// <summary>
        /// </summary>
        protected System.Windows.Forms.Label lbTitle;
    }
}