using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TouchSocket.Core;

namespace Baboon
{
    /// <summary>
    /// 模块基类
    /// </summary>
    public abstract class AppModuleBase : SafetyDisposableObject, IAppModule
    {
        private IServiceScope serviceScope;

        /// <inheritdoc/>
        public abstract ModuleDescription Description { get; }

        /// <inheritdoc/>
        public ResourceDictionary Resources { get; protected set; }

        /// <inheritdoc/>
        public virtual IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

        /// <inheritdoc/>
        public async Task InitializeAsync(BaboonApplication application, AppModuleInitEventArgs e)
        {
            await this.OnInitializeAsync(application, e);
        }

        /// <inheritdoc/>
        public async Task StartupAsync(BaboonApplication application, AppModuleStartupEventArgs e)
        {
            this.serviceScope = e.AppHost.Services.CreateScope();
            await this.OnStartupAsync(application, e);
        }

        protected abstract Task OnInitializeAsync(BaboonApplication application, AppModuleInitEventArgs e);

        protected abstract Task OnStartupAsync(BaboonApplication application, AppModuleStartupEventArgs e);

        /// <inheritdoc/>
        protected override void SafetyDispose(bool disposing)
        {
            if (disposing)
            {
                var serviceScope = this.serviceScope;
                serviceScope.SafeDispose();
            }
        }
    }
}