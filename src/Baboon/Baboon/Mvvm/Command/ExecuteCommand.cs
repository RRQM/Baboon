//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System;
using System.Windows.Input;

namespace Baboon.Mvvm
{
    /// <summary>
    /// ExecuteCommand
    /// </summary>
    public class ExecuteCommand : ICommand
    {
        /// <summary>
        /// ExecuteCommand
        /// </summary>
        /// <param name="command"></param>
        public ExecuteCommand(Action command)
        {
            this.m_delCommand = command;
        }

        private readonly Action m_delCommand;

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return this.m_delCommand != null;
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
            {
                CanExecuteChanged?.Invoke(this, null);
                this.m_delCommand.Invoke();
            }
        }
    }
}