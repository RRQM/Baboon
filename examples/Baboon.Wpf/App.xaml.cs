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
using Baboon.Wpf.MyRegions;
using Baboon.Wpf.ViewModels;
using System.IO;
using System.Windows;

namespace Baboon.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : BaboonWpfApplication
{
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.Add<BaboonCoreModule>();
        base.ConfigureModuleCatalog(moduleCatalog);
    }

    protected override Window CreateMainWindow(IWindowManager windowManager)
    {
        return windowManager.GetWindow<MainWindow>();
    }

    protected override bool FindModule(string path)
    {
        var name = Path.GetFileNameWithoutExtension(path);
        return name.EndsWith("Module");
    }

    protected override Task InitializeAsync(AppModuleInitEventArgs e)
    {
        //程序初始化时执行
        //可以在这里注册服务

        //注册单例模式的View和ViewModel
        e.Services.AddSingletonView<MainWindow, MainViewModel>();

        e.Services.AddSingletonNavigate<RegionControl, RegionControlViewModel>("RegionControl");
        return Task.CompletedTask;
    }

    protected override void OnException(Exception ex)
    {
        base.OnException(ex);

        MessageBox.Show($"异常：{ex.Message}");
    }

    protected override Task StartupAsync(AppModuleStartupEventArgs e)
    {
        //程序启动时执行
        //可以在这里通过ServiceProvider获取服务
        //this.ServiceProvider.Resolve<MainViewModel>();
        return Task.CompletedTask;
    }
}