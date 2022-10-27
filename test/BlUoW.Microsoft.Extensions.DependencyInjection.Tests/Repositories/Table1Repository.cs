using BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Model;
using BlUoW.Session;
using Dapper;

namespace BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Repositories;

internal class Table1Repository : IRepository<Table1, Table1, Guid>
{
    private readonly IDbSession _dbSession;

    public Table1Repository(IDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<int> DeleteAll()
    {
        return await _dbSession.Connection.ExecuteAsync(
            "DELETE FROM test.table1;",
            _dbSession.Transaction
        );
    }

    public async Task<Table1> AddAsync(Table1 model)
    {
        await _dbSession.Connection.ExecuteAsync(
            "INSERT IGNORE INTO test.table1 (Id, Execution, Message, InsertAt) VALUES (@Id, @Execution, @Message, @InsertAt);",
            model,
            _dbSession.Transaction
        );

        return model;
    }

    public Task<Table1> UpdateAsync(Guid id, Table1 model)
    {
        throw new NotImplementedException();
    }

    public async Task<Table1?> DeleteAsync(Guid id)
    {
        return await _dbSession.Connection.QueryFirstOrDefaultAsync<Table1>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table1 WHERE Id=@id;" +
            "DELETE FROM test.table1 WHERE Id=@id;"
            , new { Id = id },
            _dbSession.Transaction
        );
    }

    public async Task<Table1?> GetByIdOrEmptyAsync(Guid id)
    {
        return await _dbSession.Connection.QueryFirstOrDefaultAsync<Table1>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table1 WHERE Id=@id;", new { Id = id },
            _dbSession.Transaction
        );
    }

    public async Task<IEnumerable<Table1>> GetAllAsync()
    {
        return await _dbSession.Connection.QueryAsync<Table1>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table1;",
            _dbSession.Transaction
        );
    }
}