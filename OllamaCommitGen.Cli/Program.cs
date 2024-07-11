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

        var noPromptOption = rootCommand.AddOption<bool>(
            name: "--no-prompt",
            description: "Disables the interactive prompt for editing the generated commit message",
            alias: "-p",
            arity: ArgumentArity.ZeroOrOne,
            defaultValue: false
        );

        var primaryOptions = rootCommand.AddPrimaryOptions();
        var modelOptions = rootCommand.AddModelOptions();

        rootCommand.SetHandler(async (commitGenService, noPrompt) =>
        {
            Console.Write("Generated message: ");
            var startPos = Console.GetCursorPosition();

            commitGenService.SetStreamResponseHandler(stream => Console.Write(stream?.Response));
            
            string message = await commitGenService.GetMessageAsync();
            var endPos = Console.GetCursorPosition();
            ConsoleWrapper.ClearFromTo(startPos, endPos);
            
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
        }, new CommitGenBinder(primaryOptions, modelOptions), noPromptOption);

        var commandLineBuilder = new CommandLineBuilder(rootCommand);
        
        commandLineBuilder.UseErrorHandling();
        commandLineBuilder.UseDefaults();

        var parser = commandLineBuilder.Build();

        await parser.InvokeAsync(args);
    }
}