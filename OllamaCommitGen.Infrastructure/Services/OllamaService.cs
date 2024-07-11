using System.Text;
using OllamaCommitGen.Domain.Interfaces;
using OllamaSharp;
using OllamaSharp.Models;

namespace OllamaCommitGen.Infrastructure.Services;

public class OllamaService : IOllamaService
{
    private readonly IOllamaApiClient _client;
    private Action<GenerateCompletionResponseStream?>? _handler;

    public GenerateCompletionRequest Request { get; } = new();
    
    public OllamaService(string uri)
    {
        _client = new OllamaApiClient(uri);
        Request.System =
            "You are to act as the author of a commit message in git. Your mission is to create clean and comprehensive commit messages and explain WHAT were the changes and mainly WHY the changes were done. I'll send you an output of 'git diff --staged' command, and you are to convert it into a commit message. Use the present tense. Lines must not be longer than 74 characters. Your response must consist of only message for the commit.";
        Request.Model = "llama3";
        Request.Stream = false;
        Request.KeepAlive = "5m";
    }
    
    public async Task<string> GenerateCompletionAsync(string prompt)
    {
        Request.Prompt = prompt;

        var result = new StringBuilder();

        if (Request.Stream)
        {
            await foreach (var stream in _client.StreamCompletion(Request))
            {
                result.Append(stream?.Response);
                _handler?.Invoke(stream);
            }
        }
        else
        {
            var ctx = await _client.GetCompletion(Request);
            result.Append(ctx.Response);
        }

        return result.ToString();
    }

    public void SetStreamResponseHandler(Action<GenerateCompletionResponseStream?> handler)
    {
        _handler = handler;
    }
}