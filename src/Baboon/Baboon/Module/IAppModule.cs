using System;
using System.Windows;

namespace Baboon
{
    /// <summary>
    /// 能够提供模块化的接口
    /// </summary>
    public interface IAppModule : IDisposable
    {
        /// <summary>
        /// 模块描述。
        /// </summary>
        ModuleDescription Description { get; }

        /// <summary>
        /// 资源
        /// </summary>
        ResourceDictionary Resources { get;}

        void Initialize(BaboonApplication application, AppModuleInitEventArgs e);

        IServiceProvider ServiceProvider { get; }

        void Startup(BaboonApplication application, AppModuleStartupEventArgs e);
    }
}