using System.Data.Common;

namespace BlUoW.Factories;

/// <summary>
/// Connection factory
/// </summary>
public interface IConnectionFactory
{
    /// <summary>
    /// Should returns a new connection
    /// </summary>
    /// <returns><see cref="DbConnection"/></returns>
    DbConnection GetNewConnection();
}