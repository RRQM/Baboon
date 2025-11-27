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

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Baboon.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;

namespace Baboon.Avalonia.Desktop;

public abstract class BaboonAvaloniaDesktopApplication : Application, IApplication
{
    public BaboonAvaloniaDesktopApplication()
    {

    }
    /// <inheritdoc/>
    public IHost AppHost { get; private set; }

    /// <inheritdoc/>
    public ILogger<IApplication> Logger => this.ServiceProvider.GetService<ILogger<IApplication>>();

    /// <inheritdoc/>
    public IServiceProvider ServiceProvider => this.AppHost?.Services;

    /// <inheritdoc/>
    public override void OnFrameworkInitializationCompleted()
    {
        if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {

            //desktop.MainWindow = new MainWindow();

            //MainThreadTaskFactory.Initialize();
            this.PrivateOnStartupAsync(desktop).GetAwaiter().GetResult();
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// 配置模块
    /// </summary>
    /// <param name="moduleCatalog"></param>
    protected virtual void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
    }

    /// <summary>
    /// 创建应用程序构建器。
    /// </summary>
    /// <param name="e">启动参数。</param>
    /// <returns>应用程序构建器。</returns>
    protected virtual HostApplicationBuilder CreateApplicationBuilder(IClassicDesktopStyleApplicationLifetime lifetime)
    {
        var builder = Host.CreateApplicationBuilder(lifetime.Args);

        return builder;
    }

    /// <summary>
    /// 获取主窗体
    /// </summary>
    /// <returns></returns>
    protected abstract Window CreateMainWindow(IWindowManager windowManager);

    /// <summary>
    /// 查找模块。
    /// </summary>
    /// <param name="path">模块路径。</param>
    /// <returns>是否找到模块。</returns>
    protected virtual bool FindModule(string path)
    {
        var name = Path.GetFileNameWithoutExtension(path);
        return name.EndsWith("Module");
    }

    /// <summary>
    /// 初始化应用程序。
    /// </summary>
    /// <param name="e">初始化事件参数。</param>
    /// <returns>异步任务。</returns>
    protected abstract Task InitializeAsync(AppModuleInitEventArgs e);

    /// <summary>
    /// 在异常的时候
    /// </summary>
    /// <param name="ex"></param>
    protected virtual void OnException(Exception ex)
    {
        this.Logger?.LogError(ex, ex.Message);
    }

    /// <inheritdoc/>
    protected override async void OnExit(ExitEventArgs e)
    {
        var moduleCatalog = this.ServiceProvider.GetService<IModuleCatalog>();
        foreach (var appModule in moduleCatalog.GetAppModules())
        {
            appModule.SafeDispose();
        }
        await this.AppHost.StopAsync();
        base.OnExit(e);
    }


    /// <summary>
    /// 启动应用程序。
    /// </summary>
    /// <param name="e">启动事件参数。</param>
    /// <returns>异步任务。</returns>
    protected abstract Task StartupAsync(AppModuleStartupEventArgs e);

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        try
        {
            e.Handled = true; //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出
            this.OnException(e.Exception);
        }
        catch
        {
            this.Shutdown(-1);
        }
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.IsTerminating)
        {
            this.Shutdown(-1);
            return;
        }
        if (e.ExceptionObject is Exception ex)
        {
            this.OnException(ex);
        }
    }

    private async Task PrivateOnStartupAsync(IClassicDesktopStyleApplicationLifetime e)
    {
        var builder = this.CreateApplicationBuilder(e);

        #region 配置、加载插件

        var moduleCatalog = new InternalModuleCatalog(this.FindModule);
        this.ConfigureModuleCatalog(moduleCatalog);
        moduleCatalog.Build();

        #endregion 配置、加载插件

        #region 注册服务

        builder.Services.AddSingleton<IModuleCatalog>(moduleCatalog);
        builder.Services.AddSingleton<IResourceService>(new InternalResourceService(this));
        builder.Services.AddSingleton<IApplication>(this);
        builder.Services.AddWindowManager();
        builder.Services.AddRegionManager();

        await this.InitializeAsync(new AppModuleInitEventArgs(e.Args, builder.Services));

        #endregion 注册服务

        foreach (var appModule in moduleCatalog.GetAppModules())
        {
            await appModule.InitializeAsync(this, new AppModuleInitEventArgs(e.Args, builder.Services));
        }

        var host = builder.Build();
        this.AppHost = host;

        Ioc.Default.ConfigureServices(host.Services);
        await this.StartupAsync(new AppModuleStartupEventArgs(host));

        foreach (var appModule in moduleCatalog.GetAppModules())
        {
            await appModule.StartupAsync(this, new AppModuleStartupEventArgs(host));
        }

        await host.StartAsync();

        var windowManager = this.ServiceProvider.GetRequiredService<IWindowManager>();
        this.MainWindow = this.CreateMainWindow(windowManager);
        this.MainWindow.Show();
    }

    public sealed override void OnFrameworkInitializationCompleted()
    {
        if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}