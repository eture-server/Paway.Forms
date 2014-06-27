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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qqButton1 = new Paway.Forms.QQButton();
            this.btSearch = new Paway.Forms.QQButton();
            this.toolBar1 = new Paway.Forms.ToolBar();
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
            // qqButton1
            // 
            this.qqButton1.DownImage = ((System.Drawing.Image)(resources.GetObject("qqButton1.DownImage")));
            this.qqButton1.ForeColor = System.Drawing.Color.Black;
            this.qqButton1.Image = null;
            this.qqButton1.Location = new System.Drawing.Point(28, 95);
            this.qqButton1.MoveImage = ((System.Drawing.Image)(resources.GetObject("qqButton1.MoveImage")));
            this.qqButton1.Name = "qqButton1";
            this.qqButton1.NormalImage = ((System.Drawing.Image)(resources.GetObject("qqButton1.NormalImage")));
            this.qqButton1.Size = new System.Drawing.Size(59, 28);
            this.qqButton1.TabIndex = 48;
            this.qqButton1.Text = "delete";
            this.qqButton1.UseVisualStyleBackColor = false;
            this.qqButton1.Click += new System.EventHandler(this.qqButton1_Click_1);
            // 
            // btSearch
            // 
            this.btSearch.DownImage = ((System.Drawing.Image)(resources.GetObject("btSearch.DownImage")));
            this.btSearch.ForeColor = System.Drawing.Color.Black;
            this.btSearch.Image = null;
            this.btSearch.Location = new System.Drawing.Point(28, 61);
            this.btSearch.MoveImage = ((System.Drawing.Image)(resources.GetObject("btSearch.MoveImage")));
            this.btSearch.Name = "btSearch";
            this.btSearch.NormalImage = ((System.Drawing.Image)(resources.GetObject("btSearch.NormalImage")));
            this.btSearch.Size = new System.Drawing.Size(59, 28);
            this.btSearch.TabIndex = 47;
            this.btSearch.Text = "add";
            this.btSearch.UseVisualStyleBackColor = false;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click_1);
            // 
            // toolBar1
            // 
            this.toolBar1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolBar1.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolBar1.ForeColor = System.Drawing.Color.Black;
            this.toolBar1.IImageShow = false;
            this.toolBar1.ImageSize = new System.Drawing.Size(0, 0);
            toolItem1.Color = System.Drawing.Color.Empty;
            toolItem1.Desc = "x1";
            toolItem1.First = "关闭系统";
            toolItem1.Text = "关闭系统";
            toolItem2.Color = System.Drawing.Color.Empty;
            toolItem2.Desc = "x2";
            toolItem2.First = "关闭电脑";
            toolItem2.Text = "关闭电脑";
            toolItem3.Color = System.Drawing.Color.Empty;
            toolItem3.Desc = "x3";
            toolItem3.First = "关闭世界";
            toolItem3.Text = "关闭世界";
            toolItem4.Color = System.Drawing.Color.Empty;
            toolItem4.Desc = "x4";
            toolItem4.First = "关闭电力";
            toolItem4.Text = "关闭电力";
            toolItem5.Color = System.Drawing.Color.Empty;
            toolItem5.Desc = "x5";
            toolItem5.First = "关闭都会";
            toolItem5.Text = "关闭都会";
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.Items.Add(toolItem2);
            this.toolBar1.Items.Add(toolItem3);
            this.toolBar1.Items.Add(toolItem4);
            this.toolBar1.Items.Add(toolItem5);
            this.toolBar1.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar1.ItemSpace = 5;
            this.toolBar1.Location = new System.Drawing.Point(169, 47);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(277, 191);
            this.toolBar1.TabIndex = 53;
            this.toolBar1.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(50)))), ((int)(((byte)(27)))));
            this.toolBar1.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(127)))), ((int)(((byte)(104)))));
            this.toolBar1.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(72)))), ((int)(((byte)(38)))));
            this.toolBar1.TEvent = Paway.Forms.TEvent.Up;
            this.toolBar1.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolBar1.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolBar1.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolBar1.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolBar1.Trans = 150;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(628, 326);
            this.Controls.Add(this.toolBar1);
            this.Controls.Add(this.qqButton1);
            this.Controls.Add(this.btSearch);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
        private Forms.QQButton qqButton1;
        private Forms.QQButton btSearch;
        private Forms.ToolBar toolBar1;





    }
}