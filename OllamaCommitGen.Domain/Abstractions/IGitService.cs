using LibGit2Sharp;

namespace OllamaCommitGen.Domain.Abstractions;

public interface IGitService : IDisposable
{
    string GetIndexChanges();
    
    Commit MakeCommit(string message);
}