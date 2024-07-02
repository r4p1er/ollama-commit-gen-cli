namespace Domain.Interfaces;

public interface IOllamaService : IDisposable
{
    Task<string> GenerateCompletionAsync(string prompt);
}