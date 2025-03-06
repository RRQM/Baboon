using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Baboon
{
    class WindowManager:IWindowManager
    {
        private readonly ConcurrentDictionary<object, Window> pairs = new ConcurrentDictionary<object, Window>();
        private readonly IServiceProvider serviceProvider;

        public WindowManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public TWindow GetWindow<TWindow>(object? token = default) where TWindow : Window
        {
            if (token == default)
            {
                var window = ActivatorUtilities.GetServiceOrCreateInstance<TWindow>(this.serviceProvider);
                return window;
            }
            else
            {
                if (pairs.TryGetValue(token, out var Window))
                {
                    return (TWindow)Window;
                }
                Window = ActivatorUtilities.CreateInstance<TWindow>(this.serviceProvider);
                Window.Closed += (s, e) =>
                {
                    this.pairs.TryRemove(token, out _);
                };
                return (TWindow)Window;
            }
        }

        public void Show<TWindow>(object? token = default) where TWindow : Window
        {
            var Window = GetWindow<TWindow>(token);
            Window.Show();
        }

        public bool? ShowDialog<TWindow>(object? token = default) where TWindow : Window
        {
            var Window = GetWindow<TWindow>(token);
            return Window.ShowDialog();
        }
    }
}
