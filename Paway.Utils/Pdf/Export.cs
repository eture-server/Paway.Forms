using System;
using System.Collections.Generic;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;

namespace Paway.Utils.Pdf
{
    /// <summary>
    ///     导出类示例
    /// </summary>
    public class Export
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     导出为PDF
        ///     Pdf导出导出示例
        /// </summary>
        /// <param name="fileName">导出的pdf文件</param>
        /// <param name="list">需要导出的数据</param>
        /// <returns></returns>
        public static bool ToPdf(string fileName, List<string> list)
        {
            if (fileName == null || list == null) return false;
            PdfPrinter pdf = null;
            try
            {
                var header = new PdfHeaderContent();
                //header.HeaderImage = Resources.title;
                pdf = new PdfPrinter(fileName, PageSize.A4, header);
                pdf.Init();
                var document = pdf.Doc;
                var font = pdf.DefaultFont;

                //标题居中
                var p1 = new Paragraph("Title", font);
                p1.Alignment = Element.ALIGN_CENTER;
                document.Add(p1);

                //添加一个表格
                var table = new PdfPTable(6);
                table.TotalWidth = document.Right - document.Left;
                float[] widths = { 100f, 420f };
                table.SetWidths(widths);
                //应用
                table.LockedWidth = true;

                //增加表头
                table.AddCell(new PdfPCell(new Phrase("image", font)));
                table.AddCell(new PdfPCell(new Phrase("des", font)));

                //增加详细数据
                table.AddCell(Image.GetInstance(null, BaseColor.WHITE)); //图片
                table.AddCell(new PdfPCell(new Phrase("description", font))); //文字

                document.Add(table);

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(string.Empty, ex);
            }
            finally
            {
                //注意一定要关闭，否则PDF中的内容将得不到保存
                if (pdf != null)
                    pdf.Close();
            }
        }
    }
}