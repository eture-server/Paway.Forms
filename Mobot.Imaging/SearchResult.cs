// Copyright 2012 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Drawing;

namespace Mobot.Imaging
{
    /// <summary>
    ///     搜索结果。
    /// </summary>
    public class SearchResult : IComparable<SearchResult>
    {
        /// <summary>
        ///     初始化类 <see cref="SearchResult" /> 的新实例。
        /// </summary>
        /// <param name="location"></param>
        /// <param name="similarity"></param>
        public SearchResult(Rectangle item, double similarity)
        {
            Rect = item;
            Similarity = similarity;
            Center = GeometryHelper.Center(item);
        }

        /// <summary>
        ///     中心点
        /// </summary>
        public Point Center { get; private set; }

        /// <summary>
        ///     边框
        /// </summary>
        public Rectangle Rect { get; private set; }

        /// <summary>
        ///     相似度，0.0 - 1.0，1.0为完全相同。
        /// </summary>
        public double Similarity { get; private set; }

        /// <summary>
        ///     实现比较方法，相似度大的排在前面。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(SearchResult obj)
        {
            return Similarity.CompareTo(obj.Similarity) * -1;
        }

        /// <summary>
        ///     返回结果信息。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}", Similarity, Rect.Location);
        }
    }
}