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

using Avalonia.Controls;
using Avalonia.Interactivity;
using BaboonDemo.Core.Services;
using AMenuItem = Avalonia.Controls.MenuItem;
using AppMenuItem = BaboonDemo.Core.Services.MenuItem;

namespace BaboonDemo.Avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private void MenuItem_Click(object? sender, RoutedEventArgs e)
    {
        // no longer needed, kept for compatibility
        if (sender is AMenuItem menuItem && menuItem.DataContext is AppMenuItem model)
        {
            if (model.ClickCommand?.CanExecute(null) == true)
            {
                model.ClickCommand.Execute(null);
            }
            else
            {
                model.Action?.Invoke();
            }
        }
    }
}