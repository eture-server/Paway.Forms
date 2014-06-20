namespace Paway.Test
{
    partial class Form3
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
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem4 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem5 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem6 = new Paway.Forms.ToolItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSet = new Paway.Forms.ToolBar();
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
            // toolSet
            // 
            this.toolSet.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolSet.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolSet.ForeColor = System.Drawing.Color.Black;
            this.toolSet.ICheckEvent = true;
            this.toolSet.IImageShow = false;
            this.toolSet.ImageSize = new System.Drawing.Size(60, 48);
            toolItem1.Color = System.Drawing.Color.Empty;
            toolItem1.ContextMenuStrip = this.contextMenuStrip1;
            toolItem1.Desc = "编辑";
            toolItem1.First = "开始点菜";
            toolItem1.Rectangle = new System.Drawing.Rectangle(0, 0, 240, 120);
            toolItem1.Text = "开始点菜";
            toolItem2.Color = System.Drawing.Color.Empty;
            toolItem2.ContextMenuStrip = this.contextMenuStrip1;
            toolItem2.First = "1";
            toolItem2.Rectangle = new System.Drawing.Rectangle(0, 125, 240, 120);
            toolItem2.Text = "1";
            toolItem3.Color = System.Drawing.Color.Empty;
            toolItem3.ContextMenuStrip = this.contextMenuStrip1;
            toolItem3.First = "2";
            toolItem3.Rectangle = new System.Drawing.Rectangle(245, 0, 240, 120);
            toolItem3.Text = "2";
            toolItem4.Color = System.Drawing.Color.Empty;
            toolItem4.First = "3";
            toolItem4.Rectangle = new System.Drawing.Rectangle(245, 125, 240, 120);
            toolItem4.Text = "3";
            toolItem5.Color = System.Drawing.Color.Empty;
            toolItem5.Desc = "编辑";
            toolItem5.First = "4";
            toolItem5.Rectangle = new System.Drawing.Rectangle(490, 0, 240, 120);
            toolItem5.Text = "4";
            toolItem6.Color = System.Drawing.Color.Empty;
            toolItem6.First = "5";
            toolItem6.Rectangle = new System.Drawing.Rectangle(490, 125, 240, 120);
            toolItem6.Text = "5";
            this.toolSet.Items.Add(toolItem1);
            this.toolSet.Items.Add(toolItem2);
            this.toolSet.Items.Add(toolItem3);
            this.toolSet.Items.Add(toolItem4);
            this.toolSet.Items.Add(toolItem5);
            this.toolSet.Items.Add(toolItem6);
            this.toolSet.ItemSize = new System.Drawing.Size(240, 120);
            this.toolSet.ItemSpace = 5;
            this.toolSet.Location = new System.Drawing.Point(0, 0);
            this.toolSet.Name = "toolSet";
            this.toolSet.Size = new System.Drawing.Size(747, 328);
            this.toolSet.TabIndex = 15;
            this.toolSet.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(0)))), ((int)(((byte)(108)))));
            this.toolSet.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(77)))), ((int)(((byte)(185)))));
            this.toolSet.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(0)))), ((int)(((byte)(155)))));
            this.toolSet.TBackGround.ColorSpace = System.Drawing.Color.Empty;
            this.toolSet.TDesc.ColorDown = System.Drawing.Color.Blue;
            this.toolSet.TDesc.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.toolSet.TDesc.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.toolSet.TDesc.ColorSpace = System.Drawing.Color.Empty;
            this.toolSet.TDirection = Paway.Forms.TDirection.Vertical;
            this.toolSet.TEndDesc.ColorDown = System.Drawing.Color.Empty;
            this.toolSet.TEndDesc.ColorMove = System.Drawing.Color.Empty;
            this.toolSet.TEndDesc.ColorNormal = System.Drawing.Color.Empty;
            this.toolSet.TEndDesc.ColorSpace = System.Drawing.Color.Empty;
            this.toolSet.TEvent = Paway.Forms.TEvent.Up;
            this.toolSet.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolSet.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolSet.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolSet.TextFirst.ColorSpace = System.Drawing.Color.Empty;
            this.toolSet.TextSencond.ColorDown = System.Drawing.Color.Empty;
            this.toolSet.TextSencond.ColorMove = System.Drawing.Color.Empty;
            this.toolSet.TextSencond.ColorNormal = System.Drawing.Color.Empty;
            this.toolSet.TextSencond.ColorSpace = System.Drawing.Color.Empty;
            this.toolSet.THeadDesc.ColorDown = System.Drawing.Color.Empty;
            this.toolSet.THeadDesc.ColorMove = System.Drawing.Color.Empty;
            this.toolSet.THeadDesc.ColorNormal = System.Drawing.Color.Empty;
            this.toolSet.THeadDesc.ColorSpace = System.Drawing.Color.Empty;
            this.toolSet.TLocation = Paway.Forms.TILocation.Left;
            this.toolSet.Trans = 150;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 328);
            this.Controls.Add(this.toolSet);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form3";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bToolStripMenuItem;
        private Forms.ToolBar toolSet;





    }
}