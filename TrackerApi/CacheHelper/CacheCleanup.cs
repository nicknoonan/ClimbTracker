using Microsoft.AspNetCore.Mvc;
using System.Text;
using TrackerApi.DatabaseHelper;

namespace TrackerApi.CacheHelper
{
    public class CacheCleanup : ICacheCleanup, IHostedService//, IAsyncDisposable
    {
        private readonly Task _completedTask = Task.CompletedTask;
        private readonly ILogger<CacheCleanup> _logger;
        private readonly IConfiguration _config;
        private readonly ICacheHelper _cache;
        private readonly int _delay_ms = 60000;
        private Timer? _timer;
        public CacheCleanup(ICacheHelper cache, IConfiguration config, ILogger<CacheCleanup> logger) 
        {
            _config = config;
            _logger = logger;
            _cache = cache;
        }
        public async void CleanUp(object? state)
        {
            _logger.LogInformation("running cache clean up...");
            await _cache.RemoveExpiredAsync();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cache clean up service started...");
            _timer = new Timer(CleanUp, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(_delay_ms));
            return _completedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cache clean up service stopped.");
            _timer?.Change(Timeout.Infinite, 0);
            return _completedTask;
        }
        public async ValueTask DisposeAsync()
        {
            if (_timer is IAsyncDisposable timer)
            {
                await timer.DisposeAsync();
            }
            _timer = null;
        }
    }
}
