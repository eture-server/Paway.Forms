﻿namespace Paway.Test
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Paway.Forms.ToolItem toolItem10 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem11 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem12 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem13 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem14 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem15 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem16 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem17 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem18 = new Paway.Forms.ToolItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btName = new Paway.Forms.QQButton();
            this.toolBar = new Paway.Forms.ToolBar();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tbName = new Paway.Forms.QQTextBox();
            this.tbRsa2 = new Paway.Forms.QQTextBox();
            this.tControl1 = new Paway.Forms.TControl();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAdd,
            this.toolDelete});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(101, 48);
            // 
            // toolAdd
            // 
            this.toolAdd.Name = "toolAdd";
            this.toolAdd.Size = new System.Drawing.Size(100, 22);
            this.toolAdd.Text = "添加";
            // 
            // toolDelete
            // 
            this.toolDelete.Name = "toolDelete";
            this.toolDelete.Size = new System.Drawing.Size(100, 22);
            this.toolDelete.Text = "删除";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(5, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1, 1);
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // btName
            // 
            this.btName.DownImage = ((System.Drawing.Image)(resources.GetObject("btName.DownImage")));
            this.btName.Image = null;
            this.btName.Location = new System.Drawing.Point(35, 25);
            this.btName.MoveImage = ((System.Drawing.Image)(resources.GetObject("btName.MoveImage")));
            this.btName.Name = "btName";
            this.btName.NormalImage = ((System.Drawing.Image)(resources.GetObject("btName.NormalImage")));
            this.btName.Size = new System.Drawing.Size(64, 32);
            this.btName.TabIndex = 30;
            this.btName.Text = "hello";
            this.btName.UseVisualStyleBackColor = false;
            // 
            // toolBar
            // 
            this.toolBar.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolBar.ICheckEvent = true;
            this.toolBar.ImageSize = new System.Drawing.Size(32, 32);
            toolItem10.IHeard = true;
            toolItem10.Selete = false;
            toolItem10.TColor.StringVertical = System.Drawing.StringAlignment.Center;
            toolItem10.Text = "白酒";
            toolItem11.Desc = "";
            toolItem11.EndDesc = "";
            toolItem11.HeadDesc = "呵呵";
            toolItem11.IChange = true;
            toolItem11.Selete = false;
            toolItem11.Text = "头部";
            toolItem12.IHeard = true;
            toolItem12.Selete = false;
            toolItem12.Text = "1";
            toolItem13.Selete = false;
            toolItem13.Text = "2";
            toolItem14.Selete = false;
            toolItem14.Text = "3";
            toolItem15.IHeard = true;
            toolItem15.Selete = false;
            toolItem15.Text = "4";
            toolItem16.Selete = false;
            toolItem16.Text = "5";
            toolItem17.Selete = false;
            toolItem17.Text = "6";
            toolItem18.Selete = false;
            toolItem18.Text = "7";
            this.toolBar.Items.Add(toolItem10);
            this.toolBar.Items.Add(toolItem11);
            this.toolBar.Items.Add(toolItem12);
            this.toolBar.Items.Add(toolItem13);
            this.toolBar.Items.Add(toolItem14);
            this.toolBar.Items.Add(toolItem15);
            this.toolBar.Items.Add(toolItem16);
            this.toolBar.Items.Add(toolItem17);
            this.toolBar.Items.Add(toolItem18);
            this.toolBar.ItemSize = new System.Drawing.Size(128, 120);
            this.toolBar.ItemSpace = 5;
            this.toolBar.Location = new System.Drawing.Point(35, 63);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(412, 223);
            this.toolBar.TabIndex = 31;
            this.toolBar.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(50)))), ((int)(((byte)(27)))));
            this.toolBar.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(127)))), ((int)(((byte)(104)))));
            this.toolBar.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(72)))), ((int)(((byte)(38)))));
            this.toolBar.TChange.ColorDown = System.Drawing.Color.Blue;
            this.toolBar.TChange.ColorMove = System.Drawing.Color.Red;
            this.toolBar.TChange.ColorNormal = System.Drawing.Color.Yellow;
            this.toolBar.TDesc.ColorDown = System.Drawing.Color.White;
            this.toolBar.TDesc.ColorMove = System.Drawing.Color.White;
            this.toolBar.TEvent = Paway.Forms.TEvent.Up;
            this.toolBar.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolBar.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolBar.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolBar.TextFirst.StringHorizontal = System.Drawing.StringAlignment.Center;
            this.toolBar.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolBar.Trans = 150;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Paway.Test.Properties.Resources.imgLoadding_Image;
            this.pictureBox2.Location = new System.Drawing.Point(5, 284);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(510, 2);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 32;
            this.pictureBox2.TabStop = false;
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tbName.Icon = global::Paway.Test.Properties.Resources.serrch;
            this.tbName.IconIsButton = true;
            this.tbName.IsPasswordChat = '\0';
            this.tbName.Lines = new string[0];
            this.tbName.Location = new System.Drawing.Point(310, 27);
            this.tbName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbName.MaxLength = 10;
            this.tbName.Name = "tbName";
            this.tbName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.tbName.Regex = "";
            this.tbName.RegexType = Paway.Helper.RegexType.Normal;
            this.tbName.SelectedText = "";
            this.tbName.Size = new System.Drawing.Size(78, 29);
            this.tbName.TabIndex = 59;
            this.tbName.WaterText = "";
            // 
            // tbRsa2
            // 
            this.tbRsa2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tbRsa2.Icon = null;
            this.tbRsa2.IsPasswordChat = '\0';
            this.tbRsa2.Lines = new string[0];
            this.tbRsa2.Location = new System.Drawing.Point(192, 29);
            this.tbRsa2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbRsa2.MaxLength = 128;
            this.tbRsa2.Name = "tbRsa2";
            this.tbRsa2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.tbRsa2.Regex = "[0-9]+(\\.[0-9]+)?";
            this.tbRsa2.RegexType = Paway.Helper.RegexType.Custom;
            this.tbRsa2.SelectedText = "";
            this.tbRsa2.Size = new System.Drawing.Size(94, 29);
            this.tbRsa2.TabIndex = 60;
            this.tbRsa2.WaterText = "请输入Rsa值";
            // 
            // tControl1
            // 
            this.tControl1.Location = new System.Drawing.Point(399, 136);
            this.tControl1.Name = "tControl1";
            this.tControl1.Size = new System.Drawing.Size(116, 101);
            this.tControl1.TabIndex = 61;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(77)))), ((int)(((byte)(185)))));
            this.BackgroundImage = global::Paway.Test.Properties.Resources.offline;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(523, 292);
            this.Controls.Add(this.tControl1);
            this.Controls.Add(this.tbRsa2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.btName);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.White;
            this.IsDrawBorder = true;
            this.IsResize = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(0, 26, 3, 3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolAdd;
        private System.Windows.Forms.ToolStripMenuItem toolDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Forms.QQButton btName;
        private Forms.ToolBar toolBar;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Forms.QQTextBox tbName;
        private Forms.QQTextBox tbRsa2;
        private Forms.TControl tControl1;





    }
}