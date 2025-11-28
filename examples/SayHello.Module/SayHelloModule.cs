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
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

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
        return Task.CompletedTask;
    }

    protected override Task OnStartupAsync(IApplication application, AppModuleStartupEventArgs e)
    {
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

                ClickCommand = new RelayCommand2(() =>
                {
                    messenger.Send(new TextMessage("Hello Baboon!"));
                }),


            });
        }

        return Task.CompletedTask;
    }
}


public class RelayCommand2 : ICommand
{
    private readonly Action _action;

    public RelayCommand2(Action action)
    {
        this._action = action;

        CanExecuteChanged += this.RelayCommand2_CanExecuteChanged;
    }

    private void RelayCommand2_CanExecuteChanged(object? sender, EventArgs e)
    {

    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => this._action();

    public event EventHandler? CanExecuteChanged = delegate { };
}
