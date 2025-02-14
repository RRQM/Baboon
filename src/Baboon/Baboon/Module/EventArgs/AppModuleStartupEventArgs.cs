using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baboon
{
   public class AppModuleStartupEventArgs:EventArgs
    {
        public IHost AppHost { get;}

        public AppModuleStartupEventArgs(IHost appHost)
        {
            AppHost = appHost;
        }
    }
}
