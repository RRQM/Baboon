
using System.Threading.Tasks;

namespace Baboon.Winform;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static async Task Main()
    {
        //// To customize application configuration such as set high DPI settings or default font,
        //// see https://aka.ms/applicationconfiguration.
        //ApplicationConfiguration.Initialize();
        //Application.Run(new Form1());
        //var ss = Thread.CurrentThread;
        MyApp myApp = new MyApp();
        await myApp.RunAsync();
    }

    class MyApp : BaboonWinformApplication
    {
        protected override Form CreateMainForm()
        {
            return new Form1();
        }

        protected override Task InitializeAsync(AppModuleInitEventArgs e)
        {
            return Task.CompletedTask;
        }

        protected override Task StartupAsync(AppModuleStartupEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}