using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace Paway.Helper
{
    /// <summary>
    /// 一些公共业务方法
    /// </summary>
    public partial class TMethod
    {
        /// <summary>
        /// 自动调整图片显示模式(图像比控件大=Zoom,其它=CenterImage)
        /// </summary>
        public static void AutoSizeMode(PictureBox pic)
        {
            if (pic.Image == null) return;

            if (pic.Image.Width > pic.Width || pic.Image.Height > pic.Height)
                pic.SizeMode = PictureBoxSizeMode.Zoom;
            else
                pic.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        /// <summary>
        /// 根据指定List更新目标List
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="type">操作类型</param>
        /// <param name="tList">要更新的目标List</param>
        /// <param name="fList">更新源List</param>
        public static void Update<T>(OperType type, List<T> tList, List<T> fList) where T : IId
        {
            switch (type)
            {
                case OperType.Insert:
                    tList.AddRange(fList);
                    break;
                case OperType.Update:
                    for (int i = 0; i < fList.Count; i++)
                    {
                        var temp = tList.Find(c => c.Id == fList[i].Id);
                        if (temp != null)
                            fList[i].Clone(temp);
                        else
                            tList.Add(fList[i]);
                    }
                    break;
                case OperType.Delete:
                    for (int i = 0; i < fList.Count; i++)
                    {
                        var temp = tList.Find(c => c.Id == fList[i].Id);
                        if (temp != null)
                            tList.Remove(temp);
                    }
                    break;
                case OperType.Reset:
                    tList.Clear();
                    tList.AddRange(fList);
                    break;
            }
        }
    }
}