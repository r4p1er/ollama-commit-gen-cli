using Domain.Interfaces;

namespace Domain.Services;

public class CommitGenService(IGitService git, IOllamaService ollama) : ICommitGenService
{
    public async Task<string> GetMessageAsync()
    {
        var changes = git.GetIndexChanges();

        return await ollama.GenerateCompletionAsync(changes);
    }

    public void Commit(string message)
    {
        git.MakeCommit(message);
    }

    public void Dispose()
    {
        git.Dispose();
        ollama.Dispose();
    }
}