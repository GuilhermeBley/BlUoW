using BlUoW.Dapper.Tests.Model;
using BlUoW.Session;
using Dapper;

namespace BlUoW.Dapper.Tests.Repositories;

internal class SessionTestRepository
{
    private readonly IDbSession _dbSession;

    public SessionTestRepository(IDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<int> InsertTable1(Table1 table)
    {
        return await _dbSession.Connection.ExecuteAsync(
            "INSERT IGNORE INTO test.table1 (Id, Execution, Message, InsertAt) VALUES (@Id, @Execution, @Message, @InsertAt);",
            table,
            _dbSession.Transaction
        );
    }

    public async Task<int> InsertTable2(Table2 table)
    {
        return await _dbSession.Connection.ExecuteAsync(
            "INSERT IGNORE INTO test.table2 (Id, Execution, Message, InsertAt) VALUES (@Id, @Execution, @Message, @InsertAt);",
            table,
            _dbSession.Transaction
        );
    }

    public async Task<int> DeleteAllDataTable1()
    {
        return await _dbSession.Connection.ExecuteAsync(
            "DELETE FROM test.table1;",
            _dbSession.Transaction
        );
    }

    public async Task<int> DeleteAllDataTable2()
    {
        return await _dbSession.Connection.ExecuteAsync(
            "DELETE FROM test.table2;",
            _dbSession.Transaction
        );
    }

    public async Task<IEnumerable<Table1>> GetAllTable1()
    {
        return await _dbSession.Connection.QueryAsync<Table1>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table1;",
            _dbSession.Transaction
        );
    }

    public async Task<IEnumerable<Table1>> GetAllTable2()
    {
        return await _dbSession.Connection.QueryAsync<Table1>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table2;",
            _dbSession.Transaction
        );
    }
}