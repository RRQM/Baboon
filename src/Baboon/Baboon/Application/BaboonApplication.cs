using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
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
        protected BaboonApplication()
        {
            #region 异常处理

            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.App_DispatcherUnhandledException);

            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);

            #endregion 异常处理
        }

        public IHost AppHost { get; private set; }
        public ILogger<BaboonApplication> Logger => this.ServiceProvider.GetService<ILogger<BaboonApplication>>();
        public IServiceProvider ServiceProvider => AppHost?.Services;

        /// <summary>
        /// 配置模块
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected virtual void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
        }

        protected virtual HostApplicationBuilder CreateApplicationBuilder(StartupEventArgs e)
        {
            var builder = Host.CreateApplicationBuilder(e.Args);

            return builder;
        }

        /// <summary>
        /// 获取主窗体
        /// </summary>
        /// <returns></returns>
        protected abstract Window CreateMainWindow();

        protected abstract void Initialize(AppModuleInitEventArgs e);

        /// <summary>
        /// 在异常的时候
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void OnException(Exception ex)
        {
            this.Logger?.LogError(ex, ex.Message);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="e"></param>
        protected override sealed void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = this.CreateApplicationBuilder(e);

            #region 配置、加载插件
            var moduleCatalog = new ModuleCatalog();
            this.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.Build();
            #endregion

            #region 注册服务

            builder.Services.AddSingleton<IModuleCatalog>(moduleCatalog);
            builder.Services.AddSingleton(this);

            this.Initialize(new AppModuleInitEventArgs(e.Args, builder.Services));
            #endregion 注册

           


            foreach (var appModule in moduleCatalog.GetAppModules())
            {
                var resources = appModule.Resources;
                if (resources != null)
                {
                    this.Resources.MergedDictionaries.Add(resources);
                }

                appModule.Initialize(this, new AppModuleInitEventArgs(e.Args, builder.Services));
            }

            var host = builder.Build();
            this.AppHost = host;

            this.Startup(new AppModuleStartupEventArgs(host));

            foreach (var appModule in moduleCatalog.GetAppModules())
            {
                appModule.Startup(this, new AppModuleStartupEventArgs(host));
            }

            this.MainWindow = this.CreateMainWindow();
            this.MainWindow.Show();
        }

        protected abstract void Startup(AppModuleStartupEventArgs e);

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