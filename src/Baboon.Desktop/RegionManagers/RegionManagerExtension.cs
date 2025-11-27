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
using System.Windows;

namespace Baboon.Core;

public static class RegionManagerExtension
{
    public static void AddSingletonNavigate<TView, TViewModel>(this IServiceCollection services, string tag)
        where TView : FrameworkElement
    {
        AddSingletonNavigate(services, tag, typeof(TView), typeof(TViewModel));
    }

    public static void AddSingletonNavigate(this IServiceCollection services, string tag, Type viewType, Type viewModelType)
    {
        if (string.IsNullOrEmpty(tag))
        {
            throw new ArgumentException($"“{nameof(tag)}”不能为 null 或空。", nameof(tag));
        }

        if (!typeof(FrameworkElement).IsAssignableFrom(viewType))
        {
            throw new Exception($"View必须继承自FrameworkElement");
        }

        services.AddSingleton(viewModelType);
        services.AddKeyedSingleton(typeof(object), tag, (provider, o) =>
        {
            var view = (FrameworkElement)ActivatorUtilities.CreateInstance(provider, viewType);
            var viewModel = provider.GetRequiredService(viewModelType);
            view.DataContext = viewModel;

            return view;
        });
    }
}