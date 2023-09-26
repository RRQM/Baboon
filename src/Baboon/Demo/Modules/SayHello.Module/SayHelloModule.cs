
using Baboon;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace SatHello.Module
{
    public class SayHelloModule : AppModuleBase
    {
        public SayHelloModule(ILoggerFactoryService loggerFactory) : base(loggerFactory)
        {
            this.Description = ModuleDescription.CreateByDescriptionFile(Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "Description.xml"));
        }

        public override ModuleDescription Description { get; }

        public override ImageSource Icon => throw new NotImplementedException();

        public override void Show(object parameter = null)
        {
            MessageBox.Show("Hello");
        }
    }
}