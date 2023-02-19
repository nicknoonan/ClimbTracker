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
        public async Task<string> Get()
        {
            bool healthy = await _cacheHelper.CheckConnectionAsync();
            string health_message = healthy ? "healthy" : "unhealthy";
            return health_message;
        }
    }
}
