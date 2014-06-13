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
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem4 = new Paway.Forms.ToolItem();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.toolBar4 = new Paway.Forms.ToolBar();
            this.toolBar2 = new Paway.Forms.ToolBar();
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar1.ColorDownBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.toolBar1.ColorMoveBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.toolBar1.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar1.Font = new System.Drawing.Font("宋体", 16F);
            this.toolBar1.ForeColor = System.Drawing.Color.Black;
            this.toolBar1.ImageSize = new System.Drawing.Size(0, 0);
            this.toolBar1.IsCheckEvent = true;
            this.toolBar1.IsImageShow = false;
            toolItem1.Color = System.Drawing.Color.Empty;
            toolItem1.EndDesc = "world";
            toolItem1.HeadDesc = "hello";
            toolItem1.Rectangle = new System.Drawing.Rectangle(0, 0, 120, 120);
            toolItem1.Text = "你好\r\n世界";
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar1.Location = new System.Drawing.Point(26, 12);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(120, 120);
            this.toolBar1.TabIndex = 6;
            this.toolBar1.TLocation = Paway.Forms.TLocation.Left;
            this.toolBar1.Trans = 100;
            // 
            // toolBar4
            // 
            this.toolBar4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar4.ColorDownBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.toolBar4.ColorMoveBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.toolBar4.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar4.Font = new System.Drawing.Font("宋体", 16F);
            this.toolBar4.ForeColor = System.Drawing.Color.Black;
            this.toolBar4.ImageSize = new System.Drawing.Size(0, 0);
            this.toolBar4.IsCheckEvent = true;
            this.toolBar4.IsImageShow = false;
            toolItem2.Color = System.Drawing.Color.Empty;
            toolItem2.Rectangle = new System.Drawing.Rectangle(0, 0, 120, 120);
            toolItem2.Text = "点击进入\r\nClick To Enter";
            this.toolBar4.Items.Add(toolItem2);
            this.toolBar4.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar4.Location = new System.Drawing.Point(152, 12);
            this.toolBar4.Name = "toolBar4";
            this.toolBar4.Size = new System.Drawing.Size(120, 120);
            this.toolBar4.TabIndex = 7;
            this.toolBar4.TLocation = Paway.Forms.TLocation.Left;
            this.toolBar4.Trans = 100;
            // 
            // toolBar2
            // 
            this.toolBar2.ColorDownBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.toolBar2.ColorMoveBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.toolBar2.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar2.ForeColor = System.Drawing.Color.Black;
            this.toolBar2.IColorText = true;
            this.toolBar2.ImageSize = new System.Drawing.Size(16, 16);
            toolItem3.Color = System.Drawing.Color.Empty;
            toolItem3.Rectangle = new System.Drawing.Rectangle(0, 0, 120, 120);
            toolItem3.Text = "World";
            toolItem4.Color = System.Drawing.Color.Empty;
            toolItem4.Rectangle = new System.Drawing.Rectangle(121, 0, 120, 120);
            toolItem4.Text = "北京\r\nin this one";
            this.toolBar2.Items.Add(toolItem3);
            this.toolBar2.Items.Add(toolItem4);
            this.toolBar2.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar2.Location = new System.Drawing.Point(26, 138);
            this.toolBar2.Name = "toolBar2";
            this.toolBar2.Size = new System.Drawing.Size(240, 120);
            this.toolBar2.TabIndex = 8;
            this.toolBar2.TDirection = Paway.Forms.TDirection.Vertical;
            this.toolBar2.TLocation = Paway.Forms.TLocation.Left;
            this.toolBar2.Trans = 100;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Paway.Test.Properties.Resources.offline;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.toolBar2);
            this.Controls.Add(this.toolBar4);
            this.Controls.Add(this.toolBar1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.ToolBar toolBar1;
        private Forms.ToolBar toolBar4;
        private Forms.ToolBar toolBar2;

    }
}