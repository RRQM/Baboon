
using Baboon;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

namespace SatHello.Module
{
    public class SayHelloModule : AppModuleBase
    {
        public SayHelloModule()
        {
            this.Description = new ModuleDescription("D7F3274A-2526-43FD-B278-099630BDA33E", "SayHello", new Version(1, 0, 0, 0), "RRQM", "test");
        }

        public override ModuleDescription Description { get; }

        protected override Task OnInitializeAsync(IApplication application, AppModuleInitEventArgs e)
        {
            Form form = new Form();
            return Task.CompletedTask;
        }

        protected override async Task OnStartupAsync(IApplication application, AppModuleStartupEventArgs e)
        {
            var resourceService = this.ServiceProvider.GetRequiredService<IResourceService>();

            resourceService.AddResourceDictionary(new ResourceDictionary());

            Debug.WriteLine("线程Id" + Thread.CurrentThread.ManagedThreadId);

            await Task.Delay(1000);

            await Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Debug.WriteLine("这里是线程池线程执行" + Thread.CurrentThread.ManagedThreadId);
                }
                using (CancellationTokenSource tokenSource = new CancellationTokenSource(2000))
                {
                    //await Task.Delay(3000);

                    try
                    {
                        await MainThreadTaskFactory.SwitchToMainThreadAsync(tokenSource.Token);

                        Debug.WriteLine("这里是主线程执行" + Thread.CurrentThread.ManagedThreadId);

                        await MainThreadTaskFactory.ReleaseMainThreadAsync();
                        Debug.WriteLine("主线程之后执行" + Thread.CurrentThread.ManagedThreadId);
                    }
                    catch (Exception ex)
                    {


                    }

                }

            });


            System.Windows.MessageBox.Show("Hello 模块已加载");
        }
    }
}