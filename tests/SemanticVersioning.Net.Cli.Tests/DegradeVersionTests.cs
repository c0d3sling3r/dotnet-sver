using Microsoft.Extensions.DependencyInjection;

namespace SemanticVersioning.Net.Cli.Tests;

public class DegradeVersionTests : SemanticVersioningTestBase
{
    public DegradeVersionTests(SemanticVersioningTestFixture semanticVersioningTestFixture) : base(semanticVersioningTestFixture)
    {
        
    }
    
    [Fact]
    public void DegradeMajorVersion_ShouldNotDegradeZero()
    {
        // Arrange
        ProjectVersionManager projectVersionManager =
            SemanticVersioningTestFixture.ServiceProvider.GetRequiredService<ProjectVersionManager>();
        projectVersionManager.SetVersion(0, "0.1.5");
        const string expectedVersionString = "0.0.0";
        
        // Act
        projectVersionManager.DecreaseMajorVersion(0);
        string actualVersionString = projectVersionManager.GetCurrentVersionString(0);

        // Assert
        Assert.Equal(expectedVersionString, actualVersionString);
    }

    [Fact]
    public void DegradeMinorVersion_ShouldNotDegradeZero()
    {
        // Arrange
        ProjectVersionManager projectVersionManager =
            SemanticVersioningTestFixture.ServiceProvider.GetRequiredService<ProjectVersionManager>();
        projectVersionManager.SetVersion(0, "1.0.5");
        const string expectedVersionString = "1.0.0";
        
        // Act
        projectVersionManager.DecreaseMinorVersion(0);
        string actualVersionString = projectVersionManager.GetCurrentVersionString(0);

        // Assert
        Assert.Equal(expectedVersionString, actualVersionString);
    }

    [Fact]
    public void DegradePatchVersion_ShouldNotDegradeZero()
    {
        // Arrange
        ProjectVersionManager projectVersionManager =
            SemanticVersioningTestFixture.ServiceProvider.GetRequiredService<ProjectVersionManager>();
        projectVersionManager.SetVersion(0, "2.6.0");
        const string expectedVersionString = "2.6.0";
        
        // Act
        projectVersionManager.DecreasePatchVersion(0);
        string actualVersionString = projectVersionManager.GetCurrentVersionString(0);

        // Assert
        Assert.Equal(expectedVersionString, actualVersionString);
    }
}