using Microsoft.Data.SqlClient;

namespace TrackerApi.DatabaseHelper
{
    public interface IDatabaseHelper
    {
        Task<TQueryResult> ExecuteQuery(string query, Dictionary<string, object>? query_params, Func<SqlDataReader, TQueryResult> reader_handler);
        Task<int> ExecuteNonQuery(string query, Dictionary<string, object> query_params);
        Task<string> CheckConnectionAsync();
    }
}
