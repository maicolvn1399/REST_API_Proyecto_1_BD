using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace REST_API_GymTEC.Database_Resources
{
    public class ConnectionStringManager
    {

        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();
            var connection_string_db = _configuration.GetConnectionString("Db_Project");
            return connection_string_db;
        }

    }
}
