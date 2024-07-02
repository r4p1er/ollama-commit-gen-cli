namespace Infrastructure.DataObjects;

public class OllamaRequestBody
{
    public string Model { get; set; } = "llama3";
    
    public string Prompt { get; set; }
    
    public string Format { get; set; } = "json";
    
    public bool Stream { get; set; } = false;
}