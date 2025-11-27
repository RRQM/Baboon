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
using System.Collections.Generic;
using System.Windows.Controls;
using TouchSocket.Core;

namespace Baboon.Desktop;

public class RegionManager : IRegionManager
{
    private readonly Dictionary<string, ContentControl> m_rootContents = new Dictionary<string, ContentControl>();
    private readonly IServiceProvider m_serviceProvider;

    public RegionManager(IServiceProvider serviceProvider)
    {
        this.m_serviceProvider = serviceProvider;
    }

    public void AddRoot(string contentRegion, ContentControl rootContent)
    {
        contentRegion = contentRegion.HasValue() ? contentRegion : string.Empty;
        if (this.m_rootContents.ContainsKey(contentRegion))
        {
            throw new Exception($"名称为{contentRegion}的导航区域已被注册");
        }
        this.m_rootContents.Add(contentRegion, rootContent);
    }

    public void RequestNavigate(string contentRegion, string tag)
    {
        contentRegion = contentRegion.HasValue() ? contentRegion : string.Empty;
        if (!this.m_rootContents.TryGetValue(contentRegion, out var contentControl))
        {
            return;
        }

        contentControl.Content = this.m_serviceProvider.GetRequiredKeyedService<object>(tag);
    }
}