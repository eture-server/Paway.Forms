using System;
using System.Drawing;
using System.Reflection;

namespace Mobot.Imaging.Microsoft
{
    internal class SingleValueToleranceMap : Snapshot
    {
        private Color toleraceColor;

        internal SingleValueToleranceMap(Color color)
        {
            this.toleraceColor = color;
        }

        internal override Color this[int row, int column]
        {
            get
            {
                return this.toleraceColor;
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}

