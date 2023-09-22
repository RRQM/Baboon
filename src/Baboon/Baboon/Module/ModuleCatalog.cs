using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TouchSocket.Core;

namespace Baboon
{
    public class ModuleCatalog : IModuleCatalog
    {
        private readonly IContainer m_container;
        private readonly BaboonApplication m_application;
        private readonly object m_locker = new object();
        private readonly Dictionary<string, AppModuleInfo> m_modules = new Dictionary<string, AppModuleInfo>();

        public ModuleCatalog(IContainer container, BaboonApplication application)
        {
            this.m_container = container;
            this.m_application = application;
        }

        public void Add(Type moduleType)
        {
            if (!typeof(IAppModule).IsAssignableFrom(moduleType))
            {
                throw new Exception($"模块类型必须继承{nameof(IAppModule)}");
            }

            this.m_container.RegisterSingleton(moduleType);

            var appModule = (IAppModule)this.m_container.Resolve(moduleType);

            this.RemoveAppModule(appModule.Description.Id);
            this.LoadResources(appModule);
            appModule.OnInitialized(this.m_container);
            this.m_modules.Add(appModule.Description.Id, new AppModuleInfo(appModule));
        }

        private void LoadResources(IAppModule appModule)
        {
            if (appModule.Resources != null)
            {
                this.m_application.Resources.MergedDictionaries.Add(appModule.Resources);
            }
        }

        public void Add(IAppModule appModule)
        {
            this.RemoveAppModule(appModule.Description.Id);
            this.LoadResources(appModule);
            appModule.OnInitialized(this.m_container);
            this.m_modules.Add(appModule.Description.Id, new AppModuleInfo(appModule));
        }

        public void Add(ModuleDescriptionBuilder builder)
        {
            IAppModule fun()
            {
                var assembly = Assembly.LoadFrom(Path.GetFullPath(Path.Combine(builder.RootDir, builder.Module)));

                var moduleType = assembly.ExportedTypes.Where(type => typeof(IAppModule).IsAssignableFrom(type)).First();

                this.m_container.RegisterSingleton(moduleType);

                var appModule = (IAppModule)this.m_container.Resolve(moduleType);

                this.LoadResources(appModule);
                appModule.OnInitialized(this.m_container);

                return appModule;
            }
            this.RemoveAppModule(builder.Description.Id);
            this.m_modules.Add(builder.Description.Id, new AppModuleInfo(builder.Description, fun)
            {
                RootDir = builder.RootDir
            });
        }

        public bool Contains(string id)
        {
            return this.m_modules.ContainsKey(id);
        }

        public AppModuleInfo GetAppModuleInfo(string id)
        {
            if (this.m_modules.TryGetValue(id, out var appModule))
            {
                return appModule;
            }
            return default;
        }

        public IEnumerable<AppModuleInfo> GetAppModules()
        {
            return this.m_modules.Values;
        }

        public bool RemoveAppModule(string id)
        {
            if (this.m_modules.TryGetValue(id, out var appModule))
            {
                if (appModule.Loaded)
                {
                    appModule.GetApp().SafeDispose();
                }

                return this.m_modules.Remove(id);
            }
            return false;
        }

        public bool TryGetAppModuleInfo(string id, out AppModuleInfo appModuleInfo)
        {
            return this.m_modules.TryGetValue(id, out appModuleInfo);
        }

        public int UpdateLocalAppModules()
        {
            lock (this.m_locker)
            {
                var count = 0;
                foreach (var item in Directory.GetFiles(ConstUtility.Path_Dir_Plugins, "Description.xml", SearchOption.AllDirectories))
                {
                    var descriptionBuilder = ModuleDescriptionBuilder.CreateByFile(item);
                    if (this.TryGetAppModuleInfo(descriptionBuilder.Description.Id, out var appModuleInfo))
                    {
                        continue;
                    }
                    this.Add(descriptionBuilder);
                    count++;
                }
                return count;
            }
        }
    }
}