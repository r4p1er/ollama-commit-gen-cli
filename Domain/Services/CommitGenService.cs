using Domain.Interfaces;

namespace Domain.Services;

public class CommitGenService : ICommitGenService
{
    private readonly IGitService _git;
    private readonly IOllamaService _ollama;

    public CommitGenService(IGitService git, IOllamaService ollama)
    {
        _git = git;
        _ollama = ollama;
    }

    public async Task<string> GetMessageAsync()
    {
        var changes = _git.GetIndexChanges();

        return await _ollama.GenerateCompletionAsync(
            "Write only concise commit message using this information: " +
            changes);
    }

    public void Commit(string message)
    {
        _git.MakeCommit(message);
    }

    public void Dispose()
    {
        _git.Dispose();
        _ollama.Dispose();
    }
}