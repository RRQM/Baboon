using Baboon.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Baboon.Wpf.ViewModels
{
    internal class MainViewModel : ObservableRecipient
    {
        private readonly IModuleCatalog m_moduleCatalog;

        public MainViewModel(IModuleCatalog moduleCatalog, IRegionManager regionManager)
        {
            this.ThrowErrorCommand = new RelayCommand(this.ThrowError);
            this.SayHelloCommand = new RelayCommand(SayHello);
            this.m_moduleCatalog = moduleCatalog;

            //regionManager.RequestNavigate("mainRoot", "SayHello");
        }



        #region Command
        public RelayCommand ThrowErrorCommand { get; set; }
        public RelayCommand SayHelloCommand { get; set; }
        #endregion

        #region 方法
        private void ThrowError()
        {
            throw new Exception("错误");
        }

        private void SayHello()
        {
            if (!this.m_moduleCatalog.Contains("SayHello"))
            {
                MessageBox.Show("没有找到模块");
            }


        }

        #endregion
    }
}
