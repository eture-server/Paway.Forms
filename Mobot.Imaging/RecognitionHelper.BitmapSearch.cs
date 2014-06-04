using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        public static Rectangle[] SearchBitmap(Bitmap screen, Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int tolerance)
        {
            BitmapSearch search = new BitmapSearch(screen);
            return search.FindAll(searchFor, searchArea, startFrom, maxCount, tolerance);
        }
        internal class BitmapSearch
        {
            protected int _i;
            protected int _maxCount;
            protected int _maxOffset;
            protected int _offset;
            protected int _pos;
            protected ArrayList _result;
            protected int _sb;
            protected bool _searchFinished;

            /// <summary>
            /// 要查找的图片（子图）。
            /// </summary>
            protected Bitmap _searchFor;
            protected BitmapData _searchForData;
            /// <summary>
            /// 大图。
            /// </summary>
            protected Bitmap _searchOn;
            protected BitmapData _searchOnData;
            protected bool _searchStarted;
            protected int _sl;
            protected int _sr;
            protected int _st;
            protected Point _startFrom;
            protected int _tolerance;
            protected int _x;
            protected int _x0;
            protected int _y;
            protected int _y0;

            public BitmapSearch(Bitmap searchOn)
            {
                this._searchOn = BitmapHelper.CloneBitmap(searchOn);
            }

            protected virtual bool CheckLocation(int cx, int cy)
            {
                if ((((cx >= this._sl) && (cy >= this._st)) && ((cx < this._sr) && (cy < this._sb))) && BitmapHelper.CompareBitmapLocked(this._searchOnData, this._searchForData, cx, cy, this._tolerance))
                {
                    Rectangle r = new Rectangle(cx, cy, this._searchForData.Width, this._searchForData.Height);
                    this.MaskRectangle(r);
                    this._result.Add(r);
                    return true;
                }
                return false;
            }

            public virtual Rectangle[] FindAll(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int tolerance)
            {
                this.StartSearch(searchFor, searchArea, startFrom, maxCount, tolerance);
                while (!this.FindNext().IsEmpty)
                {
                }
                return (Rectangle[])this._result.ToArray(typeof(Rectangle));
            }

            public virtual Rectangle FindNext()
            {
                Rectangle empty;
                this._searchOnData = null;
                this._searchForData = null;
                try
                {
                    this._searchOnData = BitmapHelper.LockBits(this._searchOn, ImageLockMode.ReadOnly);
                    this._searchForData = BitmapHelper.LockBits(this._searchFor, ImageLockMode.ReadOnly);
                    if (this._searchFinished)
                    {
                        return Rectangle.Empty;
                    }
                    if (!this._searchStarted)
                    {
                        throw new ApplicationException("Search is not started. Use StartSearch prior to calling FindNext");
                    }
                    if (this._offset != 0)
                    {
                        goto Label_02CE;
                    }
                    this._offset++;
                    if (!this.CheckLocation(this._x, this._y))
                    {
                        goto Label_02CE;
                    }
                    return (Rectangle)this._result[this._result.Count - 1];
                Label_00A3:
                    switch (this._pos)
                    {
                        case 0:
                            this._x0 = (this._x - this._i) + 1;
                            this._y0 = this._y - this._offset;
                            break;

                        case 1:
                            this._x0 = this._x + this._i;
                            this._y0 = this._y - this._offset;
                            break;

                        case 2:
                            this._x0 = this._x + this._offset;
                            this._y0 = (this._y - this._i) + 1;
                            break;

                        case 3:
                            this._x0 = this._x + this._offset;
                            this._y0 = this._y + this._i;
                            break;

                        case 4:
                            this._x0 = (this._x + this._i) - 1;
                            this._y0 = this._y + this._offset;
                            break;

                        case 5:
                            this._x0 = this._x - this._i;
                            this._y0 = this._y + this._offset;
                            break;

                        case 6:
                            this._x0 = this._x - this._offset;
                            this._y0 = (this._y + this._i) - 1;
                            break;

                        case 7:
                            this._x0 = this._x - this._offset;
                            this._y0 = this._y - this._i;
                            break;
                    }
                    this._pos++;
                    if (this.CheckLocation(this._x0, this._y0))
                    {
                        if (this._result.Count <= this._maxCount)
                        {
                            return (Rectangle)this._result[this._result.Count - 1];
                        }
                        return Rectangle.Empty;
                    }
                Label_028A:
                    if (this._pos < 8)
                    {
                        goto Label_00A3;
                    }
                    this._i++;
                    this._pos = 0;
                Label_02AB:
                    if (this._i <= this._offset)
                    {
                        goto Label_028A;
                    }
                    this._offset++;
                    this._i = 1;
                Label_02CE:
                    if (this._offset <= this._maxOffset)
                    {
                        goto Label_02AB;
                    }
                    this._searchFinished = true;
                    empty = Rectangle.Empty;
                }
                finally
                {
                    if (this._searchForData != null)
                    {
                        BitmapHelper.UnlockBits(this._searchFor, this._searchForData);
                    }
                    if (this._searchOnData != null)
                    {
                        BitmapHelper.UnlockBits(this._searchOn, this._searchOnData);
                    }
                }
                return empty;
            }

            protected void MaskRectangle(Rectangle R)
            {
                BitmapHelper.MaskRectangle(this._searchOnData, R, true);
            }

            public virtual void StartSearch(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount, int tolerance)
            {
                BitmapHelper.CheckBitmap(searchFor);
                this._searchFor = BitmapHelper.CloneBitmap(searchFor);
                this._startFrom = startFrom;
                this._tolerance = (tolerance * 0xfe01) / 100;
                this._searchStarted = true;
                this._maxCount = maxCount;
                this._searchFinished = false;
                Rectangle rectangle = searchArea;
                rectangle.Intersect(new Rectangle(0, 0, this._searchOn.Width, this._searchOn.Height));
                rectangle.Width -= this._searchFor.Width - 1;
                rectangle.Height -= this._searchFor.Height - 1;
                this._sl = rectangle.Left;
                this._st = rectangle.Top;
                this._sr = rectangle.Right;
                this._sb = rectangle.Bottom;
                this._startFrom.X -= this._searchFor.Width / 2;
                this._startFrom.Y -= this._searchFor.Height / 2;
                if (this._startFrom.X < this._sl)
                {
                    this._startFrom.X = this._sl;
                }
                if (this._startFrom.Y < this._st)
                {
                    this._startFrom.Y = this._st;
                }
                if (this._startFrom.X >= this._sr)
                {
                    this._startFrom.X = this._sr - 1;
                }
                if (this._startFrom.Y >= this._sb)
                {
                    this._startFrom.Y = this._sb - 1;
                }
                this._maxOffset = Math.Max(Math.Max((int)(this._startFrom.X - this._sl), (int)(this._sr - this._startFrom.X)), Math.Max((int)(this._startFrom.Y - this._st), (int)(this._sb - this._startFrom.Y)));
                this._x = this._startFrom.X;
                this._y = this._startFrom.Y;
                this._offset = 0;
                this._i = 1;
                this._pos = 0;
                this._result = new ArrayList(this._maxCount);
            }

            public Rectangle[] Result
            {
                get
                {
                    return (Rectangle[])this._result.ToArray(typeof(Rectangle));
                }
            }

            public bool SearchFinished
            {
                get
                {
                    return this._searchFinished;
                }
            }

            public bool SearchStarted
            {
                get
                {
                    return this._searchStarted;
                }
            }
        }
    }
}
