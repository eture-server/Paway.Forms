using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using Mobot.Imaging.Microsoft.Extension;

namespace Mobot.Imaging.Microsoft
{
    public static class ImageComparer
    {
        internal static ColorDifference Compare(Color color1, Color color2)
        {
            return new ColorDifference { Alpha = (byte)Math.Abs((int)(color1.A - color2.A)), Red = (byte)Math.Abs((int)(color1.R - color2.R)), Green = (byte)Math.Abs((int)(color1.G - color2.G)), Blue = (byte)Math.Abs((int)(color1.B - color2.B)) };
        }

        /// <summary>
        /// 比较两个图像检查它们是否相同。
        /// </summary>
        /// <param name="actualImage">实际图像。</param>
        /// <param name="expectedImage">预期的图像。</param>
        /// <returns>如果两个图像完全相同，则为 true。</returns>
        public static bool Compare(Image actualImage, Image expectedImage)
        {
            ColorDifference argbTolerance = new ColorDifference();
            return Compare(actualImage, expectedImage, argbTolerance);
        }

        /// <summary>
        /// 由、矩形的指定区域比较两个图像的特定区域。
        /// 如果图像区别在于、内，则返回true。
        /// </summary>
        /// <param name="actualImage">实际图像。</param>
        /// <param name="expectedImage">预期的图像。</param>
        /// <param name="rectangleList">容差矩形列表指示比较和容错值区域。</param>
        /// <returns>如果两个图像满足在公差矩形中指定的值，则为 true。</returns>
        public static bool Compare(Image actualImage, Image expectedImage, List<ToleranceRectangle> rectangleList)
        {
            Image diffImage = null;
            return CompareInternal(actualImage, expectedImage, rectangleList, out diffImage, false);
        }

        /// <summary>
        /// 比较两个图像检查它们是否相同，并且计算两个图像之间的差异作为diff图像。
        /// </summary>
        /// <param name="actualImage">实际图像。</param>
        /// <param name="expectedImage">预期的图像。</param>
        /// <param name="diffImage">图像的 argb 值表示两个图像之间的差异。</param>
        /// <returns>如果两个图像相同，则为 true。</returns>
        public static bool Compare(Image actualImage, Image expectedImage, out Image diffImage)
        {
            ColorDifference argbTolerance = new ColorDifference();
            return Compare(actualImage, expectedImage, argbTolerance, out diffImage);
        }

        /// <summary>
        /// 基于整个图像的容错值比较两幅图像。
        /// </summary>
        /// <param name="actualImage">实际图像。</param>
        /// <param name="expectedImage">预期的图像。</param>
        /// <param name="argbTolerance">两个图像的argb之间的最大批准的差异。</param>
        /// <returns>True，如果两个图像由比指定的、位于的数量不同。</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        public static bool Compare(Image actualImage, Image expectedImage, ColorDifference argbTolerance)
        {
            Image diffImage = null;
            return CompareInternal(actualImage, expectedImage, argbTolerance, out diffImage, false);
        }

        /// <summary>
        /// 由、矩形的指定区域比较两个图像的特定区域。
        /// 返回true;如果图像区别在于、内，并且计算、丢失作为diff图像边距。
        /// </summary>
        /// <param name="actualImage">实际图像。</param>
        /// <param name="expectedImage">预期的图像。</param>
        /// <param name="rectangleList">容差矩形列表指示比较和容错值区域。</param>
        /// <param name="diffImage">argb值的图形表示形式、丢失的边距。</param>
        /// <returns>如果两个图像满足在公差矩形中指定的值，则为 true。</returns>
        public static bool Compare(Image actualImage, Image expectedImage, List<ToleranceRectangle> rectangleList, out Image diffImage)
        {
            return CompareInternal(actualImage, expectedImage, rectangleList, out diffImage, true);
        }

        /// <summary>
        /// 基于整个图像的容错值比较两幅图像。
        /// </summary>
        /// <param name="actualImage">实际图像。</param>
        /// <param name="expectedImage">预期的图像。</param>
        /// <param name="rectangleList">两个图像的argb之间的最大批准的差异。</param>
        /// <param name="diffImage">argb值的图形表示形式、丢失的边距。</param>
        /// <returns>True，如果两个图像由比指定的、位于的数量不同。</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        public static bool Compare(Image actualImage, Image expectedImage, ColorDifference argbTolerance, out Image diffImage)
        {
            return CompareInternal(actualImage, expectedImage, argbTolerance, out diffImage, true);
        }

        private static bool CompareInternal(Snapshot actualSnapshot, Snapshot expectedSnapshot, Snapshot toleranceMap, out Image diffImage, bool createOutImage)
        {
            bool flag = true;
            Snapshot snapshot = null;
            diffImage = null;
            if ((actualSnapshot.Width != expectedSnapshot.Width) || (actualSnapshot.Height != expectedSnapshot.Height))
            {
                throw new InvalidOperationException(Resource.ImageSizesNotEqual);
            }
            if (createOutImage)
            {
                snapshot = new Snapshot(actualSnapshot.Height, actualSnapshot.Width);
                snapshot.SetAllPixels(Color.FromArgb(0xff, 0, 0, 0));
            }
            for (int i = 0; i < actualSnapshot.Height; i++)
            {
                for (int j = 0; j < actualSnapshot.Width; j++)
                {
                    ColorDifference difference = Compare(actualSnapshot[i, j], expectedSnapshot[i, j]);
                    Color color = toleranceMap[i, j];
                    Color color2 = toleranceMap[i, j];
                    Color color3 = toleranceMap[i, j];
                    Color color4 = toleranceMap[i, j];
                    if (!difference.MeetsTolerance(new ColorDifference(color.A, color2.R, color3.G, color4.B)))
                    {
                        flag = false;
                        if (!createOutImage)
                        {
                            return flag;
                        }
                        Color color5 = toleranceMap[i, j];
                        Color color6 = toleranceMap[i, j];
                        Color color7 = toleranceMap[i, j];
                        Color color8 = toleranceMap[i, j];
                        snapshot[i, j] = difference.CalculateMargin(new ColorDifference(color5.A, color6.R, color7.G, color8.B));
                    }
                }
            }
            if (createOutImage)
            {
                diffImage = snapshot.ToImage();
            }
            return flag;
        }

        private static bool CompareInternal(Image actualImage, Image expectedImage, ColorDifference argbTolerance, out Image diffImage, bool createOutImage)
        {
            UITestUtilities.CheckForNull(actualImage, "actualImage");
            UITestUtilities.CheckForNull(expectedImage, "expectedImage");
            UITestUtilities.CheckForNull(argbTolerance, "argbTolerance");
            Snapshot actualSnapshot = Snapshot.FromImage(actualImage);
            Snapshot expectedSnapshot = Snapshot.FromImage(expectedImage);
            if ((actualSnapshot.Width != expectedSnapshot.Width) || (actualSnapshot.Height != expectedSnapshot.Height))
            {
                throw new InvalidOperationException(Resource.ImageSizesNotEqual);
            }
            SingleValueToleranceMap toleranceMap = new SingleValueToleranceMap(Color.FromArgb(argbTolerance.Alpha, argbTolerance.Red, argbTolerance.Green, argbTolerance.Blue));
            return CompareInternal(actualSnapshot, expectedSnapshot, toleranceMap, out diffImage, createOutImage);
        }

        private static bool CompareInternal(Image actualImage, Image expectedImage, List<ToleranceRectangle> rectangleList, out Image diffImage, bool createOutImage)
        {
            UITestUtilities.CheckForNull(actualImage, "actualImage");
            UITestUtilities.CheckForNull(expectedImage, "expectedImage");
            UITestUtilities.CheckForNull(rectangleList, "rectangleList");
            Snapshot actualSnapshot = Snapshot.FromImage(actualImage);
            Snapshot expectedSnapshot = Snapshot.FromImage(expectedImage);
            if ((actualSnapshot.Width != expectedSnapshot.Width) || (actualSnapshot.Height != expectedSnapshot.Height))
            {
                throw new InvalidOperationException(Resource.ImageSizesNotEqual);
            }
            Snapshot toleranceMap = CreateToleranceMap(rectangleList, actualSnapshot.Height, actualSnapshot.Width);
            return CompareInternal(actualSnapshot, expectedSnapshot, toleranceMap, out diffImage, createOutImage);
        }

        private static Snapshot CreateToleranceMap(List<ToleranceRectangle> rectangleList, int height, int width)
        {
            Snapshot snapshot = new Snapshot(height, width);
            snapshot.SetAllPixels(Color.White);
            for (int i = 0; i < rectangleList.Count; i++)
            {
                ToleranceRectangle rectangle9;
                ToleranceRectangle rectangle11;
                ToleranceRectangle rectangle13;
                ToleranceRectangle rectangle17;
                ToleranceRectangle rectangle19;
                ToleranceRectangle rectangle = rectangleList[i];
                if (rectangle.Rectangle.Left >= 0)
                {
                    ToleranceRectangle rectangle3 = rectangleList[i];
                    if (rectangle3.Rectangle.Top >= 0)
                    {
                        ToleranceRectangle rectangle5 = rectangleList[i];
                        if (rectangle5.Rectangle.Right <= width)
                        {
                            ToleranceRectangle rectangle7 = rectangleList[i];
                            if (rectangle7.Rectangle.Bottom <= height)
                            {
                                goto Label_00B3;
                            }
                        }
                    }
                }
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resource.RectangleNotInRange, new object[] { i }));
            Label_00B3:
                rectangle9 = rectangleList[i];
                int top = rectangle9.Rectangle.Top;
                goto Label_0176;
            Label_00D2:
                rectangle11 = rectangleList[i];
                int left = rectangle11.Rectangle.Left;
                goto Label_0153;
            Label_00EE:
                rectangle13 = rectangleList[i];
                ToleranceRectangle rectangle14 = rectangleList[i];
                ToleranceRectangle rectangle15 = rectangleList[i];
                ToleranceRectangle rectangle16 = rectangleList[i];
                snapshot[top, left] = Color.FromArgb(rectangle13.Difference.Alpha, rectangle14.Difference.Red, rectangle15.Difference.Green, rectangle16.Difference.Blue);
                left++;
            Label_0153:
                rectangle17 = rectangleList[i];
                if (left < rectangle17.Rectangle.Right)
                {
                    goto Label_00EE;
                }
                top++;
            Label_0176:
                rectangle19 = rectangleList[i];
                if (top < rectangle19.Rectangle.Bottom)
                {
                    goto Label_00D2;
                }
            }
            return snapshot;
        }
    }
}

