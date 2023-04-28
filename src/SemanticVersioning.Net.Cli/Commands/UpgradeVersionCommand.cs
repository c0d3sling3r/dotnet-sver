using System.CommandLine;

namespace SemanticVersioning.Net.Commands;

public class UpgradeVersionCommand : Command
{
    private readonly ProjectVersionManager _projectVersionManager;
    
    public UpgradeVersionCommand(ProjectVersionManager projectVersionManager) : base("upgrade", "Upgrades the version of the chosen project(s).")
    {
        _projectVersionManager = projectVersionManager;
        
        Argument<int> projectListNumArgument = new("project-number");
        AddArgument(projectListNumArgument);

        Option<bool> majorOption = new("--major", "Upgrades the major part.");
        Option<bool> minorOption = new("--minor", "Upgrades the minor part.");
        Option<bool> patchOption = new("--patch", "Upgrades the patch part.");
        
        AddOption(majorOption);
        AddOption(minorOption);
        AddOption(patchOption);

        this.SetHandler(Handle, projectListNumArgument, majorOption, minorOption, patchOption);
        
        AddValidator(result =>
        {
            if (result.GetValueForArgument(projectListNumArgument) == 0)
            {
                result.ErrorMessage = "<project-number> argument is required.";
            }
            else if (result.GetValueForOption(majorOption) == false && 
                     result.GetValueForOption(minorOption) == false &&
                     result.GetValueForOption(patchOption) == false)
            {
                result.ErrorMessage = "One of version options required. (--major, --minor or --patch)";
            }
        });
    }

    private void Handle(int projectListNum, bool upgradeMajor, bool upgradeMinor, bool upgradePatch)
    {
        int projectIndex = projectListNum - 1;
        
        if (upgradeMajor)
            _projectVersionManager.IncreaseMajorVersion(projectIndex);
        
        if (upgradeMinor)
            _projectVersionManager.IncreaseMinorVersion(projectIndex);
        
        if (upgradePatch)
            _projectVersionManager.IncreasePatchVersion(projectIndex);
    }
}