using Baboon.Wpf.MyRegions;
using Baboon.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace Baboon.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : BaboonWpfApplication
    {
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //可以直接注册模块
            //moduleCatalog.Add<SayHelloModule>;
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
}