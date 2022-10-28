using Microsoft.Extensions.DependencyInjection;
using BlUoW.Microsoft.Extensions.DependencyInjection.Di;
using BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Factories;
using BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Repositories;
using BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Services;

namespace BlUoW.Microsoft.Extensions.DependencyInjection.Tests;

public class DapperUowTestsDi : DiTestBase
{
    public DapperUowTestsDi()
        : base(AddServices)
    {
    }

    [Fact]
    public async Task SessionScooped_CheckSameId_SucessSameId()
    {
        await Task.CompletedTask;

        var serviceTest = GetSessionRepository();

        Assert.Equal(
            serviceTest.Table1Repository.idSession, 
            serviceTest.Table2Repository.idSession);
        
        Assert.Equal(
            serviceTest.Table1Repository.idSession, 
            serviceTest.UoW.IdUoW);
    }

    [Fact]
    public async Task SessionScooped_CheckSameIdWithTwoServices_FailedSameIdServices()
    {
        await Task.CompletedTask;

        var serviceTest1 = GetSessionRepository();
        var serviceTest2 = GetSessionRepository();

        var expected = serviceTest1.Table1Repository.idSession;
        Assert.Equal(
            expected, 
            serviceTest1.Table2Repository.idSession);
        
        Assert.Equal(
            expected, 
            serviceTest1.UoW.IdUoW);

        Assert.NotEqual(
            expected,
            serviceTest2.UoW.IdUoW);

        Assert.NotEqual(
            expected, 
            serviceTest2.Table2Repository.idSession);
    }

    [Fact]
    public async Task SessionScooped_CheckSameIdWithTwoServices_SucessSameTypeDifferenteServices()
    {
        await Task.CompletedTask;

        var serviceTest1 = GetSessionRepository();
        var serviceTest2 = GetSessionRepository();

        var expected1 = serviceTest1.Table1Repository.idSession;
        Assert.Equal(
            expected1, 
            serviceTest1.Table2Repository.idSession);
        
        Assert.Equal(
            expected1, 
            serviceTest1.UoW.IdUoW);

        var expected2 = serviceTest2.Table1Repository.idSession;
        Assert.Equal(
            expected2,
            serviceTest2.UoW.IdUoW);

        Assert.Equal(
            expected2, 
            serviceTest2.Table2Repository.idSession);
    }

    /// <summary>
    /// Gets a scope service
    /// </summary>
    /// <returns>new <see cref="ServiceTest"/></returns>
    private ServiceTest GetSessionRepository()
    {
        return ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<ServiceTest>();
    }

    /// <summary>
    /// Gets a scope service
    /// </summary>
    /// <returns>new <see cref="Table1ConnectionRepository"/></returns>
    private Table1ConnectionRepository GetConnectionTable1Repository()
    {
        return ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<Table1ConnectionRepository>();
    }

    /// <summary>
    /// Gets a scope service
    /// </summary>
    /// <returns>new <see cref="Table2ConnectionRepository"/></returns>
    private Table2ConnectionRepository GetConnectionTable2Repository()
    {
        return ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<Table2ConnectionRepository>();
    }

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