using BlUoW.Dapper.Tests.Model;
using BlUoW.Factories;
using Dapper;

namespace BlUoW.Dapper.Tests.Repositories;

internal class TestRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public TestRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> InsertTable1(Table1 table)
    {
        return await _connectionFactory.GetNewConnection().ExecuteAsync(
            "INSERT IGNORE INTO test.table1 (Id, Execution, Message, InsertAt) VALUES (@Id, @Execution, @Message, @InsertAt);",
            table
        );
    }

    public async Task<int> InsertTable2(Table2 table)
    {
        return await _connectionFactory.GetNewConnection().ExecuteAsync(
            "INSERT IGNORE INTO test.table2 (Id, Execution, Message, InsertAt) VALUES (@Id, @Execution, @Message, @InsertAt);",
            table
        );
    }

    public async Task<int> DeleteAllDataTable1()
    {
        return await _connectionFactory.GetNewConnection().ExecuteAsync(
            "DELETE FROM test.table1;"
        );
    }

    public async Task<int> DeleteAllDataTable2()
    {
        return await _connectionFactory.GetNewConnection().ExecuteAsync(
            "DELETE FROM test.table2;"
        );
    }

    public async Task<IEnumerable<Table1>> GetAllTable1()
    {
        return await _connectionFactory.GetNewConnection().QueryAsync<Table1>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table1;"
        );
    }

    public async Task<IEnumerable<Table1>> GetAllTable2()
    {
        return await _connectionFactory.GetNewConnection().QueryAsync<Table1>(
            "SELECT Id, Execution, Message, InsertAt FROM test.table2;"
        );
    }
}