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
        public virtual void Initialize(BaboonApplication application, AppModuleInitEventArgs e)
        {

        }

        /// <inheritdoc/>
        public virtual void Startup(BaboonApplication application, AppModuleStartupEventArgs e)
        {
            this.serviceScope = e.AppHost.Services.CreateScope();
        }

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