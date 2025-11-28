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

using Avalonia;
using Microsoft.Extensions.DependencyInjection;

namespace Baboon.Avalonia.Desktop;

/// <summary>
/// 提供IServiceCollection的扩展方法，用于注册视图和视图模型。
/// </summary>
public static class IServiceCollectionExtension
{
    /// <summary>
    /// 将单例视图及其视图模型添加到服务集合中。
    /// </summary>
    /// <typeparam name="TView">视图的类型。</typeparam>
    /// <typeparam name="TViewModel">视图模型的类型。</typeparam>
    /// <param name="services">服务集合。</param>
    public static void AddSingletonView<TView, TViewModel>(this IServiceCollection services)
                         where TView : StyledElement
    {
        AddSingletonView(services, typeof(TView), typeof(TViewModel));
    }

    /// <summary>
    /// 将单例视图及其视图模型添加到服务集合中。
    /// </summary>
    /// <param name="services">服务集合。</param>
    /// <param name="viewType">视图的类型。</param>
    /// <param name="viewModelType">视图模型的类型。</param>
    public static void AddSingletonView(this IServiceCollection services, Type viewType, Type viewModelType)
    {
        if (!typeof(StyledElement).IsAssignableFrom(viewType))
        {
            throw new Exception($"View必须继承自StyledElement");
        }

        services.AddSingleton(viewModelType);
        services.AddSingleton(viewType, provider =>
        {
            var view = (StyledElement)ActivatorUtilities.CreateInstance(provider, viewType);
            var viewModel = provider.GetRequiredService(viewModelType);
            view.DataContext = viewModel;

            //if (viewModel is ViewModel viewModelBase)
            //{
            //    viewModelBase.OnSetView(view);
            //}

            return view;
        });
    }

    /// <summary>
    /// 将瞬态视图及其视图模型添加到服务集合中。
    /// </summary>
    /// <typeparam name="TView">视图的类型。</typeparam>
    /// <typeparam name="TViewModel">视图模型的类型。</typeparam>
    /// <param name="services">服务集合。</param>
    public static void AddTransientView<TView, TViewModel>(this IServiceCollection services)
                 where TView : StyledElement
    {
        AddTransientView(services, typeof(TView), typeof(TViewModel));
    }

    /// <summary>
    /// 将瞬态视图及其视图模型添加到服务集合中。
    /// </summary>
    /// <param name="services">服务集合。</param>
    /// <param name="viewType">视图的类型。</param>
    /// <param name="viewModelType">视图模型的类型。</param>
    public static void AddTransientView(this IServiceCollection services, Type viewType, Type viewModelType)
    {
        if (!typeof(StyledElement).IsAssignableFrom(viewType))
        {
            throw new Exception($"View必须继承自StyledElement");
        }
        services.AddTransient(viewModelType);
        services.AddTransient(viewType, provider =>
        {
            var view = (StyledElement)ActivatorUtilities.CreateInstance(provider, viewType);
            var viewModel = provider.GetRequiredService(viewModelType);
            view.DataContext = viewModel;
            return view;
        });
    }




    #region WindowManager
    public static void AddWindowManager(this IServiceCollection services)
    {
        services.AddSingleton<IWindowManager, WindowManager>();
    }
    #endregion
}
