using System;
using System.Threading.Tasks;

namespace Baboon;

/// <summary>
/// 能够提供模块化的接口
/// </summary>
public interface IAppModule : IDisposable
{
    /// <summary>
    /// 获取模块描述信息
    /// </summary>
    ModuleDescription Description { get; }

    /// <summary>
    /// 异步初始化模块
    /// </summary>
    /// <param name="application">应用程序实例</param>
    /// <param name="e">初始化事件参数</param>
    /// <returns>表示异步操作的任务</returns>
    Task InitializeAsync(IApplication application, AppModuleInitEventArgs e);

    /// <summary>
    /// 获取服务提供者
    /// </summary>
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 异步启动模块
    /// </summary>
    /// <param name="application">应用程序实例</param>
    /// <param name="e">启动事件参数</param>
    /// <returns>表示异步操作的任务</returns>
    Task StartupAsync(IApplication application, AppModuleStartupEventArgs e);
}
