using System.Drawing;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        public static Rectangle[] SearchBorder(Bitmap screen, Bitmap searchFor, Rectangle searchArea, Point startFrom,
            int maxCount, int minLength, int tolerance)
        {
            var search = new BorderSearch(screen);
            return search.FindAll(searchFor, searchArea, startFrom, maxCount, tolerance);
        }

        internal class BorderSearch : BitmapSearch
        {
            protected int _borderWidth;

            public BorderSearch(Bitmap searchOn)
                : base(searchOn)
            {
                _borderWidth = 1;
            }

            public Rectangle[] FindAll(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount,
                int borderWidth, int tolerance)
            {
                _borderWidth = borderWidth;
                if (_borderWidth < 1)
                {
                    _borderWidth = 1;
                }
                var image = BitmapHelper.CloneBitmap(searchFor);
                var rect = new Rectangle(borderWidth, borderWidth, image.Width - 2 * borderWidth,
                    image.Height - 2 * borderWidth);
                BitmapHelper.MaskRectangle(image, rect, true);
                return base.FindAll(image, searchArea, startFrom, maxCount, tolerance);
            }

            public void StartSearch(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount,
                int borderWidth, int tolerance)
            {
                base.StartSearch(searchFor, searchArea, startFrom, maxCount, tolerance);
                _borderWidth = borderWidth;
                if (_borderWidth < 1)
                {
                    _borderWidth = 1;
                }
            }
        }
    }
}