using BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Model;
using BlUoW.Factories;
using Dapper;

namespace BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Repositories;

internal class Table2ConnectionRepository : IRepository<Table2, Table2, Guid>
{
    private readonly IConnectionFactory _connectionFactory;

    public Table2ConnectionRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<int> DeleteAll()
    {
        return await _connectionFactory.GetNewConnection().ExecuteAsync(
            "DELETE FROM test.table2;"
        );
    }

    public async Task<Table2> AddAsync(Table2 model)
    {
        await _connectionFactory.GetNewConnection().ExecuteAsync(
            "INSERT IGNORE INTO test.table2 (Id, Execution, Message, InsertAt) VALUES (@Id, @Execution, @Message, @InsertAt);",
            model
        );

        return model;
    }

    public Task<Table2> UpdateAsync(Guid id, Table2 model)
    {
        throw new NotImplementedException();
    }

    public async Task<Table2?> DeleteAsync(Guid id)
    {
        return await _connectionFactory.GetNewConnection().QueryFirstOrDefaultAsync<Table2>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table2 WHERE Id=@id;" +
            "DELETE FROM test.table2 WHERE Id=@id;"
            , new { Id = id }
        );
    }

    public async Task<Table2?> GetByIdOrEmptyAsync(Guid id)
    {
        return await _connectionFactory.GetNewConnection().QueryFirstOrDefaultAsync<Table2>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table2 WHERE Id=@id;", new { Id = id }
        );
    }

    public async Task<IEnumerable<Table2>> GetAllAsync()
    {
        return await _connectionFactory.GetNewConnection().QueryAsync<Table2>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table2;"
        );
    }
}