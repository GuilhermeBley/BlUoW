using BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Model;
using BlUoW.Factories;
using Dapper;

namespace BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Repositories;

internal class Table1ConnectionRepository : IRepository<Table1, Table1, Guid>
{
    private readonly IConnectionFactory _connectionFactory;

    public Table1ConnectionRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<int> DeleteAll()
    {
        return await _connectionFactory.GetNewConnection().ExecuteAsync(
            "DELETE FROM test.table1;"
        );
    }

    public async Task<Table1> AddAsync(Table1 model)
    {
        await _connectionFactory.GetNewConnection().ExecuteAsync(
            "INSERT IGNORE INTO test.table1 (Id, Execution, Message, InsertAt) VALUES (@Id, @Execution, @Message, @InsertAt);",
            model
        );

        return model;
    }

    public Task<Table1> UpdateAsync(Guid id, Table1 model)
    {
        throw new NotImplementedException();
    }

    public async Task<Table1?> DeleteAsync(Guid id)
    {
        return await _connectionFactory.GetNewConnection().QueryFirstOrDefaultAsync<Table1>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table1 WHERE Id=@id;" +
            "DELETE FROM test.table1 WHERE Id=@id;"
            , new { Id = id }
        );
    }

    public async Task<Table1?> GetByIdOrEmptyAsync(Guid id)
    {
        return await _connectionFactory.GetNewConnection().QueryFirstOrDefaultAsync<Table1>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table1 WHERE Id=@id;", new { Id = id }
        );
    }

    public async Task<IEnumerable<Table1>> GetAllAsync()
    {
        return await _connectionFactory.GetNewConnection().QueryAsync<Table1>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table1;"
        );
    }
}