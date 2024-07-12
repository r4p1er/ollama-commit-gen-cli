using OllamaCommitGen.OllamaSharp.Models;

namespace OllamaCommitGen.Domain.Interfaces;

public interface ICommitGenService : IDisposable
{
    Task<string> GetMessageAsync();
    
    void Commit(string message);

    void SetStreamResponseHandler(Action<GenerateCompletionResponseStream?> handler);
}