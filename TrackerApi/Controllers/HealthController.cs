using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;

namespace TrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IConfiguration _config;
        private readonly ISqlToken _sqlTokenService;

        public HealthController(ILogger<HealthController> logger, IConfiguration config, ISqlToken sqlTokenService)
        {
            _logger = logger;
            _config = config;
            _sqlTokenService = sqlTokenService;
        }

        [HttpGet(Name = "GetHealth")]
        public async Task<string> Get()
        {
            var query = "select @@version as version";
            var version = await TDatabaseHelper<string?>.ExecuteQuery(_config, _logger, _sqlTokenService, query, null, (reader) => {
                var sqlversion = "";
                while (reader.Read())
                {
                   sqlversion = reader["version"].ToString();
                }
                return sqlversion;
            });
            return version ?? "unable to get sql version";
        }
    }
}
