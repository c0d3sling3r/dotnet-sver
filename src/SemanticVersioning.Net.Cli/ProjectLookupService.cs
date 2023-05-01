using System.Text;

using Microsoft.Extensions.DependencyInjection;

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

    public FileInfo[] ProjectPathArray => GetProjectPathArray();
    public string ProjectSelectionList => GetProjectFullNamesSelectionList();

    private FileInfo[] GetProjectPathArray(string? workingDirectory = null)
    {
        if (string.IsNullOrEmpty(workingDirectory))
        {
            workingDirectory = TryGetSolutionPath(Environment.CurrentDirectory);
        }

        if (!_projectFileInfoArray.Any())
        {
            _projectFileInfoArray =
                Directory
                    .GetFiles(workingDirectory, searchPattern: "*.csproj", SearchOption.AllDirectories)
                    .Order()
                    .Select(fp => new FileInfo(fp))
                    .ToArray();
        }

        return _projectFileInfoArray;
    }

    private string GetProjectFullNamesSelectionList()
    {
        StringBuilder selectionListBuilder = new(string.Empty);

        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            ProjectFileManager fileManager = scope.ServiceProvider.GetRequiredService<ProjectFileManager>();

            if (string.IsNullOrEmpty(_projectSelectionList))
            {
                for (int i = 0; i < ProjectPathArray.Length; i++)
                {
                    selectionListBuilder.AppendLine(
                        $"{i + 1}) {_projectFileInfoArray[i].Name} {fileManager.GetVersionValue(i)}");
                }

                _projectSelectionList = selectionListBuilder.ToString();
            }
        }

        return _projectSelectionList;
    }
    
    private static string TryGetSolutionPath(string? currentDirectoryPath = null)
    {
        DirectoryInfo currentDirectoryInfo = new(currentDirectoryPath ?? Environment.CurrentDirectory);

        if (!currentDirectoryInfo.GetFiles("*.sln").Any())
            return TryGetSolutionPath(currentDirectoryInfo.Parent?.FullName);
     
        return currentDirectoryInfo.FullName;
    }
}
