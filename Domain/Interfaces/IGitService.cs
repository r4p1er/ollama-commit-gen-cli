using LibGit2Sharp;

namespace Domain.Interfaces;

public interface IGitService : IDisposable
{
    string GetIndexChanges();
    
    Commit MakeCommit(string message);
}