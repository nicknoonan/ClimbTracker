using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace DatabaseHelper
{
    public class DatabaseHelper
    {
        public static async Task<int> ExecuteNonQuery(IConfiguration config, ILogger log, string query, Dictionary<string, string> query_params)
        {
            var connection_string = config.GetValue<string>("CTDBConnectionString");
            using (SqlConnection connection = new SqlConnection(connection_string))
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