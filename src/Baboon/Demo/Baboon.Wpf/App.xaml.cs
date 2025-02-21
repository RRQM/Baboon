using Baboon.Wpf.ViewModels;
using System.Windows;
using TouchSocket.Core;

namespace Baboon.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : BaboonWpfApplication
    {
        protected override Window CreateMainWindow()
        {
            return this.ServiceProvider.Resolve<MainWindow>();
        }

        protected override Task StartupAsync(AppModuleStartupEventArgs e)
        {
            return Task.CompletedTask;
        }

        protected override Task InitializeAsync(AppModuleInitEventArgs e)
        {
            e.Services.AddSingletonView<MainWindow, MainViewModel>();
            return Task.CompletedTask;
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //可以直接注册
        }

        protected override void OnException(Exception ex)
        {
            base.OnException(ex);

            MessageBox.Show($"异常：{ex.Message}");
        }
    }
}