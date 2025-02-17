
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

        protected override void OnInitialize(BaboonApplication application, AppModuleInitEventArgs e)
        {
            
        }

        protected override void OnStartup(BaboonApplication application, AppModuleStartupEventArgs e)
        {
            MessageBox.Show("Hello");
        }
    }
}