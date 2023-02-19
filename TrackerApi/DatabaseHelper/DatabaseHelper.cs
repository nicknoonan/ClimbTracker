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
using Microsoft.AspNetCore.Mvc;

namespace TrackerApi.DatabaseHelper
{


    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly IConfiguration _config;
        private readonly ISqlToken _sqlTokenService;

        public DatabaseHelper(IConfiguration config, ISqlToken sqlTokenService)
        {
            _config = config;
            _sqlTokenService = sqlTokenService;
        }
        public async Task<string> CheckConnectionAsync()
        {
            var connection_string = _config.GetValue<string>("CTDBConnectionString");
            var query = "select @@version as version";
            var version = await ExecuteQuery(query, null, (reader) =>
            {
                var sqlversion = "";
                while (reader.Read())
                {
                    sqlversion = reader["version"].ToString();
                }
                return new TQueryResult(sqlversion);
            });
            var version_result = version.result ?? "unable to get sql version";
            return (string)version_result;
        }

        public async Task<TQueryResult> ExecuteQuery(string query, Dictionary<string, object>? query_params, Func<SqlDataReader, TQueryResult> reader_handler)
        {
            var connection_string = _config.GetValue<string>("CTDBConnectionString");
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.AccessToken = _sqlTokenService.GetToken();
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
        public async Task<int> ExecuteNonQuery(string query, Dictionary<string, object> query_params)
        {
            var connection_string = _config.GetValue<string>("CTDBConnectionString");
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.AccessToken = _sqlTokenService.GetToken();
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
