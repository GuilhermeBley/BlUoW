using BlUoW.Dapper.Tests.Repositories;
using BlUoW.Session;

namespace BlUoW.Dapper.Tests.Services;

/// <summary>
/// Provides a unit of work and repository
/// </summary>
internal class ServiceTest
{
    private readonly IUnitOfWork _uoW;
    private readonly Table1Repository _table1Repository;
    private readonly Table2Repository _table2Repository;

    public IUnitOfWork UoW => _uoW;
    public Table1Repository Table1Repository => _table1Repository;
    public Table2Repository Table2Repository => _table2Repository;

    public ServiceTest(IUnitOfWork uoW, Table1Repository table1Repository, Table2Repository table2Repository)
    {
        _uoW = uoW;
        _table1Repository = table1Repository;
        _table2Repository = table2Repository;
    }
}