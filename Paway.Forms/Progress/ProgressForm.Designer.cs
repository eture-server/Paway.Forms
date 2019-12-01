namespace Paway.Forms
{
    partial class ProgressForm
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Paway.Forms.ToolItem toolItem1 = new Paway.Forms.ToolItem();
            this.tControl1 = new Paway.Forms.TControl();
            this.toolCancel = new Paway.Forms.ToolBar();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lbCaption = new System.Windows.Forms.Label();
            this.tControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tControl1
            // 
            this.tControl1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tControl1.Controls.Add(this.toolCancel);
            this.tControl1.Controls.Add(this.progressBar1);
            this.tControl1.Controls.Add(this.lbCaption);
            this.tControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tControl1.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 11F);
            this.tControl1.Location = new System.Drawing.Point(1, 1);
            this.tControl1.Name = "tControl1";
            this.tControl1.Size = new System.Drawing.Size(474, 63);
            this.tControl1.TabIndex = 0;
            // 
            // toolCancel
            // 
            this.toolCancel.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 11F);
            this.toolCancel.IClickEvent = true;
            toolItem1.Text = "Cancel";
            this.toolCancel.Items.Add(toolItem1);
            this.toolCancel.ItemSize = new System.Drawing.Size(62, 20);
            this.toolCancel.ItemSpace = 0;
            this.toolCancel.Location = new System.Drawing.Point(389, 11);
            this.toolCancel.Name = "toolCancel";
            this.toolCancel.Size = new System.Drawing.Size(62, 20);
            this.toolCancel.TabIndex = 48;
            this.toolCancel.TBackGround.ColorDown = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolCancel.TBackGround.ColorMove = System.Drawing.Color.Transparent;
            this.toolCancel.TBackGround.ColorNormal = System.Drawing.Color.Transparent;
            this.toolCancel.TextFirst.ColorDown = System.Drawing.Color.White;
            this.toolCancel.TextFirst.ColorMove = System.Drawing.Color.Red;
            this.toolCancel.TextFirst.ColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolCancel.TextFirst.FontDown = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 11F);
            this.toolCancel.TextFirst.FontMove = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 11F);
            this.toolCancel.TextFirst.FontNormal = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 11F);
            this.toolCancel.TextFirst.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolCancel.TextSencond.StringVertical = System.Drawing.StringAlignment.Center;
            this.toolCancel.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(23, 32);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(429, 21);
            this.progressBar1.TabIndex = 47;
            this.progressBar1.Value = 10;
            // 
            // lbCaption
            // 
            this.lbCaption.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 10F);
            this.lbCaption.Location = new System.Drawing.Point(22, 11);
            this.lbCaption.Name = "lbCaption";
            this.lbCaption.Size = new System.Drawing.Size(366, 20);
            this.lbCaption.TabIndex = 46;
            this.lbCaption.Text = "label1";
            this.lbCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProgressForm
            // 
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(476, 65);
            this.ControlBox = false;
            this.Controls.Add(this.tControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProgressForm";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmProgress";
            this.tControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TControl tControl1;
        private ToolBar toolCancel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lbCaption;
    }
}
