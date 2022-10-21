using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlUoW.Dapper.Tests;

/// <summary>
/// Test base with service providier
/// </summary>
public class DiTestBase
{
    /// <summary>
    /// Services builded
    /// </summary>
    protected readonly IServiceProvider _serviceProvider;

    /// <inheritdoc cref="_serviceProvider" path="*"/>
    public IServiceProvider ServiceProvider => _serviceProvider;

    public DiTestBase(Action<IServiceCollection>? actionServices = null)
    {
        IHost host = CreateHost(actionServices);
        _serviceProvider = host.Services;
    }

    private static IHost CreateHost(Action<IServiceCollection>? actionServices = null)
    {
        return
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    // Add other configuration files...
                    builder.AddJsonFile("appsettings.Development.json", optional: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(context.Configuration);
                    actionServices?.Invoke(services);
                })
                .ConfigureLogging(logging =>
                {
                    // Add other loggers...
                })
                .Build() ?? throw new ArgumentNullException();
    } 
}