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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.AProperties.BackGround = ((Paway.Forms.TProperties)(resources.GetObject("toolBar1.AProperties.BackGround")));
            this.toolBar1.AProperties.Desc = ((Paway.Forms.TProperties)(resources.GetObject("toolBar1.AProperties.Desc")));
            this.toolBar1.AProperties.EndDesc = ((Paway.Forms.TProperties)(resources.GetObject("toolBar1.AProperties.EndDesc")));
            this.toolBar1.AProperties.HeadDesc = ((Paway.Forms.TProperties)(resources.GetObject("toolBar1.AProperties.HeadDesc")));
            this.toolBar1.AProperties.Text = ((Paway.Forms.TProperties)(resources.GetObject("toolBar1.AProperties.Text")));
            this.toolBar1.AProperties.TextSencond = ((Paway.Forms.TProperties)(resources.GetObject("toolBar1.AProperties.TextSencond")));
            this.toolBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolBar1.Font = new System.Drawing.Font("宋体", 16F);
            this.toolBar1.ForeColor = System.Drawing.Color.Black;
            this.toolBar1.IImageShow = false;
            this.toolBar1.ImageSize = new System.Drawing.Size(0, 0);
            toolItem1.Color = System.Drawing.Color.Empty;
            toolItem1.Desc = "你好餐厅都";
            toolItem1.EndDesc = "world";
            toolItem1.First = "你好";
            toolItem1.HeadDesc = "hello";
            toolItem1.Rectangle = new System.Drawing.Rectangle(0, 0, 173, 120);
            toolItem1.Sencond = resources.GetString("toolItem1.Sencond");
            toolItem1.Tag = "";
            toolItem1.Text = "你好\r\n多餐厅都推出\r\n平板电子菜谱餐厅都";
            toolItem2.Color = System.Drawing.Color.Empty;
            toolItem2.Desc = "x2";
            toolItem2.First = "aaa";
            toolItem2.Rectangle = new System.Drawing.Rectangle(174, 0, 173, 120);
            toolItem2.Text = "aaa";
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.Items.Add(toolItem2);
            this.toolBar1.ItemSize = new System.Drawing.Size(173, 120);
            this.toolBar1.Location = new System.Drawing.Point(80, 71);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(350, 120);
            this.toolBar1.TabIndex = 10;
            this.toolBar1.TLocation = Paway.Forms.TILocation.Left;
            this.toolBar1.Trans = 100;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Paway.Test.Properties.Resources.offline;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(547, 262);
            this.Controls.Add(this.toolBar1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.ToolBar toolBar1;



    }
}