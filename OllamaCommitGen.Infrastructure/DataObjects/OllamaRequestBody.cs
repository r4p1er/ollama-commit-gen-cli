using System.Text.Json.Serialization;

namespace OllamaCommitGen.Infrastructure.DataObjects;

public class OllamaRequestBody
{
    public string Model { get; set; } = "llama3";
    
    public string Prompt { get; set; }

    public string System { get; set; } =
        "You are to act as the author of a commit message in git. Your mission is to create clean and comprehensive commit messages and explain WHAT were the changes and mainly WHY the changes were done. I'll send you an output of 'git diff --staged' command, and you are to convert it into a commit message. Use the present tense. Lines must not be longer than 74 characters. Your response must consist of only message for the commit.";
    
    public bool Stream { get; set; } = false;

    [JsonPropertyName("keep_alive")]
    public string KeepAlive { get; set; } = "5m";
}