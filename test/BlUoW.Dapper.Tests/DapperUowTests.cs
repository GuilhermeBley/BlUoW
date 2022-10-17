using BlUoW.Dapper.Tests.Factories;
using BlUoW.Dapper.Tests.Repositories;
using BlUoW.Factories;
using BlUoW.Session;
using Microsoft.Extensions.DependencyInjection;

namespace BlUoW.Dapper.Tests;

//
// Tests sintaxe: MethodName_ExpectedBehavior_StateUnderTest
// Example: isAdult_AgeLessThan18_False
//

public class DapperUowTests : DiTestBase
{
    private readonly SessionTestRepository _testRepository;
    public DapperUowTests() : base(AddServices)
    {
        _testRepository 
            = ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<SessionTestRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services
            .AddSingleton<IConnectionFactory, ConnectionFactory>()
            .AddScoped<DbSession>()
            .AddScoped<IDbSession>(x => x.GetRequiredService<DbSession>())
            .AddScoped<IUnitOfWork>(x => x.GetRequiredService<DbSession>());
    }

}