namespace OllamaCommitGen.Domain.Interfaces;

public interface ICommitGenService : IDisposable
{
    Task<string> GetMessageAsync();
    
    void Commit(string message);
}