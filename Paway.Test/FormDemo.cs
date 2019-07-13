using Paway.Helper;
using Paway.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Paway.Test.Properties;
using DotNetSpeech;
using System.Reflection;

namespace Paway.Test
{
    public partial class FormDemo : Form
    {
        public FormDemo()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            BindingList<Data> list = new BindingList<Data>();
            for (int i = 0; i < 5; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Name"] = "Name" + i;
                dr["Value"] = "Value" + i;
                dt.Rows.Add(dr);
                list.Add(new Data
                {
                    Name = "Name" + i + 10,
                    Value = "Value" + i + 10
                });
            }
            InitEnum();
            //this.tComboBox21.Edit.DataSource = list;
            //this.tComboBox21.Edit.DisplayMember = "Name";
            //this.tComboBox21.Edit.ValueMember = "Value";
            this.tComboBox21.Edit.SelectedIndex = 0;

            pictureBox1.Click += pictureBox1_Click;
            pictureBox2.Click += pictureBox2_Click;
            this.toolBar3.SelectedItemChanged += toolBar3_SelectedItemChanged;
            Query();
            this.Activate();
        }
        private void Query()
        {
            var server = new SQLiteService();
            var list = server.Find<TestInfo>("1=1 limit 100");

            var tQuery = new TQuery<TestInfo>();
            tQuery.Init(qqTextBox1, list, new { Name = 0, Value = 0 }.GetType());
            tQuery.SelectedEvent += TQuery_SelectedEvent;
        }
        private void TQuery_SelectedEvent(object obj)
        {
            qqTextBox1.Text = (obj as TestInfo).Name;
        }

        private void InitEnum()
        {
            this.tComboBox21.Edit.Items.Clear();
            this.tComboBox21.Edit.Items.Add("dd");
            TDirection[] tList = (TDirection[])Enum.GetValues(typeof(TDirection));
            for (int i = 0; i < tList.Length; i++)
            {
                this.tComboBox21.Edit.Items.Add(EntityHelper.GetValue((TDirection)tList[i]));
            }
        }

        void pictureBox1_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(pictureBox1_Click), sender, e);
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select Product Image",
                Filter = "Image Files|*.gif;*.bmp;*.jpg;*.jpeg;*.png;*.tga;*.tif;*.tiff|GIF file format|*.gif|BMP file format|*.bmp|JPEG file format|*.jpg;*.jpeg|PNG file format|*.png|TGA file format|*.tga|TIFF file format|*.tif;*.tiff",
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = BitmapHelper.GetBitmapFormFile(ofd.FileName);
            }
        }

        void pictureBox2_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null) return;
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(pictureBox2_Click), sender, e);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Save Image",
                Filter = "Image Files|*.gif;*.bmp;*.jpg;*.jpeg;*.png;*.tga;*.tif;*.tiff|GIF file format|*.gif|BMP file format|*.bmp|JPEG file format|*.jpg;*.jpeg|PNG file format|*.png|TGA file format|*.tga|TIFF file format|*.tif;*.tiff",
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image.Save(sfd.FileName);
            }
        }

        void toolBar3_SelectedItemChanged(ToolItem item, EventArgs e)
        {
            Bitmap image = null;
            if (pictureBox1.Image == null)
            {
                ExceptionHelper.Show("请选择原图");
                return;
            }
            switch (toolBar3.SelectedItem.Text)
            {
                case "光暗":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.Brightness, -100);
                    break;
                case "反色":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.Anti);
                    break;
                case "浮雕":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.Relief);
                    break;
                case "滤色":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.Color);
                    break;
                case "左右":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.LeftRight);
                    break;
                case "上下":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.UpDown);
                    break;
                case "灰度":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.Grayscale);
                    break;
                case "黑白":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.BlackWhite);
                    break;
                case "透明":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.Trans, 150);
                    break;
                case "换色":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.Replace, 255);
                    break;
                case "HSL":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, TConvertType.HSL, 20);
                    break;
            }
            pictureBox2.Image = image;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.btQQ.Click += delegate { new FormQQ().ShowDialog(this); };
            this.btAbout.Click += btAbout_Click;
            this.btImage.Click += btImage_Click;
            this.btSearch.Click += btSearch_Click;
            this.btTTsRead.Click += btTTsRead_Click;
        }

        void btAbout_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog(this);
        }

        void btTTsRead_Click(object sender, EventArgs e)
        {
            SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            SpVoice voice = new SpVoice();
            voice.Speak(qqTextBox1.Text, spFlags);
        }

        void btSearch_Click(object sender, EventArgs e)
        {
            //Bitmap searchFor = BitmapHelper.GetBitmapFormFile(@"d:\for.png");
            //Bitmap searchOn = BitmapHelper.GetBitmapFormFile(@"d:\on.jpg");
            //Mobot.Imaging.SearchResult[] results = Mobot.Imaging.ImageRecognitionHelper.SearchBitmap_Test(searchFor, searchOn);
            //for (int i = 0; i < results.Length; i++)
            //{
            //    using (Graphics graphics = Graphics.FromImage(searchOn))
            //    {
            //        graphics.DrawRectangle(new Pen(Color.Red), results[i].Rect);
            //    }
            //}
            //ImageForm iform = new ImageForm(searchOn);
            //iform.BackColor = Color.Black;
            //iform.ShowDialog(this);
        }
        void btImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select Product Image",
                Filter = "Image Files|*.gif;*.bmp;*.jpg;*.jpeg;*.png;*.tga;*.tif;*.tiff|GIF file format|*.gif|BMP file format|*.bmp|JPEG file format|*.jpg;*.jpeg|PNG file format|*.png|TGA file format|*.tga|TIFF file format|*.tif;*.tiff",
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bitmap image = BitmapHelper.GetBitmapFormFile(ofd.FileName);
                ImageForm iform = new ImageForm(image);
                iform.BackColor = Color.Black;
                iform.ShowDialog(this);
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.Text = string.Format("{0}", e.Location);
        }
    }
    public class Data
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
