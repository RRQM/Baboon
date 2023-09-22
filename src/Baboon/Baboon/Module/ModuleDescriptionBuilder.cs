using System.IO;
using System.Xml;

namespace Baboon
{
    public class ModuleDescriptionBuilder
    {
        public ModuleDescription Description { get; set; }

        public string Module { get; set; }
        public string RootDir { get; set; }

        public static ModuleDescriptionBuilder CreateByFile(string path)
        {
            var builder = new ModuleDescriptionBuilder();
            builder.Description = ModuleDescription.CreateByDescriptionFile(path);

            var xml = new XmlDocument();
            xml.Load(path);
            var metadata = xml.SelectSingleNode("package/metadata");
            var module = xml.SelectSingleNode("package/module");
            builder.Module = module?.InnerText;
            builder.RootDir = Path.GetDirectoryName(path);
            return builder;
        }

        private static string GetXmlNodeText(XmlNode xmlNode, string xPath)
        {
            return xmlNode?.SelectSingleNode(xPath)?.InnerText;
        }
    }
}