using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Mobot.Imaging.Microsoft.Extension
{
    internal static class UITestUtilities
    {
        public static void CheckForNull(IntPtr parameter, string parameterName)
        {
            if (parameter == IntPtr.Zero)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void CheckForNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void CheckForNull(string parameter, string parameterName)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void CheckForPointWithinControlBounds(int offsetX, int offsetY, Rectangle controlBound, string message)
        {
            if (((offsetX < 0) || (offsetY < 0)) || ((offsetX > controlBound.Width) || (offsetY > controlBound.Height)))
            {
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static object ConvertIUnknownToTypedObject(object iunknownObject, Type type)
        {
            IntPtr ptr2;
            IntPtr iUnknownForObjectInContext = Marshal.GetIUnknownForObjectInContext(iunknownObject);
            Guid iid = Marshal.GenerateGuidForType(type);
            Marshal.ThrowExceptionForHR(Marshal.QueryInterface(iUnknownForObjectInContext, ref iid, out ptr2));
            return Marshal.GetTypedObjectForIUnknown(ptr2, type);
        }
    }
}

