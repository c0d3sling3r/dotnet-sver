using System.CommandLine;

namespace SemanticVersioning.Net;

public static class SemverOptions
{
    public static readonly Option<IEnumerable<int>> ProjectIndexesOption = new("--project-index(es)",
            "Number of the project(s) shown using list command, delimited by space to set the version.")
        { Arity = ArgumentArity.ZeroOrMore };

    public static readonly Option<bool> AllOption = new("--all", "Sets the version to all projects");

    public static readonly Option<IEnumerable<int>> ExceptOption = new("--except",
            "Sets the version to all projects except the ones specified by number(s).")
        { Arity = ArgumentArity.ZeroOrMore };

    public static IEnumerable<Option> GetAll()
    {
        yield return AllOption;
        yield return ExceptOption;
        yield return ProjectIndexesOption;
    }
}