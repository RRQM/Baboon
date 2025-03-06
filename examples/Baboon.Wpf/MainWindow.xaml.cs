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

using System.Diagnostics;
using System.Windows;

namespace Baboon.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(IRegionManager regionManager)
    {
        this.InitializeComponent();

        this.Loaded += this.MainWindow_Loaded;
        regionManager.AddRoot("mainRoot", this.contentRoot);
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await Task.Run(async () =>
        {
            //模拟耗时操作
            for (var i = 0; i < 1000; i++)
            {
                Debug.WriteLine(i);
            }

            //切换到主线程
            await MainThreadTaskFactory.SwitchToMainThreadAsync();

            //更新UI
            this.Title = "Hello";
        });
    }
}