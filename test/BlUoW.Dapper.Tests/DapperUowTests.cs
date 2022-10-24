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
    private readonly Table1ConnectionRepository _testRepository;
    public DapperUowTests() : base(AddServices)
    {
        _testRepository 
            = ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<Table1ConnectionRepository>();
    }

    [Fact]
    public async Task AddAndRemove_WithOpenConnection_SuccessAddAndRemove()
    {
        var service = GetSessionestRepository();
        var table1Repository = service.Table1Repository;
        var uoW = service.UoW;

        using (uoW.OpenConnectionAsync())
        {
            var insert = await table1Repository.AddAsync(
                new Table1
                {
                    Execution = uoW.IdUoW,
                    InsertAt = DateTime.Now,
                    Message = "inserted"
                });

            Assert.NotNull(insert);

            if (insert is null) return;

            var get = await table1Repository.GetByIdOrEmptyAsync(insert.Id);

            Assert.NotNull(get);

            if (get is null) return;

            var delete = await table1Repository.DeleteAsync(get.Id);

            Assert.NotNull(delete);

            if (delete is null) return;

            var getNull = await table1Repository.GetByIdOrEmptyAsync(delete.Id);

            Assert.Null(getNull);
        }
    }

    [Fact]
    public async Task Add_WithOutOpenConnection_FailedConnection()
    {
        var service = GetSessionestRepository();
        var table1Repository = service.Table1Repository;
        var uoW = service.UoW;

        await Assert.ThrowsAnyAsync<ArgumentNullException>(()=>table1Repository.AddAsync(
            new Table1
            {
                Execution = uoW.IdUoW,
                InsertAt = DateTime.Now,
                Message = "inserted"
            }));
    }

    [Fact]
    public async Task AddWithTransaction_WithBeginTransaction_SuccessAddAndRemove()
    {
        var service = GetSessionestRepository();
        var table1Repository = service.Table1Repository;
        var uoW = service.UoW;

        using (uoW.BeginTransactionAsync())
        {
            var insert = await table1Repository.AddAsync(
                new Table1
                {
                    Execution = uoW.IdUoW,
                    InsertAt = DateTime.Now,
                    Message = "inserted"
                });

            Assert.NotNull(insert);

            if (insert is null) return;

            var get = await table1Repository.GetByIdOrEmptyAsync(insert.Id);

            Assert.NotNull(get);

            if (get is null) return;

            var delete = await table1Repository.DeleteAsync(get.Id);

            Assert.NotNull(delete);

            if (delete is null) return;

            var getNull = await table1Repository.GetByIdOrEmptyAsync(delete.Id);

            Assert.Null(getNull);
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
            .AddScoped(typeof(Table1Repository))
            .AddScoped(typeof(Table1ConnectionRepository))
            
            .AddScoped(typeof(ServiceTest));
    }

}