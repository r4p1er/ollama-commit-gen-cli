using LibGit2Sharp;

namespace OllamaCommitGen.Domain.Interfaces;

public interface IGitService : IDisposable
{
    string GetIndexChanges();
    
    Commit MakeCommit(string message);
}