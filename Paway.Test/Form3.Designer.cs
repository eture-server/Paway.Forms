namespace Paway.Test
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            this.tControl1 = new Paway.Forms.TControl();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.btChange = new Paway.Forms.ToolBar();
            this.tControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tControl1
            // 
            this.tControl1.Controls.Add(this.toolBar1);
            this.tControl1.Location = new System.Drawing.Point(70, 65);
            this.tControl1.Name = "tControl1";
            this.tControl1.Size = new System.Drawing.Size(191, 251);
            this.tControl1.TabIndex = 0;
            // 
            // toolBar1
            // 
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolBar1.IClickEvent = true;
            this.toolBar1.IScroll = false;
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.ItemSize = new System.Drawing.Size(191, 251);
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.MDirection = Paway.Helper.TMDirection.T3DLeftToRight;
            this.toolBar1.MInterval = 5;
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(191, 251);
            this.toolBar1.TabIndex = 1;
            this.toolBar1.TranImage = ((System.Drawing.Image)(resources.GetObject("toolBar1.TranImage")));
            this.toolBar1.TranLaterImage = ((System.Drawing.Image)(resources.GetObject("toolBar1.TranLaterImage")));
            // 
            // btChange
            // 
            this.btChange.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btChange.Font = new System.Drawing.Font("Tahoma", 15F);
            this.btChange.IClickEvent = true;
            this.btChange.TLineColor.ColorDown = System.Drawing.Color.Transparent;
            this.btChange.TLineColor.ColorMove = System.Drawing.Color.Transparent;
            this.btChange.TLineColor.ColorNormal = System.Drawing.Color.Transparent;
            this.btChange.ImageSize = new System.Drawing.Size(0, 0);
            toolItem2.Text = "Change";
            this.btChange.Items.Add(toolItem2);
            this.btChange.ItemSize = new System.Drawing.Size(75, 32);
            this.btChange.ItemSpace = 5;
            this.btChange.Location = new System.Drawing.Point(280, 147);
            this.btChange.Name = "btChange";
            this.btChange.Size = new System.Drawing.Size(75, 32);
            this.btChange.TabIndex = 66;
            this.btChange.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(94)))), ((int)(((byte)(167)))));
            this.btChange.TBackGround.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(171)))), ((int)(((byte)(244)))));
            this.btChange.TBackGround.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(135)))), ((int)(((byte)(239)))));
            this.btChange.TextFirst.ColorDown = System.Drawing.Color.White;
            this.btChange.TextFirst.ColorMove = System.Drawing.Color.White;
            this.btChange.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.btChange.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.btChange.Trans = 150;
            // 
            // Form3
            // 
            this.ClientSize = new System.Drawing.Size(384, 346);
            this.Controls.Add(this.btChange);
            this.Controls.Add(this.tControl1);
            this.Name = "Form3";
            this.TBrush.ColorDown = System.Drawing.Color.Ivory;
            this.TBrush.ColorMove = System.Drawing.Color.CornflowerBlue;
            this.Text = "你好";
            this.Trans = 155;
            this.tControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.TControl tControl1;
        private Forms.ToolBar toolBar1;
        private Forms.ToolBar btChange;
    }
}