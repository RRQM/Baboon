using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using TouchSocket.Core;

namespace Baboon
{
    public class RegionManager : IRegionManager
    {
        private readonly IServiceProvider m_container;
        private readonly Dictionary<string, ContentControl> m_rootContents = new Dictionary<string, ContentControl>();
        public RegionManager(IServiceProvider container, IResolver resolver)
        {
            this.m_container = container;
            this.m_container = resolver;
        }

        public void AddRoot(string contentRegion, ContentControl rootContent)
        {
            contentRegion = contentRegion.HasValue() ? contentRegion : string.Empty;
            if (this.m_rootContents.ContainsKey(contentRegion))
            {
                throw new Exception($"名称为{contentRegion}的导航区域已被注册");
            }
            this.m_rootContents.Add(contentRegion, rootContent);
        }

        public void RequestNavigate(string contentRegion, string tag)
        {
            contentRegion = contentRegion.HasValue() ? contentRegion : string.Empty;
            if (!this.m_rootContents.TryGetValue(contentRegion, out var contentControl))
            {
                return;
            }

            contentControl.Content = this.m_container.GetRequiredKeyedService<object>(tag);
        }
    }
}
