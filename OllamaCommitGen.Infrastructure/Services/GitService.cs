using System.Text;
using LibGit2Sharp;
using OllamaCommitGen.Domain.Abstractions;

namespace OllamaCommitGen.Infrastructure.Services;

public class GitService : IGitService
{
    private readonly IRepository _repo;

    public GitService(string path)
    {
        var repoPath = Repository.Discover(path);

        if (repoPath == null) throw new LibGit2SharpException("Repository not found");

        _repo = new Repository(repoPath);
    }

    public string GetIndexChanges()
    {
        var sb = new StringBuilder();
        
        foreach (var entry in _repo.Diff.Compare<Patch>(_repo.Head.Tip?.Tree, DiffTargets.Index))
        {
            sb.AppendLine($"diff --git a/{entry.Path} b/{entry.Path}");
            sb.AppendLine($"index {entry.OldOid.ToString().Substring(0, 7)}..{entry.Oid.ToString().Substring(0, 7)} {entry.Mode.ToString()}");
            sb.AppendLine($"--- a/{entry.Path}");
            sb.AppendLine($"+++ b/{entry.Path}");
            sb.AppendLine(entry.Patch);
        }
        
        return sb.ToString();
    }

    public Commit MakeCommit(string message)
    {
        var signature = _repo.Config.BuildSignature(DateTimeOffset.Now);

        if (signature == null) throw new LibGit2SharpException("Current user is not defined in the repo config");

        return _repo.Commit(message, signature, signature);
    }

    public void Dispose()
    {
        _repo.Dispose();
    }
}