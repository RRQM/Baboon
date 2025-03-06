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
