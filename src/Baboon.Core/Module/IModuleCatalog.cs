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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Baboon.Core;

/// <summary>
/// 模块目录接口
/// </summary>
public interface IModuleCatalog
{
    /// <summary>
    /// 是否只读
    /// </summary>
    bool IsReadonly { get; }

    /// <summary>
    /// 模块目录路径
    /// </summary>
    string ModulesDirPath { get; set; }

    /// <summary>
    /// 以类型直接添加模块
    /// </summary>
    /// <param name="moduleType">模块类型</param>
    void Add([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type moduleType);

    /// <summary>
    /// 直接添加模块
    /// </summary>
    /// <param name="appModule">应用模块</param>
    void Add(IAppModule appModule);

    /// <summary>
    /// 泛型添加模块
    /// </summary>
    /// <typeparam name="TAppModule">应用模块类型</typeparam>
    void Add<[DynamicallyAccessedMembers( DynamicallyAccessedMemberTypes.PublicConstructors)]TAppModule>() where TAppModule : IAppModule, new();

    /// <summary>
    /// 构建模块
    /// </summary>
    void Build();

    /// <summary>
    /// 是否包含指定Id的模块
    /// </summary>
    /// <param name="id">模块Id</param>
    /// <returns>是否包含</returns>
    bool Contains(string id);

    /// <summary>
    /// 获取模块信息
    /// </summary>
    /// <param name="id">模块Id</param>
    /// <returns>应用模块</returns>
    IAppModule GetAppModule(string id);

    /// <summary>
    /// 获取当前目录下的所有模块
    /// </summary>
    /// <returns>应用模块集合</returns>
    IEnumerable<IAppModule> GetAppModules();
}
