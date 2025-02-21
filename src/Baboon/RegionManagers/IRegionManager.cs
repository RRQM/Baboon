using System.Windows.Controls;

namespace Baboon
{
    public interface IRegionManager
    {
        void RequestNavigate(string contentRegion, string tag);
        void AddRoot(string contentRegion, ContentControl rootContent);
    }
}