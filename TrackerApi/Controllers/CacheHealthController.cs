using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using TrackerApi.CacheHelper;

namespace TrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheHealthController : ControllerBase
    {
        private readonly ILogger<CacheHealthController> _logger;
        private readonly IConfiguration _config;
        private readonly ICacheHelper _cacheHelper;
        public CacheHealthController(ILogger<CacheHealthController> logger, IConfiguration configuration, ICacheHelper cacheHelper)
        {
            _logger = logger;
            _config = configuration;
            _cacheHelper = cacheHelper;
        }
        [HttpGet(Name = "GetCacheHealth")]
        public async Task<List<string>> Get()
        {
            int count = 100;
            List<string> list = new List<string>();
            for (int i = 0; i < count; i++)
            {
                string key = String.Format("healthcheck{0}", i.ToString());
                string value = String.Format("healthcheck{0}",i.ToString());
                await _cacheHelper.SetAsync(key, Encoding.UTF8.GetBytes(value), new DateTimeOffset(DateTime.UtcNow.AddMinutes(1)));
                byte[]? fetched_value = await _cacheHelper.GetAsync(key);
                list.Add(Encoding.UTF8.GetString(fetched_value ?? new byte[]{}));
            }
            bool healthy = await _cacheHelper.CheckConnectionAsync();
            string health_message = healthy ? "healthy" : throw new Exception("unhealthy sql cache");
            return list;
        }
    }
}
