using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        public static Rectangle[] SearchVerticalPattern(Bitmap screen, Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int minLength, int tolerance)
        {
            VerticalPatternSearch search = new VerticalPatternSearch(screen);
            return search.FindAll(searchFor, searchArea, startFrom, maxCount, tolerance);
        }
        internal class VerticalPatternSearch : ImageRecognitionHelper.HorizontalPatternSearch
        {
            public VerticalPatternSearch(Bitmap searchOn)
                : base(searchOn)
            {
            }

            protected override bool CheckLocation(int x, int y)
            {
                if ((((x >= base._sl) && (y >= base._st)) && ((x < base._sr) && (y < base._sb))) && BitmapHelper.CompareBitmapLocked(base._searchOnData, base._searchForData, x, y, base._tolerance))
                {
                    int num = y + base._searchFor.Height;
                    while ((num < base._sb) && BitmapHelper.CompareBitmapLocked(base._searchOnData, base._searchForData, x, num, base._tolerance))
                    {
                        num += base._searchFor.Height;
                    }
                    int num2 = y - base._searchFor.Height;
                    while ((num2 >= base._st) && BitmapHelper.CompareBitmapLocked(base._searchOnData, base._searchForData, x, num2, base._tolerance))
                    {
                        num2 -= base._searchFor.Height;
                    }
                    Rectangle r = Rectangle.FromLTRB(x, num2 + base._searchFor.Height, x + base._searchFor.Width, num);
                    if ((r.Height / base._searchFor.Height) >= base._minLength)
                    {
                        base.MaskRectangle(r);
                        base._result.Add(r);
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
