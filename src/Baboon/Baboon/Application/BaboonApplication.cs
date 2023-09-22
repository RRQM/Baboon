using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TouchSocket.Core;

namespace Baboon
{
    /// <summary>
    /// 由Baboon提供的根应用程序。
    /// <para>
    /// 内部已经做了异常处理、日志记录、模块注册等功能。
    /// </para>
    /// </summary>
    public abstract class BaboonApplication : Application
    {
        /// <summary>
        /// 由Baboon提供的根应用程序。
        /// <para>
        /// 内部已经做了异常处理、日志记录、模块注册等功能。
        /// </para>
        /// </summary>
        protected BaboonApplication() : this(new Container())
        {

        }

        /// <summary>
        /// 由Baboon提供的根应用程序。
        /// <para>
        /// 内部已经做了异常处理、日志记录、模块注册等功能。
        /// </para>
        /// </summary>
        protected BaboonApplication(IContainer container)
        {
            this.Container = container;

            #region 注册
            container.RegisterSingleton<IModuleCatalog, ModuleCatalog>();
            container.RegisterSingleton<BaboonApplication>(this);
            container.RegisterSingleton<ILoggerFactoryService, LoggerFactoryService>();
            container.RegisterSingleton<IConfigService, ConfigService>();
            #endregion

            #region 异常处理

            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.App_DispatcherUnhandledException);

            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);

            #endregion 异常处理
        }

        /// <summary>
        /// IOC容器
        /// </summary>
        public IContainer Container { get; }

        /// <summary>
        /// 获取主程序日志记录器
        /// </summary>
        /// <returns></returns>
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

        protected abstract void RegisterTypes(IContainer container);

        protected virtual void OnException(Exception ex)
        {
            this.GetAppLogger().Exception(ex);
        }

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
    }
}