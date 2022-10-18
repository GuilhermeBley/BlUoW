using BlUoW.Dapper.Tests.Repositories;
using BlUoW.Session;

namespace BlUoW.Dapper.Tests.Services;

/// <summary>
/// Provides a unit of work and repository
/// </summary>
internal class ServiceTest
{
    private readonly IUnitOfWork _uoW;
    private readonly Table2Repository _sessionTestRepository;

    public IUnitOfWork UoW => _uoW;
    public Table2Repository SessionTestRepository => _sessionTestRepository;

    public ServiceTest(IUnitOfWork uoW, Table2Repository sessionTestRepository)
    {
        _uoW = uoW;
        _sessionTestRepository = sessionTestRepository;
    }
}