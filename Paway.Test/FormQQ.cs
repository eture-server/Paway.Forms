using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Paway.Forms;
using Paway.Test.Properties;
using System.IO;
using Paway.Helper;

namespace Paway.Test
{
    public partial class FormQQ : QQForm
    {
        public FormQQ()
        {
            InitializeComponent();
            button1.Click += delegate { AnimalShow(); };
        }
        private void AnimalShow()
        {
            string caption = "消息提示";
            string text = @"“末日”前晒出流逝的岁月
                            上传一组证明您岁月痕迹的新老对比照片
                            即可获得抽奖资格和微博积分";
            text = string.Format("{0}\r\n{1}", text, DateTime.Now.ToLongTimeString());
            NotifyForm form = new NotifyForm();
            form.AnimalShow(caption, text, int.MaxValue);
        }
    }
}
