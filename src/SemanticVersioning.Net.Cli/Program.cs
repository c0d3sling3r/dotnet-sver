using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SemanticVersioning.Net.Commands;

namespace SemanticVersioning.Net;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder();

        builder.ConfigureServices((context, services) =>
        {
            services.AddSingleton<RunnerService>();
            services.AddSingleton<ProjectLookupService>();
            
            services.AddScoped<ProjectFileManager>();
            services.AddScoped<ProjectVersionManager>();

            services.AddScoped<SemverCommand>();
            services.AddScoped<ListCommand>();
            services.AddScoped<UpgradeVersionCommand>();
            services.AddScoped<DegradeVersionCommand>();
            services.AddScoped<SetVersionCommand>();
        });

        using IHost host = builder.Build();

        RunnerService runnerService = host.Services.GetRequiredService<RunnerService>();
        await runnerService.RunAsync(args);
    }
}