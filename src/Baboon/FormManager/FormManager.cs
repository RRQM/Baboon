using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baboon
{
    class FormManager : IFormManager
    {
        private readonly ConcurrentDictionary<object, Form> pairs = new ConcurrentDictionary<object, Form>();
        private readonly IServiceProvider serviceProvider;

        public FormManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public TForm GetForm<TForm>(object? token = default) where TForm : Form
        {
            if (token == default)
            {
                var form = ActivatorUtilities.CreateInstance<TForm>(this.serviceProvider);
                return form;
            }
            else
            {
                if (pairs.TryGetValue(token, out var form))
                {
                    return (TForm)form;
                }
                form = ActivatorUtilities.CreateInstance<TForm>(this.serviceProvider);
                form.FormClosed += (s, e) =>
                {
                    this.pairs.TryRemove(token, out _);
                };
                return (TForm)form;
            }
        }

        public void Show<TForm>(object? token = default) where TForm : Form
        {
            var form = GetForm<TForm>(token);
            form.Show();
        }

        public DialogResult ShowDialog<TForm>(object? token = default) where TForm : Form
        {
            var form = GetForm<TForm>(token);
            return form.ShowDialog();
        }
    }
}
