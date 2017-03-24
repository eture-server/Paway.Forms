using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Paway.Helper
{
    /// <summary>
    ///     对 XML 操作的辅助类
    /// </summary>
    public abstract class XmlHelper
    {
        #region 常量

        /// <summary>
        ///     XML文件的声明
        /// </summary>
        public const string XML_STATEMENT = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        private const string NAME = "add";
        private const string KEY = "key";
        private const string VALUE = "value";

        #endregion

        #region 方法

        /// <summary>
        ///     通过文件地址加载Xml文件，获取XmlDocument对象
        /// </summary>
        /// <param name="filePath">xml文件地址所在的系统目录</param>
        /// <param name="assemblyPath">文件所在的程序集地址[针对嵌入的资源]</param>
        /// <returns>XmlDocument 对象</returns>
        public static XmlDocument LoadXml(string filePath, string assemblyPath)
        {
            var document = new XmlDocument();
            if (File.Exists(filePath))
            {
                document.Load(filePath);
            }
            else
            {
                var assembly = Assembly.GetEntryAssembly();
                var stream = assembly.GetManifestResourceStream(assemblyPath);
                document.Load(stream);
            }
            return document;
        }

        /// <summary>
        ///     生成XML字符串
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="allNodes">所有的子节点</param>
        /// <param name="allValues">所有子节点对应的数据</param>
        /// <returns></returns>
        public static string CreateXmlStr(string root, string[] allNodes, string[] allValues)
        {
            #region 拼接字符串

            //StringBuilder sb = new StringBuilder();
            //sb.Append("<" + headStr + ">");
            //for (int i = 0; i < allNodes.Length; i++)
            //{
            //    sb.Append("<" + allNodes[i] + ">" + allValues[i] + "</" + allNodes[i] + ">");
            //}
            //sb.Append("/" + headStr + ">");
            //return sb.ToString();

            #endregion

            var doc = new XmlDocument();
            try
            {
                //添加xml文件的声明
                var dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(dec);
                //添加根节点
                var rootNode = doc.CreateElement(root);
                doc.AppendChild(rootNode);
                //添加子节点
                for (var i = 0; i < allNodes.Length; i++)
                {
                    var childNode = doc.CreateElement(allNodes[i]);
                    childNode.InnerText = allValues[i];
                    rootNode.AppendChild(childNode);
                }
                //doc.Save("\\demo.xml");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("XmlHelper.CreateXmlStr(string, string[], string[]) :: " + ex.Message);
                throw;
            }
            return doc.InnerXml;
        }

        /// <summary>
        ///     返回对应的实体集合
        ///     访问的XML文件与之对应的 实体类 名称必须大小写一致，数据类型必须是String
        ///     并且XML的编写格式必须按要求编写
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="document">XmlDocument对象</param>
        /// <returns></returns>
        public static List<T> Select<T>(XmlDocument document)
        {
            var list = new List<T>();
            XmlNode xmlRoot = document.DocumentElement;
            if (xmlRoot != null)
            {
                var type = typeof(T);
                var properties = TypeDescriptor.GetProperties(type);
                foreach (XmlNode xmlNode in xmlRoot.ChildNodes)
                {
                    if (string.Compare(xmlNode.Name, type.Name, true) == 0)
                    {
                        var obj = Activator.CreateInstance<T>();
                        for (int i = 0; i < properties.Count; i++)
                        {
                            var propertyName = properties[i].Name; //获取属性名称
                            object value = xmlNode.Attributes[propertyName].Value; //从XML中得到该属性的Value值
                            properties[i].SetValue(obj, value); //将得到的属性值赋给obj对象
                        }
                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        #endregion
    }
}