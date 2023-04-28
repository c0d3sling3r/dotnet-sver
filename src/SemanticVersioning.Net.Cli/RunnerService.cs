using System.CommandLine;

using SemanticVersioning.Net.Commands;

namespace SemanticVersioning.Net;

public class RunnerService
{
    private readonly SemverCommand _semverCommand;
    
    public RunnerService(SemverCommand semverCommand)
    {
        _semverCommand = semverCommand;
    }

    public async Task RunAsync(params string[] args)
    {
        await _semverCommand.InvokeAsync(args);
    }
}