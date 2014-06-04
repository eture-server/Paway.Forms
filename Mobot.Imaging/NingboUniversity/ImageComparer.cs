using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Diagnostics;

namespace Mobot.Imaging
{
    /// <summary>
    /// 提供图像比较、模糊查找等功能。
    /// </summary>
    public class ImageComparer
    {
        static bool useRemoting = false;
        static string serverIP = "127.0.0.1";
        static int serverPort = 15320;
        static bool isLocal = false;
        static ImgCompServer serviceObj = null;
        static ImageComparer()
        {
            SetOptions(false, "127.0.0.1", 15320);
        }
        /// <summary>
        /// 设置图像比对选项。
        /// </summary>
        /// <param name="useRemoting"></param>
        /// <param name="serverIP"></param>
        /// <param name="serverPort"></param>
        public static void SetOptions(bool useRemoting, string serverIP, int serverPort)
        {
            if (serviceObj != null)
                serviceObj = null;

            ImageComparer.useRemoting = useRemoting;
            ImageComparer.serverIP = serverIP;
            ImageComparer.serverPort = serverPort;
            string defaultUrl = string.Format("tcp://{0}:{1}/CompareImage", serverIP, serverPort);

            if (useRemoting)
                serviceObj = (ImgCompServer)Activator.GetObject(typeof(ImgCompServer), defaultUrl);

            isLocal = false;
            if (string.Equals(serverIP, "localhost", StringComparison.OrdinalIgnoreCase)
                || string.Equals(serverIP, "127.0.0.1", StringComparison.OrdinalIgnoreCase))
                isLocal = true;
        }
        /// <summary>
        /// 在图片中的指定区域内查找图标。
        /// </summary>
        /// <param name="image">要处理的图片</param>
        /// <param name="subImage">图标（子图）</param>
        /// <param name="area">可能出现的区域</param>
        /// <param name="location">输出：最佳位置</param>
        /// <param name="similarity">输出：相似度（0.0 - 1.0）</param>
        public static void CompareImage(Bitmap image, Bitmap subImage, Rectangle area, out Point location, out double similarity)
        {
            if (useRemoting)
            {
                try
                {
                    if (isLocal)
                    {
                        StartCompareServer(serverPort);
                    }

                    CompareImageRemoting(image, subImage, area, out location, out similarity);
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                ImgCompInterop.
                CompareImage(image, subImage, area, out location, out similarity);
            }

        }
        static void StartCompareServer(int serverPort)
        {
            Process[] list = Process.GetProcessesByName("Mobot.Core.Imaging.Server");
            if (list != null && list.Length > 0)//已启动
                return;

            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string fileName = Path.Combine(basePath, "Mobot.Core.Imaging.Server.exe");
            if (!File.Exists(fileName))//服务程序不存在
                return;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
                startInfo.Arguments = string.Format("{0} {1}",
                    Process.GetCurrentProcess().Id,
                    "127.0.0.1:" + serverPort);
                startInfo.UseShellExecute = false;
#if DEBUG
                startInfo.CreateNoWindow = false;
#else
                startInfo.CreateNoWindow = true;
#endif
                Process.Start(startInfo);
            }
            catch (Exception)
            { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="subImage"></param>
        /// <param name="area"></param>
        /// <param name="location"></param>
        /// <param name="similarity"></param>
        internal static void CompareImageRemoting(Bitmap image, Bitmap subImage, Rectangle area, out Point location, out double similarity)
        {
            byte[] imageData;
            byte[] subImageData;
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                imageData = stream.ToArray();
            }
            using (MemoryStream stream = new MemoryStream())
            {
                subImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                subImageData = stream.ToArray();
            }
            try
            {
                ResultData result = serviceObj.CompareImage(new RequestInfo(imageData, subImageData, area));
                location = new Point(result.locationX, result.locationY);
                similarity = result.Similarity;
            }
            catch
            {
                location = Point.Empty;
                similarity = 0.0;
            }
            finally
            {
                imageData = null;
                subImageData = null;
            }
        }

        public static bool UseRemoting
        {
            get { return useRemoting; }
        }
        public static string ServerIP
        {
            get { return serverIP; }
        }
        public static int ServerPort
        {
            get { return serverPort; }
        }
    }
}
