using Baboon.Wpf.ViewModels;
using SatHello.Module;
using System.Windows;
using TouchSocket.Core;

namespace Baboon.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : BaboonApplication
    {
        protected override Window CreateShell()
        {
            return this.Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainer container)
        {
            container.RegisterSingletonView<MainWindow, MainViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //以类型注册
            moduleCatalog.Add(typeof(SayHelloModule));
        }

        protected override void OnException(Exception ex)
        {
            base.OnException(ex);

            MessageBox.Show($"异常：{ex.Message}");
        }
    }
}