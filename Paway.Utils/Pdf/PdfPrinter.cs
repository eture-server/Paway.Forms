using System;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Paway.Utils
{
    /// <summary>
    /// PDF打印功能实现类
    /// </summary>
    public class PdfPrinter : PdfPageEventHelper, IPdfPageEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="printPath">指定PDF文件导出路径</param>
        public PdfPrinter(string printPath)
        {
            var rootPath = Environment.GetEnvironmentVariable("SystemRoot");
            rootPath = Path.Combine(rootPath, "Fonts", "simsun.ttc,1");
            var bfChinese = BaseFont.CreateFont(rootPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            DefaultFont = new Font(bfChinese, 12, Font.NORMAL, new BaseColor(0, 0, 0));
            PrintPath = printPath;
            DefaultPageSize = PageSize.A4;
            HeaderContent = new PdfHeaderContent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="printPath">指定PDF文件导出路径</param>
        /// <param name="paperSize">纸张大小</param>
        /// <param name="headerContent">PDF文件页眉内容</param>
        public PdfPrinter(string printPath, Rectangle paperSize, PdfHeaderContent headerContent)
        {
            var rootPath = Environment.GetEnvironmentVariable("SystemRoot");
            rootPath = Path.Combine(rootPath, "Fonts", "simsun.ttc,1");
            var bfChinese = BaseFont.CreateFont(rootPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            DefaultFont = new Font(bfChinese, 12, Font.NORMAL, new BaseColor(0, 0, 0));
            PrintPath = printPath;
            DefaultPageSize = paperSize;
            HeaderContent = headerContent;
        }

        /// <summary>
        /// 默认字体：兼容亚洲文字
        /// </summary>
        public Font DefaultFont { get; set; }

        /// <summary>
        /// PDF文件导出路径
        /// </summary>
        private string PrintPath { get; set; }

        /// <summary>
        /// 默认纸张大小
        /// </summary>
        public Rectangle DefaultPageSize { get; set; }

        /// <summary>
        /// 页眉内容集合
        /// </summary>
        public PdfHeaderContent HeaderContent { get; set; }

        /// <summary>
        /// 内部空白边
        /// </summary>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// 全局PDF文档对象
        /// </summary>
        public Document Doc { get; set; }

        /// <summary>
        /// 绘制页眉图片
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            if (HeaderContent.Image != null)
            {
                var img = Image.GetInstance(HeaderContent.Image, BaseColor.WHITE);
                img.SetAbsolutePosition(0, document.Top + 10);
                writer.DirectContent.AddImage(img);
            }
            base.OnStartPage(writer, document);
        }

        /// <summary>
        /// 初始化文档
        /// </summary>
        public void Init()
        {
            if (Doc == null)
            {
                Doc = new Document(DefaultPageSize, Rectangle.Left, Rectangle.Right, Rectangle.Top, Rectangle.Bottom);
                if (HeaderContent.Image != null)
                {
                    Doc.SetMargins(Doc.LeftMargin, Doc.RightMargin, HeaderContent.Image.Height + 10, Doc.BottomMargin);
                }
                using (var fs = new FileStream(PrintPath, FileMode.Create))
                {
                    var writer = PdfWriter.GetInstance(Doc, fs);
                    writer.PageEvent = new PdfPrinter(PrintPath, DefaultPageSize, HeaderContent);
                }
            }
            if (!Doc.IsOpen())
            {
                //创建文件
                Doc.Open();
            }
        }

        /// <summary>
        /// 保存并释放文档
        /// </summary>
        public void Close()
        {
            Doc.Close();
        }

        /// <summary>
        /// 绘制页眉
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public void OnStartPage2(PdfWriter writer, Document document)
        {
            float strWidth, fontSize;
            var padTop = 40f;

            var cb = writer.DirectContent;
            cb.BeginText();
            //标题
            if (!string.IsNullOrEmpty(HeaderContent.Title))
            {
                fontSize = 14f;
                strWidth = HeaderContent.Title.Length * fontSize;
                cb.SetFontAndSize(DefaultFont.BaseFont, fontSize);
                cb.SetCharacterSpacing(3);
                cb.SetTextMatrix((document.PageSize.Width - strWidth) / 2, document.PageSize.Height - padTop);
                cb.ShowText(HeaderContent.Title);
            }
            //时间、页码
            var str = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm") + " 第" + writer.PageNumber + "頁";
            fontSize = 10f;
            strWidth = str.Length * fontSize;
            cb.SetFontAndSize(DefaultFont.BaseFont, fontSize);
            cb.SetCharacterSpacing(2);
            cb.SetTextMatrix(document.PageSize.Width - strWidth, document.PageSize.Height - padTop + 2);
            cb.ShowText(str);
            //标题列A
            padTop = 55f;
            if (HeaderContent.NamesA != null && HeaderContent.NamesA.Count > 0)
            {
                var positions = HeaderContent.PositionsA;
                var isPosEmpty = positions == null || positions.Count != HeaderContent.NamesA.Count;
                //坐标为空或与表头数量不一致时默认位置

                fontSize = 12f;
                cb.SetFontAndSize(DefaultFont.BaseFont, fontSize);
                cb.SetCharacterSpacing(0);
                var left = document.LeftMargin; //默认起始位置
                for (var i = 0; i < HeaderContent.NamesA.Count; i++)
                {
                    if (!isPosEmpty) //如果指定了详细位置则按照指定位置绘制否则自动计算
                        left = HeaderContent.PositionsA[i];
                    cb.SetTextMatrix(left, document.PageSize.Height - padTop);
                    str = HeaderContent.NamesA[i];
                    cb.ShowText(str);
                    //left += str.Length * fontSize + 20f;
                    var step = (document.PageSize.Width - document.LeftMargin * 2) / HeaderContent.NamesA.Count;
                    left += step;
                }
            }
            //标题列B
            padTop = 68f;
            if (HeaderContent.NamesB != null && HeaderContent.NamesB.Count > 0)
            {
                var positions = HeaderContent.PositionsB;
                var isPosEmpty = positions == null || positions.Count != HeaderContent.NamesB.Count;
                //坐标为空或与表头数量不一致时默认位置

                fontSize = 12f;
                cb.SetFontAndSize(DefaultFont.BaseFont, fontSize);
                cb.SetCharacterSpacing(0);
                var left = document.LeftMargin; //默认起始位置
                for (var i = 0; i < HeaderContent.NamesB.Count; i++)
                {
                    if (!isPosEmpty) //如果指定了详细位置则按照指定位置绘制否则自动计算
                        left = HeaderContent.PositionsB[i];
                    cb.SetTextMatrix(left, document.PageSize.Height - padTop);
                    str = HeaderContent.NamesB[i];
                    cb.ShowText(str);
                    //left += str.Length * fontSize + 20f;
                    var step = (document.PageSize.Width - document.LeftMargin * 2) / HeaderContent.NamesA.Count;
                    left += step;
                }
            }
            //标题列C
            padTop = 80f;
            if (HeaderContent.NamesC != null && HeaderContent.NamesC.Count > 0)
            {
                var positions = HeaderContent.PositionsC;
                var isPosEmpty = positions == null || positions.Count != HeaderContent.NamesC.Count;
                //坐标为空或与表头数量不一致时默认位置

                fontSize = 12f;
                cb.SetFontAndSize(DefaultFont.BaseFont, fontSize);
                cb.SetCharacterSpacing(0);
                var left = document.LeftMargin; //默认起始位置
                for (var i = 0; i < HeaderContent.NamesC.Count; i++)
                {
                    if (!isPosEmpty) //如果指定了详细位置则按照指定位置绘制否则自动计算
                        left = HeaderContent.PositionsC[i];
                    cb.SetTextMatrix(left, document.PageSize.Height - padTop);
                    str = HeaderContent.NamesC[i];
                    cb.ShowText(str);
                    //left += str.Length * fontSize + 20f;
                    var step = (document.PageSize.Width - document.LeftMargin * 2) / HeaderContent.NamesA.Count;
                    left += step;
                }
            }

            cb.EndText();
            //绘制分割线
            padTop = 83f;
            cb.MoveTo(document.LeftMargin, document.PageSize.Height - padTop);
            cb.LineTo(document.PageSize.Width - document.RightMargin, document.PageSize.Height - padTop);
            cb.SetLineDash(3f, 3f);
            cb.Stroke();

            base.OnStartPage(writer, document);
        }

        /// <summary>
        /// 打印一段文字
        /// </summary>
        /// <param name="text">文字内容</param>
        /// <param name="align">文字对齐 0左、1中、2右</param>
        public void PrintString(string text, int align)
        {
            var ps = new Paragraph(text, DefaultFont)
            {
                Alignment = align
            };
            Doc.Add(ps);
        }

        /// <summary>
        /// 打印一行表格数据
        /// </summary>
        /// <param name="pCells">数据行包含的单元格集合</param>
        /// <param name="cWidth">数据行包含的单元格各自的宽度集合</param>
        public void PrintTableRow(PdfPCell[] pCells, float[] cWidth)
        {
            if (pCells == null || pCells.Length <= 0)
                throw new ArgumentException("Can not found any cell");
            var pt = new PdfPTable(pCells.Length)
            {
                WidthPercentage = 100f,
                SkipFirstHeader = false
            };
            var pRow = new PdfPRow(pCells);
            pt.Rows.Add(pRow);
            if (cWidth != null && cWidth.Length == pCells.Length)
                pt.SetWidths(cWidth);
            Doc.Add(pt);
        }

        /// <summary>
        /// 打印一个表格数据
        /// </summary>
        /// <param name="table">表格</param>
        /// <param name="cWidth">宽度</param>
        /// <param name="cAlign">对齐 0左 1中 2右</param>
        public void PrintTableContent(DataTable table, float[] cWidth, int[] cAlign)
        {
            if (table == null || table.Columns == null || table.Columns.Count <= 0)
                throw new ArgumentException("Can not found any column from table");
            var pt = new PdfPTable(table.Columns.Count)
            {
                WidthPercentage = 100f,
                SkipFirstHeader = false
            };
            PdfPCell[] pCells;
            PdfPRow pRow;
            //内容
            foreach (DataRow row in table.Rows)
            {
                pCells = new PdfPCell[table.Columns.Count];
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    pCells[i] = new PdfPCell(new Phrase(row[i].ToString(), DefaultFont));
                    if (cAlign != null && cAlign.Length == table.Columns.Count)
                    {
                        pCells[i].HorizontalAlignment = cAlign[i];
                        pCells[i].Border = 0;
                    }
                    pCells[i].VerticalAlignment = 1;
                }
                pRow = new PdfPRow(pCells);
                pt.Rows.Add(pRow);
            }
            if (cWidth != null && cWidth.Length == table.Columns.Count)
                pt.SetWidths(cWidth);
            Doc.Add(pt);
        }

        /// <summary>
        /// 换一页
        /// </summary>
        public void NewPage()
        {
            Doc.NewPage();
        }
    }
}