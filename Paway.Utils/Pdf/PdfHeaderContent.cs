using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paway.Utils.Pdf
{
    /// <summary>
    /// PDF打印页眉内容集合类
    /// </summary>
    public class PdfHeaderContent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PdfHeaderContent() { }
        /// <summary>
        /// 标题文字
        /// </summary>
        public string HeaderTitle { get; set; }
        /// <summary>
        /// 标题图片
        /// </summary>
        public Image HeaderImage { get; set; }
        /// <summary>
        /// 表头集合A
        /// </summary>
        public List<string> HeaderNamesA { get; set; }
        /// <summary>
        /// 表头坐标集合A
        /// </summary>
        public List<float> HeaderPositionsA { get; set; }
        /// <summary>
        /// 表头集合B
        /// </summary>
        public List<string> HeaderNamesB { get; set; }
        /// <summary>
        /// 表头坐标集合B
        /// </summary>
        public List<float> HeaderPositionsB { get; set; }
        /// <summary>
        /// 表头集合C
        /// </summary>
        public List<string> HeaderNamesC { get; set; }
        /// <summary>
        /// 表头坐标集合C
        /// </summary>
        public List<float> HeaderPositionsC { get; set; }
    }
}
