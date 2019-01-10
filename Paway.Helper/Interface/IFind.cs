using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 搜索接口
    /// </summary>
    public interface IFind<T>
    {
        /// <summary>
        /// AsParallel搜索
        /// </summary>
        Func<T, bool> Find(string value);
    }
}