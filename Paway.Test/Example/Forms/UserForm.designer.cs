﻿namespace Paway.Test
{
    partial class UserForm
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
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            this.toolCancel = new Paway.Forms.ToolBar();
            this.toolOk = new Paway.Forms.ToolBar();
            this.tbPad = new Paway.Forms.QQTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new Paway.Forms.QQTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbPad);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbName);
            this.panel1.Controls.Add(this.toolCancel);
            this.panel1.Controls.Add(this.toolOk);
            this.panel1.Size = new System.Drawing.Size(441, 199);
            // 
            // lbTitle
            // 
            this.lbTitle.Location = new System.Drawing.Point(175, 31);
            this.lbTitle.Size = new System.Drawing.Size(92, 27);
            this.lbTitle.Text = "添加用户";
            // 
            // toolCancel
            // 
            this.toolCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolCancel.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolCancel.ICheckEvent = true;
            this.toolCancel.IItemLine = true;
            this.toolCancel.ImageSize = new System.Drawing.Size(0, 0);
            toolItem1.Text = "取消";
            this.toolCancel.Items.Add(toolItem1);
            this.toolCancel.ItemSize = new System.Drawing.Size(80, 40);
            this.toolCancel.ItemSpace = 5;
            this.toolCancel.Location = new System.Drawing.Point(251, 139);
            this.toolCancel.Name = "toolCancel";
            this.toolCancel.Size = new System.Drawing.Size(80, 42);
            this.toolCancel.TabIndex = 22;
            this.toolCancel.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(57)))), ((int)(((byte)(0)))));
            this.toolCancel.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(134)))), ((int)(((byte)(77)))));
            this.toolCancel.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(81)))), ((int)(((byte)(0)))));
            this.toolCancel.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolCancel.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolCancel.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolCancel.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolCancel.Trans = 150;
            // 
            // toolOk
            // 
            this.toolOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolOk.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolOk.ICheckEvent = true;
            this.toolOk.IItemLine = true;
            this.toolOk.ImageSize = new System.Drawing.Size(0, 0);
            toolItem2.Text = "确认";
            this.toolOk.Items.Add(toolItem2);
            this.toolOk.ItemSize = new System.Drawing.Size(80, 40);
            this.toolOk.ItemSpace = 5;
            this.toolOk.Location = new System.Drawing.Point(115, 139);
            this.toolOk.Name = "toolOk";
            this.toolOk.Size = new System.Drawing.Size(80, 42);
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
            // tbPad
            // 
            this.tbPad.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tbPad.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbPad.IsPasswordChat = '*';
            this.tbPad.Lines = new string[0];
            this.tbPad.Location = new System.Drawing.Point(146, 62);
            this.tbPad.MaxLength = 32;
            this.tbPad.Name = "tbPad";
            this.tbPad.RegexType = Paway.Helper.RegexType.Password;
            this.tbPad.Size = new System.Drawing.Size(210, 29);
            this.tbPad.TabIndex = 12;
            this.tbPad.WaterText = "请输入密码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(71, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 51;
            this.label1.Text = "用户名：";
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
            this.tbName.Size = new System.Drawing.Size(210, 29);
            this.tbName.TabIndex = 11;
            this.tbName.WaterText = "请输入用戶名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(87, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 21);
            this.label2.TabIndex = 52;
            this.label2.Text = "密码：";
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.ClientSize = new System.Drawing.Size(443, 280);
            this.Name = "UserForm";
            this.TBrush.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(57)))), ((int)(((byte)(50)))));
            this.TBrush.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(112)))), ((int)(((byte)(99)))));
            this.TBrush.ColorNormal = System.Drawing.Color.CornflowerBlue;
            this.TBrush.ColorSpace = System.Drawing.Color.Ivory;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Paway.Forms.ToolBar toolCancel;
        private Paway.Forms.ToolBar toolOk;
        private Paway.Forms.QQTextBox tbPad;
        private System.Windows.Forms.Label label1;
        private Paway.Forms.QQTextBox tbName;
        private System.Windows.Forms.Label label2;
    }
}