using System.CommandLine;

namespace SemanticVersioning.Net.Commands;

public class ListCommand : Command
{
    private readonly ProjectLookupService _lookupService;
    
    public ListCommand(ProjectLookupService lookupService) : base("list", "Lists all project with their versions.")
    {
        _lookupService = lookupService;
        
        this.SetHandler(Handle);
    }

    private void Handle()
    {
        _lookupService.DisplayProjectsList();
    }
}