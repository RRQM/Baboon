using System;

namespace Baboon;

/// <summary>
/// 模块描述
/// </summary>
public class ModuleDescription
{
    /// <summary>
    /// 初始化 <see cref="ModuleDescription"/> 类的新实例。
    /// </summary>
    /// <param name="id">模块唯一Id。用于标识不同的模块。</param>
    /// <param name="name">显示名称</param>
    /// <param name="version">版本</param>
    /// <param name="authors">作者</param>
    /// <param name="description">描述</param>
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
    public string Authors { get; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; }
}
