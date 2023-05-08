using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SemanticVersioning.Net;

public class ProjectLookupService
{
    private FileInfo[] _projectFileInfoArray;
    private string _projectSelectionList;
    
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProjectLookupService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;

        _projectFileInfoArray = Array.Empty<FileInfo>();
        _projectSelectionList = string.Empty;
    }

    public short ProjectsCount => (short) GetProjectPathArray().Length;
    public FileInfo[] ProjectPathArray => GetProjectPathArray();

    private FileInfo[] GetProjectPathArray(string? currentDirectoryPath = null)
    {
        currentDirectoryPath ??= Environment.CurrentDirectory;
        
        if (ValidateSolutionDirectory(currentDirectoryPath) == false)
            return Array.Empty<FileInfo>();

        if (!_projectFileInfoArray.Any())
        {
            _projectFileInfoArray = Directory
                    .GetFiles(currentDirectoryPath, searchPattern: "*.csproj", SearchOption.AllDirectories)
                    .Order()
                    .Select(fp => new FileInfo(fp))
                    .ToArray();
        }

        return _projectFileInfoArray;
    }

    public string DisplayProjectsList()
    {
        StringBuilder selectionListBuilder = new(string.Empty);

        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            ProjectFileManager fileManager = scope.ServiceProvider.GetRequiredService<ProjectFileManager>();

            if (string.IsNullOrEmpty(_projectSelectionList))
            {
                for (int i = 0; i < ProjectPathArray.Length; i++)
                {
                    Console.Write($"{i + 1}) {_projectFileInfoArray[i].Name} ");

                    string? versionValue = fileManager.GetVersionValue(i);
                    if (string.IsNullOrWhiteSpace(versionValue) == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(versionValue);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("-no version-");
                    }

                    Console.ResetColor();
                }

                _projectSelectionList = selectionListBuilder.ToString();
            }
        }

        return _projectSelectionList;
    }
    
    private bool ValidateSolutionDirectory(string? currentDirectoryPath = null)
    {
        DirectoryInfo currentDirectoryInfo = new(currentDirectoryPath ?? Environment.CurrentDirectory);

        if (currentDirectoryInfo.GetFiles("*.sln").Any()) return true;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine("The solution file was not found. Try again inside the solution root directory.\r\n" +
                                "OR pass the solution directory by --sln-dir (-s) option.");
        return false;

    }
}
