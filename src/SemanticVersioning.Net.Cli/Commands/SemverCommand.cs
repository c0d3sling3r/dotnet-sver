using System.CommandLine;
using System.Reflection;

namespace SemanticVersioning.Net.Commands;

public class SemverCommand : RootCommand
{
    public SemverCommand(ListCommand listCommand, UpgradeVersionCommand upgradeVersionCommand,
        DowngradeVersionCommand downgradeVersionCommand, SetVersionCommand setVersionCommand)
    {
        Initialize();

        AddCommand(listCommand);
        AddCommand(upgradeVersionCommand);
        AddCommand(downgradeVersionCommand);
        AddCommand(setVersionCommand);
    }

    private void Initialize()
    {
        string versionString = Assembly
            .GetEntryAssembly()?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion!;

        Description = $"Semantic Versioning {versionString}";
        Name = "dotnet-sver";
    }
}