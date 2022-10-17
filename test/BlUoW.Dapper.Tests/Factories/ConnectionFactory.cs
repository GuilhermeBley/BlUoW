using System.Data.Common;
using BlUoW.Factories;
using Microsoft.Extensions.Configuration;

namespace BlUoW.Dapper.Tests.Factories;

public class ConnectionFactory : IConnectionFactory
{
    private const string ConnectionStringName = "Test";
    private readonly IConfiguration _configuration;
    public ConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public DbConnection GetNewConnection()
    {
        return 
            new MySql.Data.MySqlClient.MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName));
    }
}
