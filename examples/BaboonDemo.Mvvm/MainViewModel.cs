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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BaboonDemo.Mvvm;

public class MainViewModel : ObservableRecipient, IRecipient<TextMessage>
{
    private readonly IModuleCatalog m_moduleCatalog;

    public MainViewModel(IModuleCatalog moduleCatalog, IMenuService menuService, IMessenger messenger)
: base(messenger)
    {
        this.m_moduleCatalog = moduleCatalog;

        this.MenuItems = menuService.MenuItems;

        this.IsActive = true;
    }

    #region 属性
    private IEnumerable<MenuItem>? menuItems;
    public IEnumerable<MenuItem>? MenuItems
    {
        get { return this.menuItems; }
        set { this.SetProperty(ref this.menuItems, value); }
    }

    public ObservableCollection<string> Messages { get; } = new();
    #endregion


    #region 方法

    private void SayHello()
    {

    }

    public void Receive(TextMessage message)
    {
        Messages.Add($"[{DateTime.Now}]:{message.Message}");
    }

    #endregion
}
