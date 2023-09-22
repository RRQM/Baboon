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
        /// 日志记录器。
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// 小图标
        /// </summary>
        ImageSource Icon { get; }

        /// <summary>
        /// 模块描述。
        /// </summary>
        ModuleDescription Description { get; }

        /// <summary>
        /// 资源
        /// </summary>
        ResourceDictionary Resources { get; set; }

        /// <summary>
        /// 在模块初始化完成时调用
        /// </summary>
        /// <param name="container"></param>
        void OnInitialized(IContainer container);

        /// <summary>
        /// 当App调用显示UI时触发。
        /// </summary>
        /// <param name="parameter"></param>
        void Show(object parameter = default);
    }
}