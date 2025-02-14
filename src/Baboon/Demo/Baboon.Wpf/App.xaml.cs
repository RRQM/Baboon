using Baboon.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SatHello.Module;
using System.ComponentModel;
using System.Windows;
using TouchSocket.Core;

namespace Baboon.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : BaboonApplication
    {
        protected override Window CreateMainWindow()
        {
            return this.ServiceProvider.Resolve<MainWindow>();
        }

        protected override void Startup(AppModuleStartupEventArgs e)
        {
            
        }

        protected override void Initialize(AppModuleInitEventArgs e)
        {
            e.Services.AddSingletonView<MainWindow, MainViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //以类型注册
            moduleCatalog.Add<SayHelloModule>();
        }

        protected override void OnException(Exception ex)
        {
            base.OnException(ex);

            MessageBox.Show($"异常：{ex.Message}");
        }
    }
}