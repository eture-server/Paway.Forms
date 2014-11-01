namespace Paway.Test.UI
{
    partial class Control2
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Control2));
            Paway.Forms.ToolItem toolItem3 = new Paway.Forms.ToolItem();
            Paway.Forms.ToolItem toolItem4 = new Paway.Forms.ToolItem();
            this.label1 = new System.Windows.Forms.Label();
            this.qqTextBox1 = new Paway.Forms.QQTextBox();
            this.btQQDemo = new Paway.Forms.QQButton();
            this.toolBar2 = new Paway.Forms.ToolBar();
            this.tbPrompt = new Paway.Forms.QQTextBox();
            this.btImage = new Paway.Forms.QQButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 21F);
            this.label1.ForeColor = System.Drawing.Color.HotPink;
            this.label1.Location = new System.Drawing.Point(50, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // qqTextBox1
            // 
            this.qqTextBox1.Icon = null;
            this.qqTextBox1.IsPasswordChat = '\0';
            this.qqTextBox1.Lines = new string[] {
        "qqTextBox1"};
            this.qqTextBox1.Location = new System.Drawing.Point(195, 156);
            this.qqTextBox1.Name = "qqTextBox1";
            this.qqTextBox1.SelectedText = "";
            this.qqTextBox1.Size = new System.Drawing.Size(109, 25);
            this.qqTextBox1.TabIndex = 52;
            this.qqTextBox1.WaterText = "";
            // 
            // btQQDemo
            // 
            this.btQQDemo.DownImage = ((System.Drawing.Image)(resources.GetObject("btQQDemo.DownImage")));
            this.btQQDemo.Image = null;
            this.btQQDemo.Location = new System.Drawing.Point(310, 156);
            this.btQQDemo.MoveImage = ((System.Drawing.Image)(resources.GetObject("btQQDemo.MoveImage")));
            this.btQQDemo.Name = "btQQDemo";
            this.btQQDemo.NormalImage = ((System.Drawing.Image)(resources.GetObject("btQQDemo.NormalImage")));
            this.btQQDemo.Size = new System.Drawing.Size(109, 28);
            this.btQQDemo.TabIndex = 51;
            this.btQQDemo.Text = "QQDemo";
            this.btQQDemo.UseVisualStyleBackColor = false;
            // 
            // toolBar2
            // 
            this.toolBar2.ICheckEvent = true;
            this.toolBar2.ImageSize = new System.Drawing.Size(16, 16);
            toolItem3.Image = ((System.Drawing.Image)(resources.GetObject("toolItem3.Image")));
            toolItem3.Selete = false;
            toolItem3.Text = "创建宝贝";
            toolItem4.Selete = false;
            toolItem4.Text = "上传宝贝";
            this.toolBar2.Items.Add(toolItem3);
            this.toolBar2.Items.Add(toolItem4);
            this.toolBar2.ItemSize = new System.Drawing.Size(116, 34);
            this.toolBar2.ItemSpace = 0;
            this.toolBar2.Location = new System.Drawing.Point(319, 26);
            this.toolBar2.Name = "toolBar2";
            this.toolBar2.Size = new System.Drawing.Size(116, 93);
            this.toolBar2.TabIndex = 50;
            this.toolBar2.TDirection = Paway.Forms.TDirection.Vertical;
            this.toolBar2.TEvent = Paway.Forms.TEvent.Up;
            this.toolBar2.TLocation = Paway.Forms.TILocation.Left;
            // 
            // tbPrompt
            // 
            this.tbPrompt.BackColor = System.Drawing.Color.White;
            this.tbPrompt.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPrompt.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbPrompt.Icon = null;
            this.tbPrompt.IsPasswordChat = '\0';
            this.tbPrompt.Lines = new string[] {
        "请输入类目关键字"};
            this.tbPrompt.Location = new System.Drawing.Point(93, 216);
            this.tbPrompt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbPrompt.Name = "tbPrompt";
            this.tbPrompt.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.tbPrompt.ReadOnly = true;
            this.tbPrompt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tbPrompt.SelectedText = "";
            this.tbPrompt.Size = new System.Drawing.Size(261, 24);
            this.tbPrompt.TabIndex = 49;
            this.tbPrompt.WaterText = "";
            // 
            // btImage
            // 
            this.btImage.DownImage = ((System.Drawing.Image)(resources.GetObject("btImage.DownImage")));
            this.btImage.Image = null;
            this.btImage.Location = new System.Drawing.Point(232, 247);
            this.btImage.MoveImage = ((System.Drawing.Image)(resources.GetObject("btImage.MoveImage")));
            this.btImage.Name = "btImage";
            this.btImage.NormalImage = ((System.Drawing.Image)(resources.GetObject("btImage.NormalImage")));
            this.btImage.Size = new System.Drawing.Size(59, 28);
            this.btImage.TabIndex = 48;
            this.btImage.Text = "Image";
            this.btImage.UseVisualStyleBackColor = false;
            // 
            // Control2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.qqTextBox1);
            this.Controls.Add(this.btQQDemo);
            this.Controls.Add(this.toolBar2);
            this.Controls.Add(this.tbPrompt);
            this.Controls.Add(this.btImage);
            this.Controls.Add(this.label1);
            this.Name = "Control2";
            this.Size = new System.Drawing.Size(529, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Forms.QQTextBox qqTextBox1;
        private Forms.QQButton btQQDemo;
        private Forms.ToolBar toolBar2;
        private Forms.QQTextBox tbPrompt;
        private Forms.QQButton btImage;
    }
}
