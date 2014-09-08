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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem4 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem5 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem6 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem7 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem8 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem9 = new Paway.Forms.ToolItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btName = new Paway.Forms.QQButton();
            this.toolBar = new Paway.Forms.ToolBar();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tbName = new Paway.Forms.QQTextBox();
            this.tbRsa2 = new Paway.Forms.QQTextBox();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAdd,
            this.toolDelete});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(101, 48);
            // 
            // toolAdd
            // 
            this.toolAdd.Name = "toolAdd";
            this.toolAdd.Size = new System.Drawing.Size(100, 22);
            this.toolAdd.Text = "添加";
            // 
            // toolDelete
            // 
            this.toolDelete.Name = "toolDelete";
            this.toolDelete.Size = new System.Drawing.Size(100, 22);
            this.toolDelete.Text = "删除";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(5, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1, 1);
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // btName
            // 
            this.btName.DownImage = ((System.Drawing.Image)(resources.GetObject("btName.DownImage")));
            this.btName.Image = null;
            this.btName.Location = new System.Drawing.Point(35, 25);
            this.btName.MoveImage = ((System.Drawing.Image)(resources.GetObject("btName.MoveImage")));
            this.btName.Name = "btName";
            this.btName.NormalImage = ((System.Drawing.Image)(resources.GetObject("btName.NormalImage")));
            this.btName.Size = new System.Drawing.Size(64, 32);
            this.btName.TabIndex = 30;
            this.btName.Text = "hello";
            this.btName.UseVisualStyleBackColor = false;
            // 
            // toolBar
            // 
            this.toolBar.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolBar.ICheckEvent = true;
            this.toolBar.ImageSize = new System.Drawing.Size(32, 32);
            toolItem1.IHeard = true;
            toolItem1.Selete = false;
            toolItem1.TColor.StringVertical = System.Drawing.StringAlignment.Center;
            toolItem1.Text = "白酒";
            toolItem2.Desc = "";
            toolItem2.EndDesc = "";
            toolItem2.HeadDesc = "呵呵";
            toolItem2.Selete = false;
            toolItem2.Text = "头部";
            toolItem3.IHeard = true;
            toolItem3.Selete = false;
            toolItem3.Text = "1";
            toolItem4.Selete = false;
            toolItem4.Text = "2";
            toolItem5.Selete = false;
            toolItem5.Text = "3";
            toolItem6.IHeard = true;
            toolItem6.Selete = false;
            toolItem6.Text = "4";
            toolItem7.Selete = false;
            toolItem7.Text = "5";
            toolItem8.Selete = false;
            toolItem8.Text = "6";
            toolItem9.Selete = false;
            toolItem9.Text = "7";
            this.toolBar.Items.Add(toolItem1);
            this.toolBar.Items.Add(toolItem2);
            this.toolBar.Items.Add(toolItem3);
            this.toolBar.Items.Add(toolItem4);
            this.toolBar.Items.Add(toolItem5);
            this.toolBar.Items.Add(toolItem6);
            this.toolBar.Items.Add(toolItem7);
            this.toolBar.Items.Add(toolItem8);
            this.toolBar.Items.Add(toolItem9);
            this.toolBar.ItemSize = new System.Drawing.Size(128, 120);
            this.toolBar.ItemSpace = 5;
            this.toolBar.Location = new System.Drawing.Point(35, 63);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(342, 174);
            this.toolBar.TabIndex = 31;
            this.toolBar.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(50)))), ((int)(((byte)(27)))));
            this.toolBar.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(127)))), ((int)(((byte)(104)))));
            this.toolBar.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(72)))), ((int)(((byte)(38)))));
            this.toolBar.TDesc.ColorDown = System.Drawing.Color.White;
            this.toolBar.TDesc.ColorMove = System.Drawing.Color.White;
            this.toolBar.TDirection = Paway.Forms.TDirection.Vertical;
            this.toolBar.TEvent = Paway.Forms.TEvent.Up;
            this.toolBar.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolBar.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolBar.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolBar.TextFirst.StringHorizontal = System.Drawing.StringAlignment.Center;
            this.toolBar.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolBar.Trans = 150;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Paway.Test.Properties.Resources.imgLoadding_Image;
            this.pictureBox2.Location = new System.Drawing.Point(5, 284);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(510, 2);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 32;
            this.pictureBox2.TabStop = false;
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tbName.Icon = global::Paway.Test.Properties.Resources.serrch;
            this.tbName.IconIsButton = true;
            this.tbName.IsPasswordChat = '\0';
            this.tbName.Lines = new string[0];
            this.tbName.Location = new System.Drawing.Point(310, 27);
            this.tbName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbName.MaxLength = 10;
            this.tbName.Name = "tbName";
            this.tbName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.tbName.Regex = "";
            this.tbName.RegexType = Paway.Helper.RegexType.Normal;
            this.tbName.SelectedText = "";
            this.tbName.Size = new System.Drawing.Size(78, 29);
            this.tbName.TabIndex = 59;
            this.tbName.WaterText = "";
            // 
            // tbRsa2
            // 
            this.tbRsa2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tbRsa2.Icon = null;
            this.tbRsa2.IsPasswordChat = '\0';
            this.tbRsa2.Lines = new string[0];
            this.tbRsa2.Location = new System.Drawing.Point(192, 29);
            this.tbRsa2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbRsa2.MaxLength = 128;
            this.tbRsa2.Name = "tbRsa2";
            this.tbRsa2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.tbRsa2.Regex = "[0-9]+(\\.[0-9]+)?";
            this.tbRsa2.RegexType = Paway.Helper.RegexType.Custom;
            this.tbRsa2.SelectedText = "";
            this.tbRsa2.Size = new System.Drawing.Size(94, 29);
            this.tbRsa2.TabIndex = 60;
            this.tbRsa2.WaterText = "请输入Rsa值";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(77)))), ((int)(((byte)(185)))));
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(523, 292);
            this.Controls.Add(this.tbRsa2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.btName);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.White;
            this.IsDrawBorder = true;
            this.IsResize = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(0, 26, 3, 3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolAdd;
        private System.Windows.Forms.ToolStripMenuItem toolDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Forms.QQButton btName;
        private Forms.ToolBar toolBar;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Forms.QQTextBox tbName;
        private Forms.QQTextBox tbRsa2;





    }
}