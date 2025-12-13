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
using System.Reflection;

namespace Baboon.Core;

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
    /// 初始化 <see cref="ModuleDescription"/> 类的新实例。
    /// </summary>
    public ModuleDescription()
    {

    }

    /// <summary>
    /// 模块唯一Id。用于标识不同的模块。
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 版本
    /// </summary>
    public Version Version { get; init; }

    /// <summary>
    /// 作者
    /// </summary>
    public string Authors { get; init; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// 从指定程序集中获取模块描述信息
    /// </summary>
    /// <param name="assembly">要获取信息的程序集</param>
    /// <returns>包含程序集信息的 ModuleDescription 实例</returns>
    public static ModuleDescription FromAssembly(Assembly assembly)
    {
        // 获取程序集名称作为Id
        var id = assembly.GetName().Name;

        // 获取程序集的显示名称
        var name = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? id;

        // 获取程序集的版本
        var version = assembly.GetName().Version;

        // 获取程序集的作者
        var authors = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;

        // 获取程序集的描述
        var description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

        return new ModuleDescription(id, name, version, authors, description);
    }

    public static ModuleDescription FromAssembly<T>()
    {
        return FromAssembly(typeof(T).Assembly);
    }
}
