using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 特性.TDataGridView全选框
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ICheckBoxAttribute : Attribute { }

    /// <summary>
    /// 特性.TDataGridView按钮列
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IButtonAttribute : Attribute
    {
        /// <summary>
        /// 大小
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// 圆角
        /// </summary>
        public Padding Radiu { get; set; } = new Padding(3);
        /// <summary>
        /// 边框线
        /// </summary>
        public int Line { get; set; } = 1;
        /// <summary>
        /// 文字属性
        /// </summary>
        public TProperties Text { get; set; } = new TProperties();
        /// <summary>
        /// 背景属性
        /// </summary>
        public TProperties BackGround { get; set; } = new TProperties();
        /// <summary>
        /// 图像列
        /// </summary>
        public string ImageName { get; set; }
        /// <summary>
        /// 图像列大小
        /// </summary>
        public Size ImageSize { get; set; }

        /// <summary>
        /// TDataGridView按钮列
        /// </summary>
        public IButtonAttribute()
        {
            BackGround.Reset(Color.FromArgb(35, 175, 255));
            BackGround.ColorNormal = Color.Empty;
            Text.FontNormal = new Font("微软雅黑", 11);
            Text.StringFormat.Alignment = StringAlignment.Center;
            Text.ColorMove = Color.White;
            Text.ColorDown = Color.White;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        public IButtonAttribute(Size size, int line = 1) : this(size, new Padding(3), line) { }
        /// <summary>
        /// 设置属性
        /// </summary>
        public IButtonAttribute(Padding radiu, int line = 1) : this(Size.Empty, radiu, line) { }
        /// <summary>
        /// 设置属性
        /// </summary>
        public IButtonAttribute(Size size, Padding radiu, int line = 1) : this()
        {
            this.Size = size;
            this.Line = line;
            this.Radiu = radiu;
        }
        /// <summary>
        /// 设置图像
        /// </summary>
        public IButtonAttribute(string imageName) : this(imageName, new Size(24, 24)) { }
        /// <summary>
        /// 设置图像
        /// </summary>
        public IButtonAttribute(string imageName, Size imageSize) : this()
        {
            this.ImageName = imageName;
            this.ImageSize = imageSize;
        }
        /// <summary>
        /// 设置背景颜色
        /// </summary>
        public IButtonAttribute(Color colorBackNormal, Color colorBackMove, Color colorBackDown)
        {
            BackGround.ColorNormal = colorBackNormal;
            BackGround.ColorMove = colorBackMove;
            BackGround.ColorDown = colorBackDown;
        }
        /// <summary>
        /// 设置背景颜色
        /// </summary>
        public IButtonAttribute(Color colorBack)
        {
            BackGround.Reset(colorBack);
        }
    }
}