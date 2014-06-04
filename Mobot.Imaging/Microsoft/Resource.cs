using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobot.Imaging.Microsoft
{
    internal sealed class Resource
    {
        public static string ColorDifferenceToString
        {
            get { return "{Alpha={0}, Red={1}, Green={2}, Blue={3}}"; }
        }

        public static string RectangleNotInRange
        {
            get { return "Tolerance rectangle at index {0} does not lie within range of the image."; }
        }

        public static string ImageSizesNotEqual
        {
            get { return "Images must be of equal size to be compared."; }
        }
    }
}
