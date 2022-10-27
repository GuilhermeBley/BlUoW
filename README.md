# BlUoW
Unit of Work/Session with a shared connection.

## How it works
<p>Through a shared connection, repositories have to use a transaction and connection available for the persistency and collect of data.</p>
<p>The session is managed by a unit of work, where you have a possibility of open connection, save changes (commit), roll back, and initializes a transaction.</p>

## Benefics
<p>Unit of work, repositoy control</p>

## Care that must be taken
<p>Should be taken correctly the available connection and transaction in the repository.</p>
<p>The same about the services that uses the repositories, the unit of work should have a open connection, and if it uses a transaction, must be save or roll back in the final to persistency the data.</p>

## How to use
- <b>Dependency Injection</b>
<p>The implementation with the inversion control using the 'Microsoft.Extensions.DependencyInjection' is maked this way:</p>

```csharp
services
  .AddScoped(typeof(IConnectionFactory), connectionFactoryType)
  .AddScoped<DbSession>()
  .AddScoped<IDbSession>(x => x.GetRequiredService<DbSession>())
  .AddScoped<IUnitOfWork>(x => x.GetRequiredService<DbSession>());
```

Obs. A type which implements IConnectionFactory is necessary to use the unit of work.

- <b>Repositories</b>
<p>The repository must be use a session, this way:</p>

```csharp
using BlUoW.Session;

class Table1Repository
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
}
```

- <b>Services (using the repositories)</b>
<p>And, to use that in the services, can be used with only a connection:</p>

```csharp
using BlUoW.Session;

public class Service : IService
{
  private readonly IUnitOfWork _uoW;
  private readonly Table1Repository _table1Repository;

  public Service(IUnitOfWork uoW, Table1Repository table1Repository)
  {
      _uoW = uoW;
      _table1Repository = table1Repository;
  }

  public async Task AddWithConnection(Table1 model)
  {
      // The 'using' closes the connection in your final
      // The 'OpenConnectionAsync' try open a new async connection
      using (await _uoW.OpenConnectionAsync())
      {
          // With connection, the persistency to DB is automatic
          var insert = await _table1Repository.AddAsync(new Table1()); 
      }
  }
}
```

<p>Or, could be used too as transaction:</p>

```csharp
using BlUoW.Session;

public class Service : IService
{
  private readonly IUnitOfWork _uoW;
  private readonly Table1Repository _table1Repository;

  public Service(IUnitOfWork uoW, Table1Repository table1Repository)
  {
      _uoW = uoW;
      _table1Repository = table1Repository;
  }

  public async Task AddWithTransactionAsync(Table1 model)
  {
      // The 'using' closes the connection in your final
      // The 'BeginTransactionAsync' try open a new async connection and transaction
      using (await _uoW.BeginTransactionAsync())
      {
          var insert = await _table1Repository.AddAsync(new Table1());

          if (insert is null)
              await _uoW.RollBackAsync(); // Roll Back and releases transaction

          await _uoW.SaveChangesAsync(); // Commit and releases transaction
      }
  }
}
```
