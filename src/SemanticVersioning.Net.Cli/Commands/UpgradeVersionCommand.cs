using System.CommandLine;

namespace SemanticVersioning.Net.Commands;

public class UpgradeVersionCommand : Command
{
    private readonly ProjectVersionManager _projectVersionManager;
    
    public UpgradeVersionCommand(ProjectVersionManager projectVersionManager) : base("upgrade", "Upgrades the version of the chosen project(s).")
    {
        _projectVersionManager = projectVersionManager;
        
        Option<int> projectListNumOption = new(new [] {"--project-number", "-p"}, "Specifies the project number targeting for version upgrading.");
        Option<bool> majorOption = new("--major", "Upgrades the major part.");
        Option<bool> minorOption = new("--minor", "Upgrades the minor part.");
        Option<bool> patchOption = new("--patch", "Upgrades the patch part.");
        Option<bool> allOption = new(new [] {"--all", "-a"}, "Upgrades the patch part of all the projects.");
        
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
                                          "OR --all (-a) option to operate on all projects.";
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

    private void Handle(int projectListNum, bool upgradeMajor, bool upgradeMinor, bool upgradePatch, bool upgradeAll)
    {
        if (upgradeAll)
            UpgradeAll(upgradeMajor, upgradeMinor, upgradePatch);
        else
            UpgradeOne(projectListNum - 1, upgradeMajor, upgradeMinor, upgradePatch);
    }

    private void UpgradeOne(int projectIndex, bool upgradeMajor, bool upgradeMinor, bool upgradePatch)
    {
        if (upgradeMajor)
            _projectVersionManager.IncreaseMajorVersion(projectIndex);

        if (upgradeMinor)
            _projectVersionManager.IncreaseMinorVersion(projectIndex);

        if (upgradePatch)
            _projectVersionManager.IncreasePatchVersion(projectIndex);
    }

    private void UpgradeAll(bool upgradeMajor, bool upgradeMinor, bool upgradePatch)
    {
        if (upgradeMajor)
            _projectVersionManager.IncreaseAllMajorVersions();

        if (upgradeMinor)
            _projectVersionManager.IncreaseAllMinorVersions();

        if (upgradePatch)
            _projectVersionManager.IncreaseAllPatchVersions();
    }
}