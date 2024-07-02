using System.CommandLine.Binding;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Services;

namespace CLI.Binders;

public class CommitGenBinder : BinderBase<ICommitGenService>
{
    protected override ICommitGenService GetBoundValue(BindingContext bindingContext)
    {
        IGitService git = new GitService(Directory.GetCurrentDirectory());
        IOllamaService ollama = new OllamaService(new HttpClient(), "http://localhost:11434");

        return new CommitGenService(git, ollama);
    }
}