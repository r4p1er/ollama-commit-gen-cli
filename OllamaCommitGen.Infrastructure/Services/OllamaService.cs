using System.Net.Http.Json;
using OllamaCommitGen.Domain.Interfaces;
using OllamaCommitGen.Infrastructure.DataObjects;

namespace OllamaCommitGen.Infrastructure.Services;

public class OllamaService(HttpClient http, string uri) : IOllamaService
{
    public async Task<string> GenerateCompletionAsync(string prompt)
    {
        var body = new OllamaRequestBody()
        {
            Prompt = prompt
        };
        
        var response = await http.PostAsync($"{uri}/api/generate", JsonContent.Create(body));
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