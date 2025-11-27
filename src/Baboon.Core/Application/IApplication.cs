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

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Baboon.Core;

/// <summary>
/// 表示应用程序接口。
/// </summary>
public interface IApplication
{
    /// <summary>
    /// 获取应用程序主机。
    /// </summary>
    IHost AppHost { get; }

    /// <summary>
    /// 获取应用程序的日志记录器。
    /// </summary>
    ILogger<IApplication> Logger { get; }

    /// <summary>
    /// 获取应用程序的服务提供者。
    /// </summary>
    IServiceProvider ServiceProvider { get; }
}
