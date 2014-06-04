using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        public static Rectangle[] SearchBorder(Bitmap screen, Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int minLength, int tolerance)
        {
            BorderSearch search = new BorderSearch(screen);
            return search.FindAll(searchFor, searchArea, startFrom, maxCount, tolerance);
        }
        internal class BorderSearch : ImageRecognitionHelper.BitmapSearch
        {
            protected int _borderWidth;

            public BorderSearch(Bitmap searchOn)
                : base(searchOn)
            {
                this._borderWidth = 1;
            }

            public Rectangle[] FindAll(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int borderWidth, int tolerance)
            {
                this._borderWidth = borderWidth;
                if (this._borderWidth < 1)
                {
                    this._borderWidth = 1;
                }
                Bitmap image = BitmapHelper.CloneBitmap(searchFor);
                Rectangle rect = new Rectangle(borderWidth, borderWidth, image.Width - (2 * borderWidth), image.Height - (2 * borderWidth));
                BitmapHelper.MaskRectangle(image, rect, true);
                return base.FindAll(image, searchArea, startFrom, maxCount, tolerance);
            }

            public void StartSearch(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int borderWidth, int tolerance)
            {
                base.StartSearch(searchFor, searchArea, startFrom, maxCount, tolerance);
                this._borderWidth = borderWidth;
                if (this._borderWidth < 1)
                {
                    this._borderWidth = 1;
                }
            }
        }
    }
}
