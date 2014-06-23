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
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.toolClose = new Paway.Forms.ToolBar();
            this.tControl1 = new Paway.Forms.TControl();
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(253, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolClose
            // 
            this.toolClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolClose.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolClose.ForeColor = System.Drawing.Color.Black;
            this.toolClose.IImageShow = false;
            this.toolClose.ImageSize = new System.Drawing.Size(0, 0);
            this.toolClose.IsMultiple = true;
            toolItem1.Color = System.Drawing.Color.Empty;
            toolItem1.Desc = "x1";
            toolItem1.First = "关闭系统";
            toolItem1.Image = global::Paway.Test.Properties.Resources._1;
            toolItem1.Rectangle = new System.Drawing.Rectangle(0, 0, 120, 120);
            toolItem1.Sencond = "好吗\r\n你争\r\n我要";
            toolItem1.Text = "关闭系统\r\n好吗\r\n你争\r\n我要";
            this.toolClose.Items.Add(toolItem1);
            this.toolClose.ItemSize = new System.Drawing.Size(120, 120);
            this.toolClose.ItemSpace = 5;
            this.toolClose.Location = new System.Drawing.Point(95, 77);
            this.toolClose.Name = "toolClose";
            this.toolClose.Size = new System.Drawing.Size(120, 120);
            this.toolClose.TabIndex = 13;
            this.toolClose.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(50)))), ((int)(((byte)(27)))));
            this.toolClose.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(127)))), ((int)(((byte)(104)))));
            this.toolClose.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(72)))), ((int)(((byte)(38)))));
            this.toolClose.TBackGround.ColorSpace = System.Drawing.Color.Empty;
            this.toolClose.TDesc.ColorDown = System.Drawing.Color.White;
            this.toolClose.TDesc.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
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
            // tControl1
            // 
            this.tControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tControl1.Location = new System.Drawing.Point(0, 0);
            this.tControl1.Name = "tControl1";
            this.tControl1.Size = new System.Drawing.Size(329, 261);
            this.tControl1.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(329, 261);
            this.Controls.Add(this.toolClose);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tControl1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.ToolBar toolClose;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private Forms.TControl tControl1;





    }
}