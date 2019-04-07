using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Paway.Forms
{
    internal class ProgressState
    {
        public event Action CancelEvent;
        public IntPtr Handle { get; set; }
        private string _caption;
        public string Caption
        {
            get
            {
                TimeSpan time = DateTime.Now.Subtract(DateTime);
                if (time.TotalSeconds > 3)
                    return _caption + "  " + time.TotalSeconds.ToString("F1") + "s";
                return _caption;
            }
            set { _caption = value; }
        }
        public bool ITime { get { return CanCancel && DateTime.Now.Subtract(DateTime).TotalSeconds > 3; } }
        private readonly bool CanCancel;
        private bool _iCancel;
        public bool ICancel
        {
            get { return _iCancel; }
            set
            {
                _iCancel = value;
                CancelEvent?.BeginInvoke(null, null);
            }
        }
        public bool NoValue { get; set; }
        public int Max { get; set; }
        public int Value { get; set; }
        public DateTime DateTime { get; set; }
        public ProgressState(IntPtr handle, string caption, bool canCancel, int max)
        {
            this.Handle = handle;
            this.Caption = caption;
            this.CanCancel = canCancel;
            this.Max = max == 0 ? 100 : max;
            this.NoValue = max == 0;
            this.DateTime = DateTime.Now;
        }
    }
}

