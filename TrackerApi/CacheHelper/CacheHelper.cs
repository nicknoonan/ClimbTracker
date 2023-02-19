using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using TrackerApi.DatabaseHelper;

namespace TrackerApi.CacheHelper
{


    public class CacheHelper : ICacheHelper
    {
        private readonly ILogger<CacheHelper> _logger;
        private readonly IConfiguration _config;
        private readonly IDatabaseHelper _db;
        private static readonly double _deafualt_retention_hours = 12;
        public CacheHelper(ILogger<CacheHelper> logger, IConfiguration configuration, IDatabaseHelper db)
        {
            _logger = logger;
            _config = configuration;
            _db = db;
        }
        public async Task<bool> CheckConnectionAsync()
        {
            byte[]? tokenBytes = await GetAsync("healtcheck");
            if(tokenBytes == null)
            {
                byte[] newTokenBytes = Encoding.UTF8.GetBytes("healthcheck");
                DateTimeOffset expires_at_time = new DateTimeOffset(DateTime.UtcNow).AddSeconds(60);
                await SetAsync("healthcheck", newTokenBytes, expires_at_time);
                tokenBytes = await GetAsync("healthcheck");
            }
            if (tokenBytes != null)
            {
                string token = Encoding.UTF8.GetString(tokenBytes);
                return (token.Equals("healthcheck"));
            }
            else { return false; }
            
        }
        public async Task<byte[]?> GetAsync(string key, CancellationToken token = default(CancellationToken))
        {
            string query = @"
                select Id, cast(Value as varchar) as Value, ExpiresAtTime from cache_store
                where ExpiresAtTime > GETDATE() and Id = @key";

            Dictionary<string, object> query_params = new Dictionary<string, object>(){{ "key", key }};

            TQueryResult queryResult = await _db.ExecuteQuery(query, query_params, (reader) =>
            {
                byte[] tokenBytes = null;
                while (reader.Read())
                {
                    tokenBytes = Encoding.UTF8.GetBytes((string)reader["value"]);
                }
                return new TQueryResult(tokenBytes);
            });

            return (queryResult.result != null) ? (byte[])queryResult.result : null;
        }
        public async Task SetAsync(string key, byte[] value, DateTimeOffset expires_at_time, CancellationToken token = default(CancellationToken))
        {
            string query = @"
                if exists (select Id from cache_store where Id = @key)
	                update cache_store
	                set Value = @value, ExpiresAtTime = @expires_at_time
	                where Id = @key
                else
	                insert into cache_store (Id, Value, ExpiresAtTime)
	                values (@key,@value,@expires_at_time)";
            
            Dictionary<string, object> query_params = new Dictionary<string, object>() {
                {"key" , key},
                {"value", value},
                {"expires_at_time",expires_at_time }
            };

            await _db.ExecuteNonQuery(query, query_params);
        }
        public async Task SetAsync(string key, byte[] value, CancellationToken token = default(CancellationToken))
        {
            DateTimeOffset expires_at_time = new DateTimeOffset(DateTime.UtcNow.AddHours(_deafualt_retention_hours));
            await this.SetAsync(key, value, expires_at_time, token);
        }
        public async Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
        {
            string query = "delete from cache_store where Id = @key";
            Dictionary<string, object> query_params = new Dictionary<string, object>(){{ "key", key }};
            await _db.ExecuteNonQuery(query, query_params);
        }
        public async Task RemoveExpiredAsync(CancellationToken token = default(CancellationToken))
        {
            string query = "delete from cache_store where ExpiresAtTime < GETDATE()";
            await _db.ExecuteNonQuery(query, null);
        }
    }
}
