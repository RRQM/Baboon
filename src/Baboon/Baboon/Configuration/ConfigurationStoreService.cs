using LiteDB;
using System;
using TouchSocket.Core;

namespace Baboon
{
    public class ConfigurationStoreService : IConfigurationStoreService
    {
        private readonly IConfigService m_config;
        private LiteDatabase m_litedb;

        public ConfigurationStoreService(IConfigService config)
        {
            this.Reload();
            this.m_config = config;
        }

        public T Get<T>(string key)
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

        public void Reload()
        {
            this.m_litedb.SafeDispose();
            this.m_litedb = new LiteDatabase(m_config.GetPathFileConfigurationDb());
        }

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