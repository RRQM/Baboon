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

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Baboon;

/// <summary>
/// 提供 AppModule 初始化事件的数据。
/// </summary>
public class AppModuleInitEventArgs : EventArgs
{
    /// <summary>
    /// 初始化 <see cref="AppModuleInitEventArgs"/> 类的新实例。
    /// </summary>
    /// <param name="args">命令行参数。</param>
    /// <param name="services">服务集合。</param>
    public AppModuleInitEventArgs(string[] args, IServiceCollection services)
    {
        this.Args = args;
        this.Services = services;
    }

    /// <summary>
    /// 获取命令行参数。
    /// </summary>
    public string[] Args { get; }

    /// <summary>
    /// 获取服务集合。
    /// </summary>
    public IServiceCollection Services { get; }
}
