using Azure.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace TrackerApi.DatabaseHelper
{


    public class SqlToken : ISqlToken
    {
        private readonly IMemoryCache cache;
        private static readonly string token_cache_key = "SQL_ACCESS_TOKEN";
        private readonly IConfiguration configuration;
        public SqlToken(IMemoryCache cache, IConfiguration configuration)
        {
            this.cache = cache;
            this.configuration = configuration;
        }
        public string GetConnectionString()
        {
            string connection_string = string.Format("{0};Access Token={1};", configuration.GetValue<string>("CTDBConnectionString"), GetToken());
            return connection_string;
        }
        public string GetToken()
        {
            if (!cache.TryGetValue(token_cache_key, out SqlTokenModel token))
            {
                token = GetNewToken();
                var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(token.ExpiresOn);
                cache.Set(token_cache_key, token, options);
            }
            return token?.Token ?? "unable to fetch sql token";
        }
        private static SqlTokenModel GetNewToken()
        {
            SqlTokenModel sqlToken = new SqlTokenModel();
            var credential = new DefaultAzureCredential();
            var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));
            sqlToken.Token = token.Token.ToString();
            sqlToken.ExpiresOn = token.ExpiresOn;
            return sqlToken;
        }
    }
}
