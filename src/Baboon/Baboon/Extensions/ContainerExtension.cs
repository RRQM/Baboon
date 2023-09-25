using Baboon.Mvvm;
using System;
using System.Windows;
using TouchSocket.Core;

namespace Baboon
{
    /// <summary>
    /// ContainerExtension
    /// </summary>
    public static class ContainerExtension
    {
        #region 单例
        /// <summary>
        /// 注册单例View和ViewModel
        /// </summary>
        /// <param name="container"></param>
        /// <param name="viewType"></param>
        /// <param name="viewModelType"></param>
        public static void RegisterSingletonView(this IContainer container, Type viewType, Type viewModelType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(viewType))
            {
                throw new Exception($"View必须继承自FrameworkElement");
            }
            container.Register(new DependencyDescriptor(viewType, viewType, Lifetime.Singleton)
            {
                OnResolved = (obj) =>
                {
                    var view = (FrameworkElement)obj;
                    view.Loaded += (s, e) =>
                    {
                        var viewModel = container.Resolve(viewModelType);
                        view.DataContext = viewModel;

                        if (viewModel is ViewModelBase viewModelBase)
                        {
                            viewModelBase.OnSetView(view);
                        }
                    };
                }
            });
            container.RegisterSingleton(viewModelType);
        }

        /// <summary>
        /// 注册单例View和ViewModel
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="container"></param>
        public static void RegisterSingletonView<TView, TViewModel>(this IContainer container)
            where TView : FrameworkElement
        {
            RegisterSingletonView(container, typeof(TView), typeof(TViewModel));
        }
        #endregion

        #region 瞬态
        /// <summary>
        /// 注册瞬态View和ViewModel
        /// </summary>
        /// <param name="container"></param>
        /// <param name="viewType"></param>
        /// <param name="viewModelType"></param>
        public static void RegisterTransientView(this IContainer container, Type viewType, Type viewModelType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(viewType))
            {
                throw new Exception($"View必须继承自FrameworkElement");
            }
            container.Register(new DependencyDescriptor(viewType, viewType, Lifetime.Transient)
            {
                OnResolved = (obj) =>
                {
                    var view = (FrameworkElement)obj;
                    view.Loaded += (s, e) =>
                    {
                        var viewModel = container.Resolve(viewModelType);
                        view.DataContext = viewModel;

                        if (viewModel is ViewModelBase viewModelBase)
                        {
                            viewModelBase.OnSetView(view);
                        }
                    };
                }
            });
            container.RegisterTransient(viewModelType);
        }

        /// <summary>
        /// 注册瞬态View和ViewModel
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="container"></param>
        public static void RegisterTransientView<TView, TViewModel>(this IContainer container)
            where TView : FrameworkElement
        {
            RegisterTransientView(container, typeof(TView), typeof(TViewModel));
        }
        #endregion
    }
}
