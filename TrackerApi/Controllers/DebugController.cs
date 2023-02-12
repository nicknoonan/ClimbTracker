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
            var test = _config.GetValue<string>("testsecret");
            return String.Format("config value \"{0}\"",test);
        }
    }
}


