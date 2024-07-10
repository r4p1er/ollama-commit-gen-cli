using OllamaCommitGen.Domain.DataObjects;
using OllamaCommitGen.Domain.Interfaces;

namespace OllamaCommitGen.Domain.Services;

public class CommitGenService(IGitService git, IOllamaService ollama) : ICommitGenService
{
    public async Task<string> GetMessageAsync()
    {
        var changes = git.GetIndexChanges();

        if (string.IsNullOrWhiteSpace(changes)) throw new ArgumentException("There is no any staged changes");

        return await ollama.GenerateCompletionAsync(changes);
    }

    public void Commit(string message)
    {
        git.MakeCommit(message);
    }

    public void AddStreamResponseArrivedHandler(EventHandler<StreamResponseArrivedArgs> handler)
    {
        ollama.StreamResponseArrived += handler;
    }

    public void Dispose()
    {
        git.Dispose();
        ollama.Dispose();
    }
}