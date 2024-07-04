using System.CommandLine;
using OllamaCommitGen.Cli.Binders;

namespace OllamaCommitGen.Cli;

class Program
{
    static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand("Generates commit message via Ollama AI");
        
        var originOption = rootCommand.AddOption<string>(
            name: "--origin",
            description: "Specifies a custom remote Ollama host",
            alias: "-o",
            arity: ArgumentArity.ExactlyOne,
            defaultValue: "http://localhost:11434"
        );

        var modelOption = rootCommand.AddOption<string>(
            name: "--model",
            description: "Specifies an Ollama model to be used",
            alias: "-m",
            arity: ArgumentArity.ExactlyOne,
            defaultValue: "llama3"
        );
        
        rootCommand.SetHandler(async (commitGenService) =>
        {
            string message = await commitGenService.GetMessageAsync();
            Console.WriteLine("Generated message: " + message);
            commitGenService.Commit(message);
            commitGenService.Dispose();
        }, new CommitGenBinder(originOption, modelOption));

        await rootCommand.InvokeAsync(args);
    }
}