using System;
using System.Collections.Generic;
using System.Web;
using System.Drawing;

namespace Mobot.Imaging
{
    [Serializable]
    public class RequestInfo
    {
        public byte[] Image { get; set; }
        public byte[] SubImage { get; set; }
        public int SearchAreaX { get; set; }
        public int SearchAreaY { get; set; }
        public int SearchAreaWidth { get; set; }
        public int SearchAreaHeight { get; set; }
        public RequestInfo()
        {
            this.Image = null;
            this.SubImage = null;
            this.SearchAreaX = 0;
            this.SearchAreaY = 0;
            this.SearchAreaWidth = 0;
            this.SearchAreaHeight = 0;
        }
        public RequestInfo(byte[] image, byte[] subImage, Rectangle searchArea)
        {
            this.Image = image;
            this.SubImage = subImage;
            this.SearchAreaX = searchArea.X;
            this.SearchAreaY = searchArea.Y;
            this.SearchAreaWidth = searchArea.Width;
            this.SearchAreaHeight = searchArea.Height;
        }
    }
}