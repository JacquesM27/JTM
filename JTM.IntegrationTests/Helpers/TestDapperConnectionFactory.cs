using JTM.Data.DapperConnection;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JTM.IntegrationTests.Helpers
{
    public class TestDapperConnectionFactory : IDapperConnectionFactory
    {
        private readonly string _connectionString;

        public TestDapperConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TestConnectionString");
        }

        public IDbConnection DbConnection => new SqlConnection(_connectionString);
    }
}
