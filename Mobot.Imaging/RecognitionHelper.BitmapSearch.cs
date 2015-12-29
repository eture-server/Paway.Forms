using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mobot.Imaging
{
    partial class ImageRecognitionHelper
    {
        public static Rectangle[] SearchBitmap(Bitmap screen, Bitmap searchFor, Rectangle searchArea, Point startFrom,
            int maxCount, int tolerance)
        {
            var search = new BitmapSearch(screen);
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
            ///     要查找的图片（子图）。
            /// </summary>
            protected Bitmap _searchFor;

            protected BitmapData _searchForData;

            /// <summary>
            ///     大图。
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
                _searchOn = BitmapHelper.CloneBitmap(searchOn);
            }

            public Rectangle[] Result
            {
                get { return (Rectangle[])_result.ToArray(typeof(Rectangle)); }
            }

            public bool SearchFinished
            {
                get { return _searchFinished; }
            }

            public bool SearchStarted
            {
                get { return _searchStarted; }
            }

            protected virtual bool CheckLocation(int cx, int cy)
            {
                if ((cx >= _sl) && (cy >= _st) && (cx < _sr) && (cy < _sb) &&
                    BitmapHelper.CompareBitmapLocked(_searchOnData, _searchForData, cx, cy, _tolerance))
                {
                    var r = new Rectangle(cx, cy, _searchForData.Width, _searchForData.Height);
                    MaskRectangle(r);
                    _result.Add(r);
                    return true;
                }
                return false;
            }

            public virtual Rectangle[] FindAll(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount,
                int tolerance)
            {
                StartSearch(searchFor, searchArea, startFrom, maxCount, tolerance);
                while (!FindNext().IsEmpty)
                {
                }
                return (Rectangle[])_result.ToArray(typeof(Rectangle));
            }

            public virtual Rectangle FindNext()
            {
                Rectangle empty;
                _searchOnData = null;
                _searchForData = null;
                try
                {
                    _searchOnData = BitmapHelper.LockBits(_searchOn, ImageLockMode.ReadOnly);
                    _searchForData = BitmapHelper.LockBits(_searchFor, ImageLockMode.ReadOnly);
                    if (_searchFinished)
                    {
                        return Rectangle.Empty;
                    }
                    if (!_searchStarted)
                    {
                        throw new ApplicationException(
                            "Search is not started. Use StartSearch prior to calling FindNext");
                    }
                    if (_offset != 0)
                    {
                        goto Label_02CE;
                    }
                    _offset++;
                    if (!CheckLocation(_x, _y))
                    {
                        goto Label_02CE;
                    }
                    return (Rectangle)_result[_result.Count - 1];
                Label_00A3:
                    switch (_pos)
                    {
                        case 0:
                            _x0 = _x - _i + 1;
                            _y0 = _y - _offset;
                            break;

                        case 1:
                            _x0 = _x + _i;
                            _y0 = _y - _offset;
                            break;

                        case 2:
                            _x0 = _x + _offset;
                            _y0 = _y - _i + 1;
                            break;

                        case 3:
                            _x0 = _x + _offset;
                            _y0 = _y + _i;
                            break;

                        case 4:
                            _x0 = _x + _i - 1;
                            _y0 = _y + _offset;
                            break;

                        case 5:
                            _x0 = _x - _i;
                            _y0 = _y + _offset;
                            break;

                        case 6:
                            _x0 = _x - _offset;
                            _y0 = _y + _i - 1;
                            break;

                        case 7:
                            _x0 = _x - _offset;
                            _y0 = _y - _i;
                            break;
                    }
                    _pos++;
                    if (CheckLocation(_x0, _y0))
                    {
                        if (_result.Count <= _maxCount)
                        {
                            return (Rectangle)_result[_result.Count - 1];
                        }
                        return Rectangle.Empty;
                    }
                Label_028A:
                    if (_pos < 8)
                    {
                        goto Label_00A3;
                    }
                    _i++;
                    _pos = 0;
                Label_02AB:
                    if (_i <= _offset)
                    {
                        goto Label_028A;
                    }
                    _offset++;
                    _i = 1;
                Label_02CE:
                    if (_offset <= _maxOffset)
                    {
                        goto Label_02AB;
                    }
                    _searchFinished = true;
                    empty = Rectangle.Empty;
                }
                finally
                {
                    if (_searchForData != null)
                    {
                        BitmapHelper.UnlockBits(_searchFor, _searchForData);
                    }
                    if (_searchOnData != null)
                    {
                        BitmapHelper.UnlockBits(_searchOn, _searchOnData);
                    }
                }
                return empty;
            }

            protected void MaskRectangle(Rectangle R)
            {
                BitmapHelper.MaskRectangle(_searchOnData, R, true);
            }

            public virtual void StartSearch(Bitmap searchFor, Rectangle searchArea, Point startFrom, int maxCount,
                int tolerance)
            {
                BitmapHelper.CheckBitmap(searchFor);
                _searchFor = BitmapHelper.CloneBitmap(searchFor);
                _startFrom = startFrom;
                _tolerance = tolerance * 0xfe01 / 100;
                _searchStarted = true;
                _maxCount = maxCount;
                _searchFinished = false;
                var rectangle = searchArea;
                rectangle.Intersect(new Rectangle(0, 0, _searchOn.Width, _searchOn.Height));
                rectangle.Width -= _searchFor.Width - 1;
                rectangle.Height -= _searchFor.Height - 1;
                _sl = rectangle.Left;
                _st = rectangle.Top;
                _sr = rectangle.Right;
                _sb = rectangle.Bottom;
                _startFrom.X -= _searchFor.Width / 2;
                _startFrom.Y -= _searchFor.Height / 2;
                if (_startFrom.X < _sl)
                {
                    _startFrom.X = _sl;
                }
                if (_startFrom.Y < _st)
                {
                    _startFrom.Y = _st;
                }
                if (_startFrom.X >= _sr)
                {
                    _startFrom.X = _sr - 1;
                }
                if (_startFrom.Y >= _sb)
                {
                    _startFrom.Y = _sb - 1;
                }
                _maxOffset = Math.Max(Math.Max(_startFrom.X - _sl, _sr - _startFrom.X),
                    Math.Max(_startFrom.Y - _st, _sb - _startFrom.Y));
                _x = _startFrom.X;
                _y = _startFrom.Y;
                _offset = 0;
                _i = 1;
                _pos = 0;
                _result = new ArrayList(_maxCount);
            }
        }
    }
}