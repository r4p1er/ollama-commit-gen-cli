using OllamaCommitGen.OllamaSharp.Models;

namespace OllamaCommitGen.Domain.Interfaces;

public interface IOllamaService
{
    Task<string> GenerateCompletionAsync(string prompt);

    void SetStreamResponseHandler(Action<GenerateCompletionResponseStream?> handler);
}