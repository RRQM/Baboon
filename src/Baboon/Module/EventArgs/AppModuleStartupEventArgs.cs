using Microsoft.Extensions.Hosting;
using System;

namespace Baboon;

/// <summary>
/// 应用模块启动事件参数类
/// </summary>
public class AppModuleStartupEventArgs : EventArgs
{
    /// <summary>
    /// 获取应用主机
    /// </summary>
    public IHost AppHost { get; }

    /// <summary>
    /// 初始化 <see cref="AppModuleStartupEventArgs"/> 类的新实例
    /// </summary>
    /// <param name="appHost">应用主机</param>
    public AppModuleStartupEventArgs(IHost appHost)
    {
        this.AppHost = appHost;
    }
}
