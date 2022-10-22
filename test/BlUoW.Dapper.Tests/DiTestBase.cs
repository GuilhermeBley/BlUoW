using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
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
        //var configuration = BuildConfiguration();
        //var serviceProvider = BuildServiceProvider(configuration, actionServices);
        //_serviceProvider = serviceProvider;
        IHost host = CreateHost(actionServices);
        _serviceProvider = host.Services;
    }

    private IServiceProvider BuildServiceProvider(IConfiguration configuration, Action<IServiceCollection>? actionServices = null)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton(configuration);
        actionServices?.Invoke(services);
        return services.BuildServiceProvider();
    }

    private IConfiguration BuildConfiguration()
    {
        IConfigurationBuilder configurationBuilder =
            new ConfigurationBuilder()
            .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly(), optional: false)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables();

        return configurationBuilder.Build();
    }

    private static IHost CreateHost(Action<IServiceCollection>? actionServices = null)
    {
        return
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    // Add other configuration files...
                    builder
                        .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly(), optional: false)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    actionServices?.Invoke(services);
                })
                .ConfigureLogging(logging =>
                {
                    // Add other loggers...
                })
                .Build() ?? throw new ArgumentNullException();
    } 
}