using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Mobot.Imaging.Microsoft
{
    public class ColorDifference
    {
        public ColorDifference()
        {
            this.Alpha = 0xff;
            this.Red = 0;
            this.Green = 0;
            this.Blue = 0;
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        public ColorDifference(byte rgbTolerance)
        {
            this.Alpha = 0xff;
            this.Red = rgbTolerance;
            this.Green = rgbTolerance;
            this.Blue = rgbTolerance;
        }

        public ColorDifference(byte red, byte green, byte blue)
        {
            this.Alpha = 0xff;
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public ColorDifference(byte alpha, byte red, byte green, byte blue)
        {
            this.Alpha = alpha;
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        internal Color CalculateMargin(ColorDifference tolerance)
        {
            return Color.FromArgb(Math.Abs((int) (this.Alpha - tolerance.Alpha)), Math.Abs((int) (this.Red - tolerance.Red)), Math.Abs((int) (this.Green - tolerance.Green)), Math.Abs((int) (this.Blue - tolerance.Blue)));
        }

        public override bool Equals(object other)
        {
            ColorDifference difference = other as ColorDifference;
            if (difference == null)
            {
                return false;
            }
            return (this == difference);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        internal bool MeetsTolerance(ColorDifference tolerance)
        {
            return ((((this.Alpha <= tolerance.Alpha) && (this.Red <= tolerance.Red)) && (this.Green <= tolerance.Green)) && (this.Blue <= tolerance.Blue));
        }

        public static bool operator ==(ColorDifference left, ColorDifference right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            if ((left == null) || (right == null))
            {
                return false;
            }
            return ((((left.Alpha == right.Alpha) && (left.Red == right.Red)) && (left.Green == right.Green)) && (left.Blue == right.Blue));
        }

        public static bool operator !=(ColorDifference left, ColorDifference right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Resource.ColorDifferenceToString, new object[] { this.Alpha, this.Red, this.Green, this.Blue });
        }

        public byte Alpha { get; set; }

        public byte Blue { get; set; }

        public byte Green { get; set; }

        public byte Red { get; set; }
    }
}

