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
