using Microsoft.Extensions.DependencyInjection;
using BlUoW.Microsoft.Extensions.DependencyInjection.Di;
using BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Factories;
using BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Repositories;
using BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Services;

namespace BlUoW.Microsoft.Extensions.DependencyInjection.Tests;

public class DapperUowTestsDi : DiTestBase
{
    

    private static void AddServices(IServiceCollection services)
    {
        services
            .AddUnitOfWork<ConnectionFactory>()
            
            .AddScoped(typeof(Table2Repository))
            .AddScoped(typeof(Table1Repository))
            .AddScoped(typeof(Table1ConnectionRepository))
            .AddScoped(typeof(Table2ConnectionRepository))
            
            .AddScoped(typeof(ServiceTest));
    }
}