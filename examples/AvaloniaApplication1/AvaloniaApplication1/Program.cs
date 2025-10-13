using Avalonia;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace AvaloniaApplication1
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            AvaloniaApplication avaloniaApplication = new AvaloniaApplication();
            avaloniaApplication.Run(args);
        }

       
    }

    class AvaloniaApplication:Avalonia.Application
    {
        // Avalonia configuration, don't remove; also used by visual designer.
        public AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();

        /// <summary>
        /// 创建应用程序构建器。
        /// </summary>
        /// <param name="args">启动参数。</param>
        /// <returns>应用程序构建器。</returns>
        protected virtual HostApplicationBuilder CreateApplicationBuilder(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            return builder;
        }

        /// <summary>
        /// 运行应用程序。
        /// </summary>
        /// <param name="args">启动参数。</param>
        public void Run(params string[] args)
        {
            this.PrivateOnStartupAsync(args).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 运行应用程序。
        /// </summary>
        public void Run()
        {
            this.Run(Array.Empty<string>());
        }

        /// <summary>
        /// 私有启动方法。
        /// </summary>
        /// <param name="args">启动参数。</param>
        /// <returns>异步任务。</returns>
        private async Task PrivateOnStartupAsync(string[] args)
        {
            var builder = this.CreateApplicationBuilder(args);

            
            var host = builder.Build();
            
            await host.StartAsync();


            BuildAvaloniaApp()
               .StartWithClassicDesktopLifetime(args);
        }
    }
}
