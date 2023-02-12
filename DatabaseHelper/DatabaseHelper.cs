using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace DatabaseHelper
{
    public class DatabaseHelper
    {
        public static async Task<int> ExecuteNonQuery(ILogger log, string query, Dictionary<string, string> query_params)
        {
            using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (query_params != null)
                {
                    foreach (var q_param in query_params)
                    {
                        command.Parameters.AddWithValue(q_param.Key, q_param.Value);
                    }
                }
                int command_return = await command.ExecuteNonQueryAsync();
                return command_return;
            }
        }
    }
}