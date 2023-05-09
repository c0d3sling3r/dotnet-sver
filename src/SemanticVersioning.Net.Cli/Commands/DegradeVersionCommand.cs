using System.CommandLine;

namespace SemanticVersioning.Net.Commands;

public class DowngradeVersionCommand : Command
{
    private readonly ProjectVersionManager _projectVersionManager;
    
    public DowngradeVersionCommand(ProjectVersionManager projectVersionManager) 
        : base("downgrade", "Downgrades the version of the chosen project(s).")
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

    private Option<bool> AddAllOption()
    {
        Option<bool> allOption = new(new[] { "--all", "-a" }, "Downgrades the version of all the projects.");
        allOption.Arity = ArgumentArity.ZeroOrOne;

        AddOption(allOption);
        return allOption;
    }

    private Option<bool> AddPatchOption()
    {
        Option<bool> patchOption = new("--patch", "Downgrades the patch part.");
        patchOption.Arity = ArgumentArity.ZeroOrOne;

        AddOption(patchOption);
        return patchOption;
    }

    private Option<bool> AddMinorOption()
    {
        Option<bool> minorOption = new("--minor", "Downgrades the minor part.");
        minorOption.Arity = ArgumentArity.ZeroOrOne;

        AddOption(minorOption);
        return minorOption;
    }

    private Option<bool> AddMajorOption()
    {
        Option<bool> majorOption = new("--major", "Downgrades the major part.");
        majorOption.Arity = ArgumentArity.ZeroOrOne;

        AddOption(majorOption);
        return majorOption;
    }

    private Option<int> AddProjectListNumOption()
    {
        Option<int> projectListNumOption = new(new[] { "--project-number", "-p" },
            "Specifies the project number targeting for version downgrading.");
        projectListNumOption.Arity = ArgumentArity.ZeroOrOne;

        AddOption(projectListNumOption);
        return projectListNumOption;
    }

    private void Handle(int projectListNum, bool downgradeMajor, bool downgradeMinor, bool downgradePatch, bool downgradeAll)
    {
        if (downgradeAll)
            DowngradeAll(downgradeMajor, downgradeMinor, downgradePatch);
        else
            DowngradeOne(projectListNum - 1, downgradeMajor, downgradeMinor, downgradePatch);
    }

    private void DowngradeOne(int projectIndex, bool downgradeMajor, bool downgradeMinor, bool downgradePatch)
    {
        if (downgradeMajor) 
            _projectVersionManager.DecreaseMajorVersion(projectIndex);

        if (downgradeMinor)
            _projectVersionManager.DecreaseMinorVersion(projectIndex);
        
        if (downgradePatch)
            _projectVersionManager.DecreasePatchVersion(projectIndex);
    }

    private void DowngradeAll(bool downgradeMajor, bool downgradeMinor, bool downgradePatch)
    {
        if (downgradeMajor) 
            _projectVersionManager.DecreaseAllMajorVersions();

        if (downgradeMinor)
            _projectVersionManager.DecreaseAllMinorVersions();
        
        if (downgradePatch)
            _projectVersionManager.DecreaseAllPatchVersions();
    }
}