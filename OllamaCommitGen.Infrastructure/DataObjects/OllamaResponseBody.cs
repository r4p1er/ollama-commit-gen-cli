using System.Text.Json.Serialization;

namespace OllamaCommitGen.Infrastructure.DataObjects;

public class OllamaResponseBody
{
    public string Model { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    
    public string Response { get; set; }
    
    public bool Done { get; set; }
}