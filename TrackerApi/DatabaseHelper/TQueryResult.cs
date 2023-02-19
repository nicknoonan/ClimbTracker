namespace TrackerApi.DatabaseHelper
{
    public class TQueryResult
    {
        public object? result { get; set; }
        public TQueryResult(object? result)
        {
            this.result = result;
        }
    }
}
