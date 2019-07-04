namespace Paway.Test
{
    partial class FormEmit
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
            this.tControl1 = new Paway.Forms.TControl();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.testControl1 = new Paway.Test.TestControl();
            this.tControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tControl1
            // 
            this.tControl1.Controls.Add(this.toolBar1);
            this.tControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tControl1.Location = new System.Drawing.Point(1, 26);
            this.tControl1.Name = "tControl1";
            this.tControl1.Size = new System.Drawing.Size(675, 48);
            this.tControl1.TabIndex = 67;
            // 
            // toolBar1
            // 
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolBar1.Font = new System.Drawing.Font("Tahoma", 15F);
            this.toolBar1.IClickEvent = true;
            this.toolBar1.ImageSize = new System.Drawing.Size(0, 0);
            toolItem1.Text = "hello";
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.ItemSize = new System.Drawing.Size(100, 48);
            this.toolBar1.ItemSpace = 5;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(100, 48);
            this.toolBar1.TabIndex = 70;
            this.toolBar1.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolBar1.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolBar1.TextFirst.ColorNormal = System.Drawing.Color.Black;
            this.toolBar1.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolBar1.TLineColor.ColorDown = System.Drawing.Color.Transparent;
            this.toolBar1.TLineColor.ColorMove = System.Drawing.Color.Transparent;
            this.toolBar1.TLineColor.ColorNormal = System.Drawing.Color.Transparent;
            this.toolBar1.Trans = 150;
            // 
            // testControl1
            // 
            this.testControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testControl1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.testControl1.Location = new System.Drawing.Point(1, 74);
            this.testControl1.MInterval = 6;
            this.testControl1.Name = "testControl1";
            this.testControl1.Size = new System.Drawing.Size(675, 292);
            this.testControl1.TabIndex = 68;
            // 
            // FormEmit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 367);
            this.Controls.Add(this.testControl1);
            this.Controls.Add(this.tControl1);
            this.Name = "FormEmit";
            this.Text = "Form5";
            this.tControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.TControl tControl1;
        private Forms.ToolBar toolBar1;
        private TestControl testControl1;
    }
}