// Copyright 2012 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mobot.Imaging
{
    /// <summary>
    /// 搜索结果。
    /// </summary>
    public class SearchResult : IComparable<SearchResult>
    {
        /// <summary>
        /// 中心点
        /// </summary>
        public Point Center { get; private set; }
        /// <summary>
        /// 边框
        /// </summary>
        public Rectangle Rect { get; private set; }
        /// <summary>
        /// 相似度，0.0 - 1.0，1.0为完全相同。
        /// </summary>
        public double Similarity { get; private set; }
        /// <summary>
        /// 初始化类 <see cref="SearchResult"/> 的新实例。
        /// </summary>
        /// <param name="location"></param>
        /// <param name="similarity"></param>
        public SearchResult(Rectangle item, double similarity)
        {
            this.Rect = item;
            this.Similarity = similarity;
            this.Center = GeometryHelper.Center(item);
        }
        /// <summary>
        /// 实现比较方法，相似度大的排在前面。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(SearchResult obj)
        {
            return this.Similarity.CompareTo(obj.Similarity) * -1;
        }
        /// <summary>
        /// 返回结果信息。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}", this.Similarity, this.Rect.Location);
        }
    }
    /// <summary>
    /// 提供图像识别功能。
    /// </summary>
    public partial class ImageRecognitionHelper
    {
        /// <summary>
        /// 在目标图像内搜素图标，并返回符合容差要求的所有结果。
        /// </summary>
        /// <param name="searchForFile">搜素文件</param>
        /// <param name="searchOn"></param>
        /// <param name="searchForBounds"></param>
        /// <param name="searchArea"></param>
        /// <param name="tolerance">容差，0.0 - 1.0</param>
        /// <param name="resultList"></param>
        /// <returns>匹配的结果，按相似度从高到低排序。</returns>
        public static SearchResult[] SearchBitmap(string searchForFile, Image searchOn, Rectangle searchForBounds, Rectangle searchArea, double tolerance)
        {
            if (searchArea.Width <= 0 || searchArea.Height <= 0)
                searchArea = new Rectangle(0, 0, searchOn.Width, searchOn.Height);

            using (Bitmap image = Image.FromFile(searchForFile) as Bitmap)
            {
                return SearchBitmap(image, searchOn, searchForBounds, searchArea, tolerance);
            }
        }
        public static SearchResult[] SearchBitmap(Bitmap searchForFile, Image searchOn, Rectangle searchForBounds, Rectangle searchArea, double tolerance)
        {
            if (searchArea.Width <= 0 || searchArea.Height <= 0)
                searchArea = new Rectangle(0, 0, searchOn.Width, searchOn.Height);
            //创建小图标
            Bitmap searchFor = null;
            Rectangle rect = new Rectangle(
                searchForBounds.X,
                searchForBounds.Y,
                (searchForBounds.Width == 0) ? (searchArea.Width - 1) : searchForBounds.Width,
                (searchForBounds.Height == 0) ? (searchArea.Height - 1) : searchForBounds.Height);
            searchFor = searchForFile.Clone(rect, searchForFile.PixelFormat);

            BitmapSearch search = new BitmapSearch(searchOn as Bitmap);
            Rectangle[] results = search.FindAll(searchFor, searchArea, GeometryHelper.Center(searchArea), 50, (int)(tolerance * 100));

            List<SearchResult> list = new List<SearchResult>();
            foreach (Rectangle item in results)
            {
                list.Add(new SearchResult(item, 1.0));
            }
            list.Sort();
            return list.ToArray<SearchResult>();
        }
    }
}

