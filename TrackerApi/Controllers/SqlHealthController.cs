using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;

namespace TrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SqlHealthController : ControllerBase
    {
        private readonly ILogger<SqlHealthController> _logger;
        private readonly IConfiguration _config;
        private readonly ISqlToken _sqlTokenService;
        private readonly IDatabaseHelper _databaseHelper;

        public SqlHealthController(IDatabaseHelper databaseHelper, ILogger<SqlHealthController> logger, IConfiguration config, ISqlToken sqlTokenService)
        {
            _databaseHelper = databaseHelper;
            _logger = logger;
            _config = config;
            _sqlTokenService = sqlTokenService;
        }

        [HttpGet(Name = "GetHealth")]
        public async Task<string> Get()
        {
            string version = await _databaseHelper.CheckConnection();
            return version;
        }
    }
}
