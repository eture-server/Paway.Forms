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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            Paway.Forms.ToolItem toolItem9 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem8 = new Paway.Forms.ToolItem();
            this.qqButton1 = new Paway.Forms.QQButton();
            this.toolOk = new Paway.Forms.ToolBar();
            this.toolOk2 = new Paway.Forms.ToolBar();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 484);
            this.panel2.Size = new System.Drawing.Size(898, 26);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(1, 26);
            this.panel1.Size = new System.Drawing.Size(898, 512);
            // 
            // toolBar1
            // 
            this.toolBar1.ICheckEvent = true;
            toolItem1.Selete = false;
            toolItem1.Text = "上";
            toolItem2.Selete = false;
            toolItem2.Text = "下";
            toolItem3.Selete = false;
            toolItem3.Text = "左";
            toolItem4.Selete = false;
            toolItem4.Text = "右";
            toolItem5.Selete = false;
            toolItem5.Text = "中";
            toolItem6.Selete = false;
            toolItem6.Text = "色1";
            toolItem7.Selete = false;
            toolItem7.Text = "色2";
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.Items.Add(toolItem2);
            this.toolBar1.Items.Add(toolItem3);
            this.toolBar1.Items.Add(toolItem4);
            this.toolBar1.Items.Add(toolItem5);
            this.toolBar1.Items.Add(toolItem6);
            this.toolBar1.Items.Add(toolItem7);
            this.toolBar1.Size = new System.Drawing.Size(898, 66);
            // 
            // panel3
            // 
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.toolOk2);
            this.panel3.Controls.Add(this.toolOk);
            this.panel3.Controls.Add(this.qqButton1);
            this.panel3.Size = new System.Drawing.Size(898, 418);
            // 
            // qqButton1
            // 
            this.qqButton1.DownImage = ((System.Drawing.Image)(resources.GetObject("qqButton1.DownImage")));
            this.qqButton1.Image = null;
            this.qqButton1.Location = new System.Drawing.Point(234, 75);
            this.qqButton1.MoveImage = ((System.Drawing.Image)(resources.GetObject("qqButton1.MoveImage")));
            this.qqButton1.Name = "qqButton1";
            this.qqButton1.NormalImage = ((System.Drawing.Image)(resources.GetObject("qqButton1.NormalImage")));
            this.qqButton1.Size = new System.Drawing.Size(75, 28);
            this.qqButton1.TabIndex = 0;
            this.qqButton1.Text = "qqButton1";
            this.qqButton1.UseVisualStyleBackColor = false;
            // 
            // toolOk
            // 
            this.toolOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolOk.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolOk.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolOk.ICheckEvent = true;
            this.toolOk.IImageShow = false;
            this.toolOk.ImageSize = new System.Drawing.Size(0, 0);
            toolItem9.Selete = false;
            toolItem9.Text = "登陆";
            this.toolOk.Items.Add(toolItem9);
            this.toolOk.ItemSize = new System.Drawing.Size(90, 42);
            this.toolOk.ItemSpace = 5;
            this.toolOk.Location = new System.Drawing.Point(387, 117);
            this.toolOk.Name = "toolOk";
            this.toolOk.Size = new System.Drawing.Size(90, 42);
            this.toolOk.TabIndex = 22;
            this.toolOk.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolOk.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolOk.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(239)))));
            this.toolOk.TEvent = Paway.Forms.TEvent.Up;
            this.toolOk.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolOk.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolOk.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolOk.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolOk.Trans = 150;
            // 
            // toolOk2
            // 
            this.toolOk2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolOk2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolOk2.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolOk2.ICheckEvent = true;
            this.toolOk2.IImageShow = false;
            this.toolOk2.ImageSize = new System.Drawing.Size(0, 0);
            toolItem8.Selete = false;
            toolItem8.Text = "登陆";
            this.toolOk2.Items.Add(toolItem8);
            this.toolOk2.ItemSize = new System.Drawing.Size(90, 42);
            this.toolOk2.ItemSpace = 5;
            this.toolOk2.Location = new System.Drawing.Point(291, 117);
            this.toolOk2.Name = "toolOk2";
            this.toolOk2.Size = new System.Drawing.Size(90, 42);
            this.toolOk2.TabIndex = 23;
            this.toolOk2.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.toolOk2.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.toolOk2.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(239)))));
            this.toolOk2.TEvent = Paway.Forms.TEvent.Up;
            this.toolOk2.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolOk2.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolOk2.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolOk2.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolOk2.Trans = 150;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 540);
            this.IsDrawBorder = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Form2";
            this.Padding = new System.Windows.Forms.Padding(1, 26, 1, 2);
            this.Text = "Form2";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.QQButton qqButton1;
        private Forms.ToolBar toolOk;
        private Forms.ToolBar toolOk2;

    }
}