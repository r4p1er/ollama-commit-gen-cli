using OllamaSharp.Models;

namespace OllamaCommitGen.Domain.Abstractions;

public interface ICommitGenService : IDisposable
{
    Task<string> GetMessageAsync();
    
    void Commit(string message);

    void SetStreamResponseHandler(Action<GenerateCompletionResponseStream?> handler);
}