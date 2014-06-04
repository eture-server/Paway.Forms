using System;

namespace Mobot.Imaging
{
    public interface IResizeOptions : ICloneable
    {
        ResizeInterpolationMode InterpolationMode { get; set; }

        float Magnification { get; set; }
    }
}

