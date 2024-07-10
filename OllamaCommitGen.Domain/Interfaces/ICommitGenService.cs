using OllamaCommitGen.Domain.DataObjects;

namespace OllamaCommitGen.Domain.Interfaces;

public interface ICommitGenService : IDisposable
{
    Task<string> GetMessageAsync();
    
    void Commit(string message);

    void AddStreamResponseArrivedHandler(EventHandler<StreamResponseArrivedArgs> handler);
}