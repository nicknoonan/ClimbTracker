namespace TrackerApi.CacheHelper
{
    public class CacheItem
    {
        /*[Id] [nvarchar] (449) NOT NULL,
        [Value] [varbinary] (max) NOT NULL,
	    [ExpiresAtTime] [datetimeoffset] (7) NOT NULL*/

        public string Id { get; set; }
        public byte[] Value { get; set; }
        public DateTimeOffset ExpiresAtTime { get; set; }

        public CacheItem(string Id, byte[] Value, DateTimeOffset ExpiresAtTime)
        {
            this.Id = Id;
            this.Value = Value;
            this.ExpiresAtTime = ExpiresAtTime;
        }
    }
}
