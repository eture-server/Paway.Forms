namespace Paway.Test
{
    partial class MainForm
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
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new Paway.Forms.TControl();
            this.toolTitle = new Paway.Forms.ToolBar();
            this.toolSet = new Paway.Forms.ToolBar();
            this.panel3 = new Paway.Forms.TControl();
            this.panel2 = new Paway.Forms.TControl();
            this.lbDesc = new System.Windows.Forms.Label();
            this.lbStatu = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolTitle);
            this.panel1.Controls.Add(this.toolSet);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(880, 50);
            this.panel1.TabIndex = 68;
            // 
            // toolTitle
            // 
            this.toolTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolTitle.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.toolTitle.IClickEvent = true;
            this.toolTitle.IImageShow = true;
            this.toolTitle.ImageSize = new System.Drawing.Size(42, 42);
            this.toolTitle.INormal = true;
            toolItem1.Text = "测试系统";
            this.toolTitle.Items.Add(toolItem1);
            this.toolTitle.ItemSize = new System.Drawing.Size(350, 50);
            this.toolTitle.ItemSpace = 2;
            this.toolTitle.Location = new System.Drawing.Point(0, 0);
            this.toolTitle.Name = "toolTitle";
            this.toolTitle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolTitle.Size = new System.Drawing.Size(746, 50);
            this.toolTitle.TabIndex = 14;
            this.toolTitle.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolTitle.TextFirst.ColorMove = System.Drawing.Color.Black;
            this.toolTitle.TextFirst.ColorNormal = System.Drawing.Color.White;
            this.toolTitle.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 18F);
            this.toolTitle.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 18F);
            this.toolTitle.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 18F);
            // 
            // toolSet
            // 
            this.toolSet.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolSet.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.toolSet.IAutoWidth = true;
            this.toolSet.IClickEvent = true;
            this.toolSet.IImageShow = true;
            this.toolSet.ImageSize = new System.Drawing.Size(17, 17);
            toolItem2.Image = global::Paway.Test.Properties.Resources.about;
            toolItem2.Text = "关于";
            toolItem3.Text = "设置";
            this.toolSet.Items.Add(toolItem2);
            this.toolSet.Items.Add(toolItem3);
            this.toolSet.ItemSize = new System.Drawing.Size(0, 50);
            this.toolSet.ItemSpace = 0;
            this.toolSet.Location = new System.Drawing.Point(746, 0);
            this.toolSet.Name = "toolSet";
            this.toolSet.Size = new System.Drawing.Size(134, 50);
            this.toolSet.TabIndex = 12;
            this.toolSet.TBackGround.ColorDown = System.Drawing.Color.Transparent;
            this.toolSet.TBackGround.ColorMove = System.Drawing.Color.Transparent;
            this.toolSet.TBackGround.FontDown = new System.Drawing.Font("微软雅黑", 11F);
            this.toolSet.TBackGround.FontMove = new System.Drawing.Font("微软雅黑", 11F);
            this.toolSet.TextFirst.ColorDown = System.Drawing.Color.Black;
            this.toolSet.TextFirst.ColorMove = System.Drawing.Color.Black;
            this.toolSet.TextFirst.ColorNormal = System.Drawing.Color.White;
            this.toolSet.TextFirst.FontDown = new System.Drawing.Font("微软雅黑", 11F);
            this.toolSet.TextFirst.FontMove = new System.Drawing.Font("微软雅黑", 11F);
            this.toolSet.TextFirst.FontNormal = new System.Drawing.Font("微软雅黑", 11F);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 70);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(880, 504);
            this.panel3.TabIndex = 76;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbDesc);
            this.panel2.Controls.Add(this.lbStatu);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 574);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(880, 26);
            this.panel2.TabIndex = 75;
            // 
            // lbDesc
            // 
            this.lbDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDesc.Location = new System.Drawing.Point(0, 0);
            this.lbDesc.Name = "lbDesc";
            this.lbDesc.Size = new System.Drawing.Size(625, 26);
            this.lbDesc.TabIndex = 7;
            this.lbDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbStatu
            // 
            this.lbStatu.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbStatu.Location = new System.Drawing.Point(625, 0);
            this.lbStatu.Name = "lbStatu";
            this.lbStatu.Size = new System.Drawing.Size(255, 26);
            this.lbStatu.TabIndex = 6;
            this.lbStatu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(880, 600);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ISpecial = true;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.ShowIcon = false;
            this.TBrush.ColorDown = System.Drawing.Color.Ivory;
            this.TBrush.ColorMove = System.Drawing.Color.DodgerBlue;
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Paway.Forms.ToolBar toolSet;
        private Paway.Forms.TControl panel1;
        private Paway.Forms.ToolBar toolTitle;
        private Paway.Forms.TControl panel3;
        private Paway.Forms.TControl panel2;
        private System.Windows.Forms.Label lbDesc;
        private System.Windows.Forms.Label lbStatu;
    }
}