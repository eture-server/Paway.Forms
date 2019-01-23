using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using NPOI.HSSF.UserModel;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections.Generic;

namespace Paway.Helper
{
    /// <summary>
    ///     Excel文件导入导出DataTable
    /// </summary>
    public abstract class ExcelHelper
    {
        /// <summary>
        ///     使用OLEDB从Excel导入DataTable
        ///     HDR=yes 第一行是列名而不是数据
        /// </summary>
        public static DataTable ImportExcel(string fileName, string sheet, bool hdd = true)
        {
            var conString =
                string.Format(
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'", fileName,
                    hdd ? "yes" : "no");
            var sql = string.Format("select * from [{0}$]", sheet);
            using (var con = new OleDbConnection(conString))
            {
                con.Open();
                using (var cmd = new OleDbDataAdapter(sql, con))
                {
                    var ds = new DataSet();
                    cmd.Fill(ds);
                    var dt = ds.Tables[0];
                    return dt;
                }
            }
        }

        /// <summary>
        ///     使用OLEDB将DataTable导出到Excel
        ///     HDR=yes 第一行写入列标题
        /// </summary>
        /// <param name="table">数据源</param>
        /// <param name="fileName">excel2003文件名</param>
        /// <param name="sheet">工作薄名称</param> 
        /// <param name="title">文件描述,在F1=0</param>
        public static void ExportExcel(DataTable table, string fileName, string sheet, string title = null)
        {
            var conString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=no'", fileName);
            OleDbCommand cmd = null;
            try
            {
                var con = new OleDbConnection(conString);
                con.Open();
                var trans = con.BeginTransaction();
                cmd = new OleDbCommand()
                {
                    Connection = con,
                    Transaction = trans
                };
                string insert = null;
                string values = null;
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    insert = string.Format("{0}F{1},", insert, i + 1);
                    values = string.Format("{0}@F{1},", values, i + 1);
                }
                insert = insert.TrimEnd(',');
                values = values.TrimEnd(',');
                //写入标题
                if (!string.IsNullOrEmpty(title))
                {
                    string update = string.Format("update [{0}$] set F1 = '{1}' where F1 = (select top 1 F1 from [{0}$])", sheet, title);
                    cmd.CommandText = update;
                    cmd.ExecuteNonQuery();
                }
                //写入数据
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    var dr = table.Rows[i];
                    var pList = new OleDbParameter[table.Columns.Count];
                    for (var j = 0; j < table.Columns.Count; j++)
                    {
                        Type type = dr[j].GetType();
                        var param = new OleDbParameter()
                        {
                            ParameterName = string.Format("@F{0}", j + 1),
                            Value = dr[j]
                        };
                        pList[j] = param;
                    }
                    string sql = string.Format("insert into [{0}$]({1}) values({2})", sheet, insert, values);
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(pList);
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch
            {
                if (cmd != null && cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    if (cmd.Connection != null)
                    {
                        cmd.Connection.Close();
                        cmd.Connection.Dispose();
                    }
                    cmd.Dispose();
                }
            }
        }
        /// <summary>
        /// 使用OLEDB取第一工作薄名称
        /// </summary>
        public static string FirstSheet(string file)
        {
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file + ";Extended Properties='Excel 8.0';";
            using (var con = new OleDbConnection(connString))
            {
                con.Open();
                DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt.Rows.Count > 0) return dt.Rows[0]["TABLE_NAME"].ToString();
                return null;
            }
        }

        /// <summary>
        /// 从Excel中取值到DataTable
        /// </summary>
        public static DataTable ToDataTable(string fileName, int sheetIndex = 0)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            try
            {
                int index = 0;
                IWorkbook workbook = new HSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        ICell cell = firstRow.GetCell(i);
                        if (cell != null)
                        {
                            string cellValue = cell.StringCellValue;
                            if (cellValue != null)
                            {
                                DataColumn column = new DataColumn(cellValue);
                                dt.Columns.Add(column);
                            }
                        }
                    }
                    index = sheet.FirstRowNum + 1;

                    //最后一列的标号
                    int count = sheet.LastRowNum;
                    for (int i = index; i <= count; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = dt.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        dt.Rows.Add(dataRow);
                    }
                }
                return dt;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }
        /// <summary>
        /// 导出到Excel
        /// </summary>
        public static void ToExcel<T>(List<T> list, string fileName, Func<string, bool> action = null)
        {
            IWorkbook workbook = new HSSFWorkbook();
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            try
            {
                Type type = typeof(T);
                int index = 0, count = 0;
                ISheet sheet = workbook.CreateSheet("Sheet1");
                var properties = type.Properties();
                var descriptors = type.Descriptors();
                if (true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(count++);
                    row.Height = 20 * 20;
                    foreach (var property in properties)
                    {
                        if (property.PropertyType.IsGenericType) continue;
                        if (!property.IExcel()) continue;
                        if (action != null && action(property.Name)) continue;
                        property.IShow(out string text);
                        CreateCellDefalut(workbook, row, index++, text);
                    }
                }
                for (int i = 0; i < list.Count; ++i)
                {
                    index = 0;
                    IRow row = sheet.CreateRow(count++);
                    foreach (var property in properties)
                    {
                        if (property.PropertyType.IsGenericType) continue;
                        if (!property.IExcel()) continue;
                        if (action != null && action(property.Name)) continue;
                        var descriptor = descriptors.Find(c => c.Name == property.Name);
                        if (descriptor.PropertyType == typeof(double))
                        {
                            CreateCellNumber(workbook, row, index++, (double)descriptor.GetValue(list[i]));
                        }
                        else
                        {
                            CreateCellDefalut(workbook, row, index++, descriptor.GetValue(list[i]).ToString2());
                        }
                    }
                }
                sheet.SetColumnWidth(2, 20 * 256);
                sheet.SetColumnWidth(7, 20 * 256);
                sheet.SetColumnWidth(8, 20 * 256);
                workbook.Write(fs); //写入到excel
            }
            finally
            { if (fs != null) fs.Close(); }
        }

        /// <summary>
        /// 创建单元格并且赋值
        /// </summary>
        /// <param name="wb">IWorkbook实例</param>
        /// <param name="row">行对象</param>
        /// <param name="index">单元格索引</param>
        /// <param name="style">样式</param>
        /// <param name="value">单元格值</param>
        /// <returns></returns>
        public static ICell CreateCell(IWorkbook wb, IRow row, int index, ICellStyle style, string value)
        {
            ICell cell = row.CreateCell(index);
            cell.CellStyle = style;
            cell.SetCellValue(value);
            return cell;
        }

        /// <summary>
        /// 创建默认单元格并且赋值
        /// </summary>
        /// <param name="wb">IWorkbook实例</param>
        /// <param name="row">行对象</param>
        /// <param name="index">单元格索引</param>
        /// <param name="value">单元格值</param>
        /// <returns></returns>
        public static ICell CreateCellHeader(IWorkbook wb, IRow row, int index, string value)
        {
            ICellStyle style = Getcellstyle(wb, CellStyle.Header, HorizontalAlignment.Center);
            return CreateCell(wb, row, index, style, value);
        }

        /// <summary>
        /// 创建默认单元格并且赋值
        /// </summary>
        /// <param name="wb">IWorkbook实例</param>
        /// <param name="row">行对象</param>
        /// <param name="index">单元格索引</param>
        /// <param name="value">单元格值</param>
        /// <returns></returns>
        public static ICell CreateCellDefalut(IWorkbook wb, IRow row, int index, string value)
        {
            ICellStyle style = Getcellstyle(wb, CellStyle.Default, HorizontalAlignment.Left);
            return CreateCell(wb, row, index, style, value);
        }

        /// <summary>
        /// 创建数字单元格并且赋值
        /// </summary>
        /// <param name="wb">IWorkbook实例</param>
        /// <param name="row">行对象</param>
        /// <param name="index">单元格索引</param>
        /// <param name="value">单元格值</param>
        /// <returns></returns>
        public static ICell CreateCellNumber(IWorkbook wb, IRow row, int index, double value)
        {
            ICell cell = row.CreateCell(index);
            cell.CellStyle = Getcellstyle(wb, CellStyle.Number, HorizontalAlignment.Left);
            cell.SetCellValue(value);
            return cell;
        }

        /// <summary>
        /// 创建日期单元格并且赋值
        /// </summary>
        /// <param name="wb">IWorkbook实例</param>
        /// <param name="row">行对象</param>
        /// <param name="index">单元格索引</param>
        /// <param name="value">单元格值</param>
        /// <returns></returns>
        public static ICell CreateCellDate(IWorkbook wb, IRow row, int index, DateTime value)
        {
            ICell cell = row.CreateCell(index);
            cell.CellStyle = Getcellstyle(wb, CellStyle.DateTime, HorizontalAlignment.Left);
            cell.SetCellValue(value);
            return cell;
        }

        /// <summary>
        /// 单元格格式
        /// </summary>
        /// <param name="wb">IWorkbook实例</param>
        /// <param name="tyle">单元格类型</param>
        /// <param name="_HorizontalAlignment"></param>
        /// <returns></returns>
        public static ICellStyle Getcellstyle(IWorkbook wb, CellStyle tyle, HorizontalAlignment _HorizontalAlignment)
        {
            ICellStyle style = wb.CreateCellStyle();
            //定义几种字体  
            //也可以一种字体，写一些公共属性，然后在下面需要时加特殊的  
            IFont font = wb.CreateFont();
            font.FontName = "微软雅黑";

            //边框
            //style.BorderBottom = BorderStyle.Thin;
            //style.BorderLeft = BorderStyle.Thin;
            //style.BorderRight = BorderStyle.Thin;
            //style.BorderTop = BorderStyle.Thin;

            //边框颜色
            //style.BottomBorderColor = HSSFColor.Black.Index;
            //style.TopBorderColor = HSSFColor.Black.Index;

            //背景图形，我没有用到过。感觉很丑  
            //style.FillForegroundColor = HSSFColor.White.Index;
            //style.FillBackgroundColor = HSSFColor.Black.Index;

            //水平对齐  
            style.Alignment = _HorizontalAlignment;

            //垂直对齐  
            style.VerticalAlignment = VerticalAlignment.Center;

            //自动换行  
            //style.WrapText = true;

            //缩进;当设置为1时，前面留的空白太大了。希旺官网改进。或者是我设置的不对  
            style.Indention = 0;

            //上面基本都是设共公的设置，下面列出了常用的字段类型  
            switch (tyle)
            {
                case CellStyle.Default:
                    style.SetFont(font);
                    break;
                case CellStyle.Header:
                    IFont header = wb.CreateFont();
                    header.FontName = "微软雅黑";
                    header.FontHeightInPoints = 12;//字体大小
                    style.SetFont(header);
                    break;
                case CellStyle.Number:
                    style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.##");
                    style.SetFont(font);
                    break;
                case CellStyle.DateTime:
                    IDataFormat datastyle = wb.CreateDataFormat();
                    style.DataFormat = datastyle.GetFormat("yyyy/MM/dd");
                    style.SetFont(font);
                    break;
                case CellStyle.Url:
                    IFont url = wb.CreateFont();
                    url.FontName = "微软雅黑";
                    url.Color = HSSFColor.Blue.Index;
                    url.IsItalic = true;//下划线  
                    url.Underline = FontUnderlineType.Single;
                    style.SetFont(url);
                    break;
                case CellStyle.Science:
                    style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                    style.SetFont(font);
                    break;
            }
            return style;
        }

        /// <summary>
        /// 单元格
        /// </summary>
        public enum CellStyle
        {
            /// <summary>
            /// </summary>
            None,
            /// <summary>
            /// 微软雅黑
            /// </summary>
            Default,
            /// <summary>
            /// 微软雅黑,12
            /// </summary>
            Header,
            /// <summary>
            /// 0.##
            /// </summary>
            Number,
            /// <summary>
            /// yyyy/MM/dd
            /// </summary>
            DateTime,
            /// <summary>
            /// Url
            /// </summary>
            Url,
            /// <summary>
            /// 0.00E+00
            /// </summary>
            Science,
        }
    }
}