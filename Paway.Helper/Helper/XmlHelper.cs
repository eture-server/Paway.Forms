using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Paway.Helper
{
    /// <summary>
    ///     对 XML 操作的辅助类
    /// </summary>
    public abstract class XmlHelper
    {
        /// <summary>
        ///     生成XML文件
        /// </summary>
        public static void Save<T>(string file, T info)
        {
            Type type = typeof(T);
            XmlDocument doc = new XmlDocument();
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(decl);
            XmlElement root = doc.CreateElement(info.GetType().Name);
            {
                doc.AppendChild(root);
                List<Type> list = new List<Type>();
                if (type.IsInterface)
                {
                    list.AddRange(type.GetInterfaces());
                }
                list.Insert(0, type);
                for (int i = 0; i < list.Count; i++)
                {
                    string desc = Description(list[i]);
                    if (!desc.IsNullOrEmpty())
                    {
                        var element = doc.CreateComment(desc);
                        root.AppendChild(element);
                    }
                    var properties = TypeDescriptor.GetProperties(list[i]);
                    foreach (PropertyDescriptor item in properties)
                    {
                        if (!list[i].GetProperty(item.Name).IShow()) continue;

                        XmlElement element = doc.CreateElement(item.Name);
                        element.InnerText = item.GetValue(info).ToString2();
                        root.AppendChild(element);
                    }
                }
            }
            doc.Save(file);
        }
        /// <summary>
        ///     获取描述
        /// </summary>
        private static string Description(Type type)
        {
            var list = type.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (list.Length == 1) return list[0].Description;
            return null;
        }

        /// <summary>
        ///     返回对应的实体
        /// </summary>
        public static T Find<T>(string file)
        {
            Type type = typeof(T);
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode root = doc.DocumentElement;
            if (root.Name != type.Name) throw new Exception("文档类型不一致");
            XmlNode element = root.FirstChild;

            T obj = Activator.CreateInstance<T>();
            var properties = TypeDescriptor.GetProperties(typeof(T));
            while (element != null)
            {
                PropertyDescriptor item = properties.Find(element.Name, false);
                if (item != null && !element.InnerText.IsNullOrEmpty())
                {
                    obj.SetValue(item, element.InnerText);
                }
                element = element.NextSibling;
            }
            return obj;
        }
    }
}