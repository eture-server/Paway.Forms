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
            this.label1 = new System.Windows.Forms.Label();
            this.toolBar2 = new Paway.Forms.ToolBar();
            this.toolBar4 = new Paway.Forms.ToolBar();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(170, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "label1";
            // 
            // toolBar2
            // 
            this.toolBar2.ColorDownBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.toolBar2.ColorMoveBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.toolBar2.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar2.ForeColor = System.Drawing.Color.Black;
            this.toolBar2.IColorText = true;
            this.toolBar2.ImageSize = new System.Drawing.Size(16, 16);
            toolItem1.Color = System.Drawing.Color.Empty;
            toolItem1.Rectangle = new System.Drawing.Rectangle(0, 0, 120, 120);
            toolItem1.Text = "World";
            toolItem2.Color = System.Drawing.Color.Empty;
            toolItem2.Rectangle = new System.Drawing.Rectangle(121, 0, 120, 120);
            toolItem2.Text = "北京\r\nin this one";
            this.toolBar2.Items.Add(toolItem1);
            this.toolBar2.Items.Add(toolItem2);
            this.toolBar2.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar2.Location = new System.Drawing.Point(19, 134);
            this.toolBar2.Name = "toolBar2";
            this.toolBar2.Size = new System.Drawing.Size(240, 120);
            this.toolBar2.TabIndex = 11;
            this.toolBar2.TDirection = Paway.Forms.TDirection.Vertical;
            this.toolBar2.TLocation = Paway.Forms.TILocation.Left;
            this.toolBar2.Trans = 100;
            // 
            // toolBar4
            // 
            this.toolBar4.ColorDownBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.toolBar4.ColorMoveBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.toolBar4.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(175)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.toolBar4.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar4.Font = new System.Drawing.Font("宋体", 16F);
            this.toolBar4.ForeColor = System.Drawing.Color.Black;
            this.toolBar4.ICheckEvent = true;
            this.toolBar4.IColorText = true;
            this.toolBar4.IImageShow = false;
            this.toolBar4.ImageSize = new System.Drawing.Size(0, 0);
            toolItem3.Color = System.Drawing.Color.Empty;
            toolItem3.Enable = false;
            toolItem3.EndDesc = "你好";
            toolItem3.HeadDesc = "世界";
            toolItem3.Rectangle = new System.Drawing.Rectangle(0, 0, 120, 120);
            toolItem3.Text = "点击进入\r\nClick To Enter";
            this.toolBar4.Items.Add(toolItem3);
            this.toolBar4.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar4.Location = new System.Drawing.Point(145, 8);
            this.toolBar4.Name = "toolBar4";
            this.toolBar4.Size = new System.Drawing.Size(120, 120);
            this.toolBar4.TabIndex = 10;
            this.toolBar4.TLocation = Paway.Forms.TILocation.Left;
            this.toolBar4.Trans = 100;
            // 
            // toolBar1
            // 
            this.toolBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar1.ColorDownBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.toolBar1.ColorMoveBack = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            this.toolBar1.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar1.Font = new System.Drawing.Font("宋体", 16F);
            this.toolBar1.ForeColor = System.Drawing.Color.Black;
            this.toolBar1.ICheckEvent = true;
            this.toolBar1.IImageShow = false;
            this.toolBar1.ImageSize = new System.Drawing.Size(0, 0);
            toolItem4.Color = System.Drawing.Color.Empty;
            toolItem4.Enable = false;
            toolItem4.EndDesc = "world";
            toolItem4.HeadDesc = "hello";
            toolItem4.Rectangle = new System.Drawing.Rectangle(0, 0, 120, 120);
            toolItem4.Tag = "";
            toolItem4.Text = "你好\r\n多餐厅都推出\r\n平板电子菜谱\r\n顾客只需用\r\n指轻轻一划";
            this.toolBar1.Items.Add(toolItem4);
            this.toolBar1.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar1.Location = new System.Drawing.Point(19, 8);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(120, 120);
            this.toolBar1.TabIndex = 9;
            this.toolBar1.TLocation = Paway.Forms.TILocation.Left;
            this.toolBar1.Trans = 100;
            this.toolBar1.TTextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Paway.Test.Properties.Resources.offline;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolBar2);
            this.Controls.Add(this.toolBar4);
            this.Controls.Add(this.toolBar1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Forms.ToolBar toolBar2;
        private Forms.ToolBar toolBar4;
        private Forms.ToolBar toolBar1;
        private System.Windows.Forms.Label label1;


    }
}