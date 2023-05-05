using System.CommandLine;

namespace SemanticVersioning.Net.Commands;

public class DegradeVersionCommand : Command
{
    private readonly ProjectVersionManager _projectVersionManager;
    
    public DegradeVersionCommand(ProjectVersionManager projectVersionManager) : base("degrade", "Degrades the version of the chosen project(s).")
    {
        _projectVersionManager = projectVersionManager;

        Option<int> projectListNumOption = new(new [] {"--project-number", "-p"}, "Specifies the project number targeting for version degrading.");
        Option<bool> majorOption = new("--major", "Degrades the major part.");
        Option<bool> minorOption = new("--minor", "Degrades the minor part.");
        Option<bool> patchOption = new("--patch", "Degrades the patch part.");
        Option<bool> allOption = new(new [] {"--all", "-a"}, "Degrades the version of all the projects.");
        
        projectListNumOption.Arity = ArgumentArity.ZeroOrOne;
        majorOption.Arity = ArgumentArity.ZeroOrOne;
        minorOption.Arity = ArgumentArity.ZeroOrOne;
        patchOption.Arity = ArgumentArity.ZeroOrOne;
        allOption.Arity = ArgumentArity.ZeroOrOne;
        
        AddOption(projectListNumOption);
        AddOption(majorOption);
        AddOption(minorOption);
        AddOption(patchOption);
        AddOption(allOption);

        this.SetHandler(Handle, projectListNumOption, majorOption, minorOption, patchOption, allOption);
        
        AddValidator(result =>
        {
            try
            {
                if (result.GetValueForOption(allOption) == false)
                {
                    if (result.GetValueForOption(projectListNumOption) != 0)
                        return;
                
                    result.ErrorMessage = "To manipulate versions, you must pass --project-number (-p) option\r\n"+
                                          "OR --all option to operate on all projects.";
                }
                else if (result.GetValueForOption(allOption) == true
                         && result.GetValueForOption(projectListNumOption) != 0)
                {
                    result.ErrorMessage = "There is an ambiguity for project selection.\r\n" +
                                          "Please pass either --project-number (-p) option OR --all option for all projects.";
                }
                else if (result.GetValueForOption(majorOption) == false && 
                         result.GetValueForOption(minorOption) == false &&
                         result.GetValueForOption(patchOption) == false)
                {
                    result.ErrorMessage = "One of version options required. (--major, --minor or --patch)";
                }
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong. Please check your entries again.");
                Console.ResetColor();
            }
        });
    }

    private void Handle(int projectListNum, bool degradeMajor, bool degradeMinor, bool degradePatch, bool degradeAll)
    {
        if (degradeAll)
            DegradeAll(degradeMajor, degradeMinor, degradePatch);
        else
            DegradeOne(projectListNum - 1, degradeMajor, degradeMinor, degradePatch);
    }

    private void DegradeOne(int projectIndex, bool degradeMajor, bool degradeMinor, bool degradePatch)
    {
        if (degradeMajor) 
            _projectVersionManager.DecreaseMajorVersion(projectIndex);

        if (degradeMinor)
            _projectVersionManager.DecreaseMinorVersion(projectIndex);
        
        if (degradePatch)
            _projectVersionManager.DecreasePatchVersion(projectIndex);
    }

    private void DegradeAll(bool degradeMajor, bool degradeMinor, bool degradePatch)
    {
        if (degradeMajor) 
            _projectVersionManager.DecreaseAllMajorVersions();

        if (degradeMinor)
            _projectVersionManager.DecreaseAllMinorVersions();
        
        if (degradePatch)
            _projectVersionManager.DecreaseAllPatchVersions();
    }
}