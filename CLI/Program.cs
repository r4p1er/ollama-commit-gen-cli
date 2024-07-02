using System.CommandLine;
using CLI.Binders;

namespace CLI;

class Program
{
    static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand("Generate commit message via Ollama AI");
        
        rootCommand.SetHandler(async (commitGenService) =>
        {
            string message = await commitGenService.GetMessageAsync();
            Console.WriteLine("Generated message: " + message);
            commitGenService.Commit(message);
            commitGenService.Dispose();
        }, new CommitGenBinder());

        await rootCommand.InvokeAsync(args);
    }
}