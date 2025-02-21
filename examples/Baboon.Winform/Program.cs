
using Microsoft.Extensions.DependencyInjection;
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
        var myApp = new MyApp();
        await myApp.RunAsync();
    }

    class MyApp : BaboonWinformApplication
    {
        protected override Form CreateMainForm()
        {
            return this.ServiceProvider.GetRequiredService<Form1>();
        }

        protected override Task InitializeAsync(AppModuleInitEventArgs e)
        {
            e.Services.AddSingleton<Form1>();
            return Task.CompletedTask;
        }

        protected override Task StartupAsync(AppModuleStartupEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}