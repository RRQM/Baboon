using System.Windows;
using System.Windows.Media;
using TouchSocket.Core;

namespace Baboon
{
    /// <summary>
    /// 模块基类
    /// </summary>
    public abstract class AppModuleBase : DisposableObject, IAppModule
    {
        /// <summary>
        /// 模块基类
        /// </summary>
        /// <param name="loggerFactory"></param>
        public AppModuleBase(ILoggerFactoryService loggerFactory)
        {
            this.Logger = loggerFactory.GetLogger(this.GetType().Name);
            //this.Resources = new ResourceDictionary();
        }

        /// <inheritdoc/>
        public abstract ModuleDescription Description { get; }

        /// <inheritdoc/>
        public abstract ImageSource Icon { get; }

        /// <inheritdoc/>
        public ILogger Logger { get; protected set; }

        /// <inheritdoc/>
        public ResourceDictionary Resources { get; set; }

        /// <inheritdoc/>
        public virtual void OnInitialized(IContainer container)
        {
        }

        /// <inheritdoc/>
        public abstract void Show(object parameter = null);
    }
}