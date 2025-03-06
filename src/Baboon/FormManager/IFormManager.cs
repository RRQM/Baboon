using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baboon
{
   public interface IFormManager
    {
        TForm GetForm<TForm>(object? token = null) where TForm : Form;
        void Show<TForm>(object? token = null) where TForm : Form;
        DialogResult ShowDialog<TForm>(object? token = null) where TForm : Form;
    }
}
