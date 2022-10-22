using BlUoW.Dapper.Tests.Model;
using BlUoW.Session;
using Dapper;

namespace BlUoW.Dapper.Tests.Repositories;

internal class Table2Repository : IRepository<Table2, Table2, Guid>
{
    private readonly IDbSession _dbSession;

    public Table2Repository(IDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<int> DeleteAll()
    {
        return await _dbSession.Connection.ExecuteAsync(
            "DELETE FROM test.table2;",
            _dbSession.Transaction
        );
    }

    public async Task<Table2> AddAsync(Table2 model)
    {
        await _dbSession.Connection.ExecuteAsync(
            "INSERT IGNORE INTO test.table2 (Id, Execution, Message, InsertAt) VALUES (@Id, @Execution, @Message, @InsertAt);",
            model,
            _dbSession.Transaction
        );

        return model;
    }

    public Task<Table2> UpdateAsync(Guid id, Table2 model)
    {
        throw new NotImplementedException();
    }

    public async Task<Table2?> DeleteAsync(Guid id)
    {
        return await _dbSession.Connection.QueryFirstOrDefaultAsync<Table2>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table2 WHERE Id=@id;" +
            "DELETE FROM test.table2 WHERE Id=@id;"
            , new { Id = id },
            _dbSession.Transaction
        );
    }

    public async Task<Table2?> GetByIdOrEmptyAsync(Guid id)
    {
        return await _dbSession.Connection.QueryFirstOrDefaultAsync<Table2>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table2 WHERE Id=@id;", new { Id = id },
            _dbSession.Transaction
        );
    }

    public async Task<IEnumerable<Table2>> GetAllAsync()
    {
        return await _dbSession.Connection.QueryAsync<Table2>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table2;",
            _dbSession.Transaction
        );
    }
}