using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using Paway.Helper;

namespace Paway.Resource
{
    /// <summary>
    ///     对当前程序集的操作类
    /// </summary>
    public class AssemblyHelper
    {
        #region 常量

        /// <summary>
        ///     程序集的名称
        /// </summary>
        private static readonly string CurrentAssemblyName = "Paway.Resource";

        //Assembly.GetExecutingAssembly().GetName().Name;

        #endregion

        #region 变量

        /// <summary>
        ///     当前程序集
        /// </summary>
        private static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();

        #endregion

        #region 方法

        /// <summary>
        ///     在嵌入的资源文件中查找相应的图片
        /// </summary>
        /// <param name="name">资源图片的文件名称+扩展名</param>
        /// <returns></returns>
        public static Image GetImage(string name)
        {
            if (!Licence.Checking()) return null;

            if (!string.IsNullOrEmpty(name))
            {
                var sb = new StringBuilder();
                if (name[0] != '.')
                    sb.Append(CurrentAssemblyName + "." + name);
                else
                    sb.Append(CurrentAssemblyName + name);
                using (var stream = CurrentAssembly.GetManifestResourceStream(sb.ToString()))
                {
                    if (stream == null)
                        throw new Exception("加载资源文件失败，可能丢失" + CurrentAssemblyName + ".dll文件。");
                    return Image.FromStream(stream);
                }
            }
            return null;
        }

        #endregion
    }
}