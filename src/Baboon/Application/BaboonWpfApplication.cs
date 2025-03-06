using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TouchSocket.Core;

namespace Baboon;

/// <summary>
/// 由Baboon提供的根应用程序。
/// <para>
/// 内部已经做了异常处理、日志记录、模块注册等功能。
/// </para>
/// </summary>
public abstract class BaboonWpfApplication : Application, IApplication
{
    /// <summary>
    /// 由Baboon提供的根应用程序。
    /// <para>
    /// 内部已经做了异常处理、日志记录、模块注册等功能。
    /// </para>
    /// </summary>
    protected BaboonWpfApplication()
    {
        #region 异常处理

        //UI线程未捕获异常处理事件
        this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.App_DispatcherUnhandledException);

        //非UI线程未捕获异常处理事件
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);

        #endregion 异常处理
    }

    /// <inheritdoc/>
    public IHost AppHost { get; private set; }

    /// <inheritdoc/>
    public ILogger<BaboonWpfApplication> Logger => this.ServiceProvider.GetService<ILogger<BaboonWpfApplication>>();

    /// <inheritdoc/>
    public IServiceProvider ServiceProvider => this.AppHost?.Services;

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
    protected virtual HostApplicationBuilder CreateApplicationBuilder(StartupEventArgs e)
    {
        var builder = Host.CreateApplicationBuilder(e.Args);

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

    /// <inheritdoc/>
    protected sealed override async void OnStartup(StartupEventArgs e)
    {
        MainThreadTaskFactory.Initialize();

        base.OnStartup(e);
        await this.PrivateOnStartupAsync(e);
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

    private async Task PrivateOnStartupAsync(StartupEventArgs e)
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
        builder.Services.AddSingleton(this);
        builder.Services.AddWindowManager();
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
}