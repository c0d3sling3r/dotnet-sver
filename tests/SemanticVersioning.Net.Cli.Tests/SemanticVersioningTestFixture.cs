using Microsoft.Extensions.DependencyInjection;

namespace SemanticVersioning.Net.Cli.Tests;

public class SemanticVersioningTestFixture : IDisposable
{
    private readonly IServiceCollection _serviceCollection;
    private IServiceProvider? _serviceProvider;
    
    public SemanticVersioningTestFixture()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddSemanticVersioningServices();
    }

    public IServiceProvider ServiceProvider
    {
        get
        {
            if (_serviceProvider == null)
                _serviceProvider = _serviceCollection.BuildServiceProvider();
            
            return _serviceProvider;
        }
    }
    
    public void Dispose()
    {
        _serviceCollection.Clear();
        _serviceProvider = null;
    }
}