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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lbCopyright = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.lbDesc = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btOk = new Paway.Forms.QQButton();
            this.lbPlatform = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lbCopyright);
            this.panel1.Controls.Add(this.lbVersion);
            this.panel1.Controls.Add(this.lbDesc);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lbPlatform);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(298, 273);
            this.panel1.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(8, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(283, 1);
            this.label5.TabIndex = 9;
            // 
            // lbCopyright
            // 
            this.lbCopyright.AutoSize = true;
            this.lbCopyright.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lbCopyright.ForeColor = System.Drawing.Color.Black;
            this.lbCopyright.Location = new System.Drawing.Point(20, 76);
            this.lbCopyright.Name = "lbCopyright";
            this.lbCopyright.Size = new System.Drawing.Size(95, 20);
            this.lbCopyright.TabIndex = 8;
            this.lbCopyright.Text = "<Copyright>";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lbVersion.ForeColor = System.Drawing.Color.Black;
            this.lbVersion.Location = new System.Drawing.Point(20, 46);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(79, 20);
            this.lbVersion.TabIndex = 7;
            this.lbVersion.Text = "<Version>";
            // 
            // lbDesc
            // 
            this.lbDesc.AutoSize = true;
            this.lbDesc.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.lbDesc.ForeColor = System.Drawing.Color.Black;
            this.lbDesc.Location = new System.Drawing.Point(20, 116);
            this.lbDesc.Name = "lbDesc";
            this.lbDesc.Size = new System.Drawing.Size(115, 20);
            this.lbDesc.TabIndex = 5;
            this.lbDesc.Text = "<Description>";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btOk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 236);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(298, 37);
            this.panel2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.DodgerBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(298, 1);
            this.label1.TabIndex = 3;
            // 
            // btOk
            // 
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btOk.DownImage = ((System.Drawing.Image)(resources.GetObject("btOk.DownImage")));
            this.btOk.Image = null;
            this.btOk.Location = new System.Drawing.Point(113, 4);
            this.btOk.MoveImage = ((System.Drawing.Image)(resources.GetObject("btOk.MoveImage")));
            this.btOk.Name = "btOk";
            this.btOk.NormalImage = ((System.Drawing.Image)(resources.GetObject("btOk.NormalImage")));
            this.btOk.Size = new System.Drawing.Size(75, 28);
            this.btOk.TabIndex = 2;
            this.btOk.Text = "关闭";
            this.btOk.UseVisualStyleBackColor = false;
            // 
            // lbPlatform
            // 
            this.lbPlatform.AutoSize = true;
            this.lbPlatform.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lbPlatform.ForeColor = System.Drawing.Color.Black;
            this.lbPlatform.Location = new System.Drawing.Point(20, 17);
            this.lbPlatform.Name = "lbPlatform";
            this.lbPlatform.Size = new System.Drawing.Size(87, 20);
            this.lbPlatform.TabIndex = 3;
            this.lbPlatform.Text = "<Platform>";
            // 
            // AboutForm
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DodgerBlue;
            this.CancelButton = this.btOk;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.IsDrawRound = false;
            this.IsResize = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.SysButton = Paway.Forms.TSysButton.Close;
            this.Text = "关于我们";
            this.TextShow = "关于我们";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        /// <summary>
        /// </summary>
        protected System.Windows.Forms.Panel panel1;
        /// <summary>
        /// </summary>
        protected System.Windows.Forms.Panel panel2;
        /// <summary>
        /// </summary>
        protected QQButton btOk;
        /// <summary>
        /// 描述
        /// </summary>
        protected System.Windows.Forms.Label lbDesc;
        private System.Windows.Forms.Label lbCopyright;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label lbPlatform;
        private System.Windows.Forms.Label label5;

    }
}