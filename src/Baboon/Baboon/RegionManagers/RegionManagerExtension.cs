using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Baboon
{
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
}