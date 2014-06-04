using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        public static Rectangle[] SearchHorizontalPattern(Bitmap screen, Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int minLength, int tolerance)
        {
            HorizontalPatternSearch search = new HorizontalPatternSearch(screen);
            return search.FindAll(searchFor, searchArea, startFrom, maxCount, tolerance);
        }

        internal class HorizontalPatternSearch : ImageRecognitionHelper.BitmapSearch
        {
            protected int _minLength;

            public HorizontalPatternSearch(Bitmap searchOn)
                : base(searchOn)
            {
                this._minLength = 2;
            }

            protected override bool CheckLocation(int x, int y)
            {
                if ((((x >= base._sl) && (y >= base._st)) && ((x < base._sr) && (y < base._sb))) && BitmapHelper.CompareBitmapLocked(base._searchOnData, base._searchForData, x, y, base._tolerance))
                {
                    int num = x + base._searchFor.Width;
                    while ((num < base._sr) && BitmapHelper.CompareBitmapLocked(base._searchOnData, base._searchForData, num, y, base._tolerance))
                    {
                        num += base._searchFor.Width;
                    }
                    int num2 = x - base._searchFor.Width;
                    while ((num2 >= base._sl) && BitmapHelper.CompareBitmapLocked(base._searchOnData, base._searchForData, num2, y, base._tolerance))
                    {
                        num2 -= base._searchFor.Width;
                    }
                    Rectangle r = Rectangle.FromLTRB(num2 + base._searchFor.Width, y, num, y + base._searchForData.Height);
                    if ((r.Width / base._searchFor.Width) >= this._minLength)
                    {
                        base.MaskRectangle(r);
                        base._result.Add(r);
                        return true;
                    }
                }
                return false;
            }

            public Rectangle[] FindAll(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int minLength, int tolerance)
            {
                this._minLength = minLength;
                return base.FindAll(searchFor, searchArea, startFrom, maxCount, tolerance);
            }

            public void StartSearch(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int minLength, int tolerance)
            {
                this._minLength = minLength;
                base.StartSearch(searchFor, searchArea, startFrom, maxCount, tolerance);
            }
        }
    }
}
