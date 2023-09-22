using System.Windows;
using System.Windows.Media;
using TouchSocket.Core;

namespace Baboon
{
    public abstract class AppModuleBase : DisposableObject, IAppModule
    {
        public AppModuleBase(ILoggerFactoryService loggerFactory)
        {
            this.Logger = loggerFactory.GetLogger(this.GetType().Name);
            //this.Resources = new ResourceDictionary();
        }

        public abstract ModuleDescription Description { get; }
        public abstract ImageSource Icon { get; }
        public ILogger Logger { get; protected set; }
        public ResourceDictionary Resources { get; set; }

        public virtual void OnInitialized(IContainer container)
        {
        }

        public abstract void Show(object parameter = null);
    }
}