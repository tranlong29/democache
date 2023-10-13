using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SiUtils
{
    public sealed class CacheDuLieu
    {

        private static readonly Lazy<CacheDuLieu> instance = new Lazy<CacheDuLieu>(() => new CacheDuLieu());
        private ConnectionMultiplexer __ConnectionMultiplexer;
        private IDatabase _IDatabase;
        private CacheDuLieu()
        {
            // Khởi tạo các giá trị cần thiết trong constructor
            // tạo và quản lý kết nối tới redis cache
            __ConnectionMultiplexer = ConnectionMultiplexer.Connect("localhost,allowAdmin=true");
            //__ConnectionMultiplexer = ConnectionMultiplexer.Connect("10.10.20.46,password=simax,abortConnect=false,allowAdmin=true,connectTimeout=1000,responseTimeout=1000");
        }

        public static CacheDuLieu Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public ConnectionMultiplexer ConnectionMultiplexer
        {
            get
            {
                return __ConnectionMultiplexer;
            }
        }

        public IDatabase Database
        {
            get
            {
                if (_IDatabase == null)
                {
                    _IDatabase = ConnectionMultiplexer.GetDatabase();
                }
                return _IDatabase;
            }
        }

        public string GenerateKey(object input, bool GenMD5 = true)
        {
            if (!GenMD5)
                return JsonConvert.SerializeObject(input);
            else
            {
                string str = JsonConvert.SerializeObject(input);
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

                StringBuilder sbHash = new StringBuilder();

                foreach (byte b in bHash)
                {
                    sbHash.Append(String.Format("{0:x2}", b));
                }
                return sbHash.ToString();
            }
        }

        public string AllKeyAvalible()
        {
            string keyAvali = "";
            var endpoints = ConnectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = ConnectionMultiplexer.GetServer(endpoint);

                foreach (var RedisKey in server.Keys())
                {
                    keyAvali += $"<RedisKey>{RedisKey.ToString()}</RedisKey>";
                }
            }
            return keyAvali;
        }


        public bool IsKeyExists(string key)
        {
            var keyvalue = key;
            return Database.KeyExists(keyvalue);
        }

        public void SetString(string key, string value, int MinExpiress = 30)
        {
            var keyvalue = key;
            TimeSpan expiryTimeSpan = TimeSpan.FromMinutes(MinExpiress);
            Database.StringSet(keyvalue, value, expiryTimeSpan);
        }

        public void SetInts(string key, int value, int MinExpiress = 30)
        {
            var keyvalue = key;
            TimeSpan expiryTimeSpan = TimeSpan.FromMinutes(MinExpiress);
            Database.StringSet(keyvalue, value, expiryTimeSpan);
        }

        public int GetInts(string key)
        {
            var keyvalue = key;
            RedisValue _value = Database.StringGet(keyvalue);
            if (_value.HasValue)
            {
                return (int)_value;
            }
            else
            {
                return 0;
            }
        }

        public void IncreaseInts(string key, int value)
        {
            var keyvalue = key;
            RedisValue _value = Database.StringGet(keyvalue);
            TimeSpan expiryTimeSpan = TimeSpan.FromMinutes(30);
            Database.StringSet(key, (int)_value + value, expiryTimeSpan);
        }

        public void DecreaseInts(string key, int value)
        {
            var keyvalue = key;
            RedisValue _value = Database.StringGet(keyvalue);
            TimeSpan expiryTimeSpan = TimeSpan.FromMinutes(30);
            Database.StringSet(key, (int)_value - value, expiryTimeSpan);
        }

        public string GetString(string key, bool checkCache = true)
        {


            RedisValue _value = Database.StringGet(key);
            if (_value.HasValue)
            {
                return _value;
            }
            else
            {
                return null;
            }
        }

        public void SetObj(string key, object value, int MinExpiress = 30)
        {
            var keyvalue = key;
            TimeSpan expiryTimeSpan = TimeSpan.FromMinutes(MinExpiress);
            string json = JsonConvert.SerializeObject(value, Formatting.Indented);
            Database.StringSet(keyvalue, json, expiryTimeSpan);
        }

        public T GetObj<T>(string key) where T : new()
        {
            RedisValue _value = Database.StringGet(key);
            if (_value.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(_value);
            }
            else
            {
                return new T();
            }
        }

        public void RemoveKey(params string[] Keys)
        {
            foreach (var _key in Keys)
            {
                Database.KeyDelete(_key, CommandFlags.HighPriority);
            }
        }

        public void Clear()
        {
            var endpoints = ConnectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = ConnectionMultiplexer.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

        public void ClearAdmin()
        {
            var endpoints = ConnectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = ConnectionMultiplexer.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }
        public void RemoveKeys(string key)
        {
            string prefixToRemove = key;
            List<string> cacheKeys = AllKeyAvalible().Split(new string[] { "<RedisKey>" }, StringSplitOptions.None).ToList();


            foreach (var cacheKey in cacheKeys)
            {
                string cleanedCacheKey = cacheKey.Replace("</RedisKey>", "");
                if (cacheKey.StartsWith(prefixToRemove))
                {
                    RemoveKey(cleanedCacheKey);
                }
            }
        }
    }
}
