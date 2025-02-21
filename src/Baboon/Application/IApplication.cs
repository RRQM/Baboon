using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Baboon;

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
    ILogger<BaboonWpfApplication> Logger { get; }

    /// <summary>
    /// 获取应用程序的服务提供者。
    /// </summary>
    IServiceProvider ServiceProvider { get; }
}
