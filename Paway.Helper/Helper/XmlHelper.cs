using System;
using System.Collections;
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
            XmlDeclaration xml = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(xml);
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
                    string desc = list[i].Description();
                    if (!desc.IsNullOrEmpty())
                    {
                        var comment = doc.CreateComment(desc);
                        root.AppendChild(comment);
                    }
                    var properties = TypeDescriptor.GetProperties(list[i]);
                    foreach (PropertyDescriptor item in properties)
                    {
                        if (!list[i].GetProperty(item.Name).IShow()) continue;

                        XmlElement element = doc.CreateElement(item.Name);
                        if (!item.Description.IsNullOrEmpty())
                        {
                            var comment = doc.CreateComment(item.Description);
                            root.AppendChild(comment);
                        }
                        element.InnerText = item.GetValue(info).ToString2();
                        root.AppendChild(element);
                    }
                }
            }
            doc.Save(file);
        }

        /// <summary>
        ///     返回对应的实体
        /// </summary>
        public static T Load<T>(string file)
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
                if (item != null && element.InnerText != null)
                {
                    obj.SetValue(item, element.InnerText);
                }
                element = element.NextSibling;
            }
            return obj;
        }

        /// <summary>
        ///     生成XML文件
        /// </summary>
        public static void Save<T>(string file, T info, string ns = null, Dictionary<string, string> args = null)
        {
            Type type = typeof(T);
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xml = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(xml);
            string desc = type.Description() ?? type.Name;
            XmlElement root = ns == null ? doc.CreateElement(desc) : doc.CreateElement(ns, desc, "n:s");
            if (args != null)
            {
                foreach (var item in args)
                {
                    root.SetAttribute(item.Key, item.Value);
                }
            }
            doc.AppendChild(root);
            Add(doc, root, info, type);
            doc.Save(file);
        }
        private static void Add<T>(XmlDocument doc, XmlElement root, T info, Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo item in fields)
            {
                if (!item.IBrowsable()) continue;
                XmlElement element = doc.CreateElement(item.Name);
                root.AppendChild(element);
                object obj = item.GetValue(info);
                if (obj is IList)
                {
                    var list = obj as IList;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i > 0)
                        {
                            element = doc.CreateElement(item.Name);
                            root.AppendChild(element);
                        }
                        Add(doc, element, list[i], list.GenericType());
                    }
                }
                else if (item.FieldType.IsClass && item.FieldType != typeof(string))
                {
                    Add(doc, element, obj, item.FieldType);
                    continue;
                }
                else
                {
                    var value = obj.ToString2();
                    if (!value.IsNullOrEmpty()) element.InnerText = value;
                    else root.RemoveChild(element);
                }
            }
        }
    }
}