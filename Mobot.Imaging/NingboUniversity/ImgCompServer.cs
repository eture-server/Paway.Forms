using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Mobot.Imaging
{
    public class ImgCompServer : MarshalByRefObject
    {
        private static Object thisLock = new Object();
        int count = 1;
        public ResultData CompareImage(RequestInfo info)
        {
            int currentCount = count++;
            Console.WriteLine("[{0}] comparing...", currentCount);

            Bitmap image = null;
            Bitmap subImage = null;
            MemoryStream streamImage = new MemoryStream(info.Image);
            image = Image.FromStream(streamImage) as Bitmap;
            MemoryStream streamSubImage = new MemoryStream(info.SubImage);
            subImage = Image.FromStream(streamSubImage) as Bitmap;

            Rectangle rect = new Rectangle(
                info.SearchAreaX,
                info.SearchAreaY,
                info.SearchAreaWidth,
                info.SearchAreaHeight);

            Point location = Point.Empty;
            double similarity = 0.0;
            string message = "";
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                lock (thisLock)
                {
                    ImgCompInterop.CompareImage(image, subImage, rect, out location, out similarity);
                }
                message = string.Format("[{0}]SUCCESS bounds=({1:000},{2:000},{3:000},{4:000}) similarity={5:00.0%} time={6:000ms}",
                    currentCount,
                    location.X, location.Y, subImage.Width, subImage.Height,
                    similarity,
                    stopwatch.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                message = string.Format("[{0}]FAILED Error={1}", currentCount, ex.Message);
            }
            finally
            {
                stopwatch.Stop();
                Console.SetCursorPosition(0, Console.CursorTop);
            }
            
            Process p = Process.GetCurrentProcess();
            TimeSpan timeSpan = DateTime.Now.Subtract(p.StartTime);
            string processInfo = string.Format("Process: Time={0:00}:{1:00}:{2:00}.{3:000}",
                timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            processInfo += string.Format(" Memory={0:000,000}KB", (long)p.MinWorkingSet / 1024);
            processInfo += string.Format(" {0:000,000}KB", (long)p.MaxWorkingSet / 1024);
            processInfo += string.Format(" {0:000,000}KB", (long)p.WorkingSet64 / 1024);
            processInfo += string.Format(" {0:000,000}KB", (long)p.PeakWorkingSet64 / 1024);
            Console.WriteLine(processInfo);

            return new ResultData(location.X, location.Y, similarity);
        }
        public static void Start(string ipAddress, int port)
        {
            //注册信道
            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ImgCompServer), "CompareImage", WellKnownObjectMode.Singleton);
        }
    }
}
