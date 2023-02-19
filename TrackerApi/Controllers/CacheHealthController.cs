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
        public async Task<List<string>> Get(int count)
        {
            List<string> list = new List<string>();
            bool healthy = await _cacheHelper.CheckConnectionAsync();
            string health_message = healthy ? "healthy" : "unhealthy";
            list.Add(health_message);
            count = (count <= 1000) ? count : 1000;
            for (int i = 0; i < count; i++)
            {
                string key = String.Format("healthcheck{0}", i.ToString());
                string value = String.Format("healthcheck{0}",i.ToString());
                await _cacheHelper.SetAsync(key, Encoding.UTF8.GetBytes(value));
                byte[]? fetched_value = await _cacheHelper.GetAsync(key);
                string check = (fetched_value == null) ? String.Format("unhealthy{0}",i.ToString()) : Encoding.UTF8.GetString(fetched_value);
                list.Add(check);
            }
            return list;
        }
    }
}
