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
        ///     返回对应的实体(包含子级)
        /// </summary>
        public static T Load<T>(string file)
        {
            Type type = typeof(T);
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode root = doc.DocumentElement;
            XmlNode element = root.FirstChild;

            T obj = Activator.CreateInstance<T>();
            Load(doc, element, obj, type);
            return obj;
        }
        private static void Load<T>(XmlDocument doc, XmlNode element, T info, Type type)
        {
            var descriptors = type.Descriptors();
            while (element != null)
            {
                var descriptor = descriptors.Find(c => c.Name == element.Name);
                if (descriptor != null && element.InnerText != null)
                {
                    if (descriptor.PropertyType.Name == typeof(List<>).Name)
                    {
                        IList obj = (IList)descriptor.GetValue(info);
                        var type2 = descriptor.PropertyType.GenericType();
                        if (obj == null)
                        {
                            obj = type2.CreateList();
                            info.SetValue(descriptor, obj);
                        }
                        var obj2 = Assembly.GetAssembly(type2).CreateInstance(type2.FullName);
                        obj.Add(obj2);
                        Load(doc, element.FirstChild, obj2, type2);
                    }
                    else if (descriptor.PropertyType.IsClass && descriptor.PropertyType != typeof(string))
                    {
                        var obj = Assembly.GetAssembly(descriptor.PropertyType).CreateInstance(descriptor.PropertyType.FullName);
                        info.SetValue(descriptor, obj);
                        Load(doc, element.FirstChild, obj, descriptor.PropertyType);
                    }
                    else
                    {
                        info.SetValue(descriptor, element.InnerText);
                    }
                }
                element = element.NextSibling;
            }
        }

        /// <summary>
        /// 生成XML文件(包含子级)
        /// </summary>
        /// <typeparam name="T">数据类或数据接口(按接口生成部分)</typeparam>
        /// <param name="file">生成文件</param>
        /// <param name="info">实体数据</param>
        /// <param name="allowEmpty">允许空数据</param>
        /// <param name="standalone"></param>
        public static void Save<T>(string file, T info, bool allowEmpty = true, string standalone = null)
        {
            Saves(file, info, null, null, allowEmpty, standalone);
        }
        /// <summary>
        /// 生成XML文件(包含子级)
        /// </summary>
        /// <typeparam name="T">数据类或数据接口(按接口生成部分)</typeparam>
        /// <param name="file">生成文件</param>
        /// <param name="info">实体数据</param>
        /// <param name="ns">头属性</param>
        /// <param name="args">头属性参数</param>
        /// <param name="allowEmpty">允许空数据</param>
        /// <param name="standalone"></param>
        public static void Saves<T>(string file, T info, string ns = null, Dictionary<string, string> args = null, bool allowEmpty = false, string standalone = null)
        {
            Type type = typeof(T);
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xml = doc.CreateXmlDeclaration("1.0", "utf-8", standalone);
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
            Save(doc, root, info, type, allowEmpty);
            doc.Save(file);
        }
        private static void Save<T>(XmlDocument doc, XmlElement root, T info, Type type, bool allowEmpty = false)
        {
            var properties = type.Properties();
            var descriptors = type.Descriptors();
            foreach (PropertyInfo property in properties)
            {
                if (!property.IBrowsable()) continue;
                XmlElement element = doc.CreateElement(property.Name);
                root.AppendChild(element);
                var descriptor = descriptors.Find(c => c.Name == property.Name);
                object obj = descriptor.GetValue(info);
                if (obj is IList)
                {
                    var list = obj as IList;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i > 0)
                        {
                            element = doc.CreateElement(property.Name);
                            root.AppendChild(element);
                        }
                        Save(doc, element, list[i], list.GenericType());
                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    Save(doc, element, obj, property.PropertyType);
                    continue;
                }
                else
                {
                    var value = obj.ToString2();
                    if (allowEmpty || !value.IsNullOrEmpty())
                    {
                        if (!descriptor.Description.IsNullOrEmpty())
                        {
                            var comment = doc.CreateComment(descriptor.Description);
                            root.InsertBefore(comment, element);
                        }
                        element.InnerText = value;
                    }
                    else root.RemoveChild(element);
                }
            }
        }
    }
}