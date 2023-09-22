using Baboon.Mvvm;
using System;
using System.Collections.Concurrent;
using System.Windows;
using TouchSocket.Core;

namespace Baboon
{
    public class WindowsManager<TWindow> where TWindow : BaboonWindowBase
    {
        /// <summary>
        /// 单例窗口模式
        /// </summary>
        public bool SingletonMode { get; set; }

        public IContainer Provider { get; }

        public ConcurrentDictionary<object, TWindow> Wins { get; } = new ConcurrentDictionary<object, TWindow>();

        public WindowsManager(IContainer provider)
        {
            this.Provider = provider;
        }

        public virtual void Show(object key)
        {
            if (this.Wins.TryGetValue(key, out var window))
            {
                if (window.IsLoaded)
                {
                    window.Activate();
                }
                else
                {
                    window.Show();
                }
            }
        }

        public virtual void ShowDialog(object key)
        {
            if (this.Wins.TryGetValue(key, out var window))
            {
                if (window.IsLoaded)
                {
                    window.Activate();
                }
                else
                {
                    window.ShowDialog();
                }
            }
        }

        public virtual bool TryGetWin(object key, out TWindow window)
        {
            return this.Wins.TryGetValue(key, out window);
        }

        public virtual bool NewCreate(object key)
        {
            return this.NewCreate(key, out _);
        }

        public virtual bool NewCreate(object key, out TWindow window)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (this.Wins.TryGetValue(key, out window))
            {
                return false;
            }
            window = this.Provider.Resolve<TWindow>();
            window.Key = key;
            window.Closing += this.Window_Closing;
            window.Closed += this.Window_Closed;
            this.Wins.TryAdd(key, window);
            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.SingletonMode)
            {
                e.Cancel = true;
                var window = sender as TWindow;
                window.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (sender is TWindow window)
            {
                window.Closing -= this.Window_Closing;
                window.Closed -= this.Window_Closed;
                var key = window.Key;
                if (key != null)
                {
                    this.Wins.TryRemove(key, out _);
                }

                if (window.DataContext is IDisposable dis)
                {
                    dis.SafeDispose();
                }
            }
        }
    }

    public class WindowsManager<TWindow, TViewModel> : WindowsManager<TWindow> where TWindow : BaboonWindowBase where TViewModel : ViewModelBase
    {
        public WindowsManager(IContainer provider) : base(provider)
        {
        }

        public virtual TViewModel GetViewModel(object key)
        {
            if (this.TryGetWin(key, out var window))
            {
                return window.DataContext as TViewModel;
            }
            return default;
        }

        public override bool NewCreate(object key, out TWindow window)
        {
            var b = base.NewCreate(key, out window);
            if (b)
            {
                var viewModel = this.Provider.Resolve<TViewModel>();
                viewModel.OnSetView(window);
                window.OnSetViewModel(viewModel);
            }
            return b;
        }

        public virtual bool NewCreate(object key, out TWindow window, out TViewModel viewModel)
        {
            var b = base.NewCreate(key, out window);
            if (b)
            {
                viewModel = this.Provider.Resolve<TViewModel>();
                viewModel.OnSetView(window);
                window.OnSetViewModel(viewModel);
            }
            else
            {
                viewModel = window.DataContext as TViewModel;
            }
            return b;
        }
    }
}