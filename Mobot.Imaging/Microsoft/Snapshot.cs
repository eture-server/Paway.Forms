using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace Mobot.Imaging.Microsoft
{
    internal class Snapshot
    {
        private Color[,] buffer;

        protected Snapshot()
        {
        }

        internal Snapshot(int height, int width)
        {
            this.buffer = new Color[height, width];
        }

        private Bitmap CreateBitmap()
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    bitmap.SetPixel(j, i, this.buffer[i, j]);
                }
            }
            return bitmap;
        }

        internal static Snapshot FromImage(Image sourceImage)
        {
            Snapshot snapshot = new Snapshot(sourceImage.Height, sourceImage.Width);
            Bitmap bitmap = new Bitmap(sourceImage);
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    snapshot[i, j] = bitmap.GetPixel(j, i);
                }
            }
            return snapshot;
        }

        internal void SetAllPixels(Color colorToSet)
        {
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    this[i, j] = colorToSet;
                }
            }
        }

        internal Image ToImage()
        {
            return this.CreateBitmap();
        }

        internal virtual int Height
        {
            get
            {
                return this.buffer.GetLength(0);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1023")]
        internal virtual Color this[int row, int column]
        {
            get
            {
                return this.buffer[row, column];
            }
            set
            {
                this.buffer[row, column] = value;
            }
        }

        internal virtual int Width
        {
            get
            {
                return this.buffer.GetLength(1);
            }
        }
    }
}

