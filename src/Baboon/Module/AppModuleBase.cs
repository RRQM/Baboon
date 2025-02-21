using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace Baboon;

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