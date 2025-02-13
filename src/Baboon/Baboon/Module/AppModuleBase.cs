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
        public IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

        /// <inheritdoc/>
        public abstract void Initialize(IServiceCollection services);

        /// <inheritdoc/>
        public void Run(IServiceProvider serviceProvider)
        {
            this.serviceScope = serviceProvider.CreateScope();
            this.OnStartup();
        }

        /// <summary>
        /// 当程序模块启动的时候。
        /// </summary>
        protected abstract void OnStartup();

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