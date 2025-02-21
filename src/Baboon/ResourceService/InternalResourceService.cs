using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Baboon
{
    internal class InternalResourceService : IResourceService
    {
        private BaboonWpfApplication application;

        public InternalResourceService(BaboonWpfApplication baboonWpfApplication)
        {
            this.application = baboonWpfApplication;
        }

        public void AddResourceDictionary(ResourceDictionary resourceDictionary)
        {
            application.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }
}
