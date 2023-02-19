namespace TrackerApi.DatabaseHelper
{
    public class SqlTokenModel
    {
        public string? Token { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
    }
}
