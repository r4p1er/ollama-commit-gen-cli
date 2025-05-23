using OllamaSharp.Models;

namespace OllamaCommitGen.Domain.Abstractions;

public interface IOllamaService
{
    Task<string> GenerateCompletionAsync(string prompt);

    void SetStreamResponseHandler(Action<GenerateCompletionResponseStream?> handler);
}