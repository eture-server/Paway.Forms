using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paway.Test
{
    public partial class FormImage : Form
    {
        public FormImage()
        {
            InitializeComponent();
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            pictureBox1.Click += pictureBox1_Click;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            btClear.Click += btClear_Click;
            btColor.Click += btColor_Click;
            btSave.Click += btSave_Click;

            this.normal = BitmapHelper.GetBitmapFormFile(@"d:\p2.jpg");
            btAdd.Click += btAdd_Click;
            btLess.Click += btLess_Click;
            btChange.Click += btChange_Click;
        }


        #region 临时操作生成图片
        private Rectangle rect = new Rectangle(117, 29, 905, 558);
        void btChange_Click(object sender, EventArgs e)
        {
            if (normal == null) return;

            //Image temp = BitmapHelper.ConvertTo(normal, BConvertType.Trans, rect, 0);
            //temp.Save(@"d:\p3.jpg",ImageFormat.Jpeg);
            //this.pictureBox1.Image = temp;
        }
        void btLess_Click(object sender, EventArgs e)
        {
            //rect.X--;
            //rect.Width--;
            rect.Height--;
        }

        void btAdd_Click(object sender, EventArgs e)
        {
            //rect.X++;
            //rect.Width++;
            rect.Height++;
        }

        #endregion

        #region 取图与坐标
        private Point point;
        private Image normal;
        void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (normal == null)
            {
                point = e.Location;
            }
            else
            {
                int cx = normal.Width * e.X / pictureBox1.ClientSize.Width;
                int cy = normal.Height * e.Y / pictureBox1.ClientSize.Height;
                point = new Point(cx, cy);
            }
            this.Text = point.ToString();
        }
        void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Title = "Select Product Image",
                    Filter = "Image Files|*.gif;*.bmp;*.jpg;*.jpeg;*.png;*.tga;*.tif;*.tiff|GIF file format|*.gif|BMP file format|*.bmp|JPEG file format|*.jpg;*.jpeg|PNG file format|*.png|TGA file format|*.tga|TIFF file format|*.tif;*.tiff",
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    normal = BitmapHelper.GetBitmapFormFile(ofd.FileName);
                    this.pictureBox1.Image = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
                    ToRefresh();
                }
            }
        }

        #endregion

        #region 取色、清空、保存
        private Color color = Color.White;
        void btColor_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            color.ShowDialog(this);
            this.color = color.Color;
            this.label1.Text = this.color.ToString();
        }
        void btClear_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            normal = null;
        }
        void btSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Select Product Image",
                Filter = "Image Files|*.png",
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                normal.Save(sfd.FileName);
            }
        }

        #endregion

        #region 点击重绘
        void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (normal == null) return;
            if (pictureBox1.Image != null && color != Color.Empty)
            {
                //normal = BitmapHelper.ReplaceByPixel(normal, point.X, point.Y, Color.FromArgb(textBox1.Text.ToInt(), color.R, color.G, color.B));
                //ToRefresh();
                //pictureBox1.Refresh();
            }
        }
        private void ToRefresh()
        {
            if (normal == null) return;
            Image image = BitmapHelper.HighImage(this.normal, new Rectangle(Point.Empty, pictureBox1.Size));
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                g.DrawImageUnscaled(image, 0, 0);
            }
        }

        #endregion
    }
}
