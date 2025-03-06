// ------------------------------------------------------------------------------
// 此代码版权（除特别声明或在XREF结尾的命名空间的代码）归作者本人若汝棋茗所有
// 源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
// CSDN博客：https://blog.csdn.net/qq_40374647
// 哔哩哔哩视频：https://space.bilibili.com/94253567
// Gitee源代码仓库：https://gitee.com/RRQM_Home
// Github源代码仓库：https://github.com/RRQM
// API首页：https://touchsocket.net/
// 交流QQ群：234762506
// 感谢您的下载和使用
// ------------------------------------------------------------------------------

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
