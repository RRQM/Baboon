using System;
using System.Threading.Tasks;
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

        Task InitializeAsync(IApplication application,AppModuleInitEventArgs e);

        IServiceProvider ServiceProvider { get; }

        Task StartupAsync(IApplication application,AppModuleStartupEventArgs e);
    }
}