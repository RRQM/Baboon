using Baboon.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Baboon.Wpf.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly IModuleCatalog m_moduleCatalog;

        public MainViewModel(IModuleCatalog moduleCatalog)
        {
            this.ThrowErrorCommand = new ExecuteCommand(this.ThrowError);
            this.SayHelloCommand = new ExecuteCommand(SayHello);
            this.m_moduleCatalog = moduleCatalog;
        }



        #region Command
        public ExecuteCommand ThrowErrorCommand { get; set; }
        public ExecuteCommand SayHelloCommand { get; set; }
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

            if (this.m_moduleCatalog.TryGetAppModuleInfo("SayHello", out var appModuleInfo))
            {
                if (!appModuleInfo.Loaded)//没有加载到主程序
                {
                    if (MessageBox.Show("模块没有加载，是否加载？", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var app = appModuleInfo.GetApp();

                        app.Show();
                    }
                }
                else
                {
                    var app = appModuleInfo.GetApp();

                    app.Show();
                }
            }
        }

        #endregion
    }
}
