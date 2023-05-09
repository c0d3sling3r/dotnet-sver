using Microsoft.Extensions.DependencyInjection;

namespace SemanticVersioning.Net.Cli.Tests;

public class DowngradeVersionTests : SemanticVersioningTestBase
{
    public DowngradeVersionTests(SemanticVersioningTestFixture semanticVersioningTestFixture) : base(semanticVersioningTestFixture)
    {
        
    }
    
    [Fact]
    public void DowngradeMajorVersion_ShouldNotDowngradeZero()
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
    public void DowngradeMinorVersion_ShouldNotDowngradeZero()
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
    public void DowngradePatchVersion_ShouldNotDowngradeZero()
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