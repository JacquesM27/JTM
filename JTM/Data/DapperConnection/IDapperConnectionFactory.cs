using System.Data;

namespace JTM.Data.DapperConnection
{
    public interface IDapperConnectionFactory
    {
        public IDbConnection DbConnection { get; }
    }
}
