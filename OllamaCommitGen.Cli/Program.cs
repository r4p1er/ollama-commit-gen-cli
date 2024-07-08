using System.CommandLine;
using OllamaCommitGen.Cli.Binders;
using OllamaCommitGen.Cli.Utils;

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

        var noPromptOption = rootCommand.AddOption<bool>(
            name: "--no-prompt",
            description: "Disables the interactive prompt for editing the generated commit message",
            alias: "-n",
            arity: ArgumentArity.ZeroOrOne,
            defaultValue: false
        );
        
        rootCommand.SetHandler(async (commitGenService, noPrompt) =>
        {
            string message = await commitGenService.GetMessageAsync();
            Console.Write("Generated message: ");
            
            if (noPrompt)
            {
                Console.WriteLine(message);
                commitGenService.Commit(message);
            }
            else
            {
                var newMessage = ConsoleWrapper.WriteEditableLine(message);
                commitGenService.Commit(newMessage);
            }
            
            commitGenService.Dispose();
        }, new CommitGenBinder(originOption, modelOption), noPromptOption);

        await rootCommand.InvokeAsync(args);
    }
}