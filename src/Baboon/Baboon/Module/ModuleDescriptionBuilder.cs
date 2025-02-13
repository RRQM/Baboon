//using System.IO;
//using System.Xml;

//namespace Baboon
//{
//    /// <summary>
//    /// 模块描述构建器
//    /// </summary>
//    public class ModuleDescriptionBuilder
//    {
//        /// <summary>
//        /// 描述
//        /// </summary>
//        public ModuleDescription Description { get; set; }

//        /// <summary>
//        /// 模块全名称
//        /// </summary>
//        public string Module { get; set; }

//        /// <summary>
//        /// 根文件夹
//        /// </summary>
//        public string RootDir { get; set; }

//        /// <summary>
//        /// 从文件创建
//        /// </summary>
//        /// <param name="path"></param>
//        /// <returns></returns>
//        public static ModuleDescriptionBuilder CreateByFile(string path)
//        {
//            var builder = new ModuleDescriptionBuilder();
//            builder.Description = ModuleDescription.CreateByDescriptionFile(path);

//            var xml = new XmlDocument();
//            xml.Load(path);
//            var metadata = xml.SelectSingleNode("package/metadata");
//            var module = xml.SelectSingleNode("package/module");
//            builder.Module = module?.InnerText;
//            builder.RootDir = Path.GetDirectoryName(path);
//            return builder;
//        }

//        private static string GetXmlNodeText(XmlNode xmlNode, string xPath)
//        {
//            return xmlNode?.SelectSingleNode(xPath)?.InnerText;
//        }
//    }
//}