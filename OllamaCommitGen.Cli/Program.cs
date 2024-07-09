using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using OllamaCommitGen.Cli.Binders;
using OllamaCommitGen.Cli.Extensions;
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

        var langOption = rootCommand.AddOption<string>(
            name: "--lang",
            description: "Specifies a language (ISO-639-3) to be used for generating commit message",
            alias: "-l",
            arity: ArgumentArity.ExactlyOne,
            defaultValue: "eng"
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
        }, new CommitGenBinder(originOption, modelOption, langOption), noPromptOption);

        var commandLineBuilder = new CommandLineBuilder(rootCommand);
        
        commandLineBuilder.UseErrorHandling();
        commandLineBuilder.UseDefaults();

        var parser = commandLineBuilder.Build();

        await parser.InvokeAsync(args);
    }
}