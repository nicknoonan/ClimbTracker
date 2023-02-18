using Azure.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace TrackerApi
{
    public interface ISqlToken
    {
        string GetToken();
    }
    public class SqlTokenModel
    {
        public string? Token { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
    }
    public class SqlToken : ISqlToken
    {
        private readonly IMemoryCache cache;
        private readonly string token_cache_key = "SQL_ACCESS_TOKEN";
        public SqlToken(IMemoryCache cache)
        {
            this.cache = cache;
        }
        public string GetToken()
        {
            SqlTokenModel token;

            if (!cache.TryGetValue(token_cache_key, out token))
            {
                token = GetNewToken();
                var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(token.ExpiresOn);
                cache.Set(token_cache_key, token, options);
            }
            return token.Token ?? "unable to fetch sql token";
        }
        private static SqlTokenModel GetNewToken()
        {
            SqlTokenModel sqlToken = new SqlTokenModel();
            var credential = new Azure.Identity.DefaultAzureCredential();
            var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));
            sqlToken.Token = token.Token.ToString();
            sqlToken.ExpiresOn = token.ExpiresOn;
            return sqlToken;
        }
    }
}
