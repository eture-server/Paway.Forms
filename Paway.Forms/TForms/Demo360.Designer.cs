using System.ComponentModel;
namespace Paway.Forms
{
    partial class Demo360
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
            this.panel1 = new Paway.Forms.TControl();
            this.panel3 = new Paway.Forms.TControl();
            this.toolBar1 = new Paway.Forms.ToolBar();
            this.panel2 = new Paway.Forms.TControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.toolBar1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.panel1.Size = new System.Drawing.Size(900, 514);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 66);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(900, 420);
            this.panel3.TabIndex = 13;
            // 
            // toolBar1
            // 
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolBar1.ImageSize = new System.Drawing.Size(32, 32);
            this.toolBar1.ItemSize = new System.Drawing.Size(64, 66);
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.Size = new System.Drawing.Size(900, 66);
            this.toolBar1.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 486);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(900, 26);
            this.panel2.TabIndex = 2;
            // 
            // Demo360
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(900, 540);
            this.Controls.Add(this.panel1);
            this.ISpecial = true;
            this.Name = "Demo360";
            this.Padding = new System.Windows.Forms.Padding(0, 26, 0, 0);
            this.ShowIcon = false;
            this.TBrush.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(110)))), ((int)(((byte)(230)))));
            this.TBrush.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(244)))));
            this.TBrush.ColorNormal = System.Drawing.Color.CornflowerBlue;
            this.TBrush.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Text = "Demo360";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// 主控件窗口
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected Paway.Forms.TControl panel2;
        /// <summary>
        /// 主容器
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected Paway.Forms.TControl panel1;
        /// <summary>
        /// 工具栏
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected ToolBar toolBar1;
        /// <summary>
        /// 控件区
        /// </summary>
        protected Paway.Forms.TControl panel3;

    }
}