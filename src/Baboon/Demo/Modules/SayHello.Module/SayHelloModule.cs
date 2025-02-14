
using Baboon;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace SatHello.Module
{
    public class SayHelloModule : AppModuleBase
    {
        public SayHelloModule()
        {
            this.Description = new ModuleDescription("D7F3274A-2526-43FD-B278-099630BDA33E", "SayHello",new Version(1,0,0,0),"Carywang","test");
        }

        public override ModuleDescription Description { get; }

        public override void Initialize(BaboonApplication application, AppModuleInitEventArgs e)
        {
            base.Initialize(application, e);
        }

        public override void Startup(BaboonApplication application, AppModuleStartupEventArgs e)
        {
            base.Startup(application, e);

            MessageBox.Show("Hello");
        }
    }
}