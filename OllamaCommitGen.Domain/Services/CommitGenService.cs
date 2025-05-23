using OllamaCommitGen.Domain.Abstractions;
using OllamaSharp.Models;

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

    public void SetStreamResponseHandler(Action<GenerateCompletionResponseStream?> handler)
    {
        ollama.SetStreamResponseHandler(handler);
    }

    public void Dispose()
    {
        git.Dispose();
    }
}