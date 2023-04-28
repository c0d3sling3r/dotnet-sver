using System.CommandLine;
using System.Diagnostics;

namespace SemanticVersioning.Net.Commands;

public class DegradeVersionCommand : Command
{
    private readonly ProjectVersionManager _projectVersionManager;
    
    public DegradeVersionCommand(ProjectVersionManager projectVersionManager) : base("degrade", "Degrades the version of the chosen project(s).")
    {
        _projectVersionManager = projectVersionManager;
        
        Argument<int> projectListNumArgument = new("project-number");
        AddArgument(projectListNumArgument);

        Option<bool> majorOption = new("--major", "Degrades the major part.");
        Option<bool> minorOption = new("--minor", "Degrades the minor part.");
        Option<bool> patchOption = new("--patch", "Degrades the patch part.");
        
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
                result.ErrorMessage = result.ErrorMessage = "One of version options required. (--major, --minor or --patch)";
            }
        });
    }

    private void Handle(int projectListNum, bool degradeMajor, bool degradeMinor, bool degradePatch)
    {
        int projectIndex = projectListNum - 1;
        
        if (degradeMajor) 
            _projectVersionManager.DecreaseMajorVersion(projectIndex);

        if (degradeMinor)
            _projectVersionManager.DecreaseMinorVersion(projectIndex);
        
        if (degradePatch)
            _projectVersionManager.DecreasePatchVersion(projectIndex);
    }
}