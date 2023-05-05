namespace SemanticVersioning.Net.Cli.Tests;

public class SemanticVersioningTestBase : IClassFixture<SemanticVersioningTestFixture>
{
    public SemanticVersioningTestFixture SemanticVersioningTestFixture;

    public SemanticVersioningTestBase(SemanticVersioningTestFixture semanticVersioningTestFixture)
    {
        SemanticVersioningTestFixture = semanticVersioningTestFixture;
    }
}