namespace Paway.Test
{
    partial class Login
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
            this.tbName = new Paway.Forms.QQTextBox();
            this.toolOk = new Paway.Forms.ToolBar();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPad = new Paway.Forms.QQTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbPad);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbName);
            this.panel1.Controls.Add(this.toolOk);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Size = new System.Drawing.Size(441, 199);
            // 
            // lbTitle
            // 
            this.lbTitle.Location = new System.Drawing.Point(185, 31);
            this.lbTitle.Size = new System.Drawing.Size(72, 27);
            this.lbTitle.Text = "欢迎您";
            // 
            // tbName
            // 
            this.tbName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tbName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbName.Lines = new string[0];
            this.tbName.Location = new System.Drawing.Point(146, 22);
            this.tbName.MaxLength = 32;
            this.tbName.Name = "tbName";
            this.tbName.RegexType = Paway.Helper.RegexType.Normal;
            this.tbName.RLength = 1;
            this.tbName.Size = new System.Drawing.Size(198, 29);
            this.tbName.TabIndex = 11;
            this.tbName.WaterText = "请输入用户名";
            // 
            // toolOk
            // 
            this.toolOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolOk.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolOk.IClickEvent = true;
            this.toolOk.TLineColor.ColorDown = System.Drawing.Color.Transparent;
            this.toolOk.TLineColor.ColorMove = System.Drawing.Color.Transparent;
            this.toolOk.TLineColor.ColorNormal = System.Drawing.Color.Transparent;
            this.toolOk.ImageSize = new System.Drawing.Size(0, 0);
            toolItem1.Text = "登陆";
            this.toolOk.Items.Add(toolItem1);
            this.toolOk.ItemSize = new System.Drawing.Size(90, 42);
            this.toolOk.ItemSpace = 5;
            this.toolOk.Location = new System.Drawing.Point(176, 139);
            this.toolOk.Name = "toolOk";
            this.toolOk.Size = new System.Drawing.Size(90, 42);
            this.toolOk.TabIndex = 21;
            this.toolOk.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolOk.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolOk.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(239)))));
            this.toolOk.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolOk.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolOk.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolOk.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolOk.Trans = 150;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(71, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 61;
            this.label2.Text = "用户名：";
            // 
            // tbPad
            // 
            this.tbPad.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tbPad.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbPad.PasswordChat = '*';
            this.tbPad.Lines = new string[0];
            this.tbPad.Location = new System.Drawing.Point(146, 60);
            this.tbPad.MaxLength = 32;
            this.tbPad.Name = "tbPad";
            this.tbPad.RegexType = Paway.Helper.RegexType.Normal;
            this.tbPad.RLength = 1;
            this.tbPad.Size = new System.Drawing.Size(198, 29);
            this.tbPad.TabIndex = 12;
            this.tbPad.WaterText = "请输入密码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(72, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 21);
            this.label1.TabIndex = 63;
            this.label1.Text = "密   码：";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.ClientSize = new System.Drawing.Size(443, 280);
            this.Name = "Login";
            this.ShowInTaskbar = true;
            this.TBrush.ColorMove = System.Drawing.Color.CornflowerBlue;
            this.TBrush.ColorDown = System.Drawing.Color.Ivory;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Paway.Forms.QQTextBox tbName;
        private Paway.Forms.ToolBar toolOk;
        private System.Windows.Forms.Label label2;
        private Paway.Forms.QQTextBox tbPad;
        private System.Windows.Forms.Label label1;
    }
}