using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Core;

namespace TrackerApi
{
    public static class TDatabaseHelper<T>
    {
        public static async Task<T?> ExecuteQuery(IConfiguration config, ILogger log, ISqlToken sqlTokenService, string query, Dictionary<string, string>? query_params, Func<SqlDataReader, T> reader_handler)
        {
            var connection_string = config.GetValue<string>("CTDBConnectionString");
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.AccessToken = sqlTokenService.GetToken();
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
