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
        var service = GetSessionRepository();
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
        var service = GetSessionRepository();
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
        var service = GetSessionRepository();
        var table1Repository = service.Table1Repository;
        var uoW = service.UoW;

        using (await uoW.BeginTransactionAsync())
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
    public async Task AddWithTransaction_CheckBeginTransactionWithoutSave_FailedCheck()
    {
        var service = GetSessionRepository();
        var connectionRepository = GetConnectionTable1Repository();
        var table1Repository = service.Table1Repository;
        var uoW = service.UoW;

        Table1 tableInserted;
        using (await uoW.BeginTransactionAsync())
        {
            tableInserted = await table1Repository.AddAsync(
                new Table1
                {
                    Execution = uoW.IdUoW,
                    InsertAt = DateTime.Now,
                    Message = "inserted"
                });

            Assert.NotNull(tableInserted);

            if (tableInserted is null) return;
        }

        Assert.Null(await connectionRepository.GetByIdOrEmptyAsync(tableInserted.Id));
    }
    
    [Fact]
    public async Task AddWithTransaction_CheckBeginTransactionWithSave_SuccessCheck()
    {
        var service = GetSessionRepository();
        var connectionRepository = GetConnectionTable1Repository();
        var table1Repository = service.Table1Repository;
        var uoW = service.UoW;

        Table1 tableInserted;
        using (await uoW.BeginTransactionAsync())
        {
            tableInserted = await table1Repository.AddAsync(
                new Table1
                {
                    Execution = uoW.IdUoW,
                    InsertAt = DateTime.Now,
                    Message = "inserted"
                });

            Assert.NotNull(tableInserted);

            if (tableInserted is null) return;

            await uoW.SaveChangesAsync();
        }

        Assert.NotNull(await connectionRepository.GetByIdOrEmptyAsync(tableInserted.Id));
    }

    [Fact]
    public async Task AddWithConnection_CheckBeginTransactionWithoutSave_SuccessCheck()
    {
        var service = GetSessionRepository();
        var connectionRepository = GetConnectionTable1Repository();
        var table1Repository = service.Table1Repository;
        var uoW = service.UoW;

        Table1 tableInserted;
        using (await uoW.OpenConnectionAsync())
        {
            tableInserted = await table1Repository.AddAsync(
                new Table1
                {
                    Execution = uoW.IdUoW,
                    InsertAt = DateTime.Now,
                    Message = "inserted"
                });

            Assert.NotNull(tableInserted);

            if (tableInserted is null) return;
        }

        Assert.NotNull(await connectionRepository.GetByIdOrEmptyAsync(tableInserted.Id));
    }
    
    [Fact]
    public async Task AddWithConnection_CheckBeginTransactionWithSave_SuccessCheck()
    {
        var service = GetSessionRepository();
        var connectionRepository = GetConnectionTable1Repository();
        var table1Repository = service.Table1Repository;
        var uoW = service.UoW;

        Table1 tableInserted;
        using (await uoW.BeginTransactionAsync())
        {
            tableInserted = await table1Repository.AddAsync(
                new Table1
                {
                    Execution = uoW.IdUoW,
                    InsertAt = DateTime.Now,
                    Message = "inserted"
                });

            Assert.NotNull(tableInserted);

            if (tableInserted is null) return;

            await uoW.SaveChangesAsync();
        }

        Assert.NotNull(await connectionRepository.GetByIdOrEmptyAsync(tableInserted.Id));
    }

    [Fact]
    public async Task Add_WithConnectionAndTransaction_SuccessConnection()
    {
        var service = GetSessionRepository();
        var connectionRepository = GetConnectionTable1Repository();
        var table1Repository = service.Table1Repository;
        var uoW = service.UoW;

        Table1 tableInserted;
        using (await uoW.OpenConnectionAsync())
        {
            await uoW.BeginTransactionAsync();
            tableInserted = await table1Repository.AddAsync(
                new Table1
                {
                    Execution = uoW.IdUoW,
                    InsertAt = DateTime.Now,
                    Message = "inserted"
                });

            Assert.Null(await connectionRepository.GetByIdOrEmptyAsync(tableInserted.Id));

            await uoW.SaveChangesAsync();
        }

        Assert.NotNull(await connectionRepository.GetByIdOrEmptyAsync(tableInserted.Id));
    }

    [Fact]
    public async Task AddWithTwoRepositories_WithTransaction_SuccessAddedInRepositories()
    {
        var service = GetSessionRepository();
        var table1ConnectionRepository = GetConnectionTable1Repository();
        var table2ConnectionRepository = GetConnectionTable2Repository();
        var table1Repository = service.Table1Repository;
        var table2Repository = service.Table2Repository;
        var uoW = service.UoW;

        Table1 table1Inserted;
        Table2 table2Inserted;
        using (await uoW.BeginTransactionAsync())
        {
            table1Inserted = await table1Repository.AddAsync(
                new Table1
                {
                    Execution = uoW.IdUoW,
                    InsertAt = DateTime.Now,
                    Message = "inserted"
                });

                
            table2Inserted = await table2Repository.AddAsync(
                new Table2
                {
                    Execution = uoW.IdUoW,
                    InsertAt = DateTime.Now,
                    Message = "inserted"
                });

            Assert.Null(await table1ConnectionRepository.GetByIdOrEmptyAsync(table1Inserted.Id));
            Assert.Null(await table2ConnectionRepository.GetByIdOrEmptyAsync(table2Inserted.Id));

            await uoW.SaveChangesAsync();
        }

        Assert.NotNull(await table1ConnectionRepository.GetByIdOrEmptyAsync(table1Inserted.Id));
        Assert.NotNull(await table2ConnectionRepository.GetByIdOrEmptyAsync(table2Inserted.Id));
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
            .AddScoped<IConnectionFactory, ConnectionFactory>()
            .AddScoped<DbSession>()
            .AddScoped<IDbSession>(x => x.GetRequiredService<DbSession>())
            .AddScoped<IUnitOfWork>(x => x.GetRequiredService<DbSession>())
            
            .AddScoped(typeof(Table2Repository))
            .AddScoped(typeof(Table1Repository))
            .AddScoped(typeof(Table1ConnectionRepository))
            .AddScoped(typeof(Table2ConnectionRepository))
            
            .AddScoped(typeof(ServiceTest));
    }

}