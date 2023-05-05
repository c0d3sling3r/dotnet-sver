using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SemanticVersioning.Net;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder();

        builder.ConfigureServices((context, services) =>
        {
            services.AddSemanticVersioningServices();
        });

        using IHost host = builder.Build();

        RunnerService runnerService = host.Services.GetRequiredService<RunnerService>();
        await runnerService.RunAsync(args);
    }
}