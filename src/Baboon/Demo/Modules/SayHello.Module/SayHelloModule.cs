
using Baboon;
using System.Diagnostics;
using System.Windows;

namespace SatHello.Module
{
    public class SayHelloModule : AppModuleBase
    {
        public SayHelloModule()
        {
            this.Description = new ModuleDescription("D7F3274A-2526-43FD-B278-099630BDA33E", "SayHello", new Version(1, 0, 0, 0), "Carywang", "test");
        }

        public override ModuleDescription Description { get; }

        protected override Task OnInitializeAsync(BaboonApplication application, AppModuleInitEventArgs e)
        {
            return Task.CompletedTask;
        }

        protected override async Task OnStartupAsync(BaboonApplication application, AppModuleStartupEventArgs e)
        {
            Debug.WriteLine("�߳�Id" + Thread.CurrentThread.ManagedThreadId);
            await Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Debug.WriteLine("�������̳߳��߳�ִ��" + Thread.CurrentThread.ManagedThreadId);
                }
                using (CancellationTokenSource tokenSource = new CancellationTokenSource(2000))
                {
                    //await Task.Delay(3000);

                    try
                    {
                        await MainThreadTaskFactory.SwitchToMainThreadAsync(tokenSource.Token);

                        Debug.WriteLine("���������߳�ִ��" + Thread.CurrentThread.ManagedThreadId);

                        await MainThreadTaskFactory.ReleaseMainThreadAsync();
                        Debug.WriteLine("���߳�֮��ִ��" + Thread.CurrentThread.ManagedThreadId);
                    }
                    catch (Exception ex)
                    {

                      
                    }
                    
                }

            });


            MessageBox.Show("Hello");
        }
    }
}