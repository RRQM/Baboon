using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Baboon;

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
                         where TView : FrameworkElement
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
        if (!typeof(FrameworkElement).IsAssignableFrom(viewType))
        {
            throw new Exception($"View必须继承自FrameworkElement");
        }

        services.AddSingleton(viewModelType);
        services.AddSingleton(viewType, provider =>
        {
            var view = (FrameworkElement)ActivatorUtilities.CreateInstance(provider, viewType);
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
                 where TView : FrameworkElement
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
        if (!typeof(FrameworkElement).IsAssignableFrom(viewType))
        {
            throw new Exception($"View必须继承自FrameworkElement");
        }
        services.AddTransient(viewModelType);
        services.AddTransient(viewType, provider =>
        {
            var view = (FrameworkElement)ActivatorUtilities.CreateInstance(provider, viewType);
            var viewModel = provider.GetRequiredService(viewModelType);
            view.DataContext = viewModel;
            return view;
        });
    }
}
