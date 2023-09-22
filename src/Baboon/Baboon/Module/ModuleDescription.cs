using System;
using System.IO;
using System.Xml;

namespace Baboon
{
    public class ModuleDescription
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Version { get; set; }
        public string Authors { get; set; }
        public string Description { get; set; }
        public bool HasView { get; set; }
        public string CoverImage { get; set; }

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