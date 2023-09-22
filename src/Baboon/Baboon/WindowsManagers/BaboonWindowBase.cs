using Baboon.Mvvm;
using System.Windows;

namespace Baboon
{
    public abstract class BaboonWindowBase : Window
    {
        public object Key { get; set; }

        public virtual void OnSetViewModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase
        {
            this.DataContext = viewModel;
        }
    }
}