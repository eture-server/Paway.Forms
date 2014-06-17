namespace Paway.Test
{
    partial class Form2
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
            Paway.Forms.ToolItem toolItem5 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem6 = new Paway.Forms.ToolItem();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.AutoScroll = true;
            this.toolBar1.ICheckEvent = true;
            this.toolBar1.IImageShow = false;
            this.toolBar1.ImageSize = new System.Drawing.Size(16, 16);
            toolItem1.Color = System.Drawing.Color.Maroon;
            toolItem1.Image = global::Paway.Test.Properties.Resources.Delete_32x32;
            toolItem1.Rectangle = new System.Drawing.Rectangle(0, 0, 93, 34);
            toolItem1.Text = "创建宝贝";
            toolItem2.Color = System.Drawing.Color.Empty;
            toolItem2.Rectangle = new System.Drawing.Rectangle(98, 0, 93, 34);
            toolItem2.Text = "上传宝贝";
            toolItem3.Color = System.Drawing.Color.Empty;
            toolItem3.Rectangle = new System.Drawing.Rectangle(196, 0, 93, 34);
            toolItem3.Text = "Hello";
            toolItem4.Color = System.Drawing.Color.Empty;
            toolItem4.Rectangle = new System.Drawing.Rectangle(0, 39, 93, 34);
            toolItem4.Text = "World";
            toolItem5.Color = System.Drawing.Color.Empty;
            toolItem5.Rectangle = new System.Drawing.Rectangle(98, 39, 93, 34);
            toolItem5.Text = "5";
            toolItem6.Color = System.Drawing.Color.Empty;
            toolItem6.Rectangle = new System.Drawing.Rectangle(196, 39, 93, 34);
            toolItem6.Text = "6";
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.Items.Add(toolItem2);
            this.toolBar1.Items.Add(toolItem3);
            this.toolBar1.Items.Add(toolItem4);
            this.toolBar1.Items.Add(toolItem5);
            this.toolBar1.Items.Add(toolItem6);
            this.toolBar1.ItemSize = new System.Drawing.Size(93, 34);
            this.toolBar1.ItemSpace = 5;
            this.toolBar1.Location = new System.Drawing.Point(33, 12);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ProgressImage = global::Paway.Test.Properties.Resources.process;
            this.toolBar1.Size = new System.Drawing.Size(302, 82);
            this.toolBar1.TabIndex = 19;
            this.toolBar1.TEvent = Paway.Forms.TEvent.Up;
            this.toolBar1.TLocation = Paway.Forms.TILocation.Left;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(405, 265);
            this.Controls.Add(this.toolBar1);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.ToolBar toolBar1;
    }
}