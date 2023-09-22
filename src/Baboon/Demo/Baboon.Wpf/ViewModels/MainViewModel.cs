using Baboon.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baboon.Wpf.ViewModels
{
    internal class MainViewModel:ViewModelBase
    {
        public MainViewModel()
        {
            this.ThrowErrorCommand = new ExecuteCommand(this.ThrowError);
        }

        #region Command
        public ExecuteCommand ThrowErrorCommand { get; set; }
        #endregion

        #region 方法
        private void ThrowError()
        {
            throw new Exception("错误");
        }
        #endregion
    }
}
