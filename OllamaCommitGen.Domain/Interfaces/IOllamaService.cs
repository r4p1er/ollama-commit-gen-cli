using OllamaCommitGen.Domain.DataObjects;

namespace OllamaCommitGen.Domain.Interfaces;

public interface IOllamaService : IDisposable
{
    Task<string> GenerateCompletionAsync(string prompt);
    
    event EventHandler<StreamResponseArrivedArgs>? StreamResponseArrived;
}