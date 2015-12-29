using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        /// <summary>
        ///     检索比对图像
        /// </summary>
        /// <param name="searchFor">小图</param>
        /// <param name="searchOn">大图</param>
        /// <returns></returns>
        public static SearchResult[] SearchBitmap_Test(Bitmap searchFor, Bitmap searchOn)
        {
            return SearchBitmap_Test(searchFor, searchOn, 0.03);
        }

        public static SearchResult[] SearchBitmap_Test(Bitmap searchFor, Bitmap searchOn, double tolerance)
        {
            var searchArea = new Rectangle(0, 0, searchOn.Width, searchOn.Height);
            var search = new BitmapSearch(searchOn);
            var results = search.FindAll(searchFor, searchArea, GeometryHelper.Center(searchArea), 50,
                (int)(tolerance * 100));

            var list = new List<SearchResult>();
            foreach (var item in results)
            {
                list.Add(new SearchResult(item, 1.0));
            }
            list.Sort();
            return list.ToArray<SearchResult>();
        }
    }
}