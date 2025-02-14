using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TouchSocket.Core;

namespace Baboon
{
    /// <summary>
    /// ModuleCatalog
    /// </summary>
    public class ModuleCatalog : IModuleCatalog
    {
        private readonly List<Type> appModuleTypes = new List<Type>();
        private readonly object m_locker = new object();
        private readonly Dictionary<string, IAppModule> m_modules = new Dictionary<string, IAppModule>();
        private volatile bool isReadonly;

        /// <inheritdoc/>
        public bool IsReadonly => isReadonly;

        /// <inheritdoc/>
        public string ModulesDirPath { get; set; } = Path.GetFullPath("Modules");

        /// <inheritdoc/>
        public void Add<TAppModule>() where TAppModule : IAppModule, new()
        {
            Add(typeof(TAppModule));
        }

        /// <inheritdoc/>
        public void Add(Type moduleType)
        {
            lock (this.m_locker)
            {
                ThrowIfReadonly();
                if (!typeof(IAppModule).IsAssignableFrom(moduleType))
                {
                    throw new Exception($"模块类型必须继承{nameof(IAppModule)}");
                }

                if (appModuleTypes.Contains(moduleType))
                {
                    return;
                }

                foreach (var item in this.m_modules)
                {
                    if (item.Value.GetType() == moduleType)
                    {
                        return;
                    }
                }

                appModuleTypes.Add(moduleType);
            }

        }

        /// <inheritdoc/>
        public void Add(IAppModule appModule)
        {
            lock (this.m_locker)
            {
                ThrowIfReadonly();
                if (appModule is null)
                {
                    throw new ArgumentNullException(nameof(appModule));
                }

                m_modules.Remove(appModule.Description.Id);

                this.m_modules.Add(appModule.Description.Id, appModule);
            }

        }

        /// <inheritdoc/>
        public void Build()
        {
            lock (this.m_locker)
            {
                this.ThrowIfReadonly();

                if (this.ModulesDirPath.HasValue() && Directory.Exists(this.ModulesDirPath))
                {
                    foreach (var path in Directory.GetFiles(this.ModulesDirPath, "*.dll", SearchOption.AllDirectories))
                    {
                        var assembly = Assembly.LoadFrom(Path.GetFullPath(path));

                        var moduleTypes = assembly.ExportedTypes.Where(type => typeof(IAppModule).IsAssignableFrom(type));

                        foreach (var moduleType in moduleTypes)
                        {
                            this.Add(moduleType);
                        }
                    }
                }

                foreach (var moduleType in this.appModuleTypes)
                {
                    var module = (IAppModule)Activator.CreateInstance(moduleType);
                    this.Add(module);
                }

                MakeReadonly();
            }

        }

        /// <inheritdoc/>
        public bool Contains(string id)
        {
            lock (this.m_locker)
            {
                return this.m_modules.ContainsKey(id);
            }

        }

        /// <inheritdoc/>
        public IAppModule GetAppModule(string id)
        {
            lock (this.m_locker)
            {
                if (this.m_modules.TryGetValue(id, out var appModule))
                {
                    return appModule;
                }

                return default;
            }

        }

        /// <inheritdoc/>
        public IEnumerable<IAppModule> GetAppModules()
        {
            return this.m_modules.Values;
        }

        /// <inheritdoc/>
        public void MakeReadonly()
        {
            this.isReadonly = true;
        }

        private void ThrowIfReadonly()
        {
            if (this.isReadonly)
            {
                throw new Exception("The module catalog has been built, so it is not allowed to modify the collection content anymore.");
            }
        }

        //private void LoadResources(IAppModule appModule)
        //{
        //    if (appModule.Resources != null)
        //    {
        //        this.m_application.Resources.MergedDictionaries.Add(appModule.Resources);
        //    }
        //}
    }
}