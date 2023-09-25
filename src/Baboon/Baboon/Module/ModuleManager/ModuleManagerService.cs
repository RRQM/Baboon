using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

namespace Baboon
{
    internal class ModuleManagerService : IModuleManagerService
    {
        private readonly IConfigService m_configService;
        private readonly IConfigurationStoreService m_configurationStoreService;
        private readonly string m_installToDoListKey = $"{nameof(ModuleManagerService)}-InstallToDoList";
        private readonly IModuleCatalog m_moduleCatalog;
        private readonly string m_uninstallToDoListKey = $"{nameof(ModuleManagerService)}-UninstallToDoList";

        public ModuleManagerService(IModuleCatalog moduleCatalog, IConfigService configService, IConfigurationStoreService configurationStoreService)
        {
            this.m_moduleCatalog = moduleCatalog;
            this.m_configService = configService;
            this.m_configurationStoreService = configurationStoreService;
        }

        public void Install(Stream stream)
        {
            using (var zipArchive = new ZipArchive(stream))
            {
                var zipArchiveEntry = zipArchive.GetEntry("Description.xml");
                if (zipArchiveEntry is null)
                {
                    throw new Exception("没有找到相关描述文件。");
                }
                ModuleDescription description;
                using (var descriptionStream = zipArchiveEntry.Open())
                {
                    description = ModuleDescription.CreateByDescriptionStream(descriptionStream);
                }

                if (this.m_moduleCatalog.TryGetAppModuleInfo(description.Id, out var appModuleInfo))
                {
                    if (appModuleInfo.Loaded)
                    {
                        throw new Exception("程序已被加载");
                    }
                }
                ZipArchive(zipArchive, Path.Combine(this.m_configService.GetPathDirModules(), description.Id));
            }
        }

        public void Uninstall(string id)
        {
            if (this.m_moduleCatalog.TryGetAppModuleInfo(id, out var appModuleInfo))
            {
                if (appModuleInfo.Loaded)
                {
                    throw new Exception("程序已被加载，无法执行卸载任务。");
                }
            }
            var path = Path.Combine(this.m_configService.GetPathDirModules(), id);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            this.m_moduleCatalog.RemoveAppModule(id);
        }

        private static void ZipArchive(ZipArchive zipArchive, string basePath)
        {
            foreach (var zipArchiveEntry in zipArchive.Entries)
            {
                if (!zipArchiveEntry.FullName.EndsWith("/"))
                {
                    var entryFilePath = Regex.Replace(zipArchiveEntry.FullName.Replace("/", @"\"),
                        @"^\\*", "");
                    var filePath = Path.Combine(basePath, entryFilePath); //设置解压路径

                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    }
                    zipArchiveEntry.ExtractToFile(filePath, true);
                }
            }
        }

        #region 卸载

        public void AddUninstallToDo(string id)
        {
            var list = this.m_configurationStoreService.Get<List<string>>(this.m_uninstallToDoListKey) ?? new List<string>();

            list.Add(id);
            list = list.Distinct().ToList();

            this.m_configurationStoreService.Set(this.m_uninstallToDoListKey, list);
        }

        public void ClearUninstallToDo()
        {
            this.m_configurationStoreService.Set(this.m_uninstallToDoListKey, new List<string>());
        }

        public List<string> GetUninstallToDoList()
        {
            return this.m_configurationStoreService.Get<List<string>>(this.m_uninstallToDoListKey) ?? new List<string>();
        }

        public void RemoveUninstallToDo(string id)
        {
            var list = this.m_configurationStoreService.Get<List<string>>(this.m_uninstallToDoListKey) ?? new List<string>();

            list.Remove(id);
            list = list.Distinct().ToList();

            this.m_configurationStoreService.Set(this.m_uninstallToDoListKey, list);
        }

        #endregion 卸载

        #region 安装

        public void AddInstallToDo(string filePath)
        {
            var list = this.m_configurationStoreService.Get<List<string>>(this.m_installToDoListKey) ?? new List<string>();

            list.Add(filePath);
            list = list.Distinct().ToList();

            this.m_configurationStoreService.Set(this.m_installToDoListKey, list);
        }

        public void ClearInstallToDo()
        {
            this.m_configurationStoreService.Set(this.m_installToDoListKey, new List<string>());
        }

        public List<string> GetInstallToDoList()
        {
            return this.m_configurationStoreService.Get<List<string>>(this.m_installToDoListKey) ?? new List<string>();
        }

        public void RemoveInstallToDo(string id)
        {
            var list = this.m_configurationStoreService.Get<List<string>>(this.m_installToDoListKey) ?? new List<string>();
            list.Remove(id);
            list = list.Distinct().ToList();

            this.m_configurationStoreService.Set(this.m_installToDoListKey, list);
        }

        #endregion 安装
    }
}