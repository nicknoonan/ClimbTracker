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
            var kv_name = _config.GetValue<string>("AZURE_KEY_VAULT_NAME");
            var Kv_prefix = _config.GetValue<string>("AZURE_KEY_VAULT_SECRET_PREFIX");
            var test = _config.GetValue<string>("testsecret");
            return String.Format("AZURE_KEY_VAULT_NAME: \"{0}\"\ntestsecret: \"{1}\"", kv_name, test);
        }
    }
}


