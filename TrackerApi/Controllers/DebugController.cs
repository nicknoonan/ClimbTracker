using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly ILogger<DebugController> _logger;
        private readonly IConfiguration _config;

        public DebugController(ILogger<DebugController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet(Name = "GetDebug")]
        public string Get()
        {
            var str = String.Format("db connectrion string : \"{0}\"", _config.GetValue<string>("CTDBConnectionString"));
            return str;
        }
    }
}


