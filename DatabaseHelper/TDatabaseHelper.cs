using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DatabaseHelper
{
    public class TDatabaseHelper<T>
    {
        public static async Task<T?> ExecuteQuery(ILogger log, string query, Dictionary<string, string> query_params, Func<SqlDataReader, T> reader_handler)
        {
            var connection_string = Environment.GetEnvironmentVariable("OPENAISBX_DB_CONNECTION_STRING");
            /*log.LogInformation(String.Format("connecting to \"{0}\"", connection_string));*/
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
                SqlDataReader reader = await command.ExecuteReaderAsync();
                var handler_return = reader_handler(reader);
                return handler_return;
            }
        }
    }
}
