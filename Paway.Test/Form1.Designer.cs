namespace Paway.Test
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
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem4 = new Paway.Forms.ToolItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qqButton1 = new Paway.Forms.QQButton();
            this.btSearch = new Paway.Forms.QQButton();
            this.tControl1 = new Paway.Forms.TControl();
            this.toolClose = new Paway.Forms.ToolBar();
            this.contextMenuStrip1.SuspendLayout();
            this.tControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aToolStripMenuItem,
            this.bToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(83, 48);
            // 
            // aToolStripMenuItem
            // 
            this.aToolStripMenuItem.Name = "aToolStripMenuItem";
            this.aToolStripMenuItem.Size = new System.Drawing.Size(82, 22);
            this.aToolStripMenuItem.Text = "a";
            // 
            // bToolStripMenuItem
            // 
            this.bToolStripMenuItem.Name = "bToolStripMenuItem";
            this.bToolStripMenuItem.Size = new System.Drawing.Size(82, 22);
            this.bToolStripMenuItem.Text = "b";
            // 
            // qqButton1
            // 
            this.qqButton1.DownImage = ((System.Drawing.Image)(resources.GetObject("qqButton1.DownImage")));
            this.qqButton1.ForeColor = System.Drawing.Color.Black;
            this.qqButton1.Image = null;
            this.qqButton1.Location = new System.Drawing.Point(21, 101);
            this.qqButton1.MoveImage = ((System.Drawing.Image)(resources.GetObject("qqButton1.MoveImage")));
            this.qqButton1.Name = "qqButton1";
            this.qqButton1.NormalImage = ((System.Drawing.Image)(resources.GetObject("qqButton1.NormalImage")));
            this.qqButton1.Size = new System.Drawing.Size(59, 28);
            this.qqButton1.TabIndex = 48;
            this.qqButton1.Text = "delete";
            this.qqButton1.UseVisualStyleBackColor = false;
            // 
            // btSearch
            // 
            this.btSearch.DownImage = ((System.Drawing.Image)(resources.GetObject("btSearch.DownImage")));
            this.btSearch.ForeColor = System.Drawing.Color.Black;
            this.btSearch.Image = null;
            this.btSearch.Location = new System.Drawing.Point(21, 48);
            this.btSearch.MoveImage = ((System.Drawing.Image)(resources.GetObject("btSearch.MoveImage")));
            this.btSearch.Name = "btSearch";
            this.btSearch.NormalImage = ((System.Drawing.Image)(resources.GetObject("btSearch.NormalImage")));
            this.btSearch.Size = new System.Drawing.Size(59, 28);
            this.btSearch.TabIndex = 47;
            this.btSearch.Text = "add";
            this.btSearch.UseVisualStyleBackColor = false;
            // 
            // tControl1
            // 
            this.tControl1.Controls.Add(this.toolClose);
            this.tControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tControl1.Location = new System.Drawing.Point(109, 0);
            this.tControl1.Name = "tControl1";
            this.tControl1.Size = new System.Drawing.Size(345, 259);
            this.tControl1.TabIndex = 44;
            // 
            // toolClose
            // 
            this.toolClose.CausesValidation = false;
            this.toolClose.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolClose.ForeColor = System.Drawing.Color.Black;
            this.toolClose.IImageShow = false;
            this.toolClose.ImageSize = new System.Drawing.Size(0, 0);
            toolItem1.Color = System.Drawing.Color.Empty;
            toolItem1.ContextMenuStrip = this.contextMenuStrip1;
            toolItem1.Desc = "x1";
            toolItem1.First = "关闭系统";
            toolItem1.Image = global::Paway.Test.Properties.Resources._1;
            toolItem1.Sencond = "好吗\r\n你争\r\n我要";
            toolItem1.Text = "关闭系统\r\n好吗\r\n你争\r\n我要";
            toolItem2.Color = System.Drawing.Color.Empty;
            toolItem2.Desc = "x2";
            toolItem2.First = "a";
            toolItem2.Text = "a";
            toolItem3.Color = System.Drawing.Color.Empty;
            toolItem4.Color = System.Drawing.Color.Empty;
            this.toolClose.Items.Add(toolItem1);
            this.toolClose.Items.Add(toolItem2);
            this.toolClose.Items.Add(toolItem3);
            this.toolClose.Items.Add(toolItem4);
            this.toolClose.ItemSize = new System.Drawing.Size(120, 120);
            this.toolClose.ItemSpace = 5;
            this.toolClose.Location = new System.Drawing.Point(50, 68);
            this.toolClose.Name = "toolClose";
            this.toolClose.Padding = new System.Windows.Forms.Padding(1);
            this.toolClose.Size = new System.Drawing.Size(274, 122);
            this.toolClose.TabIndex = 45;
            this.toolClose.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(50)))), ((int)(((byte)(27)))));
            this.toolClose.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(127)))), ((int)(((byte)(104)))));
            this.toolClose.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(72)))), ((int)(((byte)(38)))));
            this.toolClose.TDesc.ColorDown = System.Drawing.Color.White;
            this.toolClose.TDesc.ColorMove = System.Drawing.Color.Lime;
            this.toolClose.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolClose.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolClose.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolClose.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolClose.TextSencond.FontDown = new System.Drawing.Font("微软雅黑", 9F);
            this.toolClose.TextSencond.FontMove = new System.Drawing.Font("微软雅黑", 9F);
            this.toolClose.TextSencond.FontNormal = new System.Drawing.Font("微软雅黑", 9F);
            this.toolClose.Trans = 150;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(454, 259);
            this.ControlBox = false;
            this.Controls.Add(this.qqButton1);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.tControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.contextMenuStrip1.ResumeLayout(false);
            this.tControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bToolStripMenuItem;
        private Forms.TControl tControl1;
        private Forms.ToolBar toolClose;
        private Forms.QQButton qqButton1;
        private Forms.QQButton btSearch;





    }
}