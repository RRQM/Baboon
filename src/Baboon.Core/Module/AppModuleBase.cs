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
using TouchSocket.Core;

namespace Baboon.Core;

/// <summary>
/// 模块基类
/// </summary>
public abstract class AppModuleBase : SafetyDisposableObject, IAppModule
{
    private IServiceScope m_serviceScope;

    /// <inheritdoc/>
    public abstract ModuleDescription Description { get; }

    /// <inheritdoc/>
    public virtual IServiceProvider ServiceProvider => this.m_serviceScope.ServiceProvider;

    /// <inheritdoc/>
    public async Task InitializeAsync(IApplication application, AppModuleInitEventArgs e)
    {
        await this.OnInitializeAsync(application, e);
    }

    /// <inheritdoc/>
    public async Task StartupAsync(IApplication application, AppModuleStartupEventArgs e)
    {
        this.m_serviceScope = e.AppHost.Services.CreateScope();
        await this.OnStartupAsync(application, e);
    }

    /// <inheritdoc cref="IAppModule.InitializeAsync(IApplication, AppModuleInitEventArgs)"/>
    protected abstract Task OnInitializeAsync(IApplication application, AppModuleInitEventArgs e);

    /// <inheritdoc cref="IAppModule.StartupAsync(IApplication, AppModuleStartupEventArgs)"/>
    protected abstract Task OnStartupAsync(IApplication application, AppModuleStartupEventArgs e);

    /// <inheritdoc/>
    protected override void SafetyDispose(bool disposing)
    {
        if (disposing)
        {
            var serviceScope = this.m_serviceScope;
            serviceScope.SafeDispose();
        }
    }
}