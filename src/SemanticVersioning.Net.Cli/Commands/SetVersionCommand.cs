using System.CommandLine;

namespace SemanticVersioning.Net.Commands;

public class SetVersionCommand : Command
{
    private readonly ProjectVersionManager _projectVersionManager;
    
    public SetVersionCommand(ProjectVersionManager projectVersionManager) : base("set", "Sets the version of the chosen project(s).")
    {
        _projectVersionManager = projectVersionManager;
        
        Argument<int> projectListNumArgument = new("project-number");
        AddArgument(projectListNumArgument);
        
        Argument<string> versionArgument = new("version");
        versionArgument.AddValidator(validate =>
        {
            if (SemanticVersion.TryParse(validate.GetValueForArgument(versionArgument)) == null)
            {
                validate.ErrorMessage = "<version> argument is invalid.";
            }
        });
        AddArgument(versionArgument);
        
        this.SetHandler(Handle, projectListNumArgument, versionArgument);
    }

    private void Handle(int projectNumber, string version)
    {
        _projectVersionManager.SetVersion(projectNumber - 1, version);
    }
}