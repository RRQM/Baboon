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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Baboon;

internal class WindowManager : IWindowManager
{
    private readonly ConcurrentDictionary<object, Window> m_pairs = new ConcurrentDictionary<object, Window>();
    private readonly IServiceProvider m_serviceProvider;

    public WindowManager(IServiceProvider serviceProvider)
    {
        this.m_serviceProvider = serviceProvider;
    }
    public TWindow GetWindow<TWindow>(object? token = default) where TWindow : Window
    {
        if (token == default)
        {
            var window = ActivatorUtilities.GetServiceOrCreateInstance<TWindow>(this.m_serviceProvider);
            return window;
        }
        else
        {
            if (this.m_pairs.TryGetValue(token, out var Window))
            {
                return (TWindow)Window;
            }
            Window = ActivatorUtilities.GetServiceOrCreateInstance<TWindow>(this.m_serviceProvider);
            Window.Closed += (s, e) =>
            {
                this.m_pairs.TryRemove(token, out _);
            };
            return (TWindow)Window;
        }
    }

    public void Show<TWindow>(object? token = default) where TWindow : Window
    {
        var Window = this.GetWindow<TWindow>(token);
        Window.Show();
    }

    public bool? ShowDialog<TWindow>(object? token = default) where TWindow : Window
    {
        var Window = this.GetWindow<TWindow>(token);
        return Window.ShowDialog();
    }
}
