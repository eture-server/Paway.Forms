using Paway.Forms;
using Paway.Helper;
using Paway.Win32;
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
            e.Graphics.DrawString(label1.Text, label1.Font, new SolidBrush(color), label1.Location);
        }
    }
    /// <summary>
    /// 启动控制
    /// </summary>
    public static class LoadHelper
    {
        private static ILoadForm form;
        private static Thread thread;
        /// <summary>
        /// 显示启动界面
        /// </summary>
        /// <typeparam name="T">窗体</typeparam>
        /// <param name="title">标题</param>
        /// <param name="load2">静态大图</param>
        /// <param name="desc">描述</param>
        /// <returns>启动窗体实例</returns>
        public static Form ShowForm<T>(string title, Image load2 = null, string desc = "Loading...") where T : ILoadForm
        {
            var asmb = Assembly.GetAssembly(typeof(T));

            var form = asmb.CreateInstance(typeof(T).FullName) as Form;
            LoadHelper.form = (ILoadForm)form;
            form.TopMost = true;
            form.Text = title;
            LoadHelper.form.Update(desc);
            LoadHelper.form.Update(null, load2);
            form.Load += Form1_Load;
            thread = new Thread(ToShowForm);
            thread.Start(form);
            return form;
        }
        /// <summary>
        /// 更新标题
        /// </summary>
        /// <param name="title">标题</param>
        public static void Title(string title)
        {
            form.Title(title);
        }
        /// <summary>
        /// 更新描述
        /// </summary>
        /// <param name="desc">描述</param>
        public static void Update(string desc)
        {
            form.Update(desc);
        }
        /// <summary>
        /// 更新显示图片
        /// </summary>
        /// <param name="load">动态小图</param>
        /// <param name="load2">静态大图</param>
        public static void Update(Image load, Image load2)
        {
            form.Update(load, load2);
        }
        /// <summary>
        /// 设置大图显示
        /// </summary>
        /// <param name="mode">图像定位</param>
        public static void Update(PictureBoxSizeMode mode)
        {
            form.Update(mode);
        }
        private static void Form1_Load(object sender, EventArgs e)
        {
            Form form = sender as Form;
            form.TopMost = true;
        }
        private static void ToShowForm(object obj)
        {
            Application.Run(obj as Form);
        }
        /// <summary>
        /// 关闭启动窗体
        /// </summary>
        public static void CloseForm()
        {
            Form form = LoadHelper.form as Form;
            if (form != null && !form.IsDisposed)
                form.BeginInvoke(new Action<Form>(CloseForm), form);
        }
        private static void CloseForm(Form form)
        {
            form.Close();
            if (thread != null && thread.IsAlive)
            {
                thread.Join(100);
            }
        }
    }
}
