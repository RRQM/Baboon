using System.Windows;

namespace Baboon;

internal class InternalResourceService : IResourceService
{
    private readonly BaboonWpfApplication application;

    public InternalResourceService(BaboonWpfApplication baboonWpfApplication)
    {
        this.application = baboonWpfApplication;
    }

    public void AddResourceDictionary(ResourceDictionary resourceDictionary)
    {
        this.application.Resources.MergedDictionaries.Add(resourceDictionary);
    }
}
