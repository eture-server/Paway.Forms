using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        public static Rectangle[] SearchFourCorners(Bitmap screen, Bitmap topLeft, Bitmap topRight, Bitmap bottomLeft, Bitmap bottomRight, Rectangle searchArea, Point startFrom, int maxCount, int tolerance)
        {
            FourCornersSearch search = new FourCornersSearch(screen);
            return search.FindAll(topLeft, topRight, bottomLeft, bottomRight, searchArea, startFrom, maxCount, tolerance);
        }

        internal class FourCornersSearch : ImageRecognitionHelper.BitmapSearch
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
                if (((x >= base._sl) && (y >= base._st)) && ((x < base._sr) && (y < base._sb)))
                {
                    switch (this._startCorner)
                    {
                        case 0:
                            if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topLeftData, x, y, base._tolerance))
                            {
                                for (int i = x + this._topLeft.Width; i < base._sr; i++)
                                {
                                    if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topRightData, i, y, base._tolerance))
                                    {
                                        for (int j = y + this._topLeft.Height; j < base._sb; j++)
                                        {
                                            if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomRightData, i, j, base._tolerance) && BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomLeftData, x, j, base._tolerance))
                                            {
                                                Rectangle rectangle = this.Expand(Rectangle.FromLTRB(x, y, i + this._bottomRight.Width, j + this._bottomRight.Height));
                                                base._result.Add(rectangle);
                                                rectangle.Inflate(-1, -1);
                                                base.MaskRectangle(rectangle);
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        case 1:
                            if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topRightData, x, y, base._tolerance))
                            {
                                for (int k = y + this._topRight.Height; k < base._sb; k++)
                                {
                                    if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomRightData, x, k, base._tolerance))
                                    {
                                        for (int m = x - this._bottomLeft.Width; m >= base._sl; m--)
                                        {
                                            if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomLeftData, m, k, base._tolerance) && BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topLeftData, m, y, base._tolerance))
                                            {
                                                Rectangle rectangle2 = this.Expand(Rectangle.FromLTRB(m, y, x + this._topRight.Width, k + this._bottomRight.Height));
                                                base._result.Add(rectangle2);
                                                rectangle2.Inflate(-1, -1);
                                                base.MaskRectangle(rectangle2);
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        case 2:
                            if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomLeftData, x, y, base._tolerance))
                            {
                                for (int n = y - this._topLeft.Height; n >= base._st; n--)
                                {
                                    if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topLeftData, x, n, base._tolerance))
                                    {
                                        for (int num6 = x + this._topLeft.Width; num6 < base._sr; num6++)
                                        {
                                            if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topRightData, num6, n, base._tolerance) && BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomRightData, num6, y, base._tolerance))
                                            {
                                                Rectangle rectangle3 = this.Expand(Rectangle.FromLTRB(x, n, num6 + this._topRight.Width, y + this._bottomLeft.Height));
                                                base._result.Add(rectangle3);
                                                rectangle3.Inflate(-1, -1);
                                                base.MaskRectangle(rectangle3);
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        case 3:
                            if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomRightData, x, y, base._tolerance))
                            {
                                for (int num7 = x - this._bottomLeft.Width; num7 >= base._sl; num7--)
                                {
                                    if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomLeftData, num7, y, base._tolerance))
                                    {
                                        for (int num8 = y - this._topLeft.Height; num8 >= base._st; num8--)
                                        {
                                            if (BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topLeftData, num7, num8, base._tolerance) && BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topRightData, x, num8, base._tolerance))
                                            {
                                                Rectangle rectangle4 = this.Expand(Rectangle.FromLTRB(num7, num8, x + this._bottomRight.Width, y + this._bottomRight.Height));
                                                base._result.Add(rectangle4);
                                                rectangle4.Inflate(-1, -1);
                                                base.MaskRectangle(rectangle4);
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
                uint* numPtr = (uint*)cornerBitmapData.Scan0;
                int num = 0;
                int capacity = cornerBitmapData.Width * cornerBitmapData.Height;
                ArrayList list = new ArrayList(capacity);
                int num3 = 0;
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
                Rectangle rectangle = rect;
                rectangle = this.ExpandToRight(rectangle);
                rectangle = this.ExpandToLeft(rectangle);
                rectangle = this.ExpandToTop(rectangle);
                return this.ExpandToBottom(rectangle);
            }

            protected Rectangle ExpandToBottom(Rectangle rect)
            {
                Rectangle rectangle = rect;
                int x = rect.X;
                int num2 = rect.Right - this._bottomRightData.Width;
                int num3 = (rect.Bottom - this._bottomRightData.Height) + 1;
                for (int i = num3; i < base._sb; i++)
                {
                    if (!BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomLeftData, x, i, base._tolerance) || !BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomRightData, num2, i, base._tolerance))
                    {
                        return rectangle;
                    }
                    rectangle.Height++;
                }
                return rectangle;
            }

            protected Rectangle ExpandToLeft(Rectangle rect)
            {
                Rectangle rectangle = rect;
                int num = rect.X - 1;
                int y = rect.Y;
                int num3 = rect.Bottom - this._bottomLeftData.Height;
                for (int i = num; i >= base._sl; i--)
                {
                    if (!BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topLeftData, i, y, base._tolerance) || !BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomLeftData, i, num3, base._tolerance))
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
                Rectangle rectangle = rect;
                int num = (rect.Right - this._topRightData.Width) + 1;
                int y = rect.Y;
                int num3 = rect.Bottom - this._bottomRightData.Height;
                for (int i = num; i < base._sr; i++)
                {
                    if (!BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topRightData, i, y, base._tolerance) || !BitmapHelper.CompareBitmapLocked(base._searchOnData, this._bottomRightData, i, num3, base._tolerance))
                    {
                        return rectangle;
                    }
                    rectangle.Width++;
                }
                return rectangle;
            }

            protected Rectangle ExpandToTop(Rectangle rect)
            {
                Rectangle rectangle = rect;
                int x = rect.X;
                int num2 = rect.Right - this._topRightData.Width;
                int num3 = rect.Y - 1;
                for (int i = num3; i >= base._st; i--)
                {
                    if (!BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topLeftData, x, i, base._tolerance) || !BitmapHelper.CompareBitmapLocked(base._searchOnData, this._topRightData, num2, i, base._tolerance))
                    {
                        return rectangle;
                    }
                    rectangle.Y--;
                    rectangle.Height++;
                }
                return rectangle;
            }

            public Rectangle[] FindAll(Bitmap topLeft, Bitmap topRight, Bitmap bottomLeft, Bitmap bottomRight, Rectangle searchArea, Point startFrom, int maxCount, int tolerance)
            {
                this.StartSearch(topLeft, topRight, bottomLeft, bottomRight, searchArea, startFrom, maxCount, tolerance);
                while (!this.FindNext().IsEmpty)
                {
                }
                return (Rectangle[])base._result.ToArray(typeof(Rectangle));
            }

            public override Rectangle FindNext()
            {
                Rectangle rectangle;
                this._topLeftData = null;
                this._topRightData = null;
                this._bottomLeftData = null;
                this._bottomRightData = null;
                try
                {
                    this._topLeftData = BitmapHelper.LockBits(this._topLeft, ImageLockMode.ReadOnly);
                    this._topRightData = BitmapHelper.LockBits(this._topRight, ImageLockMode.ReadOnly);
                    this._bottomLeftData = BitmapHelper.LockBits(this._bottomLeft, ImageLockMode.ReadOnly);
                    this._bottomRightData = BitmapHelper.LockBits(this._bottomRight, ImageLockMode.ReadOnly);
                    rectangle = base.FindNext();
                }
                finally
                {
                    if (this._topLeftData != null)
                    {
                        BitmapHelper.UnlockBits(this._topLeft, this._topLeftData);
                    }
                    if (this._topRightData != null)
                    {
                        BitmapHelper.UnlockBits(this._topRight, this._topRightData);
                    }
                    if (this._bottomLeftData != null)
                    {
                        BitmapHelper.UnlockBits(this._bottomLeft, this._bottomLeftData);
                    }
                    if (this._bottomRightData != null)
                    {
                        BitmapHelper.UnlockBits(this._bottomRight, this._bottomRightData);
                    }
                }
                return rectangle;
            }

            public void StartSearch(Bitmap topLeft, Bitmap topRight, Bitmap bottomLeft, Bitmap bottomRight, Rectangle searchArea, Point startFrom, int maxCount, int tolerance)
            {
                int[] numArray;
                this._topLeft = BitmapHelper.CloneBitmap(topLeft);
                this._topRight = BitmapHelper.CloneBitmap(topRight);
                this._bottomLeft = BitmapHelper.CloneBitmap(bottomLeft);
                this._bottomRight = BitmapHelper.CloneBitmap(bottomRight);
                base.StartSearch(topLeft, searchArea, startFrom, maxCount, tolerance);
                this._topLeftData = null;
                this._topRightData = null;
                this._bottomLeftData = null;
                this._bottomRightData = null;
                try
                {
                    this._topLeftData = BitmapHelper.LockBits(this._topLeft, ImageLockMode.ReadOnly);
                    this._topRightData = BitmapHelper.LockBits(this._topRight, ImageLockMode.ReadOnly);
                    this._bottomLeftData = BitmapHelper.LockBits(this._bottomLeft, ImageLockMode.ReadOnly);
                    this._bottomRightData = BitmapHelper.LockBits(this._bottomRight, ImageLockMode.ReadOnly);
                    numArray = new int[] { CornerQuality(this._topLeftData), CornerQuality(this._topRightData), CornerQuality(this._bottomLeftData), CornerQuality(this._bottomRightData) };
                }
                finally
                {
                    if (this._topLeftData != null)
                    {
                        BitmapHelper.UnlockBits(this._topLeft, this._topLeftData);
                    }
                    if (this._topRightData != null)
                    {
                        BitmapHelper.UnlockBits(this._topRight, this._topRightData);
                    }
                    if (this._bottomLeftData != null)
                    {
                        BitmapHelper.UnlockBits(this._bottomLeft, this._bottomLeftData);
                    }
                    if (this._bottomRightData != null)
                    {
                        BitmapHelper.UnlockBits(this._bottomRight, this._bottomRightData);
                    }
                }
                int num = -2147483648;
                this._startCorner = -1;
                for (int i = 0; i < numArray.Length; i++)
                {
                    if (numArray[i] > num)
                    {
                        num = numArray[i];
                        this._startCorner = i;
                    }
                }
            }
        }
    }
}
