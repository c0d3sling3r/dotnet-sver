using Microsoft.Extensions.DependencyInjection;

using SemanticVersioning.Net.Commands;

namespace SemanticVersioning.Net;

public static class DependencyConfigurationsExtensions
{
    public static IServiceCollection AddSemanticVersioningServices(this IServiceCollection services)
    {
        services.AddSingleton<RunnerService>();
        services.AddSingleton<ProjectLookupService>();
            
        services.AddScoped<ProjectFileManager>();
        services.AddScoped<ProjectVersionManager>();

        services.AddScoped<SemverCommand>();
        services.AddScoped<ListCommand>();
        services.AddScoped<UpgradeVersionCommand>();
        services.AddScoped<DowngradeVersionCommand>();
        services.AddScoped<SetVersionCommand>();

        return services;
    }
}