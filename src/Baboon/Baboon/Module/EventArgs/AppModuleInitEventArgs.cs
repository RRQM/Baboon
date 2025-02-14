using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baboon
{
   public class AppModuleInitEventArgs:EventArgs
    {
        public AppModuleInitEventArgs(string[] args, IServiceCollection services)
        {
            Args = args;
            Services = services;
        }

        public string[] Args { get;}
        public IServiceCollection Services { get;}
    }
}
