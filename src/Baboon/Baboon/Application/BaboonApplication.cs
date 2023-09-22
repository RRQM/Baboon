using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TouchSocket.Core;

namespace Baboon
{
    public abstract class BaboonApplication : Application
    {
        protected BaboonApplication()
        {
            this.Container.RegisterSingleton<IModuleCatalog, ModuleCatalog>();
            this.Container.RegisterSingleton<BaboonApplication>(this);
            this.Container.RegisterSingleton<ILoggerFactoryService, LoggerFactoryService>();

            #region 异常处理

            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.App_DispatcherUnhandledException);

            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += this.TaskScheduler_UnobservedTaskException;

            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);

            #endregion 异常处理
        }

        public IContainer Container { get; } = new Container();

        public ILogger GetAppLogger()
        {
            return this.Container.Resolve<ILoggerFactoryService>().GetLogger(this.GetType().Name);
        }

        protected virtual void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
        }

        protected abstract Window CreateShell();

        protected override sealed void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.RegisterTypes(this.Container);

            var moduleCatalog = this.Container.Resolve<IModuleCatalog>();
            this.ConfigureModuleCatalog(moduleCatalog);
            this.MainWindow = this.CreateShell();
            this.MainWindow.Show();
        }

        protected virtual void RegisterTypes(IContainer container)
        {
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true; //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出

                this.GetAppLogger().Exception(e.Exception);
            }
            catch
            {
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var sbEx = new StringBuilder();
            if (e.IsTerminating)
            {
                sbEx.Append("程序发生致命错误，将终止！\n");
            }
            sbEx.Append("捕获Thread未处理异常：");
            if (e.ExceptionObject is Exception)
            {
                sbEx.Append(((Exception)e.ExceptionObject).Message);
            }
            else
            {
                sbEx.Append(e.ExceptionObject);
            }

            var errorStr = sbEx.ToString();
            this.GetAppLogger().Error(errorStr);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //task线程内未处理捕获

            var errorStr = "捕获Task未处理异常：" + e.Exception.Message;
            this.GetAppLogger().Error(errorStr);
            e.SetObserved();//设置该异常已察觉（这样处理后就不会引起程序崩溃）
        }
    }
}