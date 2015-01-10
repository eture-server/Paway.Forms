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
            tPictureBox1.Click += pictureBox1_Click;
            tPictureBox1.MouseDown += pictureBox1_MouseDown;
            btClear.Click += btClear_Click;
            btColor.Click += btColor_Click;
            btSave.Click += btSave_Click;

            btAdd.Click += btAdd_Click;
            btLess.Click += btLess_Click;
            btChange.Click += btChange_Click;

            //ToSave();
            //CreateRoundedCorner(@"C:\Users\Tinn\Desktop\pk\d1.png", @"d:\2.png", null);
        }

        #region 替换图片圆角颜色
        private void ToSave()
        {
            //string name = "d";
            //ToSave(name);
            //name = "h";
            //ToSave(name);
            //name = "k";
            //ToSave(name);
            //name = "m";
            //ToSave(name);
            string temp = string.Format(@"C:\Users\Tinn\Desktop\pk\king.png");
            Image image = BitmapHelper.GetBitmapFormFile(temp);
            image = BottomRight(image);
            image.Save(string.Format(@"d:\king.png"));
        }
        private void ToSave(string name)
        {
            for (int i = 1; i < 14; i++)
            {
                string temp = string.Format(@"C:\Users\Tinn\Desktop\pk\{0}{1}.png", name, i);
                Image image = BitmapHelper.GetBitmapFormFile(temp);
                image = BottomRight(image);
                image.Save(string.Format(@"d:\{0}{1}.png", name, i));
            }
        }
        private Image BottomRight(Image image)
        {
            image = BottomLeft(image);

            image = BitmapHelper.ReplaceByPixel(image, 128, 175, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 128, 176, Color.FromArgb(255, 245, 243, 243));
            image = BitmapHelper.ReplaceByPixel(image, 128, 177, Color.FromArgb(200, 251, 249, 249));//.
            for (int i = 178; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 128, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 127, 177, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 127, 178, Color.FromArgb(255, 245, 243, 243));
            for (int i = 179; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 127, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 126, 178, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 126, 179, Color.FromArgb(255, 245, 243, 243));
            for (int i = 180; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 126, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 125, 179, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 125, 180, Color.FromArgb(255, 245, 243, 243));
            for (int i = 181; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 125, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 124, 180, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 124, 181, Color.FromArgb(255, 245, 243, 243));
            for (int i = 182; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 124, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 123, 181, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 123, 182, Color.FromArgb(255, 245, 243, 243));
            for (int i = 183; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 123, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 122, 182, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 122, 183, Color.FromArgb(255, 245, 243, 243));
            for (int i = 184; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 122, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 121, 183, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 121, 184, Color.FromArgb(200, 251, 249, 249));//.

            image = BitmapHelper.ReplaceByPixel(image, 120, 184, Color.FromArgb(255, 245, 243, 243));
            image = BitmapHelper.ReplaceByPixel(image, 119, 184, Color.FromArgb(255, 238, 231, 231));


            return image;
        }
        private Image BottomLeft(Image image)
        {
            image = TopRight(image);
            image = BitmapHelper.ReplaceByPixel(image, 0, 175, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 0, 176, Color.FromArgb(255, 245, 243, 243));
            image = BitmapHelper.ReplaceByPixel(image, 0, 177, Color.FromArgb(200, 251, 249, 249));//.
            for (int i = 178; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 0, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 1, 177, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 1, 178, Color.FromArgb(255, 245, 243, 243));
            for (int i = 179; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 1, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 2, 178, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 2, 179, Color.FromArgb(255, 245, 243, 243));
            for (int i = 180; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 2, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 3, 179, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 3, 180, Color.FromArgb(255, 245, 243, 243));
            for (int i = 181; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 3, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 4, 180, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 4, 181, Color.FromArgb(255, 245, 243, 243));
            for (int i = 182; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 4, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 5, 181, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 5, 182, Color.FromArgb(255, 245, 243, 243));
            for (int i = 183; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 5, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 6, 182, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 6, 183, Color.FromArgb(255, 245, 243, 243));
            for (int i = 184; i < 185; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 6, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 7, 183, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 7, 184, Color.FromArgb(255, 245, 243, 243));

            image = BitmapHelper.ReplaceByPixel(image, 8, 183, Color.FromArgb(255, 238, 235, 235));
            image = BitmapHelper.ReplaceByPixel(image, 8, 184, Color.FromArgb(255, 238, 231, 231));

            Bitmap temp = image as Bitmap;
            for (int i = 9; i < 119; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, i, 184, temp.GetPixel(i, 183));
            }

            //image = BitmapHelper.ReplaceByPixel(image, 9, 18, Color.FromArgb(255, 238, 231, 231));
            //image = BitmapHelper.ReplaceByPixel(image, 9, 181, Color.FromArgb(255, 245, 243, 243));

            return image;
        }
        private Image TopRight(Image image)
        {
            image = TopLeft(image);
            image = BitmapHelper.ReplaceByPixel(image, 120, 0, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 121, 0, Color.FromArgb(255, 245, 243, 243));
            image = BitmapHelper.ReplaceByPixel(image, 122, 0, Color.FromArgb(200, 251, 249, 249));//.
            for (int i = 123; i < 129; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, i, 0, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 123, 1, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 124, 1, Color.FromArgb(255, 245, 243, 243));
            for (int i = 125; i < 129; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, i, 1, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 124, 2, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 125, 2, Color.FromArgb(255, 245, 243, 243));
            for (int i = 126; i < 129; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, i, 2, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 125, 3, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 126, 3, Color.FromArgb(255, 245, 243, 243));
            for (int i = 127; i < 129; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, i, 3, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 126, 4, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 127, 4, Color.FromArgb(255, 245, 243, 243));
            for (int i = 128; i < 129; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, i, 4, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 127, 6, Color.FromArgb(255, 227, 220, 220));
            image = BitmapHelper.ReplaceByPixel(image, 127, 5, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 128, 5, Color.FromArgb(200, 251, 249, 249));//.

            image = BitmapHelper.ReplaceByPixel(image, 128, 6, Color.FromArgb(255, 245, 243, 243));
            image = BitmapHelper.ReplaceByPixel(image, 128, 7, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 128, 8, Color.FromArgb(255, 227, 220, 219));

            return image;
        }
        private Image TopLeft(Image image)
        {
            image = BitmapHelper.ReplaceByPixel(image, 0, 9, Color.FromArgb(255, 231, 220, 220));
            image = BitmapHelper.ReplaceByPixel(image, 0, 8, Color.FromArgb(255, 231, 220, 220));

            image = BitmapHelper.ReplaceByPixel(image, 0, 7, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 0, 6, Color.FromArgb(255, 245, 243, 243));
            for (int i = 0; i < 6; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 0, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 1, 6, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 1, 5, Color.FromArgb(255, 245, 243, 243));
            image = BitmapHelper.ReplaceByPixel(image, 1, 4, Color.FromArgb(200, 251, 249, 249));//.
            for (int i = 0; i < 4; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 1, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 2, 4, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 2, 3, Color.FromArgb(255, 245, 243, 243));
            for (int i = 0; i < 3; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 2, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 3, 3, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 3, 2, Color.FromArgb(255, 245, 243, 243));
            for (int i = 0; i < 2; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 3, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 4, 2, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 4, 1, Color.FromArgb(200, 251, 249, 249)); //.
            for (int i = 0; i < 1; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 4, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 5, 1, Color.FromArgb(255, 245, 243, 243));
            for (int i = 0; i < 1; i++)
            {
                image = BitmapHelper.ReplaceByPixel(image, 5, i, Color.Transparent);
            }

            image = BitmapHelper.ReplaceByPixel(image, 6, 1, Color.FromArgb(255, 238, 231, 231));
            image = BitmapHelper.ReplaceByPixel(image, 6, 0, Color.FromArgb(255, 245, 243, 243));

            image = BitmapHelper.ReplaceByPixel(image, 7, 0, Color.FromArgb(255, 238, 231, 231));

            image = BitmapHelper.ReplaceByPixel(image, 8, 0, Color.FromArgb(255, 231, 220, 220));

            return image;
        }

        #endregion

        #region 将图片剪成圆角
        //创建 圆角图片的方法
        public static void CreateRoundedCorner(string sSrcFilePath, string sDstFilePath, string sCornerLocation)
        {
            Image image = Image.FromFile(sSrcFilePath);
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            GraphicsPath rectPath = CreateRoundRectanglePath(rect, 5, "TopLeft"); //构建圆角外部路径
            Brush b = new SolidBrush(Color.Black);//圆角背景白色
            g.DrawPath(new Pen(b), rectPath);
            g.FillPath(b, rectPath);

            rectPath = CreateRoundRectanglePath(rect, 7, "TopRight");
            g.DrawPath(new Pen(b), rectPath);
            g.FillPath(b, rectPath);

            rectPath = CreateRoundRectanglePath(rect, 10, "BottomLeft");
            g.DrawPath(new Pen(b), rectPath);
            g.FillPath(b, rectPath);

            rectPath = CreateRoundRectanglePath(rect, 12, "BottomRight");
            g.DrawPath(new Pen(b), rectPath);
            g.FillPath(b, rectPath);

            g.Dispose();
            image.Save(sDstFilePath);
            image.Dispose();
        }
        private static GraphicsPath CreateRoundRectanglePath(Rectangle rect, int radius, string sPosition)
        {
            GraphicsPath rectPath = new GraphicsPath();
            switch (sPosition)
            {
                case "TopLeft":
                    {
                        rectPath.AddArc(rect.Left, rect.Top, radius * 2, radius * 2, 180, 90);
                        rectPath.AddLine(rect.Left, rect.Top, rect.Left, rect.Top + radius);
                        break;
                    }

                case "TopRight":
                    {
                        rectPath.AddArc(rect.Right - radius * 2, rect.Top, radius * 2, radius * 2, 270, 90);
                        rectPath.AddLine(rect.Right, rect.Top, rect.Right - radius, rect.Top);
                        break;
                    }

                case "BottomLeft":
                    {
                        rectPath.AddArc(rect.Left, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                        rectPath.AddLine(rect.Left, rect.Bottom - radius, rect.Left, rect.Bottom);
                        break;
                    }

                case "BottomRight":
                    {
                        rectPath.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                        rectPath.AddLine(rect.Right - radius, rect.Bottom, rect.Right, rect.Bottom);
                        break;
                    }
            }
            return rectPath;
        }

        #endregion

        #region 临时操作生成图片
        /// <summary>
        /// 
        /// </summary>
        private Rectangle rect = Rectangle.Empty;
        void btChange_Click(object sender, EventArgs e)
        {
            if (tPictureBox1.Image == null) return;
            //主区域
            rect = new Rectangle(117, 29, 905, 558);
            //按钮区域
            rect = new Rectangle(84, 940, 600, 693);

            //Image temp = BitmapHelper.ConvertTo(normal, BConvertType.Trans, 0, rect.X, rect.Width, rect.Y, rect.Height);
            //temp.Save(@"d:\p14.jpg", ImageFormat.Jpeg);
            //this.pictureBox1.Image = temp;
        }
        void btLess_Click(object sender, EventArgs e)
        {
            //rect.X--;
            //rect.Height--;
            //rect.Width--;
        }

        void btAdd_Click(object sender, EventArgs e)
        {
            //rect.X++;
            //rect.Height++;
            //rect.Width++;
        }

        #endregion

        #region 取图与坐标
        void pictureBox1_Click(object sender, EventArgs e)
        {
            if (tPictureBox1.Image == null)
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Title = "Select Product Image",
                    Filter = "Image Files|*.gif;*.bmp;*.jpg;*.jpeg;*.png;*.tga;*.tif;*.tiff|GIF file format|*.gif|BMP file format|*.bmp|JPEG file format|*.jpg;*.jpeg|PNG file format|*.png|TGA file format|*.tga|TIFF file format|*.tif;*.tiff",
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    tPictureBox1.Image = BitmapHelper.GetBitmapFormFile(ofd.FileName);
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
            this.lbColor.Text = this.color.ToString();
        }
        void btClear_Click(object sender, EventArgs e)
        {
            tPictureBox1.Image = null;
            tPictureBox1.Image = null;
        }
        void btSave_Click(object sender, EventArgs e)
        {
            if (tPictureBox1.Image == null) return;
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Select Product Image",
                Filter = "Image Files|*.png",
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                tPictureBox1.Image.Save(sfd.FileName);
            }
        }

        #endregion

        #region 点击重绘
        void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (tPictureBox1.Image == null) return;

            if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
            {
                Bitmap temp = tPictureBox1.Image as Bitmap;
                Point point = tPictureBox1.GetPoint(e.Location);
                color = temp.GetPixel(point.X, point.Y);
                textBox1.Text = color.A.ToString();
                this.lbColor.BackColor = this.color;
            }
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (tPictureBox1.Image != null && color != Color.Empty)
                {
                    Point point = tPictureBox1.GetPoint(e.Location);
                    tPictureBox1.Image = BitmapHelper.ReplaceByPixel(tPictureBox1.Image, point.X, point.Y, Color.FromArgb(textBox1.Text.ToInt(), color.R, color.G, color.B));
                }
            }
        }

        #endregion
    }
}
