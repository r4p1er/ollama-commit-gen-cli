using System.Net.Http.Json;
using System.Text.Json;
using OllamaCommitGen.Domain.Interfaces;
using OllamaCommitGen.Infrastructure.DataObjects;

namespace OllamaCommitGen.Infrastructure.Services;

public class OllamaService(HttpClient http, string uri) : IOllamaService
{
    public OllamaRequestBody RequestBody { get; } = new OllamaRequestBody();
    
    public async Task<string> GenerateCompletionAsync(string prompt)
    {
        RequestBody.Prompt = prompt;
        
        var response = await http.PostAsync($"{uri}/api/generate", JsonContent.Create(RequestBody));
        response.EnsureSuccessStatusCode();
        var res = await response.Content.ReadFromJsonAsync<OllamaResponseBody>();
        
        if (res == null) throw new ArgumentNullException(nameof(OllamaResponseBody));

        return res.Response;
    }

    public void Dispose()
    {
        http.Dispose();
    }
}