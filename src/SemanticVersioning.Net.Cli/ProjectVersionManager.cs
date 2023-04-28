namespace SemanticVersioning.Net;

public class ProjectVersionManager
{
    private readonly ProjectFileManager _projectFileManager;

    public ProjectVersionManager(ProjectFileManager projectFileManager)
    {
        _projectFileManager = projectFileManager;
    }

    public void IncreaseMajorVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex) ?? "1.0.0";
        
        currentVersion.Major += 1;
        currentVersion.Minor = 0;
        currentVersion.Patch = 0;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void DecreaseMajorVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex) ?? "1.0.0";
        
        currentVersion.Major -= 1;
        currentVersion.Minor = 0;
        currentVersion.Patch = 0;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void IncreaseMinorVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex) ?? "1.0.0";
        
        currentVersion.Minor += 1;
        currentVersion.Patch = 0;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void DecreaseMinorVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex) ?? "1.0.0";
        
        currentVersion.Minor -= 1;
        currentVersion.Patch = 0;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void IncreasePatchVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex) ?? "1.0.0";
        
        currentVersion.Patch += 1;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void DecreasePatchVersion(int projectIndex)
    {
        SemanticVersion currentVersion = _projectFileManager.GetVersionValue(projectIndex) ?? "1.0.0";
        
        currentVersion.Patch -= 1;
        
        _projectFileManager.TryUpdateVersion(projectIndex, currentVersion);
    }

    public void SetVersion(int projectIndex, SemanticVersion version)
    {
        _projectFileManager.TryUpdateVersion(projectIndex, version);
    }
}