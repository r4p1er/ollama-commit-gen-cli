using System.CommandLine;
using OllamaCommitGen.Cli.Binders;

namespace OllamaCommitGen.Cli;

class Program
{
    static async Task Main(string[] args)
    {
        var originOption = new Option<string>(name: "--origin", description: "Specifies a custom remote Ollama host")
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = false
        };
        originOption.AddAlias("-o");
        originOption.SetDefaultValue("http://localhost:11434");

        var modelOption = new Option<string>(name: "--model", description: "Specifies an Ollama model to be used")
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = false
        };
        modelOption.AddAlias("-m");
        modelOption.SetDefaultValue("llama3");
        
        var rootCommand = new RootCommand("Generates commit message via Ollama AI");
        rootCommand.AddOption(originOption);
        rootCommand.AddOption(modelOption);
        
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