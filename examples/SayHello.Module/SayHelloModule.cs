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

using Baboon.Core;
using BaboonDemo.Core.Messages;
using BaboonDemo.Core.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace SatHello.Module;

public class SayHelloModule : AppModuleBase
{
    public SayHelloModule()
    {
        this.Description = ModuleDescription.FromAssembly(this.GetType().Assembly);
    }

    public override ModuleDescription Description { get; }

    protected override Task OnInitializeAsync(IApplication application, AppModuleInitEventArgs e)
    {
        e.Services.AddHostedService<MyClass>();
        return Task.CompletedTask;
    }

    protected override async Task OnStartupAsync(IApplication application, AppModuleStartupEventArgs e)
    {
        await Task.Delay(100).ConfigureAwait(false);
        var serviceProvider = this.ServiceProvider;
        var messenger = serviceProvider.GetRequiredService<IMessenger>();
        if (serviceProvider is not null)
        {
            var menuService = serviceProvider.GetRequiredService<IMenuService>();

            menuService.AddMenuItem(new MenuItem()
            {
                Id = Guid.NewGuid(),
                Text = "Say Hello",
                Action = () =>
                {
                    messenger.Send(new TextMessage("Hello Baboon!"));
                },

                ClickCommand = new RelayCommand(() =>
                {
                    messenger.Send(new TextMessage("Hello Baboon!"));
                }),


            });
        }

        await Task.CompletedTask;
    }
}

class MyClass : BackgroundService
{
    public MyClass(IHost host)
    {
        string ss=host.GetType().FullName!;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            await Task.Delay(1000, stoppingToken);
            Console.WriteLine("Hello Baboon!");
        }
    }
}