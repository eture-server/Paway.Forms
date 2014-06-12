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
            Paway.Forms.ToolItem toolItem5 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem6 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem7 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem8 = new Paway.Forms.ToolItem();
            this.toolBar3 = new Paway.Forms.ToolBar();
            this.toolBar2 = new Paway.Forms.ToolBar();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.SuspendLayout();
            // 
            // toolBar3
            // 
            this.toolBar3.ColorDownBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.toolBar3.ColorMoveBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.toolBar3.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar3.Font = new System.Drawing.Font("宋体", 12F);
            this.toolBar3.ForeColor = System.Drawing.Color.Black;
            this.toolBar3.IColorText = true;
            this.toolBar3.ImageEDirection = Paway.Forms.LDirection.Left;
            this.toolBar3.ImageSize = new System.Drawing.Size(16, 16);
            this.toolBar3.IsCheckEvent = true;
            toolItem5.ContextMenuStrip = null;
            toolItem5.Desc = "你好";
            toolItem5.Image = null;
            toolItem5.Rectangle = new System.Drawing.Rectangle(0, 0, 120, 120);
            toolItem5.Tag = null;
            toolItem5.Text = "Hello\r\n世界";
            this.toolBar3.Items.Add(toolItem5);
            this.toolBar3.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar3.Location = new System.Drawing.Point(158, 12);
            this.toolBar3.Name = "toolBar3";
            this.toolBar3.Size = new System.Drawing.Size(120, 120);
            this.toolBar3.TabIndex = 3;
            this.toolBar3.Trans = 100;
            // 
            // toolBar2
            // 
            this.toolBar2.ColorDownBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.toolBar2.ColorMoveBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.toolBar2.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar2.ForeColor = System.Drawing.Color.Black;
            this.toolBar2.IColorText = true;
            this.toolBar2.ImageEDirection = Paway.Forms.LDirection.Left;
            this.toolBar2.ImageSize = new System.Drawing.Size(16, 16);
            this.toolBar2.ItemIDirection = Paway.Forms.IDirection.Vertical;
            toolItem6.ContextMenuStrip = null;
            toolItem6.Desc = null;
            toolItem6.Image = null;
            toolItem6.Rectangle = new System.Drawing.Rectangle(0, 0, 120, 120);
            toolItem6.Tag = null;
            toolItem6.Text = "World";
            toolItem7.ContextMenuStrip = null;
            toolItem7.Desc = null;
            toolItem7.Image = null;
            toolItem7.Rectangle = new System.Drawing.Rectangle(121, 0, 120, 120);
            toolItem7.Tag = null;
            toolItem7.Text = "北京\r\nin this one";
            this.toolBar2.Items.Add(toolItem6);
            this.toolBar2.Items.Add(toolItem7);
            this.toolBar2.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar2.Location = new System.Drawing.Point(32, 130);
            this.toolBar2.Name = "toolBar2";
            this.toolBar2.Size = new System.Drawing.Size(240, 120);
            this.toolBar2.TabIndex = 1;
            this.toolBar2.Trans = 100;
            // 
            // toolBar1
            // 
            this.toolBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar1.ColorDownBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.toolBar1.ColorMoveBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.toolBar1.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar1.Font = new System.Drawing.Font("宋体", 12F);
            this.toolBar1.ForeColor = System.Drawing.Color.Black;
            this.toolBar1.ImageEDirection = Paway.Forms.LDirection.Left;
            this.toolBar1.ImageSize = new System.Drawing.Size(16, 16);
            this.toolBar1.IsCheckEvent = true;
            toolItem8.ContextMenuStrip = null;
            toolItem8.Desc = "hello";
            toolItem8.Image = null;
            toolItem8.Rectangle = new System.Drawing.Rectangle(0, 0, 120, 120);
            toolItem8.Tag = null;
            toolItem8.Text = "你好\r\nworld";
            this.toolBar1.Items.Add(toolItem8);
            this.toolBar1.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar1.Location = new System.Drawing.Point(32, 12);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(120, 120);
            this.toolBar1.TabIndex = 0;
            this.toolBar1.Trans = 100;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Paway.Test.Properties.Resources.offline;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.toolBar3);
            this.Controls.Add(this.toolBar2);
            this.Controls.Add(this.toolBar1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.ToolBar toolBar1;
        private Forms.ToolBar toolBar2;
        private Forms.ToolBar toolBar3;
    }
}