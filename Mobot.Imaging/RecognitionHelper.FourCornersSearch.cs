using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        public static Rectangle[] SearchFourCorners(Bitmap screen, Bitmap topLeft, Bitmap topRight, Bitmap bottomLeft,
            Bitmap bottomRight, Rectangle searchArea, Point startFrom, int maxCount, int tolerance)
        {
            var search = new FourCornersSearch(screen);
            return search.FindAll(topLeft, topRight, bottomLeft, bottomRight, searchArea, startFrom, maxCount, tolerance);
        }

        internal class FourCornersSearch : BitmapSearch
        {
            private Bitmap _bottomLeft;
            protected BitmapData _bottomLeftData;
            private Bitmap _bottomRight;
            protected BitmapData _bottomRightData;
            protected int _startCorner;
            private Bitmap _topLeft;
            protected BitmapData _topLeftData;
            private Bitmap _topRight;
            protected BitmapData _topRightData;

            public FourCornersSearch(Bitmap searchOn)
                : base(searchOn)
            {
            }

            protected override bool CheckLocation(int x, int y)
            {
                if ((x >= _sl) && (y >= _st) && (x < _sr) && (y < _sb))
                {
                    switch (_startCorner)
                    {
                        case 0:
                            if (BitmapHelper.CompareBitmapLocked(_searchOnData, _topLeftData, x, y, _tolerance))
                            {
                                for (var i = x + _topLeft.Width; i < _sr; i++)
                                {
                                    if (BitmapHelper.CompareBitmapLocked(_searchOnData, _topRightData, i, y, _tolerance))
                                    {
                                        for (var j = y + _topLeft.Height; j < _sb; j++)
                                        {
                                            if (
                                                BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomRightData, i, j,
                                                    _tolerance) &&
                                                BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomLeftData, x, j,
                                                    _tolerance))
                                            {
                                                var rectangle =
                                                    Expand(Rectangle.FromLTRB(x, y, i + _bottomRight.Width,
                                                        j + _bottomRight.Height));
                                                _result.Add(rectangle);
                                                rectangle.Inflate(-1, -1);
                                                MaskRectangle(rectangle);
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        case 1:
                            if (BitmapHelper.CompareBitmapLocked(_searchOnData, _topRightData, x, y, _tolerance))
                            {
                                for (var k = y + _topRight.Height; k < _sb; k++)
                                {
                                    if (BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomRightData, x, k,
                                        _tolerance))
                                    {
                                        for (var m = x - _bottomLeft.Width; m >= _sl; m--)
                                        {
                                            if (
                                                BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomLeftData, m, k,
                                                    _tolerance) &&
                                                BitmapHelper.CompareBitmapLocked(_searchOnData, _topLeftData, m, y,
                                                    _tolerance))
                                            {
                                                var rectangle2 =
                                                    Expand(Rectangle.FromLTRB(m, y, x + _topRight.Width,
                                                        k + _bottomRight.Height));
                                                _result.Add(rectangle2);
                                                rectangle2.Inflate(-1, -1);
                                                MaskRectangle(rectangle2);
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        case 2:
                            if (BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomLeftData, x, y, _tolerance))
                            {
                                for (var n = y - _topLeft.Height; n >= _st; n--)
                                {
                                    if (BitmapHelper.CompareBitmapLocked(_searchOnData, _topLeftData, x, n, _tolerance))
                                    {
                                        for (var num6 = x + _topLeft.Width; num6 < _sr; num6++)
                                        {
                                            if (
                                                BitmapHelper.CompareBitmapLocked(_searchOnData, _topRightData, num6, n,
                                                    _tolerance) &&
                                                BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomRightData, num6,
                                                    y, _tolerance))
                                            {
                                                var rectangle3 =
                                                    Expand(Rectangle.FromLTRB(x, n, num6 + _topRight.Width,
                                                        y + _bottomLeft.Height));
                                                _result.Add(rectangle3);
                                                rectangle3.Inflate(-1, -1);
                                                MaskRectangle(rectangle3);
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        case 3:
                            if (BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomRightData, x, y, _tolerance))
                            {
                                for (var num7 = x - _bottomLeft.Width; num7 >= _sl; num7--)
                                {
                                    if (BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomLeftData, num7, y,
                                        _tolerance))
                                    {
                                        for (var num8 = y - _topLeft.Height; num8 >= _st; num8--)
                                        {
                                            if (
                                                BitmapHelper.CompareBitmapLocked(_searchOnData, _topLeftData, num7, num8,
                                                    _tolerance) &&
                                                BitmapHelper.CompareBitmapLocked(_searchOnData, _topRightData, x, num8,
                                                    _tolerance))
                                            {
                                                var rectangle4 =
                                                    Expand(Rectangle.FromLTRB(num7, num8, x + _bottomRight.Width,
                                                        y + _bottomRight.Height));
                                                _result.Add(rectangle4);
                                                rectangle4.Inflate(-1, -1);
                                                MaskRectangle(rectangle4);
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                return false;
            }

            protected static unsafe int CornerQuality(BitmapData cornerBitmapData)
            {
                var numPtr = (uint*)cornerBitmapData.Scan0;
                var num = 0;
                var capacity = cornerBitmapData.Width * cornerBitmapData.Height;
                var list = new ArrayList(capacity);
                var num3 = 0;
                while (num3 < capacity)
                {
                    if ((numPtr[0] & 0xff000000) == 0xff000000)
                    {
                        if (list.IndexOf(numPtr[0]) < 0)
                        {
                            list.Add(numPtr[0]);
                            num++;
                        }
                    }
                    else
                    {
                        num--;
                    }
                    num3++;
                    numPtr++;
                }
                return num;
            }

            protected Rectangle Expand(Rectangle rect)
            {
                var rectangle = rect;
                rectangle = ExpandToRight(rectangle);
                rectangle = ExpandToLeft(rectangle);
                rectangle = ExpandToTop(rectangle);
                return ExpandToBottom(rectangle);
            }

            protected Rectangle ExpandToBottom(Rectangle rect)
            {
                var rectangle = rect;
                var x = rect.X;
                var num2 = rect.Right - _bottomRightData.Width;
                var num3 = rect.Bottom - _bottomRightData.Height + 1;
                for (var i = num3; i < _sb; i++)
                {
                    if (!BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomLeftData, x, i, _tolerance) ||
                        !BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomRightData, num2, i, _tolerance))
                    {
                        return rectangle;
                    }
                    rectangle.Height++;
                }
                return rectangle;
            }

            protected Rectangle ExpandToLeft(Rectangle rect)
            {
                var rectangle = rect;
                var num = rect.X - 1;
                var y = rect.Y;
                var num3 = rect.Bottom - _bottomLeftData.Height;
                for (var i = num; i >= _sl; i--)
                {
                    if (!BitmapHelper.CompareBitmapLocked(_searchOnData, _topLeftData, i, y, _tolerance) ||
                        !BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomLeftData, i, num3, _tolerance))
                    {
                        return rectangle;
                    }
                    rectangle.X--;
                    rectangle.Width++;
                }
                return rectangle;
            }

            protected Rectangle ExpandToRight(Rectangle rect)
            {
                var rectangle = rect;
                var num = rect.Right - _topRightData.Width + 1;
                var y = rect.Y;
                var num3 = rect.Bottom - _bottomRightData.Height;
                for (var i = num; i < _sr; i++)
                {
                    if (!BitmapHelper.CompareBitmapLocked(_searchOnData, _topRightData, i, y, _tolerance) ||
                        !BitmapHelper.CompareBitmapLocked(_searchOnData, _bottomRightData, i, num3, _tolerance))
                    {
                        return rectangle;
                    }
                    rectangle.Width++;
                }
                return rectangle;
            }

            protected Rectangle ExpandToTop(Rectangle rect)
            {
                var rectangle = rect;
                var x = rect.X;
                var num2 = rect.Right - _topRightData.Width;
                var num3 = rect.Y - 1;
                for (var i = num3; i >= _st; i--)
                {
                    if (!BitmapHelper.CompareBitmapLocked(_searchOnData, _topLeftData, x, i, _tolerance) ||
                        !BitmapHelper.CompareBitmapLocked(_searchOnData, _topRightData, num2, i, _tolerance))
                    {
                        return rectangle;
                    }
                    rectangle.Y--;
                    rectangle.Height++;
                }
                return rectangle;
            }

            public Rectangle[] FindAll(Bitmap topLeft, Bitmap topRight, Bitmap bottomLeft, Bitmap bottomRight,
                Rectangle searchArea, Point startFrom, int maxCount, int tolerance)
            {
                StartSearch(topLeft, topRight, bottomLeft, bottomRight, searchArea, startFrom, maxCount, tolerance);
                while (!FindNext().IsEmpty)
                {
                }
                return (Rectangle[])_result.ToArray(typeof(Rectangle));
            }

            public override Rectangle FindNext()
            {
                Rectangle rectangle;
                _topLeftData = null;
                _topRightData = null;
                _bottomLeftData = null;
                _bottomRightData = null;
                try
                {
                    _topLeftData = BitmapHelper.LockBits(_topLeft, ImageLockMode.ReadOnly);
                    _topRightData = BitmapHelper.LockBits(_topRight, ImageLockMode.ReadOnly);
                    _bottomLeftData = BitmapHelper.LockBits(_bottomLeft, ImageLockMode.ReadOnly);
                    _bottomRightData = BitmapHelper.LockBits(_bottomRight, ImageLockMode.ReadOnly);
                    rectangle = base.FindNext();
                }
                finally
                {
                    if (_topLeftData != null)
                    {
                        BitmapHelper.UnlockBits(_topLeft, _topLeftData);
                    }
                    if (_topRightData != null)
                    {
                        BitmapHelper.UnlockBits(_topRight, _topRightData);
                    }
                    if (_bottomLeftData != null)
                    {
                        BitmapHelper.UnlockBits(_bottomLeft, _bottomLeftData);
                    }
                    if (_bottomRightData != null)
                    {
                        BitmapHelper.UnlockBits(_bottomRight, _bottomRightData);
                    }
                }
                return rectangle;
            }

            public void StartSearch(Bitmap topLeft, Bitmap topRight, Bitmap bottomLeft, Bitmap bottomRight,
                Rectangle searchArea, Point startFrom, int maxCount, int tolerance)
            {
                int[] numArray;
                _topLeft = BitmapHelper.CloneBitmap(topLeft);
                _topRight = BitmapHelper.CloneBitmap(topRight);
                _bottomLeft = BitmapHelper.CloneBitmap(bottomLeft);
                _bottomRight = BitmapHelper.CloneBitmap(bottomRight);
                base.StartSearch(topLeft, searchArea, startFrom, maxCount, tolerance);
                _topLeftData = null;
                _topRightData = null;
                _bottomLeftData = null;
                _bottomRightData = null;
                try
                {
                    _topLeftData = BitmapHelper.LockBits(_topLeft, ImageLockMode.ReadOnly);
                    _topRightData = BitmapHelper.LockBits(_topRight, ImageLockMode.ReadOnly);
                    _bottomLeftData = BitmapHelper.LockBits(_bottomLeft, ImageLockMode.ReadOnly);
                    _bottomRightData = BitmapHelper.LockBits(_bottomRight, ImageLockMode.ReadOnly);
                    numArray = new[]
                    {
                        CornerQuality(_topLeftData), CornerQuality(_topRightData), CornerQuality(_bottomLeftData),
                        CornerQuality(_bottomRightData)
                    };
                }
                finally
                {
                    if (_topLeftData != null)
                    {
                        BitmapHelper.UnlockBits(_topLeft, _topLeftData);
                    }
                    if (_topRightData != null)
                    {
                        BitmapHelper.UnlockBits(_topRight, _topRightData);
                    }
                    if (_bottomLeftData != null)
                    {
                        BitmapHelper.UnlockBits(_bottomLeft, _bottomLeftData);
                    }
                    if (_bottomRightData != null)
                    {
                        BitmapHelper.UnlockBits(_bottomRight, _bottomRightData);
                    }
                }
                var num = -2147483648;
                _startCorner = -1;
                for (var i = 0; i < numArray.Length; i++)
                {
                    if (numArray[i] > num)
                    {
                        num = numArray[i];
                        _startCorner = i;
                    }
                }
            }
        }
    }
}