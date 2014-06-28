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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolClose = new Paway.Forms.ToolBar();
            this.qqTextBox1 = new Paway.Forms.QQTextBox();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAdd,
            this.toolDelete});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(99, 48);
            // 
            // toolAdd
            // 
            this.toolAdd.Name = "toolAdd";
            this.toolAdd.Size = new System.Drawing.Size(98, 22);
            this.toolAdd.Text = "添加";
            // 
            // toolDelete
            // 
            this.toolDelete.Name = "toolDelete";
            this.toolDelete.Size = new System.Drawing.Size(98, 22);
            this.toolDelete.Text = "删除";
            // 
            // toolClose
            // 
            this.toolClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolClose.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolClose.ForeColor = System.Drawing.Color.Black;
            this.toolClose.ICheckEvent = true;
            this.toolClose.IImageShow = false;
            this.toolClose.ImageSize = new System.Drawing.Size(0, 0);
            toolItem1.First = "关闭系统";
            toolItem1.Text = "关闭系统";
            this.toolClose.Items.Add(toolItem1);
            this.toolClose.ItemSize = new System.Drawing.Size(120, 120);
            this.toolClose.ItemSpace = 5;
            this.toolClose.Location = new System.Drawing.Point(133, 69);
            this.toolClose.Name = "toolClose";
            this.toolClose.Size = new System.Drawing.Size(120, 120);
            this.toolClose.TabIndex = 13;
            this.toolClose.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(50)))), ((int)(((byte)(27)))));
            this.toolClose.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(127)))), ((int)(((byte)(104)))));
            this.toolClose.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(72)))), ((int)(((byte)(38)))));
            this.toolClose.TEvent = Paway.Forms.TEvent.Up;
            this.toolClose.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolClose.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolClose.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolClose.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolClose.Trans = 150;
            // 
            // qqTextBox1
            // 
            this.qqTextBox1.Icon = null;
            this.qqTextBox1.IsPasswordChat = '\0';
            this.qqTextBox1.Lines = new string[0];
            this.qqTextBox1.Location = new System.Drawing.Point(133, 30);
            this.qqTextBox1.Name = "qqTextBox1";
            this.qqTextBox1.RegexType = Paway.Helper.RegexType.Normal;
            this.qqTextBox1.SelectedText = "";
            this.qqTextBox1.Size = new System.Drawing.Size(120, 24);
            this.qqTextBox1.TabIndex = 14;
            this.qqTextBox1.WaterText = "你好";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(394, 275);
            this.Controls.Add(this.qqTextBox1);
            this.Controls.Add(this.toolClose);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolAdd;
        private System.Windows.Forms.ToolStripMenuItem toolDelete;
        private Forms.ToolBar toolClose;
        private Forms.QQTextBox qqTextBox1;





    }
}