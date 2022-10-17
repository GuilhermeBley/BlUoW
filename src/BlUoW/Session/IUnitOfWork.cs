namespace BlUoW.Session;

/// <summary>
/// Unit of work give a shared transactions to repositorys
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Identifier of unit
    /// </summary>
    Guid IdUoW { get; }

    /// <summary>
    /// Necessary to create and open a new connection
    /// </summary>
    /// <returns><see cref="Task"/> result of <see cref="IUnitOfWork"/> opened</returns>
    Task<IUnitOfWork> OpenConnectionAsync();

    /// <summary>
    /// Creates a connection (if method <see cref="OpenConnectionAsync"/> haven't executed) and transaction
    /// </summary>
    /// <remarks>
    ///     <para>Starts a transaction to the repositorys</para>
    ///     <para>Use <see cref="SaveChangesAsync"/>> after execute</para>
    /// </remarks>
    /// <returns><see cref="Task"/> result of <see cref="IUnitOfWork"/> opened</returns>
    Task<IUnitOfWork> BeginTransactionAsync();

    /// <summary>
    /// Commits a transaction if is ok or roll back if throw a exception
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    Task SaveChangesAsync();

    /// <summary>
    /// Roll back transaction
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    Task RollBackAsync();
}

