namespace Paway.Test
{
    partial class FormQQ
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
            Paway.Forms.TreeItem treeItem1 = new Paway.Forms.TreeItem();
            Paway.Forms.TreeItem treeItem2 = new Paway.Forms.TreeItem();
            Paway.Forms.TreeItem treeItem3 = new Paway.Forms.TreeItem();
            Paway.Forms.TreeItem treeItem4 = new Paway.Forms.TreeItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQQ));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.在线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.离开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.离开ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.treeView1 = new Paway.Forms.TTreeView();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.在线ToolStripMenuItem,
            this.离开ToolStripMenuItem,
            this.toolStripSeparator1,
            this.离开ToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 76);
            // 
            // 在线ToolStripMenuItem
            // 
            this.在线ToolStripMenuItem.Name = "在线ToolStripMenuItem";
            this.在线ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.在线ToolStripMenuItem.Text = "在线";
            // 
            // 离开ToolStripMenuItem
            // 
            this.离开ToolStripMenuItem.Name = "离开ToolStripMenuItem";
            this.离开ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.离开ToolStripMenuItem.Text = "离开";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(97, 6);
            // 
            // 离开ToolStripMenuItem1
            // 
            this.离开ToolStripMenuItem1.Name = "离开ToolStripMenuItem1";
            this.离开ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.离开ToolStripMenuItem1.Text = "离开";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(198, 190);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(91, 29);
            this.button3.TabIndex = 4;
            this.button3.Text = "DeleteItem";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(198, 155);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 29);
            this.button2.TabIndex = 5;
            this.button2.Text = "AddItem";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(198, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 29);
            this.button1.TabIndex = 6;
            this.button1.Text = "UpdateItem";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.Transparent;
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            treeItem1.Name = "Statu";
            treeItem1.Type = Paway.Helper.TreeItemType.Image;
            treeItem1.Width = 16;
            treeItem2.IAlign = true;
            treeItem2.Name = "Name";
            treeItem3.Name = "Custom";
            treeItem3.Width = 60;
            treeItem4.Name = "Value";
            treeItem4.Width = 80;
            this.treeView1.Items.Add(treeItem1);
            this.treeView1.Items.Add(treeItem2);
            this.treeView1.Items.Add(treeItem3);
            this.treeView1.Items.Add(treeItem4);
            this.treeView1.Location = new System.Drawing.Point(0, 120);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(299, 286);
            this.treeView1.TabIndex = 3;
            // 
            // FormQQ
            // 
            this.BackColor = System.Drawing.Color.DodgerBlue;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(300, 486);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.treeView1);
            this.IBorder = true;
            this.Name = "FormQQ";
            this.Padding = new System.Windows.Forms.Padding(0, 120, 1, 80);
            this.SysButton = Paway.Helper.TSysButton.Close;
            this.Text = "Form2";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 在线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 离开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 离开ToolStripMenuItem1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private Forms.TTreeView treeView1;
    }
}