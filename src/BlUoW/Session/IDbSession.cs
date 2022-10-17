using System.Data;

namespace BlUoW.Session;

/// <summary>
/// Provides a session connection
/// </summary>
public interface IDbSession
{
    /// <summary>
    /// Id
    /// </summary>
    Guid IdSession { get; }

    /// <summary>
    /// Avaliable connection
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    IDbConnection Connection { get; }

    /// <summary>
    /// Avaliable Transaction
    /// </summary>
    IDbTransaction? Transaction { get; }
}