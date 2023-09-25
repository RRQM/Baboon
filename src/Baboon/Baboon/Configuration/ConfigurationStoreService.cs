using LiteDB;
using System;
using TouchSocket.Core;

namespace Baboon
{
    /// <summary>
    /// ConfigurationStoreService
    /// </summary>
    public class ConfigurationStoreService : IConfigurationStoreService
    {
        private readonly IConfigService m_config;
        private LiteDatabase m_litedb;

        /// <summary>
        /// ConfigurationStoreService
        /// </summary>
        /// <param name="config"></param>
        public ConfigurationStoreService(IConfigService config)
        {
            this.Reload();
            this.m_config = config;
        }

        /// <inheritdoc/>
        public T Get<T>(string key) where T : new()
        {
            var kv = this.m_litedb.GetCollection<KV>(nameof(ConfigurationStoreService)).FindById(key);
            if (kv == null)
            {
                return default;
            }

            if (kv.Value.IsNullOrEmpty())
            {
                return default;
            }

            return SerializeConvert.JsonDeserializeFromString<T>(kv.Value);
        }

        /// <inheritdoc/>
        public void Reload()
        {
            this.m_litedb.SafeDispose();
            this.m_litedb = new LiteDatabase(this.m_config.GetPathFileConfigurationDb());
        }

        /// <inheritdoc/>
        public void Set<T>(string key, T value)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var col = this.m_litedb.GetCollection<KV>(nameof(ConfigurationStoreService));
            var kv = col.FindById(key);
            if (kv == null)
            {
                col.Insert(new KV() { Key = key, Value = value == null ? null : value.ToJsonString() });
            }
            else
            {
                kv.Value = value == null ? null : value.ToJsonString();
                col.Update(kv);
            }
        }

        private class KV
        {
            [BsonId(false)]
            public string Key { get; set; }

            public string Value { get; set; }
        }
    }
}