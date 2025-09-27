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
using System.Diagnostics;

namespace Baboon.Winform;

public partial class Form1 : Form
{
    private readonly IMenuService m_menuService;

    public Form1(IMenuService menuService)
    {
        this.InitializeComponent();
        this.m_menuService = menuService;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        this.menuStrip1.Items.Clear();
        foreach (var item in this.m_menuService.MenuItems)
        {
            var menuItem = CreateToolStripMenuItem(item);
            this.menuStrip1.Items.Add(menuItem);
        }
    }

    private ToolStripMenuItem CreateToolStripMenuItem(Baboon.Core.Services.MenuItem item)
    {
        var toolStripItem = new ToolStripMenuItem(item.Text ?? string.Empty);
        if (item.Action != null)
        {
            toolStripItem.Click += (s, e) => item.Action();
        }
        else if (item.ClickCommand != null)
        {
            toolStripItem.Click += (s, e) =>
            {
                if (item.ClickCommand.CanExecute(null))
                {
                    item.ClickCommand.Execute(null);
                }
            };
        }
        foreach (var child in item.Items)
        {
            toolStripItem.DropDownItems.Add(CreateToolStripMenuItem(child));
        }
        return toolStripItem;
    }

    private async void Run(object sender, EventArgs e)
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
            this.Text = "Hello";
        });
        await Task.Run(async () =>
        {
            //this.Text = "1132";
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
                    this.Text = "1111";
                    Debug.WriteLine("这里是主线程执行" + Thread.CurrentThread.ManagedThreadId);

                    await MainThreadTaskFactory.ReleaseMainThreadAsync();
                    Debug.WriteLine("主线程之后执行" + Thread.CurrentThread.ManagedThreadId);

                    //下面的设置应该不会生效，因为是在其他线程操作的
                    this.Text = "2222";
                }
                catch (Exception)
                {


                }

            }

        });
    }
}
