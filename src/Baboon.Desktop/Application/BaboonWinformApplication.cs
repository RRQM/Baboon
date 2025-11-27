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

using Baboon.Core;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;
using TouchSocket.Core;

namespace Baboon.Desktop;

/// <summary>
/// 抽象类，表示一个WinForm应用程序。
/// </summary>
public abstract class BaboonWinformApplication : IApplication
{
    /// <summary>
    /// 构造函数，初始化应用程序并设置异常处理。
    /// </summary>
    protected BaboonWinformApplication()
    {
        #region 异常处理

        //非UI线程未捕获异常处理事件
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
        Application.ThreadException += this.Application_ThreadException;

        #endregion 异常处理

        Application.ApplicationExit += this.Application_ApplicationExit;
        Application.Idle += this.Application_Idle;

        this.ApplicationConfiguration();
    }

    /// <summary>
    /// 应用程序空闲时的处理。
    /// </summary>
    private void Application_Idle(object sender, EventArgs e)
    {

    }

    /// <inheritdoc/>
    public IHost AppHost { get; private set; }

    /// <inheritdoc/>
    public ILogger<BaboonWpfApplication> Logger => this.ServiceProvider.GetService<ILogger<BaboonWpfApplication>>();

    /// <summary>
    /// 获取主窗体。
    /// </summary>
    public Form MainForm { get; private set; }

    /// <inheritdoc/>
    public IServiceProvider ServiceProvider => this.AppHost?.Services;

    /// <summary>
    /// 运行应用程序。
    /// </summary>
    /// <param name="args">启动参数。</param>
    public void Run(params string[] args)
    {
        this.PrivateOnStartupAsync(args).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 运行应用程序。
    /// </summary>
    public void Run()
    {
        this.Run(Array.Empty<string>());
    }

    /// <summary>
    /// 配置应用程序。
    /// </summary>
    protected virtual void ApplicationConfiguration()
    {
        global::System.Windows.Forms.Application.EnableVisualStyles();
        global::System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

#if NET6_0_OR_GREATER
        global::System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif

    }

    /// <summary>
    /// 配置模块目录。
    /// </summary>
    /// <param name="moduleCatalog">模块目录。</param>
    protected virtual void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
    }

    /// <summary>
    /// 创建应用程序构建器。
    /// </summary>
    /// <param name="args">启动参数。</param>
    /// <returns>应用程序构建器。</returns>
    protected virtual HostApplicationBuilder CreateApplicationBuilder(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        return builder;
    }

    /// <summary>
    /// 获取主窗体。
    /// </summary>
    /// <returns>主窗体。</returns>
    protected abstract Form CreateMainForm(IFormManager formManager);

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
    /// 处理异常。
    /// </summary>
    /// <param name="ex">异常对象。</param>
    protected virtual void OnException(Exception ex)
    {
        this.Logger?.LogError(ex, ex.Message);
    }

    /// <summary>
    /// 启动应用程序。
    /// </summary>
    /// <param name="e">启动事件参数。</param>
    /// <returns>异步任务。</returns>
    protected abstract Task StartupAsync(AppModuleStartupEventArgs e);

    /// <summary>
    /// 应用程序退出时的处理。
    /// </summary>
    private async void Application_ApplicationExit(object sender, EventArgs e)
    {
        var moduleCatalog = this.ServiceProvider.GetService<IModuleCatalog>();
        foreach (var appModule in moduleCatalog.GetAppModules())
        {
            appModule.SafeDispose();
        }
        await this.AppHost.StopAsync();
    }

    /// <summary>
    /// UI线程未捕获异常处理。
    /// </summary>
    private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
    {
        this.OnException(e.Exception);
    }

    /// <summary>
    /// 非UI线程未捕获异常处理。
    /// </summary>
    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.IsTerminating)
        {
            Application.Exit();
            return;
        }
        if (e.ExceptionObject is Exception ex)
        {
            this.OnException(ex);
        }
    }

    /// <summary>
    /// 私有启动方法。
    /// </summary>
    /// <param name="args">启动参数。</param>
    /// <returns>异步任务。</returns>
    private async Task PrivateOnStartupAsync(string[] args)
    {
        var builder = this.CreateApplicationBuilder(args);

        #region 配置、加载插件

        var moduleCatalog = new InternalModuleCatalog(this.FindModule);
        this.ConfigureModuleCatalog(moduleCatalog);
        moduleCatalog.Build();

        #endregion 配置、加载插件

        #region 注册服务

        builder.Services.AddSingleton<IModuleCatalog>(moduleCatalog);
        builder.Services.AddSingleton<IApplication>(this);
        builder.Services.AddFormManager();

        await this.InitializeAsync(new AppModuleInitEventArgs(args, builder.Services));

        #endregion 注册服务

        foreach (var appModule in moduleCatalog.GetAppModules())
        {
            await appModule.InitializeAsync(this, new AppModuleInitEventArgs(args, builder.Services));
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

        var formManager = this.ServiceProvider.GetRequiredService<IFormManager>();

        this.MainForm = this.CreateMainForm(formManager);

        MainThreadTaskFactory.Initialize();
        Application.Run(this.MainForm);
    }
}
