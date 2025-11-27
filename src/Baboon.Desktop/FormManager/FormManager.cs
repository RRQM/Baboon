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
using System.Windows.Forms;

namespace Baboon.Desktop;

internal class FormManager : IFormManager
{
    private readonly ConcurrentDictionary<object, Form> m_pairs = new ConcurrentDictionary<object, Form>();
    private readonly IServiceProvider m_serviceProvider;

    public FormManager(IServiceProvider serviceProvider)
    {
        this.m_serviceProvider = serviceProvider;
    }
    public TForm GetForm<TForm>(object? token = default) where TForm : Form
    {
        if (token == default)
        {
            var form = ActivatorUtilities.GetServiceOrCreateInstance<TForm>(this.m_serviceProvider);
            return form;
        }
        else
        {
            if (this.m_pairs.TryGetValue(token, out var form))
            {
                return (TForm)form;
            }
            form = ActivatorUtilities.GetServiceOrCreateInstance<TForm>(this.m_serviceProvider);
            form.FormClosed += (s, e) =>
            {
                this.m_pairs.TryRemove(token, out _);
            };
            return (TForm)form;
        }
    }

    public void Show<TForm>(object? token = default) where TForm : Form
    {
        var form = this.GetForm<TForm>(token);
        form.Show();
    }

    public DialogResult ShowDialog<TForm>(object? token = default) where TForm : Form
    {
        var form = this.GetForm<TForm>(token);
        return form.ShowDialog();
    }
}
