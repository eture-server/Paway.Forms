using System;
using System.Collections.Generic;
using System.Web;

namespace Mobot.Imaging
{
    [Serializable]
    public class ResultData
    {
        public int locationX;
        public int locationY;
        public double Similarity;

        public ResultData()
        {
            //
        }
        public ResultData(int x, int y, double similarity)
        {
            this.locationX = x;
            this.locationY = y;
            this.Similarity = similarity;
        }
    }
}