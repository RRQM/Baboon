using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baboon
{
    public interface IApplication
    {
        IHost AppHost { get; }
        ILogger<BaboonWpfApplication> Logger { get; }
        IServiceProvider ServiceProvider { get; }
    }
}
