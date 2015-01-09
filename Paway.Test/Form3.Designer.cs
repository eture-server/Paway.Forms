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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            this.tControl1 = new Paway.Forms.TControl();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.btChange = new Paway.Forms.QQButton();
            this.tControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tControl1
            // 
            this.tControl1.BackColor = System.Drawing.Color.Gainsboro;
            this.tControl1.Controls.Add(this.toolBar1);
            this.tControl1.Location = new System.Drawing.Point(70, 65);
            this.tControl1.Name = "tControl1";
            this.tControl1.Size = new System.Drawing.Size(191, 251);
            this.tControl1.TabIndex = 0;
            // 
            // toolBar1
            // 
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolBar1.DownImage = ((System.Drawing.Image)(resources.GetObject("toolBar1.DownImage")));
            this.toolBar1.ICheckEvent = true;
            this.toolBar1.IScroll = false;
            this.toolBar1.IScrollHide = false;
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.ItemSize = new System.Drawing.Size(191, 251);
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.MDirection = Paway.Forms.TMDirection.Left;
            this.toolBar1.MoveImage = ((System.Drawing.Image)(resources.GetObject("toolBar1.MoveImage")));
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.NormalImage = global::Paway.Test.Properties.Resources.noon;
            this.toolBar1.Size = new System.Drawing.Size(191, 251);
            this.toolBar1.TabIndex = 1;
            // 
            // btChange
            // 
            this.btChange.DownImage = ((System.Drawing.Image)(resources.GetObject("btChange.DownImage")));
            this.btChange.Image = null;
            this.btChange.Location = new System.Drawing.Point(297, 170);
            this.btChange.MoveImage = ((System.Drawing.Image)(resources.GetObject("btChange.MoveImage")));
            this.btChange.Name = "btChange";
            this.btChange.NormalImage = ((System.Drawing.Image)(resources.GetObject("btChange.NormalImage")));
            this.btChange.Size = new System.Drawing.Size(75, 28);
            this.btChange.TabIndex = 1;
            this.btChange.Text = "Change";
            this.btChange.UseVisualStyleBackColor = false;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.ClientSize = new System.Drawing.Size(384, 346);
            this.Controls.Add(this.btChange);
            this.Controls.Add(this.tControl1);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Form3";
            this.Text = "Form3";
            this.tControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.TControl tControl1;
        private Forms.ToolBar toolBar1;
        private Forms.QQButton btChange;
    }
}