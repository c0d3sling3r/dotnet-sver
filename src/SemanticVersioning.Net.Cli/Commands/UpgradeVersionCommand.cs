using System.CommandLine;

namespace SemanticVersioning.Net.Commands;

public class UpgradeVersionCommand : Command
{
    private readonly ProjectVersionManager _projectVersionManager;
    
    public UpgradeVersionCommand(ProjectVersionManager projectVersionManager) 
        : base("upgrade", "Upgrades the version of the chosen project(s).")
    {
        _projectVersionManager = projectVersionManager;
        
        Option<int> projectListNumOption = AddProjectListNumOption();
        Option<bool> majorOption = AddMajorOption();
        Option<bool> minorOption = AddMinorOption();
        Option<bool> patchOption = AddPatchOption();
        Option<bool> allOption = AddAllOption();

        this.SetHandler(Handle, projectListNumOption, majorOption, minorOption, patchOption, allOption);
        
        AddValidator(allOption, projectListNumOption, majorOption, minorOption, patchOption);
    }

    private void AddValidator(Option<bool> allOption, Option<int> projectListNumOption, Option<bool> majorOption, Option<bool> minorOption,
        Option<bool> patchOption)
    {
        base.AddValidator(result =>
        {
            try
            {
                if (result.GetValueForOption(allOption) == false)
                {
                    if (result.GetValueForOption(projectListNumOption) != 0)
                        return;

                    result.ErrorMessage = "To manipulate versions, you must pass --project-number (-p) option\r\n" +
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

    private Option<bool> AddAllOption()
    {
        Option<bool> allOption = new(new[] { "--all", "-a" }, "Upgrades the patch part of all the projects.");
        allOption.Arity = ArgumentArity.ZeroOrOne;

        AddOption(allOption);
        return allOption;
    }

    private Option<bool> AddPatchOption()
    {
        Option<bool> patchOption = new("--patch", "Upgrades the patch part.");
        patchOption.Arity = ArgumentArity.ZeroOrOne;

        AddOption(patchOption);
        return patchOption;
    }

    private Option<bool> AddMinorOption()
    {
        Option<bool> minorOption = new("--minor", "Upgrades the minor part.");
        minorOption.Arity = ArgumentArity.ZeroOrOne;

        AddOption(minorOption);
        return minorOption;
    }

    private Option<bool> AddMajorOption()
    {
        Option<bool> majorOption = new("--major", "Upgrades the major part.");
        majorOption.Arity = ArgumentArity.ZeroOrOne;

        AddOption(majorOption);
        return majorOption;
    }

    private Option<int> AddProjectListNumOption()
    {
        Option<int> projectListNumOption = new(new[] { "--project-number", "-p" },
            "Specifies the project number targeting for version upgrading.");
        projectListNumOption.Arity = ArgumentArity.ZeroOrOne;

        AddOption(projectListNumOption);
        return projectListNumOption;
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