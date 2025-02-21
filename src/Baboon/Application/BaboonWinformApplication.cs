using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using TouchSocket.Core;

namespace Baboon
{
    public abstract class BaboonWinformApplication :IApplication
    {
        protected BaboonWinformApplication()
        {
            #region 异常处理

            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
            Application.ThreadException += Application_ThreadException;

            #endregion 异常处理

            Application.ApplicationExit += Application_ApplicationExit;
            Application.Idle += Application_Idle;

            ApplicationConfiguration();
        }

        private void Application_Idle(object sender, EventArgs e)
        {

        }

        public IHost AppHost { get; private set; }

        public ILogger<BaboonWpfApplication> Logger => this.ServiceProvider.GetService<ILogger<BaboonWpfApplication>>();

        public Form MainForm { get; private set; }

        public IServiceProvider ServiceProvider => AppHost?.Services;

        public async Task RunAsync(string[] args)
        {
            await PrivateOnStartupAsync(args);
        }

        public async Task RunAsync()
        {
            await this.RunAsync([]);
        }

        protected virtual void ApplicationConfiguration()
        {
            global::System.Windows.Forms.Application.EnableVisualStyles();
            global::System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

#if NET6_0_OR_GREATER
            global::System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif

        }

        /// <summary>
        /// 配置模块
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected virtual void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
        }

        protected virtual HostApplicationBuilder CreateApplicationBuilder(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            return builder;
        }

        /// <summary>
        /// 获取主窗体
        /// </summary>
        /// <returns></returns>
        protected abstract Form CreateMainForm();

        protected virtual bool FindModule(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            return name.EndsWith("Module");
        }

        protected abstract Task InitializeAsync(AppModuleInitEventArgs e);

        /// <summary>
        /// 在异常的时候
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void OnException(Exception ex)
        {
            this.Logger?.LogError(ex, ex.Message);
        }

        protected abstract Task StartupAsync(AppModuleStartupEventArgs e);

        private async void Application_ApplicationExit(object sender, EventArgs e)
        {
            var moduleCatalog = this.ServiceProvider.GetService<IModuleCatalog>();
            foreach (var appModule in moduleCatalog.GetAppModules())
            {
                appModule.SafeDispose();
            }
            await this.AppHost.StopAsync();
        }

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            this.OnException(e.Exception);
        }

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

        private async Task PrivateOnStartupAsync(string[] args)
        {
            var builder = this.CreateApplicationBuilder(args);

            #region 配置、加载插件

            var moduleCatalog = new InternalModuleCatalog(FindModule);
            this.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.Build();

            #endregion 配置、加载插件

            #region 注册服务

            builder.Services.AddSingleton<IModuleCatalog>(moduleCatalog);
            builder.Services.AddSingleton(this);

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
            this.MainForm = this.CreateMainForm();

            MainThreadTaskFactory.Initialize();
            Application.Run(this.MainForm);
        }
    }
}