using System.Net.Http.Json;
using System.Text.Json;
using Domain.Interfaces;
using Infrastructure.DataObjects;

namespace Infrastructure.Services;

public class OllamaService : IOllamaService
{
    private readonly HttpClient _http;
    private readonly string _uri;

    public OllamaService(HttpClient http, string uri)
    {
        _http = http;
        _uri = uri;
    }

    public async Task<string> GenerateCompletionAsync(string prompt)
    {
        var body = new OllamaRequestBody()
        {
            Prompt = prompt
        };
        
        var response = await _http.PostAsync($"{_uri}/api/generate", JsonContent.Create(body));
        response.EnsureSuccessStatusCode();
        var res = await response.Content.ReadFromJsonAsync<OllamaResponseBody>();
        
        if (res == null) throw new ArgumentNullException(nameof(OllamaResponseBody));

        return res.Response;
    }

    public void Dispose()
    {
        _http.Dispose();
    }
}