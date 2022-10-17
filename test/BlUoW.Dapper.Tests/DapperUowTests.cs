using BlUoW.Dapper.Tests.Factories;
using BlUoW.Factories;
using BlUoW.Session;
using Microsoft.Extensions.DependencyInjection;

namespace BlUoW.Dapper.Tests;

public class DapperUowTests : DiTestBase
{
    public DapperUowTests() : base(AddServices)
    { }

    private static void AddServices(IServiceCollection services)
    {
        services
            .AddSingleton<IConnectionFactory, ConnectionFactory>()
            .AddScoped<DbSession>()
            .AddScoped<IDbSession>(x => x.GetRequiredService<DbSession>())
            .AddScoped<IUnitOfWork>(x => x.GetRequiredService<DbSession>());
    }
}