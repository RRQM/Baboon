using System;
using System.IO;
using System.Xml;

namespace Baboon
{
    /// <summary>
    /// 模块描述
    /// </summary>
    public readonly struct ModuleDescription
    {
        public ModuleDescription(string id, string name, Version version, string authors, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Version = version;
            this.Authors = authors;
            this.Description = description;
        }

        /// <summary>
        /// 模块唯一Id。用于标识不同的模块。
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 版本
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Authors { get;}

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get;}
    }
}