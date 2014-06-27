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
            Paway.Forms.ToolItem toolItem5 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem6 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem7 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem8 = new Paway.Forms.ToolItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qqButton1 = new Paway.Forms.QQButton();
            this.btSearch = new Paway.Forms.QQButton();
            this.toolSet = new Paway.Forms.ToolBar();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.toolCarte = new Paway.Forms.ToolBar();
            this.toolCash = new Paway.Forms.ToolBar();
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
            this.qqButton1.Location = new System.Drawing.Point(34, 104);
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
            // toolSet
            // 
            this.toolSet.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolSet.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolSet.ForeColor = System.Drawing.Color.Black;
            this.toolSet.ICheckEvent = true;
            this.toolSet.IImageShow = false;
            this.toolSet.ImageSize = new System.Drawing.Size(0, 0);
            toolItem5.Color = System.Drawing.Color.Empty;
            toolItem5.First = "系统设置";
            toolItem5.Text = "系统设置";
            this.toolSet.Items.Add(toolItem5);
            this.toolSet.ItemSize = new System.Drawing.Size(120, 120);
            this.toolSet.ItemSpace = 5;
            this.toolSet.Location = new System.Drawing.Point(236, 103);
            this.toolSet.Name = "toolSet";
            this.toolSet.Size = new System.Drawing.Size(120, 120);
            this.toolSet.TabIndex = 53;
            this.toolSet.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(110)))), ((int)(((byte)(18)))));
            this.toolSet.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(187)))), ((int)(((byte)(95)))));
            this.toolSet.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(26)))));
            this.toolSet.TEvent = Paway.Forms.TEvent.Up;
            this.toolSet.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolSet.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolSet.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolSet.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolSet.Trans = 150;
            // 
            // toolBar1
            // 
            this.toolBar1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolBar1.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolBar1.ForeColor = System.Drawing.Color.Black;
            this.toolBar1.ICheckEvent = true;
            this.toolBar1.IImageShow = false;
            this.toolBar1.ImageSize = new System.Drawing.Size(0, 0);
            toolItem6.Color = System.Drawing.Color.Empty;
            toolItem6.First = "关闭系统";
            toolItem6.Text = "关闭系统";
            this.toolBar1.Items.Add(toolItem6);
            this.toolBar1.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar1.ItemSpace = 5;
            this.toolBar1.Location = new System.Drawing.Point(101, 103);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(120, 120);
            this.toolBar1.TabIndex = 52;
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
            // toolCarte
            // 
            this.toolCarte.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolCarte.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolCarte.ForeColor = System.Drawing.Color.Black;
            this.toolCarte.ICheckEvent = true;
            this.toolCarte.IImageShow = false;
            this.toolCarte.ImageSize = new System.Drawing.Size(0, 0);
            toolItem7.Color = System.Drawing.Color.Empty;
            toolItem7.First = "开始点菜";
            toolItem7.Text = "开始点菜";
            this.toolCarte.Items.Add(toolItem7);
            this.toolCarte.ItemSize = new System.Drawing.Size(120, 120);
            this.toolCarte.ItemSpace = 5;
            this.toolCarte.Location = new System.Drawing.Point(506, 103);
            this.toolCarte.Name = "toolCarte";
            this.toolCarte.Size = new System.Drawing.Size(120, 120);
            this.toolCarte.TabIndex = 51;
            this.toolCarte.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(0)))), ((int)(((byte)(108)))));
            this.toolCarte.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(77)))), ((int)(((byte)(185)))));
            this.toolCarte.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(0)))), ((int)(((byte)(155)))));
            this.toolCarte.TEvent = Paway.Forms.TEvent.Up;
            this.toolCarte.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolCarte.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolCarte.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolCarte.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolCarte.Trans = 150;
            // 
            // toolCash
            // 
            this.toolCash.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolCash.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolCash.ForeColor = System.Drawing.Color.Black;
            this.toolCash.ICheckEvent = true;
            this.toolCash.IImageShow = false;
            this.toolCash.ImageSize = new System.Drawing.Size(0, 0);
            toolItem8.Color = System.Drawing.Color.Empty;
            toolItem8.First = "收银功能";
            toolItem8.Text = "收银功能";
            this.toolCash.Items.Add(toolItem8);
            this.toolCash.ItemSize = new System.Drawing.Size(120, 120);
            this.toolCash.ItemSpace = 5;
            this.toolCash.Location = new System.Drawing.Point(371, 103);
            this.toolCash.Name = "toolCash";
            this.toolCash.Size = new System.Drawing.Size(120, 120);
            this.toolCash.TabIndex = 50;
            this.toolCash.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolCash.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolCash.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(239)))));
            this.toolCash.TEvent = Paway.Forms.TEvent.Up;
            this.toolCash.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolCash.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolCash.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolCash.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolCash.Trans = 150;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(726, 326);
            this.Controls.Add(this.toolSet);
            this.Controls.Add(this.toolBar1);
            this.Controls.Add(this.toolCarte);
            this.Controls.Add(this.toolCash);
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
        private Forms.ToolBar toolSet;
        private Forms.ToolBar toolBar1;
        private Forms.ToolBar toolCarte;
        private Forms.ToolBar toolCash;





    }
}