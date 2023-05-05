namespace SemanticVersioning.Net;

public class ProjectVersionManager
{
    private readonly ProjectFileManager _projectFileManager;
    private readonly ProjectLookupService _projectLookupService;

    public ProjectVersionManager(ProjectFileManager projectFileManager, ProjectLookupService projectLookupService)
    {
        _projectFileManager = projectFileManager;
        _projectLookupService = projectLookupService;
    }

    public void IncreaseAllMajorVersions()
    {
        for (int i = 0; i < _projectLookupService.ProjectsCount; i++)
        {
            IncreaseMajorVersion(i);
        }
    }

    public void IncreaseMajorVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex);
        
        currentVersion.Major += 1;
        currentVersion.Minor = 0;
        currentVersion.Patch = 0;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void DecreaseAllMajorVersions()
    {
        for (int i = 0; i < _projectLookupService.ProjectsCount; i++)
        {
            DecreaseMajorVersion(i);
        }
    }

    public void DecreaseMajorVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex);
        
        if (currentVersion.Major > 0)
            currentVersion.Major -= 1;
        
        currentVersion.Minor = 0;
        currentVersion.Patch = 0;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void IncreaseAllMinorVersions()
    {
        for (int i = 0; i < _projectLookupService.ProjectsCount; i++)
        {
            IncreaseMinorVersion(i);
        }
    }

    public void IncreaseMinorVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex);
        
        currentVersion.Minor += 1;
        currentVersion.Patch = 0;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void DecreaseAllMinorVersions()
    {
        for (int i = 0; i < _projectLookupService.ProjectsCount; i++)
        {
            DecreaseMinorVersion(i);
        }
    }

    public void DecreaseMinorVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex);
        
        if (currentVersion.Minor > 0)
            currentVersion.Minor -= 1;
        
        currentVersion.Patch = 0;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void IncreaseAllPatchVersions()
    {
        for (int i = 0; i < _projectLookupService.ProjectsCount; i++)
        {
            IncreasePatchVersion(i);
        }
    }

    public void IncreasePatchVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex);
        
        currentVersion.Patch += 1;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void DecreaseAllPatchVersions()
    {
        for (int i = 0; i < _projectLookupService.ProjectsCount; i++)
        {
            DecreasePatchVersion(i);
        }
    }

    public void DecreasePatchVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex);
        
        if (currentVersion.Patch > 0)
            currentVersion.Patch -= 1;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void SetVersion(int projectIndex, SemanticVersion version)
    {
        _projectFileManager.TryUpdateVersion(projectIndex, version, createNodeIfNotExists: true);
    }

    public void SetAllVersions(string version)
    {
        for (int i = 0; i < _projectLookupService.ProjectsCount; i++)
        {
            SetVersion(i, version);
        }
    }

    public string GetCurrentVersionString(int projectIndex) => _projectFileManager.GetVersionValue(projectIndex);
}