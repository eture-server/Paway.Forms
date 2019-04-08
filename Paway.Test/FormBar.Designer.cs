namespace Paway.Test
{
    partial class FormBar
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
            this.toolBar2 = new Paway.Forms.ToolBar();
            this.SuspendLayout();
            // 
            // toolBar2
            // 
            this.toolBar2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.toolBar2.ColorLine = System.Drawing.Color.Red;
            toolItem1.Image = global::Paway.Test.Properties.Resources.Delete_32x32;
            toolItem1.Text = "0\r\n创建宝贝";
            toolItem2.Text = "Hello";
            this.toolBar2.Items.Add(toolItem1);
            this.toolBar2.Items.Add(toolItem2);
            this.toolBar2.ItemSize = new System.Drawing.Size(110, 110);
            this.toolBar2.ItemSpace = 2;
            this.toolBar2.Location = new System.Drawing.Point(49, 40);
            this.toolBar2.Name = "toolBar2";
            this.toolBar2.Padding = new System.Windows.Forms.Padding(2, 3, 0, 0);
            this.toolBar2.Size = new System.Drawing.Size(276, 164);
            this.toolBar2.TabIndex = 44;
            this.toolBar2.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(160)))), ((int)(((byte)(64)))));
            this.toolBar2.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.toolBar2.TBackGround.ColorNormal = System.Drawing.SystemColors.AppWorkspace;
            this.toolBar2.TDirection = Paway.Helper.TDirection.Vertical;
            this.toolBar2.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 24F);
            this.toolBar2.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 24F);
            this.toolBar2.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 24F);
            this.toolBar2.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolBar2.TextSencond.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolBar2.TRadiu = 110;
            // 
            // FormBar
            // 
            this.ClientSize = new System.Drawing.Size(402, 261);
            this.Controls.Add(this.toolBar2);
            this.Name = "FormBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ToolBarForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.ToolBar toolBar2;
    }
}