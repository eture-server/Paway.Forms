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
            Paway.Forms.ToolItem toolItem5 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem6 = new Paway.Forms.ToolItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btSearch = new Paway.Forms.QQButton();
            this.toolClose = new Paway.Forms.ToolBar();
            this.contextMenuStrip1.SuspendLayout();
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
            // btSearch
            // 
            this.btSearch.DownImage = ((System.Drawing.Image)(resources.GetObject("btSearch.DownImage")));
            this.btSearch.ForeColor = System.Drawing.Color.Black;
            this.btSearch.Image = null;
            this.btSearch.Location = new System.Drawing.Point(363, 177);
            this.btSearch.MoveImage = ((System.Drawing.Image)(resources.GetObject("btSearch.MoveImage")));
            this.btSearch.Name = "btSearch";
            this.btSearch.NormalImage = ((System.Drawing.Image)(resources.GetObject("btSearch.NormalImage")));
            this.btSearch.Size = new System.Drawing.Size(59, 28);
            this.btSearch.TabIndex = 25;
            this.btSearch.Text = "Search";
            this.btSearch.UseVisualStyleBackColor = false;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // toolClose
            // 
            this.toolClose.CausesValidation = false;
            this.toolClose.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolClose.ForeColor = System.Drawing.Color.Black;
            this.toolClose.IImageShow = false;
            this.toolClose.ImageSize = new System.Drawing.Size(0, 0);
            this.toolClose.IsMultiple = true;
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
            toolItem3.Desc = "x3";
            toolItem3.First = "c";
            toolItem3.Text = "c";
            toolItem4.Color = System.Drawing.Color.Empty;
            toolItem4.Desc = "x4";
            toolItem4.First = "d";
            toolItem4.Text = "d";
            toolItem5.Color = System.Drawing.Color.Empty;
            toolItem5.Desc = "x5";
            toolItem5.First = "d";
            toolItem5.Text = "d";
            toolItem6.Color = System.Drawing.Color.Empty;
            toolItem6.Desc = "x6";
            toolItem6.First = "d";
            toolItem6.Text = "d";
            this.toolClose.Items.Add(toolItem1);
            this.toolClose.Items.Add(toolItem2);
            this.toolClose.Items.Add(toolItem3);
            this.toolClose.Items.Add(toolItem4);
            this.toolClose.Items.Add(toolItem5);
            this.toolClose.Items.Add(toolItem6);
            this.toolClose.ItemSize = new System.Drawing.Size(120, 120);
            this.toolClose.ItemSpace = 5;
            this.toolClose.Location = new System.Drawing.Point(71, 19);
            this.toolClose.Name = "toolClose";
            this.toolClose.Padding = new System.Windows.Forms.Padding(1);
            this.toolClose.Size = new System.Drawing.Size(274, 179);
            this.toolClose.TabIndex = 28;
            this.toolClose.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(50)))), ((int)(((byte)(27)))));
            this.toolClose.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(127)))), ((int)(((byte)(104)))));
            this.toolClose.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(72)))), ((int)(((byte)(38)))));
            this.toolClose.TBackGround.ColorSpace = System.Drawing.Color.Empty;
            this.toolClose.TDesc.ColorDown = System.Drawing.Color.White;
            this.toolClose.TDesc.ColorMove = System.Drawing.Color.Lime;
            this.toolClose.TDesc.ColorNormal = System.Drawing.Color.Empty;
            this.toolClose.TDesc.ColorSpace = System.Drawing.Color.Empty;
            this.toolClose.TEndDesc.ColorDown = System.Drawing.Color.Empty;
            this.toolClose.TEndDesc.ColorMove = System.Drawing.Color.Empty;
            this.toolClose.TEndDesc.ColorNormal = System.Drawing.Color.Empty;
            this.toolClose.TEndDesc.ColorSpace = System.Drawing.Color.Empty;
            this.toolClose.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolClose.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolClose.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolClose.TextFirst.ColorSpace = System.Drawing.Color.Empty;
            this.toolClose.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolClose.TextSencond.ColorDown = System.Drawing.Color.Empty;
            this.toolClose.TextSencond.ColorMove = System.Drawing.Color.Empty;
            this.toolClose.TextSencond.ColorNormal = System.Drawing.Color.Empty;
            this.toolClose.TextSencond.ColorSpace = System.Drawing.Color.Empty;
            this.toolClose.TextSencond.FontDown = new System.Drawing.Font("微软雅黑", 9F);
            this.toolClose.TextSencond.FontMove = new System.Drawing.Font("微软雅黑", 9F);
            this.toolClose.TextSencond.FontNormal = new System.Drawing.Font("微软雅黑", 9F);
            this.toolClose.THeadDesc.ColorDown = System.Drawing.Color.Empty;
            this.toolClose.THeadDesc.ColorMove = System.Drawing.Color.Empty;
            this.toolClose.THeadDesc.ColorNormal = System.Drawing.Color.Empty;
            this.toolClose.THeadDesc.ColorSpace = System.Drawing.Color.Empty;
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
            this.ClientSize = new System.Drawing.Size(528, 286);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.toolClose);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bToolStripMenuItem;
        private Forms.ToolBar toolClose;
        private Forms.QQButton btSearch;





    }
}