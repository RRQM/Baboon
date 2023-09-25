using System;
using System.IO;
using System.Xml;

namespace Baboon
{
    /// <summary>
    /// 模块描述
    /// </summary>
    public class ModuleDescription
    {
        /// <summary>
        /// 模块唯一Id。用于标识不同的模块。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Authors { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否包含可视化UI
        /// </summary>
        public bool HasView { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string CoverImage { get; set; }

        /// <summary>
        /// 从xml文件路径创建描述
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ModuleDescription CreateByDescriptionFile(string path)
        {
            var description = new ModuleDescription();

            var xml = new XmlDocument();
            xml.Load(path);
            var metadata = xml.SelectSingleNode("package/metadata");
            if (metadata is null)
            {
                throw new Exception("在相关描述文件中没有找到metadata");
            }

            description.Id = GetXmlNodeText(metadata, nameof(Id));
            description.DisplayName = GetXmlNodeText(metadata, nameof(DisplayName));
            description.Version = GetXmlNodeText(metadata, nameof(Version));
            description.Authors = GetXmlNodeText(metadata, nameof(Authors));
            description.Description = GetXmlNodeText(metadata, nameof(Description));
            description.CoverImage = GetXmlNodeText(metadata, nameof(CoverImage));
            description.HasView = bool.Parse(GetXmlNodeText(metadata, nameof(HasView)));
            return description;
        }

        /// <summary>
        /// 从流创建描述
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ModuleDescription CreateByDescriptionStream(Stream stream)
        {
            var description = new ModuleDescription();

            var xml = new XmlDocument();
            xml.Load(stream);
            var metadata = xml.SelectSingleNode("package/metadata");
            if (metadata is null)
            {
                throw new Exception("在相关描述文件中没有找到metadata");
            }

            description.Id = GetXmlNodeText(metadata, nameof(Id));
            description.DisplayName = GetXmlNodeText(metadata, nameof(DisplayName));
            description.Version = GetXmlNodeText(metadata, nameof(Version));
            description.Authors = GetXmlNodeText(metadata, nameof(Authors));
            description.Description = GetXmlNodeText(metadata, nameof(Description));
            description.CoverImage = GetXmlNodeText(metadata, nameof(CoverImage));
            description.HasView = bool.Parse(GetXmlNodeText(metadata, nameof(HasView)));
            return description;
        }

        private static string GetXmlNodeText(XmlNode xmlNode, string xPath)
        {
            return xmlNode?.SelectSingleNode(xPath)?.InnerText;
        }
    }
}