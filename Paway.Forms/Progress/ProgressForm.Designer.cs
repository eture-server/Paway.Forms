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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lbCaption = new System.Windows.Forms.Label();
            this.toolCancel = new Paway.Forms.ToolBar();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(24, 29);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(429, 21);
            this.progressBar1.TabIndex = 7;
            this.progressBar1.Value = 10;
            // 
            // lbCaption
            // 
            this.lbCaption.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 9F);
            this.lbCaption.Location = new System.Drawing.Point(24, 12);
            this.lbCaption.Name = "lbCaption";
            this.lbCaption.Size = new System.Drawing.Size(366, 16);
            this.lbCaption.TabIndex = 6;
            this.lbCaption.Text = "label1";
            this.lbCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolCancel
            // 
            this.toolCancel.IClickEvent = true;
            toolItem1.Text = "Cancel";
            this.toolCancel.Items.Add(toolItem1);
            this.toolCancel.ItemSize = new System.Drawing.Size(62, 16);
            this.toolCancel.ItemSpace = 0;
            this.toolCancel.Location = new System.Drawing.Point(390, 12);
            this.toolCancel.Name = "toolCancel";
            this.toolCancel.Size = new System.Drawing.Size(62, 16);
            this.toolCancel.TabIndex = 45;
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
            // ProgressForm
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(476, 63);
            this.ControlBox = false;
            this.Controls.Add(this.toolCancel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbCaption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmProgress";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lbCaption;
        private ToolBar toolCancel;
    }
}
