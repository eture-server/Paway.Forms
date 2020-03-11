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
        /// 图像列
        /// </summary>
        public string ImageName { get; set; }
        public int ColorBackR { get; }

        /// <summary>
        /// 图像列大小
        /// </summary>
        public Size ImageSize { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// 圆角
        /// </summary>
        public Padding Radiu { get; set; } = new Padding(5);
        /// <summary>
        /// 边框线
        /// </summary>
        public int Line { get; set; } = 1;
        /// <summary>
        /// 背景属性
        /// </summary>
        public TProperties BackGround { get; set; }
        /// <summary>
        /// 文字属性
        /// </summary>
        public TProperties Text { get; set; }

        /// <summary>
        /// TDataGridView按钮列
        /// </summary>
        public IButtonAttribute(string imageName = null, int imageWidth = 20, int imageHeight = 20,
            int width = 0, int height = 0,
            int raidu = 5,
            int colorBackR = 0, int colorBackG = 0, int colorBackB = 0, bool iNormalEmpty = true,
            int colorTextR = 0, int colorTextG = 0, int colorTextB = 0, bool iMoveWhite = true)
        {
            this.ImageName = imageName;
            this.ImageSize = new Size(imageWidth, imageHeight);
            this.Size = new Size(width, height);
            this.Radiu = new Padding(raidu);
            if (colorBackR != 0 || colorBackG != 0 || colorBackB != 0)
            {
                var colorBack = Color.FromArgb(colorBackR, colorBackG, colorBackB);
                BackGround = new TProperties();
                BackGround.Reset(colorBack, 15);
                if (iNormalEmpty) BackGround.ColorNormal = Color.Empty;
            }
            if (colorTextR != 0 || colorTextG != 0 || colorTextB != 0)
            {
                var colorText = Color.FromArgb(colorTextR, colorTextG, colorTextB);
                Text = new TProperties();
                Text.Reset(colorText, 15);
                Text.StringFormat.Alignment = StringAlignment.Center;
                if (iMoveWhite)
                {
                    Text.ColorMove = Color.White;
                    Text.ColorDown = Color.White;
                }
            }
        }
    }
}