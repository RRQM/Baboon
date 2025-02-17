using Microsoft.Extensions.DependencyInjection;
using System;
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
        public void Initialize(BaboonApplication application, AppModuleInitEventArgs e)
        {
            this.OnInitialize(application, e);
        }

        /// <inheritdoc/>
        public void Startup(BaboonApplication application, AppModuleStartupEventArgs e)
        {
            this.serviceScope = e.AppHost.Services.CreateScope();
            this.OnStartup(application, e);
        }

        protected abstract void OnInitialize(BaboonApplication application, AppModuleInitEventArgs e);

        protected abstract void OnStartup(BaboonApplication application, AppModuleStartupEventArgs e);

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