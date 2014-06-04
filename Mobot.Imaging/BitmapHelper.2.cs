// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mobot.Imaging
{
    partial class BitmapHelper
    {
        public static byte[] ToRawData(Bitmap image)
        {
            BitmapData bmpData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.WriteOnly,
                image.PixelFormat);
            IntPtr ptr = bmpData.Scan0;

            byte[] rawData = new byte[bmpData.Stride * image.Height];
            Marshal.Copy(ptr, rawData, 0, rawData.Length);
            image.UnlockBits(bmpData);

            return rawData;
        }
        public static Bitmap FromRawData(byte[] rawData, int width, int height, PixelFormat format)
        {
            Bitmap bmp = new Bitmap(width, height, format);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            IntPtr pRaw = bmpData.Scan0;
            Marshal.Copy(rawData, 0, pRaw, rawData.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
        public static Bitmap SetRawData(Bitmap destBmp, byte[] rawData, int width, int height)
        {
            BitmapData bmpData = destBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, destBmp.PixelFormat);
            IntPtr pRaw = bmpData.Scan0;
            Marshal.Copy(rawData, 0, pRaw, rawData.Length);
            destBmp.UnlockBits(bmpData);

            return destBmp;
        }
        //public static Bitmap FromRawData(byte[] data, int width, int height)
        //{
        //    IntPtr ptr = ppBuffer[i];
        //    byte[] pRawBuffer = new byte[imageWidth * imageHeight];
        //    Marshal.Copy(ptr, pRawBuffer, 0, pRawBuffer.Length);

        //    Bitmap image = new Bitmap(imageWidth, imageHeight, imageWidth * 1, PixelFormat.Format8bppIndexed, ppBuffer[0]);
        //    ColorPalette palette = image.Palette;
        //    for (int y = 0; y < palette.Entries.Length; y++)
        //    {
        //        palette.Entries[y] = Color.FromArgb(y, y, y);
        //    }
        //    image.Palette = palette;

        //    images[i] = image;
        //}

        /// <summary>
        /// 获取图像亮度信息。
        /// </summary>
        public unsafe static void GetLuminance(Bitmap screen, out double maxLuminance)
        {
            Rectangle rect = new Rectangle(0, 0, screen.Width, screen.Height);
            BitmapData data = screen.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            maxLuminance = 0;
            uint* numPtr = (uint*)((((byte*)data.Scan0) + (data.Stride * rect.Top)) + (rect.Left * 4));

            for (int i = 0; i < rect.Height; i++)
            {
                for (int j = 0; j < rect.Width; j++)
                {
                    uint pixel = (numPtr++)[0];
                    uint A = (pixel >> 24) & 0xFF;
                    uint R = (pixel >> 16) & 0xff;
                    uint G = (pixel >> 8) & 0xff;
                    uint B = pixel & 0xff;
                    double Y = 0.299 * R + 0.587 * G + 0.114 * B;
                    if (maxLuminance < Y)
                        maxLuminance = Y;
                }
            }
#if DEBUG
            stopwatch.Stop();
            Debug.WriteLine(stopwatch.ElapsedMilliseconds);
#endif
            screen.UnlockBits(data);
        }
    }
}
