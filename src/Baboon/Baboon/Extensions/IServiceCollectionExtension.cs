using Baboon.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using TouchSocket.Core;

namespace Baboon
{
    public static class IServiceCollectionExtension
    {
        public static void AddSingletonView<TView, TViewModel>(this IServiceCollection services)
                             where TView : FrameworkElement
        {
            AddSingletonView(services, typeof(TView), typeof(TViewModel));
        }

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

        public static void AddTransientView<TView, TViewModel>(this IServiceCollection services)
                     where TView : FrameworkElement
        {
            AddTransientView(services, typeof(TView), typeof(TViewModel));
        }

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
}
