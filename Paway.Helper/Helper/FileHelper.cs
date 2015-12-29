using System;
using System.IO;
using System.Reflection;

namespace Paway.Helper
{
    /// <summary>
    ///     文件操作
    /// </summary>
    public abstract class FileHelper
    {
        /// <summary>
        ///     获取程序集平台定义
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static ProcessorArchitecture GetPlatform(string file)
        {
            if (string.IsNullOrEmpty(file) || !File.Exists(file)) return ProcessorArchitecture.None;
            var assembly = Assembly.LoadFrom(file);
            var attrTitle =
                Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
            var attrCopyright =
                Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute)) as
                    AssemblyCopyrightAttribute;
            //log.Info(attrCopyright.Copyright.Replace("\u00A9", "(C)"));
            var mark = assembly.GetName();
            return mark.ProcessorArchitecture;
        }

        /// <summary>
        ///     判断dll文件是否64位
        ///     此方法不可区分x86与AnyCpu
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsX64(string file)
        {
            if (string.IsNullOrEmpty(file) || !File.Exists(file)) return false;
            var result = GetPEArchitecture(file);
            if (result == 0x10b) return false;
            if (result == 0x20b) return true;
            return false;
        }

        /// <summary>
        ///     通过dll的PE Image File中指定位置的字节来判断dll文件是x86的还是x64
        /// </summary>
        /// <param name="pFilePath">dll文件</param>
        /// <returns>字节</returns>
        public static ushort GetPEArchitecture(string pFilePath)
        {
            ushort architecture = 0;
            try
            {
                using (var fStream = new FileStream(pFilePath, FileMode.Open, FileAccess.Read))
                {
                    var bReader = new BinaryReader(fStream);
                    if (bReader.ReadUInt16() == 23117) //check the MZ signature
                    {
                        fStream.Seek(0x3A, SeekOrigin.Current); //seek to e_lfanew.
                        fStream.Seek(bReader.ReadUInt32(), SeekOrigin.Begin); //seek to the start of the NT header.
                        if (bReader.ReadUInt32() == 17744) //check the PE\0\0 signature.
                        {
                            fStream.Seek(20, SeekOrigin.Current); //seek past the file header,
                            architecture = bReader.ReadUInt16(); //read the magic number of the optional header.
                        }
                    }
                }
            }
            catch (Exception)
            {
                /* TODO: Any exception handling you want to do, personally I just take 0 as a sign of failure */
            }
            //if architecture returns 0, there has been an error.
            return architecture;
        }
    }
}