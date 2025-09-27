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

using Baboon.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Baboon.Wpf.ViewModels;

internal class MainViewModel : ObservableRecipient
{
    private readonly IModuleCatalog m_moduleCatalog;

    public MainViewModel(IModuleCatalog moduleCatalog, IRegionManager regionManager, IMenuService menuService)
    {
        this.ThrowErrorCommand = new RelayCommand(this.ThrowError);
        this.SayHelloCommand = new RelayCommand(this.SayHello);
        this.m_moduleCatalog = moduleCatalog;

        this.MenuItems = menuService.MenuItems;
    }

    #region 属性
    private IEnumerable<MenuItem> menuItems;
    public IEnumerable<MenuItem> MenuItems
    {
        get { return this.menuItems; }
        set { this.SetProperty(ref this.menuItems, value); }
    }
    #endregion

    #region Command
    public RelayCommand ThrowErrorCommand { get; set; }
    public RelayCommand SayHelloCommand { get; set; }
    #endregion

    #region 方法
    private void ThrowError()
    {
        throw new Exception("错误");
    }

    private void SayHello()
    {
        if (!this.m_moduleCatalog.Contains("SayHello"))
        {
            MessageBox.Show("没有找到模块");
        }


    }

    #endregion
}
