using BlUoW.Factories;
using BlUoW.Session;
using Microsoft.Extensions.DependencyInjection;

namespace BlUoW.Microsoft.Extensions.DependencyInjection.Di;

public static class ExtensionDi
{

    /// <summary>
    /// Add sessions and connection factory
    /// </summary>
    /// <param name="services">extension of services</param>
    /// <param name="connectionFactoryType">type of factory</param>
    /// <returns>same <paramref name="services"/></returns>
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services, Type connectionFactoryType)
    {
        if (!connectionFactoryType.IsClass)
            throw new ArgumentException($"{nameof(connectionFactoryType)} is not a class.");

        if (!typeof(IConnectionFactory).IsAssignableFrom(connectionFactoryType))
            throw new ArgumentException($"{nameof(connectionFactoryType)} is not a instance of type {nameof(IConnectionFactory)}.");

        services
            .AddScoped(typeof(IConnectionFactory), connectionFactoryType)
            .AddScoped<DbSession>()
            .AddScoped<IDbSession>(x => x.GetRequiredService<DbSession>())
            .AddScoped<IUnitOfWork>(x => x.GetRequiredService<DbSession>());

        return services;
    }

    
    /// <summary>
    /// Add sessions and connection factory
    /// </summary>
    /// <param name="services">extension of services</param>
    /// <typeparam name="T">type of factory</typeparam>
    /// <returns>same <paramref name="services"/></returns>
    public static IServiceCollection AddUnitOfWork<T>(this IServiceCollection services)
        where T : class, IConnectionFactory
    {
        return AddUnitOfWork(services, typeof(T));
    }
}
