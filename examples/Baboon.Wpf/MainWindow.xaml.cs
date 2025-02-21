using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Baboon.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();

            this.Loaded += this.MainWindow_Loaded;
            regionManager.AddRoot("mainRoot",this.contentRoot);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () =>
            {
                //模拟耗时操作
                for (int i = 0; i < 1000; i++)
                {
                    Debug.WriteLine(i);
                }

                //切换到主线程
                await MainThreadTaskFactory.SwitchToMainThreadAsync();

                //更新UI
                this.Title = "Hello";
            });
        }
    }
}