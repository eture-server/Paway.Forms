using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mobot.Imaging.Microsoft
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ToleranceRectangle
    {
        public System.Drawing.Rectangle Rectangle { get; set; }
        public ColorDifference Difference { get; set; }

        public static ToleranceRectangle Empty;

        public override bool Equals(object obj)
        {
            return ((obj is ToleranceRectangle) && (this == ((ToleranceRectangle) obj)));
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}{1}", new object[] { this.Rectangle, this.Difference });
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static bool operator !=(ToleranceRectangle left, ToleranceRectangle right)
        {
            return !(left == right);
        }

        public static bool operator ==(ToleranceRectangle left, ToleranceRectangle right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            if ((left == Empty) || (right == Empty))
            {
                return false;
            }
            return ((left.Rectangle == right.Rectangle) && (left.Difference == right.Difference));
        }
    }
}

