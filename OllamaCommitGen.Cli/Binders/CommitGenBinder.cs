using System.CommandLine;
using System.CommandLine.Binding;
using OllamaCommitGen.Domain.Interfaces;
using OllamaCommitGen.Domain.Services;
using OllamaCommitGen.Infrastructure.Services;

namespace OllamaCommitGen.Cli.Binders;

public class CommitGenBinder(Option<string> originOption, Option<string> modelOption) : BinderBase<ICommitGenService>
{
    protected override ICommitGenService GetBoundValue(BindingContext bindingContext)
    {
        var git = new GitService(Directory.GetCurrentDirectory());

        string uri = bindingContext.ParseResult.GetValueForOption(originOption)!;
        string model = bindingContext.ParseResult.GetValueForOption(modelOption)!;
        
        var ollama = new OllamaService(new HttpClient(), uri);

        ollama.RequestBody.Model = model;

        return new CommitGenService(git, ollama);
    }
}