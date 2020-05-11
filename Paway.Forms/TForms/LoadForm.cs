using Paway.Forms;
using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Paway.Forms
{
    /// <summary>
    /// 启动界面
    /// </summary>
    public partial class LoadForm : TForm, ILoadForm
    {
        /// <summary>
        /// 构造
        /// </summary>
        public LoadForm()
        {
            InitializeComponent();
            this.pictureBox2.Paint += PictureBox2_Paint;
            this.TMouseMove(this.pictureBox2);
        }
        /// <summary>
        /// 更新标题
        /// </summary>
        /// <param name="title">标题</param>
        public void Title(string title)
        {
            this.Text = title;
        }
        /// <summary>
        /// 更新描述
        /// </summary>
        /// <param name="desc">描述</param>
        public void Update(string desc)
        {
            this.label1.Text = desc;
        }
        /// <summary>
        /// 更新显示图片
        /// </summary>
        /// <param name="load">动态小图</param>
        /// <param name="load2">静态大图</param>
        public void Update(Image load, Image load2)
        {
            if (load != null)
                this.pictureBox1.Image = load;
            if (load2 != null)
                this.pictureBox2.Image = load2;
        }
        /// <summary>
        /// 设置大图显示
        /// </summary>
        /// <param name="mode">图像定位</param>
        public void Update(PictureBoxSizeMode mode)
        {
            this.pictureBox2.SizeMode = mode;
        }
        private void PictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Bitmap image = pictureBox2.Image as Bitmap;
            var color = image.GetPixel(label1.Left, label1.Right);
            color = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
            using (var solidBrush = new SolidBrush(color))
            {
                e.Graphics.DrawString(label1.Text, label1.Font, solidBrush, label1.Location);
            }
        }
    }
}
