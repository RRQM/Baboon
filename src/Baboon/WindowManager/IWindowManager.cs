using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Baboon
{
    public interface IWindowManager
    {
        TWindow GetWindow<TWindow>(object? token = null) where TWindow : Window;
        void Show<TWindow>(object? token = null) where TWindow : Window;
        bool? ShowDialog<TWindow>(object? token = null) where TWindow : Window;
    }
}
