using Microsoft.Data.SqlClient;
using System.Data;

namespace JTM.Data.DapperConnection
{
    public class DapperConnectionFactory : IDapperConnectionFactory
    {
        private readonly string _connectionString;

        public DapperConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnectionString");
        }

        public IDbConnection DbConnection => new SqlConnection(_connectionString);
    }
}
