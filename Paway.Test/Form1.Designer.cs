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
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem4 = new Paway.Forms.ToolItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btName = new Paway.Forms.QQButton();
            this.toolBar = new Paway.Forms.ToolBar();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
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
            toolItem3.ContextMenuStrip = this.contextMenuStrip2;
            toolItem3.Desc = "x1";
            toolItem3.EndDesc = "";
            toolItem3.Image = global::Paway.Test.Properties.Resources.Delete_32x32;
            toolItem3.Selete = false;
            toolItem3.Text = "呵呵0呵呵1呵呵2呵呵3呵呵4\r\n呵呵5呵呵6呵呵7呵呵8呵呵9";
            toolItem4.Desc = "呵呵";
            toolItem4.EndDesc = "";
            toolItem4.Selete = false;
            toolItem4.Text = "你好";
            this.toolBar.Items.Add(toolItem3);
            this.toolBar.Items.Add(toolItem4);
            this.toolBar.ItemSize = new System.Drawing.Size(120, 120);
            this.toolBar.ItemSpace = 5;
            this.toolBar.Location = new System.Drawing.Point(35, 63);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(266, 138);
            this.toolBar.TabIndex = 31;
            this.toolBar.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(50)))), ((int)(((byte)(27)))));
            this.toolBar.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(127)))), ((int)(((byte)(104)))));
            this.toolBar.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(72)))), ((int)(((byte)(38)))));
            this.toolBar.TDesc.ColorDown = System.Drawing.Color.White;
            this.toolBar.TDesc.ColorMove = System.Drawing.Color.White;
            this.toolBar.TEvent = Paway.Forms.TEvent.Up;
            this.toolBar.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolBar.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolBar.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolBar.Trans = 150;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Paway.Test.Properties.Resources.imgLoadding_Image;
            this.pictureBox2.Location = new System.Drawing.Point(5, 212);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(309, 2);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 32;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::Paway.Test.Properties.Resources.noon;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(319, 222);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.btName);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.White;
            this.IsDrawBorder = true;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(3, 26, 3, 3);
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





    }
}