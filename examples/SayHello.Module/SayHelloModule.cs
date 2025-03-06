// ------------------------------------------------------------------------------
// 此代码版权（除特别声明或在XREF结尾的命名空间的代码）归作者本人若汝棋茗所有
// 源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
// CSDN博客：https://blog.csdn.net/qq_40374647
// 哔哩哔哩视频：https://space.bilibili.com/94253567
// Gitee源代码仓库：https://gitee.com/RRQM_Home
// Github源代码仓库：https://github.com/RRQM
// API首页：https://touchsocket.net/
// 交流QQ群：234762506
// 感谢您的下载和使用
// ------------------------------------------------------------------------------

using Baboon;
using System.Diagnostics;
using System.Windows.Forms;

namespace SatHello.Module;

public class SayHelloModule : AppModuleBase
{
    public SayHelloModule()
    {
        this.Description = new ModuleDescription("D7F3274A-2526-43FD-B278-099630BDA33E", "SayHello", new Version(1, 0, 0, 0), "RRQM", "test");
    }

    public override ModuleDescription Description { get; }

    protected override Task OnInitializeAsync(IApplication application, AppModuleInitEventArgs e)
    {
        var form = new Form();
        return Task.CompletedTask;
    }

    protected override async Task OnStartupAsync(IApplication application, AppModuleStartupEventArgs e)
    {
        //wpf中
        //var resourceService = this.ServiceProvider.GetRequiredService<IResourceService>();

        //resourceService.AddResourceDictionary(new ResourceDictionary());

        Debug.WriteLine("线程Id" + Thread.CurrentThread.ManagedThreadId);

        await Task.Delay(1000);

        await Task.Run(async () =>
        {
            for (var i = 0; i < 10; i++)
            {
                Debug.WriteLine("这里是线程池线程执行" + Thread.CurrentThread.ManagedThreadId);
            }
            using (var tokenSource = new CancellationTokenSource(2000))
            {
                //await Task.Delay(3000);

                try
                {
                    await MainThreadTaskFactory.SwitchToMainThreadAsync(tokenSource.Token);

                    Debug.WriteLine("这里是主线程执行" + Thread.CurrentThread.ManagedThreadId);

                    await MainThreadTaskFactory.ReleaseMainThreadAsync();
                    Debug.WriteLine("主线程之后执行" + Thread.CurrentThread.ManagedThreadId);
                }
                catch (Exception)
                {


                }

            }

        });


        System.Windows.MessageBox.Show("Hello 模块已加载");
    }
}