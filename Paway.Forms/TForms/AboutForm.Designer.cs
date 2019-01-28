namespace Paway.Forms
{
    partial class AboutForm
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
            this.lbDesc = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbCopyright = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbDesc
            // 
            this.lbDesc.AutoSize = true;
            this.lbDesc.BackColor = System.Drawing.Color.Transparent;
            this.lbDesc.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.lbDesc.ForeColor = System.Drawing.Color.White;
            this.lbDesc.Location = new System.Drawing.Point(20, 72);
            this.lbDesc.Name = "lbDesc";
            this.lbDesc.Size = new System.Drawing.Size(115, 20);
            this.lbDesc.TabIndex = 5;
            this.lbDesc.Text = "<Description>";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.BackColor = System.Drawing.Color.Transparent;
            this.lbVersion.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lbVersion.ForeColor = System.Drawing.Color.White;
            this.lbVersion.Location = new System.Drawing.Point(20, 29);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(79, 20);
            this.lbVersion.TabIndex = 7;
            this.lbVersion.Text = "<Version>";
            this.lbVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(1, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(298, 1);
            this.label1.TabIndex = 12;
            // 
            // lbCopyright
            // 
            this.lbCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lbCopyright.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbCopyright.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lbCopyright.ForeColor = System.Drawing.Color.White;
            this.lbCopyright.Location = new System.Drawing.Point(1, 269);
            this.lbCopyright.Name = "lbCopyright";
            this.lbCopyright.Size = new System.Drawing.Size(298, 30);
            this.lbCopyright.TabIndex = 11;
            this.lbCopyright.Text = "<Copyright>";
            this.lbCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AboutForm
            // 
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbCopyright);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.lbDesc);
            this.IRound = false;
            this.IResize = false;
            this.IShadow = false;
            this.ITransfer = true;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SysButton = Paway.Helper.TSysButton.Close;
            this.Text = "关于我们";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbVersion;
        /// <summary>
        /// </summary>
        protected System.Windows.Forms.Label lbDesc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbCopyright;



    }
}