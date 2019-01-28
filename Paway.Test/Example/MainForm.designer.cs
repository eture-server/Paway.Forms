namespace Paway.Test
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem2 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolPad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(1, 527);
            this.panel2.Size = new System.Drawing.Size(848, 26);
            // 
            // panel1
            // 
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(850, 554);
            // 
            // toolBar1
            // 
            toolItem1.Text = "主页";
            toolItem2.Text = "用户";
            toolItem3.Text = "设置";
            this.toolBar1.Items.Add(toolItem1);
            this.toolBar1.Items.Add(toolItem2);
            this.toolBar1.Items.Add(toolItem3);
            this.toolBar1.ItemSize = new System.Drawing.Size(64, 52);
            this.toolBar1.Location = new System.Drawing.Point(1, 1);
            this.toolBar1.Size = new System.Drawing.Size(848, 52);
            this.toolBar1.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolBar1.TextFirst.ColorMove = System.Drawing.Color.White;
            this.toolBar1.TextFirst.ColorNormal = System.Drawing.Color.White;
            this.toolBar1.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(1, 53);
            this.panel3.Size = new System.Drawing.Size(848, 474);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolPad,
            this.toolStripSeparator1,
            this.toolAbout});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 62);
            // 
            // toolPad
            // 
            this.toolPad.Name = "toolPad";
            this.toolPad.Size = new System.Drawing.Size(144, 26);
            this.toolPad.Text = "修改密码";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(141, 6);
            // 
            // toolAbout
            // 
            this.toolAbout.Name = "toolAbout";
            this.toolAbout.Size = new System.Drawing.Size(144, 26);
            this.toolAbout.Text = "关于我们";
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(149)))), ((int)(((byte)(237)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(850, 580);
            this.ContextMenuShow = this.contextMenuStrip1;
            this.IBorder = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "MainForm";
            this.TBrush.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(110)))), ((int)(((byte)(230)))));
            this.TBrush.ColorMove = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(244)))));
            this.TBrush.ColorNormal = System.Drawing.Color.CornflowerBlue;
            this.TBrush.ColorSpace = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Text = "";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolPad;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolAbout;

    }
}

