using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Media;
using TouchSocket.Core;

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

        void Initialize(IServiceCollection services);

        IServiceProvider ServiceProvider { get; }

        void Run(IServiceProvider serviceProvider);
    }
}