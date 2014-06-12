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
            Paway.Forms.ToolItem toolItem7 = new Paway.Forms.ToolItem();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.BackColor = System.Drawing.Color.LightSalmon;
            this.toolBar1.ColorDownBack = System.Drawing.Color.DeepSkyBlue;
            this.toolBar1.ColorMoveBack = System.Drawing.Color.LightBlue;
            this.toolBar1.ColorSpace = System.Drawing.Color.White;
            this.toolBar1.ImageSize = new System.Drawing.Size(16, 16);
            this.toolBar1.IsImageShow = false;
            this.toolBar1.IsMultiple = true;
            toolItem1.Color = System.Drawing.Color.Maroon;
            toolItem1.ContextMenuStrip = null;
            toolItem1.EndDesc = null;
            toolItem1.HeadDesc = null;
            toolItem1.Image = global::Paway.Test.Properties.Resources.Delete_32x32;
            toolItem1.Rectangle = new System.Drawing.Rectangle(0, 0, 93, 34);
            toolItem1.Tag = null;
            toolItem1.Text = "创建宝贝";
            toolItem2.Color = System.Drawing.Color.Empty;
            toolItem2.ContextMenuStrip = null;
            toolItem2.EndDesc = null;
            toolItem2.HeadDesc = null;
            toolItem2.Image = null;
            toolItem2.Rectangle = new System.Drawing.Rectangle(98, 0, 93, 34);
            toolItem2.Tag = null;
            toolItem2.Text = "上传宝贝";
            toolItem3.Color = System.Drawing.Color.Empty;
            toolItem3.ContextMenuStrip = null;
            toolItem3.EndDesc = null;
            toolItem3.HeadDesc = null;
            toolItem3.Image = null;
            toolItem3.Rectangle = new System.Drawing.Rectangle(196, 0, 93, 34);
            toolItem3.Tag = null;
            toolItem3.Text = "Hello";
            toolItem4.Color = System.Drawing.Color.Empty;
            toolItem4.ContextMenuStrip = null;
            toolItem4.EndDesc = null;
            toolItem4.HeadDesc = null;
            toolItem4.Image = null;
            toolItem4.Rectangle = new System.Drawing.Rectangle(0, 39, 93, 34);
            toolItem4.Tag = null;
            toolItem4.Text = "World";
            toolItem5.Color = System.Drawing.Color.Empty;
            toolItem5.ContextMenuStrip = null;
            toolItem5.EndDesc = null;
            toolItem5.HeadDesc = null;
            toolItem5.Image = null;
            toolItem5.Rectangle = new System.Drawing.Rectangle(98, 39, 93, 34);
            toolItem5.Tag = null;
            toolItem5.Text = "5";
            toolItem6.Color = System.Drawing.Color.Empty;
            toolItem6.ContextMenuStrip = null;
            toolItem6.EndDesc = null;
            toolItem6.HeadDesc = null;
            toolItem6.Image = null;
            toolItem6.Rectangle = new System.Drawing.Rectangle(196, 39, 93, 34);
            toolItem6.Tag = null;
            toolItem6.Text = "6";
            toolItem7.Color = System.Drawing.Color.Empty;
            toolItem7.ContextMenuStrip = null;
            toolItem7.EndDesc = null;
            toolItem7.HeadDesc = null;
            toolItem7.Image = null;
            toolItem7.Rectangle = new System.Drawing.Rectangle(0, 78, 93, 34);
            toolItem7.Tag = null;
            toolItem7.Text = "7";
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.Items.Add(toolItem2);
            this.toolBar1.Items.Add(toolItem3);
            this.toolBar1.Items.Add(toolItem4);
            this.toolBar1.Items.Add(toolItem5);
            this.toolBar1.Items.Add(toolItem6);
            this.toolBar1.Items.Add(toolItem7);
            this.toolBar1.ItemSize = new System.Drawing.Size(93, 34);
            this.toolBar1.ItemSpace = 5;
            this.toolBar1.Location = new System.Drawing.Point(35, 54);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ProgressImage = global::Paway.Test.Properties.Resources.process;
            this.toolBar1.Size = new System.Drawing.Size(290, 113);
            this.toolBar1.TabIndex = 19;
            this.toolBar1.TEvent = Paway.Forms.TEvent.Up;
            this.toolBar1.TLocation = Paway.Forms.TLocation.Left;
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