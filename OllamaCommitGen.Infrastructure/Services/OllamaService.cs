using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using OllamaCommitGen.Domain.DataObjects;
using OllamaCommitGen.Domain.Interfaces;
using OllamaCommitGen.Infrastructure.DataObjects;

namespace OllamaCommitGen.Infrastructure.Services;

public class OllamaService(HttpClient http, string uri) : IOllamaService
{
    public OllamaRequestBody RequestBody { get; } = new OllamaRequestBody();
    
    public event EventHandler<StreamResponseArrivedArgs>? StreamResponseArrived;
    
    public async Task<string> GenerateCompletionAsync(string prompt)
    {
        RequestBody.Prompt = prompt;
        
        var response = await http.PostAsync($"{uri}/api/generate", JsonContent.Create(RequestBody));
        response.EnsureSuccessStatusCode();
        var result = new StringBuilder();

        if (RequestBody.Stream)
        {
            using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                var done = false;

                while (!done)
                {
                    var json = await reader.ReadLineAsync();
                    var body = JsonSerializer.Deserialize<OllamaResponseBody>(json!,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    if (body == null) throw new ArgumentNullException(nameof(OllamaResponseBody));

                    result.Append(body.Response);
                    done = body.Done;
                    StreamResponseArrived?.Invoke(this, new StreamResponseArrivedArgs(body.Response, body.Done));
                }
            }
        }
        else
        {
            var body = await response.Content.ReadFromJsonAsync<OllamaResponseBody>();
            
            if (body == null) throw new ArgumentNullException(nameof(OllamaResponseBody));

            result.Append(body.Response);
        }

        return result.ToString();
    }

    public void Dispose()
    {
        http.Dispose();
    }
}