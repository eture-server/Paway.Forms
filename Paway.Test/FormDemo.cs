using Paway.Helper;
using Paway.Forms;
using Paway.Forms.Metro;
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
            this.tbDesc.Font = new System.Drawing.Font(this.tbDesc.Font.Name, 12f);
            drawCombobox2.SelectedIndex = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            tComboBox1.DataSource = dt;
            tComboBox1.DisplayMember = "Name";
            tComboBox1.ValueMember = "Value";
            BindingList<Data> list = new BindingList<Data>();
            tComboBox2.DataSource = list;
            tComboBox2.DisplayMember = "Name";
            tComboBox2.ValueMember = "Value";
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
            pictureBox1.Click += pictureBox1_Click;
            this.toolBar3.SelectedItemChanged += toolBar3_SelectedItemChanged;
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

        void toolBar3_SelectedItemChanged(object sender, EventArgs e)
        {
            Bitmap image = null;
            if (pictureBox1.Image == null)
            {
                MessageBox.Show(this, "请选择原图");
                return;
            }
            switch (toolBar3.SelectedItem.Text)
            {
                case "光暗":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, BConvertType.Brightness, -100);
                    break;
                case "反色":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, BConvertType.Anti);
                    break;
                case "浮雕":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, BConvertType.Relief);
                    break;
                case "滤色":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, BConvertType.Color);
                    break;
                case "左右":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, BConvertType.LeftRight);
                    break;
                case "上下":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, BConvertType.UpDown);
                    break;
                case "灰度":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, BConvertType.Grayscale);
                    break;
                case "黑白":
                    image = BitmapHelper.ConvertTo(pictureBox1.Image as Bitmap, BConvertType.BlackWhite);
                    break;
            }
            pictureBox2.Image = image;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.bt360.Click += delegate { new Form360().Show(); };
            this.btQQ.Click += delegate { new FormQQ().ShowDialog(this); };
            this.btAbout.Click += delegate { new AboutForm().ShowDialog(); };
            this.btGridView.Click += delegate { new FormGridView().Show(); };
            this.btImage.Click += btImage_Click;
            toolBar1.ItemClick += toolBar1_ItemClick;
            this.qqTextBoxEx1.TextChanged += qqTextBox1_TextChanged;
            this.btSearch.Click += btSearch_Click;
            this.tbCat.MouseEnter += tbCat_MouseEnter;
            this.tbCat.MouseLeave += tbCat_MouseLeave;
            this.btTTsRead.Click += btTTsRead_Click;
        }

        void btTTsRead_Click(object sender, EventArgs e)
        {
            SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            SpVoice voice = new SpVoice();
            voice.Speak(tbCat.Text, spFlags);
        }

        void tbCat_MouseLeave(object sender, EventArgs e)
        {
            this.tbPrompt.TMouseLeave();
        }

        void tbCat_MouseEnter(object sender, EventArgs e)
        {
            this.tbPrompt.TMouseEnter();
        }

        void btSearch_Click(object sender, EventArgs e)
        {
            Bitmap searchFor = Paway.Helper.BitmapHelper.GetBitmapFormFile(@"d:\for.png");
            Bitmap searchOn = Paway.Helper.BitmapHelper.GetBitmapFormFile(@"d:\on.jpg");
            Mobot.Imaging.SearchResult[] results = Mobot.Imaging.ImageRecognitionHelper.SearchBitmap_Test(searchFor, searchOn);
            for (int i = 0; i < results.Length; i++)
            {
                using (Graphics graphics = Graphics.FromImage(searchOn))
                {
                    graphics.DrawRectangle(new Pen(Color.Red), results[i].Rect);
                }
            }
            ImageForm iform = new ImageForm(searchOn);
            iform.BackColor = Color.Black;
            iform.Show(this);
        }

        void qqTextBox1_TextChanged(object sender, EventArgs e)
        {
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
                iform.Show(this);
            }
        }

        void toolBar1_ItemClick(object sender, EventArgs e)
        {
            switch (toolBar1.SelectedItem.Text)
            {
                case "创建宝贝":
                    toolBar1.ProgressStop();
                    new FormSql().ShowDialog();
                    break;
                case "上传宝贝":
                    new FormQQ().Show();
                    toolBar1.ProgressStart("上传宝贝");
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.Text = string.Format("{0}", e.Location);
        }

        #region 示例 解析与写入Xml
        public byte[] CommXml(string function, params object[] pList)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(decl);
            XmlElement root = doc.CreateElement("packet");
            {
                doc.AppendChild(root);
                root.SetAttribute("type", "type");
                root.SetAttribute("version", "version");
            }
            XmlElement field = doc.CreateElement("FieldList");
            {
                root.AppendChild(field);
                XmlElement fun = doc.CreateElement("Function");
                field.AppendChild(fun);
                fun.InnerXml = function;
            }
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            byte[] buffer = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        public object FromXml(byte[] data, string file)
        {
            MemoryStream stream = new MemoryStream(data);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            //doc.Load(file);
            List<string> list = new List<string>();
            XmlElement root = doc.DocumentElement;
            if (root == null || root.Name != "Screen") throw new Exception("脚本文件格式不正确。");
            XmlNode element = root.FirstChild;
            while (element != null)
            {
                if (element.Name != "Item") throw new Exception("脚本文件格式不正确。");
                XmlNode item = element.FirstChild;
                string result = null;
                while (item != null)
                {
                    if (item.Name == "ScreenName")
                    {
                        result = Path.Combine(item.InnerXml);
                    }
                    if (item.Name == "CreateTime")
                    {
                    }
                    item = item.NextSibling;
                }
                list.Add(result);
                element = element.NextSibling;
            }
            return list;
        }

        #endregion
    }
    public class Data
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    internal class MyMessager : IMessageFilter
    {
        private IntPtr parent = IntPtr.Zero;

        public MyMessager(IntPtr parent)
        {
            this.parent = parent;
        }
        //截取消息，进行处理
        public bool PreFilterMessage(ref Message m)
        {
            Console.WriteLine(m.HWnd);
            //拦截左键单击事件　
            if (m.Msg == 516 && m.HWnd == parent)
            {
                return true;
            }
            else
            {
                return false; //返回false则消息未被裁取，系统会处理
            }
        }
    }
}
