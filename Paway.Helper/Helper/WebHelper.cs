using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Paway.Helper
{
    /// <summary>
    /// 网络时间
    /// </summary>
    public abstract class WebHelper
    {
        /// <summary>
        /// 获取标准北京时间，读取http://www.beijing-time.org/time.asp
        /// </summary>
        /// <returns>返回网络时间</returns>
        public static DateTime BeijingTime()
        {
            DateTime dt;
            WebRequest wrt = null;
            WebResponse wrp = null;
            try
            {
                wrt = WebRequest.Create("http://www.beijing-time.org/time.asp");
                wrp = wrt.GetResponse();

                var html = string.Empty;
                var stream = wrp.GetResponseStream();
                using (var sr = new StreamReader(stream, Encoding.UTF8))
                {
                    html = sr.ReadToEnd();
                }

                var tempArray = html.Split(';');
                for (var i = 0; i < tempArray.Length; i++)
                {
                    tempArray[i] = tempArray[i].Replace("\r\n", string.Empty);
                }

                var year = tempArray[1].Split('=')[1];
                var month = tempArray[2].Split('=')[1];
                var day = tempArray[3].Split('=')[1];
                var hour = tempArray[5].Split('=')[1];
                var minite = tempArray[6].Split('=')[1];
                var second = tempArray[7].Split('=')[1];

                dt = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minite + ":" + second);
            }
            finally
            {
                if (wrp != null)
                    wrp.Close();
                if (wrt != null)
                    wrt.Abort();
            }
            return dt;
        }

        /// <summary>
        /// 获取网页内容，并过滤
        /// </summary>
        public static string Html(string url, Regex regex, Encoding code, params object[] args)
        {
            using (var client = new WebClient())
            {
                //获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                client.Credentials = CredentialCache.DefaultCredentials;
                //从指定网站下载数据
                client.Encoding = code;
                url = string.Format(url, args);
                var html = client.DownloadString(url);
                if (regex != null && regex.IsMatch(html))
                {
                    html = regex.Replace(html, string.Empty);
                }
                return html;
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        public static Image Image(string url)
        {
            using (var client = new WebClient())
            {
                //获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                client.Credentials = CredentialCache.DefaultCredentials;
                //从指定网站下载图片
                client.DownloadFile(url, "tmp.png");
                return BitmapHelper.GetBitmapFormFile("tmp.png");
            }
        }
    }
}