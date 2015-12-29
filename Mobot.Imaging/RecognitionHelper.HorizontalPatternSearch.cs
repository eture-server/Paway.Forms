using System.Drawing;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        public static Rectangle[] SearchHorizontalPattern(Bitmap screen, Bitmap searchFor, Rectangle searchArea,
            Point startFrom, int maxCount, int minLength, int tolerance)
        {
            var search = new HorizontalPatternSearch(screen);
            return search.FindAll(searchFor, searchArea, startFrom, maxCount, tolerance);
        }

        internal class HorizontalPatternSearch : BitmapSearch
        {
            protected int _minLength;

            public HorizontalPatternSearch(Bitmap searchOn)
                : base(searchOn)
            {
                _minLength = 2;
            }

            protected override bool CheckLocation(int x, int y)
            {
                if ((x >= _sl) && (y >= _st) && (x < _sr) && (y < _sb) &&
                    BitmapHelper.CompareBitmapLocked(_searchOnData, _searchForData, x, y, _tolerance))
                {
                    var num = x + _searchFor.Width;
                    while ((num < _sr) &&
                           BitmapHelper.CompareBitmapLocked(_searchOnData, _searchForData, num, y, _tolerance))
                    {
                        num += _searchFor.Width;
                    }
                    var num2 = x - _searchFor.Width;
                    while ((num2 >= _sl) &&
                           BitmapHelper.CompareBitmapLocked(_searchOnData, _searchForData, num2, y, _tolerance))
                    {
                        num2 -= _searchFor.Width;
                    }
                    var r = Rectangle.FromLTRB(num2 + _searchFor.Width, y, num, y + _searchForData.Height);
                    if (r.Width / _searchFor.Width >= _minLength)
                    {
                        MaskRectangle(r);
                        _result.Add(r);
                        return true;
                    }
                }
                return false;
            }

            public Rectangle[] FindAll(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount,
                int minLength, int tolerance)
            {
                _minLength = minLength;
                return base.FindAll(searchFor, searchArea, startFrom, maxCount, tolerance);
            }

            public void StartSearch(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int minLength,
                int tolerance)
            {
                _minLength = minLength;
                base.StartSearch(searchFor, searchArea, startFrom, maxCount, tolerance);
            }
        }
    }
}