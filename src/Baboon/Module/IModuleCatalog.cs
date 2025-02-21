using System;
using System.Collections.Generic;

namespace Baboon;

/// <summary>
/// 模块目录接口
/// </summary>
public interface IModuleCatalog
{
    /// <summary>
    /// 是否只读
    /// </summary>
    bool IsReadonly { get; }

    /// <summary>
    /// 模块目录路径
    /// </summary>
    string ModulesDirPath { get; set; }

    /// <summary>
    /// 以类型直接添加模块
    /// </summary>
    /// <param name="moduleType">模块类型</param>
    void Add(Type moduleType);

    /// <summary>
    /// 直接添加模块
    /// </summary>
    /// <param name="appModule">应用模块</param>
    void Add(IAppModule appModule);

    /// <summary>
    /// 泛型添加模块
    /// </summary>
    /// <typeparam name="TAppModule">应用模块类型</typeparam>
    void Add<TAppModule>() where TAppModule : IAppModule, new();

    /// <summary>
    /// 构建模块
    /// </summary>
    void Build();

    /// <summary>
    /// 是否包含指定Id的模块
    /// </summary>
    /// <param name="id">模块Id</param>
    /// <returns>是否包含</returns>
    bool Contains(string id);

    /// <summary>
    /// 获取模块信息
    /// </summary>
    /// <param name="id">模块Id</param>
    /// <returns>应用模块</returns>
    IAppModule GetAppModule(string id);

    /// <summary>
    /// 获取当前目录下的所有模块
    /// </summary>
    /// <returns>应用模块集合</returns>
    IEnumerable<IAppModule> GetAppModules();
}
