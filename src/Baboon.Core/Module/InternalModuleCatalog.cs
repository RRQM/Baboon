// ------------------------------------------------------------------------------
// 此代码版权（除特别声明或在XREF结尾的命名空间的代码）归作者本人若汝棋茗所有
// 源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
// CSDN博客：https://blog.csdn.net/qq_40374647
// 哔哩哔哩视频：https://space.bilibili.com/94253567
// Gitee源代码仓库：https://gitee.com/RRQM_Home
// Github源代码仓库：https://github.com/RRQM
// API首页：https://touchsocket.net/
// 交流QQ群：234762506
// 感谢您的下载和使用
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TouchSocket.Core;

namespace Baboon.Core;

internal class InternalModuleCatalog : IModuleCatalog
{
    public InternalModuleCatalog(Func<string, bool> findModuleFunc)
    {
        this.findModuleFunc = findModuleFunc; 1
    }
    private readonly List<Type> appModuleTypes = new List<Type>();
    private readonly object m_locker = new object();
    private readonly Dictionary<string, IAppModule> m_modules = new Dictionary<string, IAppModule>();
    private volatile bool isReadonly;
    private readonly Func<string, bool> findModuleFunc;

    /// <inheritdoc/>
    public bool IsReadonly => this.isReadonly;

    /// <inheritdoc/>
    public string ModulesDirPath { get; set; } = Path.GetFullPath("Modules");

    /// <inheritdoc/>
    public void Add<TAppModule>() where TAppModule : IAppModule, new()
    {
        this.Add(typeof(TAppModule));
    }

    /// <inheritdoc/>
    public void Add(Type moduleType)
    {
        lock (this.m_locker)
        {
            this.ThrowIfReadonly();
            if (!typeof(IAppModule).IsAssignableFrom(moduleType))
            {
                throw new Exception($"模块类型必须继承{nameof(IAppModule)}");
            }

            if (this.appModuleTypes.Contains(moduleType))
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

            this.appModuleTypes.Add(moduleType);
        }

    }

    /// <inheritdoc/>
    public void Add(IAppModule appModule)
    {
        lock (this.m_locker)
        {
            this.ThrowIfReadonly();
            if (appModule is null)
            {
                throw new ArgumentNullException(nameof(appModule));
            }

            this.m_modules.Remove(appModule.Description.Id);

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
                    if (!this.findModuleFunc.Invoke(path))
                    {
                        continue;
                    }
                    var assembly = Assembly.LoadFrom(Path.GetFullPath(path));

                    var moduleTypes = assembly.ExportedTypes;

                    foreach (var moduleType in moduleTypes)
                    {
                        if (moduleType.IsAbstract || moduleType.IsInterface || moduleType.IsNotPublic)
                        {
                            continue;
                        }
                        if (typeof(IAppModule).IsAssignableFrom(moduleType))
                        {
                            this.Add(moduleType);
                        }
                    }
                }
            }

            foreach (var moduleType in this.appModuleTypes)
            {
                var module = (IAppModule)Activator.CreateInstance(moduleType);
                this.Add(module);
            }

            this.MakeReadonly();
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