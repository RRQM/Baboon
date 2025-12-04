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

using Baboon.Avalonia.Desktop;
using Baboon.Core;
using Baboon.Desktop;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Baboon.Maui;


public abstract partial class BaboonMauiApplicationCore: Application,Baboon.Core.IApplication
{
    /// <summary>
    /// 初始化应用程序。
    /// </summary>
    /// <param name="e">初始化事件参数。</param>
    /// <returns>异步任务。</returns>
    protected abstract Task InitializeAsync(AppModuleInitEventArgs e);
    /// <inheritdoc/>
    public IHost AppHost { get; private set; }

    /// <inheritdoc/>
    public ILogger<Core.IApplication> Logger => this.ServiceProvider.GetService<ILogger<Core.IApplication>>();

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
    /// 查找模块。
    /// </summary>
    /// <param name="path">模块路径。</param>
    /// <returns>是否找到模块。</returns>
    protected virtual bool FindModule(string path)
    {
        var name = Path.GetFileNameWithoutExtension(path);
        return name.EndsWith("Module");
    }

    public void Initialize(IServiceCollection services)
    {
        #region 配置、加载插件

        var moduleCatalog = new InternalModuleCatalog(this.FindModule);
        this.ConfigureModuleCatalog(moduleCatalog);
        moduleCatalog.Build();

        #endregion 配置、加载插件

        #region 注册服务

        services.AddSingleton<IModuleCatalog>(moduleCatalog);

       services.AddSingleton<Core.IApplication>(this);
        services.AddSingleton<Avalonia.Desktop.IWindowManager,Avalonia.Desktop.WindowManager>();


        this.InitializeAsync(new AppModuleInitEventArgs([], services)).GetAwaiter().GetResult();

        #endregion 注册服务

        foreach (var appModule in moduleCatalog.GetAppModules())
        {
            appModule.InitializeAsync(this, new AppModuleInitEventArgs([], services)).GetAwaiter().GetResult();
        }

       
    }

    public void Startup(IServiceProvider serviceProvider)
    {
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
        e.MainWindow = this.CreateMainWindow(windowManager);
    }
}
