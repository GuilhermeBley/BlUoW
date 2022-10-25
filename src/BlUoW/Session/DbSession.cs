using System.Data;
using System.Data.Common;
using BlUoW.Factories;

namespace BlUoW.Session;

/// <summary>
/// Manage connections and transactions
/// </summary>
/// <remarks>
///     <para>Instace of class should be used by only one thread</para>
/// </remarks>
public class DbSession : IUnitOfWork, IDbSession
{
    /// <summary>
    /// Provides a <see cref="DbConnection"/>>
    /// </summary>
    private readonly IConnectionFactory _connectionFactory;

    /// <summary>
    /// Id
    /// </summary>
    private Guid _identifier { get; } = Guid.NewGuid();

    /// <summary>
    /// Connection
    /// </summary>
    private DbConnection? _connection { get; set; }

    /// <summary>
    /// Transaction
    /// </summary>
    private DbTransaction? _transaction { get; set; }

    /// <summary>
    /// Id Session
    /// </summary>
    public Guid IdSession => _identifier;

    /// <summary>
    /// Id of Unit of work
    /// </summary>
    public Guid IdUoW => _identifier;

    /// <inheritdoc cref="_connection" path="*"/>
    public IDbConnection Connection => _connection ?? throw new ArgumentNullException(nameof(Connection));

    /// <inheritdoc cref="_transaction" path="*"/>
    public IDbTransaction? Transaction => _transaction;

    /// <summary>
    /// Instance with connection factory
    /// </summary>
    /// <param name="connectionFactory">Provides a connection</param>
    public DbSession(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Creates a new connection if don't exists and a new transaction
    /// </summary>
    /// <returns>this</returns>
    public async Task<IUnitOfWork> BeginTransactionAsync()
    {
        await TryCreateAndOpenConnection();

        if (_connection is null)
            throw new ArgumentNullException(nameof(Connection));

        if (_transaction is null)
            _transaction = await _connection.BeginTransactionAsync();

        return this;
    }

    /// <summary>
    /// Try releases connection and transaction
    /// </summary>
    public void Dispose()
    {
        if (_transaction is not null)
        {
            _transaction.Dispose();
            _transaction = null;
        }

        if (_connection is not null)
        {
            _connection.Dispose();
            _connection = null;
        }
    }

    /// <summary>
    /// Creates a new connection if don't exists
    /// </summary>
    /// <returns>this</returns>
    public async Task<IUnitOfWork> OpenConnectionAsync()
    {
        await TryCreateAndOpenConnection();
        return this;
    }

    /// <summary>
    /// Commit async transaction and releases it
    /// </summary>
    /// <remarks>
    ///     <para>If occurs exception, will be do a Rollback and throw the exception</para>
    /// </remarks>
    public async Task SaveChangesAsync()
    {
        if (_transaction is null)
            return;

        try
        {
            await _transaction.CommitAsync();
        }
        catch
        {
            await _transaction.RollbackAsync();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    /// <summary>
    /// Roll back async transaction and releases it
    /// </summary>
    public async Task RollBackAsync()
    {
        if (_transaction is null)
            return;

        try
        {
            await _transaction.RollbackAsync();
        }
        catch
        {
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    /// <summary>
    /// Try create async new connection
    /// </summary>
    private async Task TryCreateAndOpenConnection()
    {
        if (_connection is null)
        {
            _connection = _connectionFactory.GetNewConnection();
            await _connection.OpenAsync();
        }
    }
}
