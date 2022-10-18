using BlUoW.Dapper.Tests.Factories;
using BlUoW.Dapper.Tests.Model;
using BlUoW.Dapper.Tests.Repositories;
using BlUoW.Dapper.Tests.Services;
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
    private readonly TestRepository _testRepository;
    public DapperUowTests() : base(AddServices)
    {
        _testRepository 
            = ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<TestRepository>();
    }

    [Fact]
    public async Task AddAndRemove_WithOpenConnection_SuccessAddAndRemove()
    {
        var service = GetSessionestRepository();
        var repository = service.SessionTestRepository;
        var uoW = service.UoW;

        using (uoW.OpenConnectionAsync())
        {
            Assert.Equal(1, await repository.InsertTable1(
                new Table1
                {
                    Execution = uoW.IdUoW
                }
            ));
        }
    }

    /// <summary>
    /// Gets a scope service
    /// </summary>
    /// <returns>new <see cref="ServiceTest"/></returns>
    private ServiceTest GetSessionestRepository()
    {
        return ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<ServiceTest>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services
            .AddSingleton<IConnectionFactory, ConnectionFactory>()
            .AddScoped<DbSession>()
            .AddScoped<IDbSession>(x => x.GetRequiredService<DbSession>())
            .AddScoped<IUnitOfWork>(x => x.GetRequiredService<DbSession>())
            
            .AddScoped(typeof(Table2Repository))
            .AddScoped(typeof(TestRepository))
            
            .AddScoped(typeof(ServiceTest));
    }

}