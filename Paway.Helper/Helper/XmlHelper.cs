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
    /// 对 XML 操作的辅助类
    /// </summary>
    public abstract class XmlHelper
    {
        /// <summary>
        /// 返回对应的实体(实体及子级)
        /// </summary>
        public static T Load<T>(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            return Load<T>(doc);
        }
        /// <summary>
        /// 返回对应的实体(实体及子级)
        /// </summary>
        public static T Load<T>(XmlDocument doc)
        {
            Type type = typeof(T);
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
                        var typeList = descriptor.PropertyType.GenericType();
                        var list = typeList.GenericList();
                        info.SetValue(descriptor, list);
                        var elementChild = element.FirstChild;
                        while (elementChild != null)
                        {
                            var objList = Activator.CreateInstance(typeList);
                            list.Add(objList);
                            Load(doc, elementChild.FirstChild, objList, typeList);
                            elementChild = elementChild.NextSibling;
                        }
                    }
                    else if (descriptor.PropertyType.IsClass && descriptor.PropertyType != typeof(string))
                    {
                        var obj = Activator.CreateInstance(descriptor.PropertyType);
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
        /// 生成XML文件(实体及子级)
        /// </summary>
        /// <typeparam name="T">数据类或数据接口(按接口生成部分)</typeparam>
        /// <param name="file">生成文件</param>
        /// <param name="info">实体数据</param>
        /// <param name="allowEmpty">允许空数据</param>
        /// <param name="standalone"></param>
        public static void Save<T>(T info, string file, bool allowEmpty = true, string standalone = null)
        {
            Saves(info, file, null, null, allowEmpty, standalone);
        }
        /// <summary>
        /// 生成XML文件(实体及子级)
        /// </summary>
        /// <typeparam name="T">数据类或数据接口(按接口生成部分)</typeparam>
        /// <param name="file">生成文件</param>
        /// <param name="info">实体数据</param>
        /// <param name="ns">头属性</param>
        /// <param name="args">头属性参数</param>
        /// <param name="allowEmpty">允许空数据</param>
        /// <param name="standalone"></param>
        public static void Saves<T>(T info, string file, string ns = null, Dictionary<string, string> args = null, bool allowEmpty = false, string standalone = null)
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
            var properties = type.PropertiesCache();
            foreach (PropertyInfo property in properties)
            {
                if (!property.IBrowsable()) continue;
                XmlElement element = doc.CreateElement(property.Name);
                root.AppendChild(element);
                object obj = info.GetValue(property.Name);
                if (obj is IList list)
                {
                    AddDescription(doc, root, element, property);
                    var typeList = list.GenericType();
                    for (int i = 0; i < list.Count; i++)
                    {
                        XmlElement elementChild = doc.CreateElement(typeList.Name);
                        element.AppendChild(elementChild);
                        Save(doc, elementChild, list[i], typeList);
                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    if (obj != null)
                    {
                        AddDescription(doc, root, element, property);
                        Save(doc, element, obj, property.PropertyType);
                    }
                    else root.RemoveChild(element);
                    continue;
                }
                else
                {
                    var value = obj.ToStrs();
                    if (allowEmpty || !value.IsNullOrEmpty())
                    {
                        AddDescription(doc, root, element, property);
                        element.InnerText = value;
                    }
                    else root.RemoveChild(element);
                }
            }
        }
        private static void AddDescription(XmlDocument doc, XmlElement root, XmlElement element, PropertyInfo property)
        {
            if (!property.Description().IsNullOrEmpty())
            {
                var comment = doc.CreateComment(property.Description());
                root.InsertBefore(comment, element);
            }
        }
    }
}