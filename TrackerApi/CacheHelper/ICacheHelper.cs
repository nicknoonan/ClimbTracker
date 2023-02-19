namespace TrackerApi.CacheHelper
{
    public interface ICacheHelper
    {
        public Task<bool> CheckConnectionAsync();

        /*byte[]? Get(string key);*/


        Task<byte[]?> GetAsync(string key, CancellationToken token = default(CancellationToken));


        /*void Set(string key, byte[] value, DistributedCacheEntryOptions options);*/


        Task SetAsync(string key, byte[] value, DateTimeOffset expires_at_time, CancellationToken token = default(CancellationToken));
        Task SetAsync(string key, byte[] value, CancellationToken token = default(CancellationToken));

        /*void Refresh(string key);*/

        /*Task RefreshAsync(string key, CancellationToken token = default(CancellationToken));*/

        /*void Remove(string key);*/

        Task RemoveAsync(string key, CancellationToken token = default(CancellationToken));
        Task RemoveExpiredAsync(CancellationToken token= default(CancellationToken));
    }
}
