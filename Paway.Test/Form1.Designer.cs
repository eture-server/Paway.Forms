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
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolClose = new Paway.Forms.ToolBar();
            this.btName = new Paway.Forms.QQButton();
            this.tbName = new Paway.Forms.QQTextBox();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            // toolClose
            // 
            this.toolClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolClose.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolClose.ForeColor = System.Drawing.Color.Black;
            this.toolClose.ICheckEvent = true;
            this.toolClose.ImageSize = new System.Drawing.Size(32, 32);
            toolItem1.ContextMenuStrip = this.contextMenuStrip2;
            toolItem1.Desc = "x1";
            toolItem1.EndDesc = "";
            toolItem1.Image = global::Paway.Test.Properties.Resources.Delete_32x32;
            toolItem1.Selete = false;
            toolItem1.Text = "呵呵0呵呵1呵呵2呵呵3呵呵4\r\n呵呵5呵呵6呵呵7呵呵8呵呵9";
            toolItem2.Desc = "呵呵";
            toolItem2.EndDesc = "";
            toolItem2.Selete = false;
            toolItem2.Text = "你好";
            this.toolClose.Items.Add(toolItem1);
            this.toolClose.Items.Add(toolItem2);
            this.toolClose.ItemSize = new System.Drawing.Size(120, 120);
            this.toolClose.ItemSpace = 5;
            this.toolClose.Location = new System.Drawing.Point(50, 121);
            this.toolClose.Name = "toolClose";
            this.toolClose.Size = new System.Drawing.Size(265, 150);
            this.toolClose.TabIndex = 31;
            this.toolClose.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(50)))), ((int)(((byte)(27)))));
            this.toolClose.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(127)))), ((int)(((byte)(104)))));
            this.toolClose.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(72)))), ((int)(((byte)(38)))));
            this.toolClose.TDesc.ColorDown = System.Drawing.Color.White;
            this.toolClose.TDesc.ColorMove = System.Drawing.Color.White;
            this.toolClose.TEvent = Paway.Forms.TEvent.Up;
            this.toolClose.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolClose.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolClose.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolClose.Trans = 150;
            // 
            // btName
            // 
            this.btName.DownImage = ((System.Drawing.Image)(resources.GetObject("btName.DownImage")));
            this.btName.ForeColor = System.Drawing.Color.Black;
            this.btName.Image = null;
            this.btName.Location = new System.Drawing.Point(110, 75);
            this.btName.MoveImage = ((System.Drawing.Image)(resources.GetObject("btName.MoveImage")));
            this.btName.Name = "btName";
            this.btName.NormalImage = ((System.Drawing.Image)(resources.GetObject("btName.NormalImage")));
            this.btName.Size = new System.Drawing.Size(59, 28);
            this.btName.TabIndex = 30;
            this.btName.Text = "hello";
            this.btName.UseVisualStyleBackColor = false;
            // 
            // tbName
            // 
            this.tbName.Icon = null;
            this.tbName.IsPasswordChat = '\0';
            this.tbName.Lines = new string[0];
            this.tbName.Location = new System.Drawing.Point(99, 45);
            this.tbName.Name = "tbName";
            this.tbName.RegexType = Paway.Helper.RegexType.Normal;
            this.tbName.SelectedText = "";
            this.tbName.Size = new System.Drawing.Size(120, 24);
            this.tbName.TabIndex = 29;
            this.tbName.WaterText = "你好";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackgroundImage = global::Paway.Test.Properties.Resources.i1;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(399, 275);
            this.Controls.Add(this.toolClose);
            this.Controls.Add(this.btName);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolAdd;
        private System.Windows.Forms.ToolStripMenuItem toolDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Forms.QQTextBox tbName;
        private Forms.QQButton btName;
        private Forms.ToolBar toolClose;





    }
}