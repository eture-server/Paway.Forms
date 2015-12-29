using System.Drawing;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        public static Rectangle[] SearchVerticalPattern(Bitmap screen, Bitmap searchFor, Rectangle searchArea,
            Point startFrom, int maxCount, int minLength, int tolerance)
        {
            var search = new VerticalPatternSearch(screen);
            return search.FindAll(searchFor, searchArea, startFrom, maxCount, tolerance);
        }

        internal class VerticalPatternSearch : HorizontalPatternSearch
        {
            public VerticalPatternSearch(Bitmap searchOn)
                : base(searchOn)
            {
            }

            protected override bool CheckLocation(int x, int y)
            {
                if ((x >= _sl) && (y >= _st) && (x < _sr) && (y < _sb) &&
                    BitmapHelper.CompareBitmapLocked(_searchOnData, _searchForData, x, y, _tolerance))
                {
                    var num = y + _searchFor.Height;
                    while ((num < _sb) &&
                           BitmapHelper.CompareBitmapLocked(_searchOnData, _searchForData, x, num, _tolerance))
                    {
                        num += _searchFor.Height;
                    }
                    var num2 = y - _searchFor.Height;
                    while ((num2 >= _st) &&
                           BitmapHelper.CompareBitmapLocked(_searchOnData, _searchForData, x, num2, _tolerance))
                    {
                        num2 -= _searchFor.Height;
                    }
                    var r = Rectangle.FromLTRB(x, num2 + _searchFor.Height, x + _searchFor.Width, num);
                    if (r.Height / _searchFor.Height >= _minLength)
                    {
                        MaskRectangle(r);
                        _result.Add(r);
                        return true;
                    }
                }
                return false;
            }
        }
    }
}