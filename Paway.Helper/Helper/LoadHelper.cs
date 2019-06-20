using log4net;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 启动控制
    /// </summary>
    public abstract class LoadHelper
    {
        private static ILoadForm form;
        /// <summary>
        /// 启动界面
        /// </summary>
        public static Form Form { get { return (Form)form; } }
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

            var form = (Form)asmb.CreateInstance(typeof(T).FullName);
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
            Form form = (Form)sender;
            form.TopMost = true;
        }
        private static void ToShowForm(object obj)
        {
            Application.Run((Form)obj);
        }
        /// <summary>
        /// 关闭启动窗体
        /// </summary>
        public static void CloseForm()
        {
            Form form = (Form)LoadHelper.form;
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