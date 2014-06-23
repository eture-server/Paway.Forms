using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        }

        int x, y;
        void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            x = e.X / 10 ;
            y = e.Y / 10 ;
            this.Text = x + "," + y;
        }

        void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (normal == null) return;
            if (pictureBox1.Image != null && color != Color.Empty)
            {
                normal = BitmapHelper.ReplaceByPixel(normal, x, y, Color.FromArgb(textBox1.Text.ToInt(), color.R, color.G, color.B));
                ToRefresh();
            }
        }

        private Color color = Color.White;
        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            color.ShowDialog(this);
            this.color = color.Color;
            this.label1.Text = this.color.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void button4_Click(object sender, EventArgs e)
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

        private Image normal;
        private Image image;
        private Graphics g;
        void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                GetImage();
            }
        }
        private void GetImage()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select Product Image",
                Filter = "Image Files|*.gif;*.bmp;*.jpg;*.jpeg;*.png;*.tga;*.tif;*.tiff|GIF file format|*.gif|BMP file format|*.bmp|JPEG file format|*.jpg;*.jpeg|PNG file format|*.png|TGA file format|*.tga|TIFF file format|*.tif;*.tiff",
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
                this.g = Graphics.FromImage(this.pictureBox1.Image);
                normal = BitmapHelper.GetBitmapFormFile(ofd.FileName);
                ToRefresh();
            }
        }
        private void ToRefresh()
        {
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                //graphics.FillRectangle(Brushes.Red, rect);
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(normal, new Rectangle(Point.Empty, pictureBox1.Size), new Rectangle(Point.Empty, normal.Size), GraphicsUnit.Pixel);
                graphics.PixelOffsetMode = PixelOffsetMode.Default;
            }
            ToRefresh2();
        }
        private void ToRefresh2()
        {
            this.g.ResetTransform();
            this.g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.pictureBox1.Image.Width, this.pictureBox1.Image.Height);
            this.g.TranslateTransform(0, 0);
            this.g.DrawImageUnscaled(this.image, 0, 0, this.pictureBox1.Image.Width, this.pictureBox1.Image.Height);
            this.g.TranslateTransform(0, 0);
            this.pictureBox1.Refresh();
        }
    }
}
