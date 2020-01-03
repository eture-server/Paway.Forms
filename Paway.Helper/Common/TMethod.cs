using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 一些公共方法
    /// </summary>
    public abstract partial class TMethod
    {
        #region 硬件
        /// <summary>
        /// 获取本机IP地址(局域网地址)
        /// </summary>
        public static IPAddress GetInternalIP()
        {
            IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
            var ip = entry.AddressList.FirstOrDefault(e => e.AddressFamily.ToString().Equals("InterNetwork"));
            return ip;
        }

        #endregion

        #region 时间格式化
        /// <summary>
        /// 时间转化
        /// </summary>
        /// <param name="time">秒</param>
        /// <returns>HH:mm:ss</returns>
        public static string Times(int time)
        {
            var day = time / 24 / 3600;
            var hour = time / 3600 % 24;
            var minutes = time / 60 % 60;
            var seconds = time % 60;
            return Times(new TimeSpan(day, hour, minutes, seconds));
        }
        /// <summary>
        /// 时间转化
        /// </summary>
        /// <param name="time">TimeSpan</param>
        /// <returns>HH:mm:ss</returns>
        public static string Times(TimeSpan time)
        {
            if (time.TotalMinutes < 60)
            {
                return string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
            }
            if (time.TotalHours < 24)
            {
                return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
            }
            return string.Format("{0}.{1:D2}:{2:D2}:{3:D2}", time.Days, time.Hours, time.Minutes, time.Seconds);
        }
        /// <summary>
        /// Sqlite日期查询格式
        /// </summary>
        public static string SqlTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 系统格式日期
        /// </summary>
        public static string AllTime(DateTime dt)
        {
            return dt.ToString("g");
        }
        /// <summary>
        /// 自动显示日期长度
        /// </summary>
        public static string AutoTime(DateTime dt)
        {
            if (dt.Year != DateTime.Now.Year || dt.Month != DateTime.Now.Month || dt.Day != DateTime.Now.Day)
            {
                return dt.ToString("g");
            }
            return dt.ToString("t");
        }

        #endregion

        #region 行列转化
        /// <summary>
        /// 行列转化
        /// </summary>
        /// <typeparam name="T">转化类型</typeparam>
        /// <typeparam name="I">数据类型</typeparam>
        /// <param name="list">数据列表</param>
        /// <returns>转化实例</returns>
        public static T Conversion<T, I>(List<I> list) where I : IInfo
        {
            Type type = typeof(T);
            T obj = Activator.CreateInstance<T>();
            var descriptors = type.Descriptors();
            foreach (IInfo info in list)
            {
                var descriptor = descriptors.Find(c => c.Name == info.Name);
                if (descriptor != null)
                {
                    obj.SetValue(descriptor, info.Value);
                }
            }
            return obj;
        }

        #endregion

        #region 自定义配置
        /// <summary>
        /// 初始化(加载)配置文件
        /// </summary>
        public static T Load<T>(string xmlFile = null)
        {
            T obj;
            if (xmlFile.IsNullOrEmpty())
            {
                xmlFile = AppDomain.CurrentDomain.FriendlyName.Replace(".vshost", string.Empty).Replace("exe", "xml");
                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
            }
            if (File.Exists(xmlFile))
            {
                obj = XmlHelper.Load<T>(xmlFile);
            }
            else
            {
                obj = Activator.CreateInstance<T>();
            }
            XmlHelper.Save(xmlFile, obj);
            return obj;
        }
        /// <summary>
        /// 保存配置实例到文件
        /// </summary>
        public static void Save<T>(T obj, string xmlFile = null)
        {
            if (xmlFile.IsNullOrEmpty())
            {
                xmlFile = AppDomain.CurrentDomain.FriendlyName.Replace(".vshost", string.Empty).Replace("exe", "xml");
                xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
            }
            XmlHelper.Save(xmlFile, obj);
        }

        #endregion

        #region 关于四舍五入
        /// <summary>
        /// 中国式四舍五入,默认两位
        /// </summary>
        public static double Round(double value, int decimals = 2)
        {
            return Math.Round(value.ClearError(), decimals, MidpointRounding.AwayFromZero);

        }
        /// <summary>
        /// 关于数字格式化
        /// </summary>
        public static string Rounds(double value, int max = 2, int min = 0)
        {
            string length = string.Empty;
            for (int i = 0; i < max && i < min; i++)
                length += "0";
            for (int i = min; i < max; i++)
                length += "#";
            return string.Format("{0:0." + length + "}", value);
        }
        /// <summary>
        /// 关于显示数字(最低两位)
        /// </summary>
        public static string Number(double number, int decimals = 2)
        {
            return TMethod.Rounds(number, decimals, 2);
        }
        /// <summary>
        /// 关于货币格式化(最低两位)
        /// </summary>
        public static string Money(double money, int decimals = 2)
        {
            return CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol + TMethod.Rounds(money, decimals, 2);
        }

        #endregion

        #region 关于随机数
        private static Random random;
        /// <summary>
        /// 获取一个随机整数
        /// </summary>
        public static int Random()
        {
            return Random(int.MaxValue / 100);
        }
        /// <summary>
        /// 获取一个随机整数(指定最大值)
        /// </summary>
        public static int Random(int max)
        {
            return Random(0, max);
        }
        /// <summary>
        /// 获取一个随机整数(指定最大、最小值)
        /// </summary>
        public static int Random(int min, int max)
        {
            if (max <= min) return min;
            if (random == null)
            {
                random = new Random();
            }
            int index = random.Next(0, max * 100);
            index %= max - min;
            index += min;
            return index;
        }

        #endregion

        #region 最大公约数
        /// <summary>
        /// 两个数的最大公约数
        /// </summary>
        public static int MaxDivisor(int x, int y)
        {
            int z;
            while (y != 0)
            {
                z = x % y;
                x = y;
                y = z;
            }
            return (x);
        }

        #endregion
    }
}